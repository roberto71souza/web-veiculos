﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using WebVeiculos.Models.Entities;
using WebVeiculos.Models.Entities.Paginacao;
using WebVeiculos.Models.Repositories.Contratos;

namespace WebVeiculos.Models.Repositories.Implementacao
{
    public class VeiculoRepository : IVeiculoRepository
    {
        private ConexaoDbDapper _conexaoDb { get; set; }
        public VeiculoRepository(ConexaoDbDapper conexaoDb)
        {
            _conexaoDb = conexaoDb;
        }

        public async Task<bool> CreateVeiculo(Veiculo veiculo)
        {
            using (var conexao = _conexaoDb.GetConnectionDb())
            {
                using (var transaction = await conexao.BeginTransactionAsync())
                {
                    if (veiculo.EhValido)
                    {
                        var query = @"INSERT INTO tbl_veiculo(nome_proprietario,modelo_veiculo,fabricante_veiculo,ano_veiculo,cor_veiculo,estado,cidade,informacoes_gerais)
                              VALUES(@NomeProprietario,@ModeloVeiculo,@FabricanteVeiculo,@AnoVeiculo,@CorVeiculo,@Estado,@Cidade,@InformacoesGerais);
                              SELECT LAST_INSERT_ID();";

                        var resultQuery = await conexao.QueryMultipleAsync(query, veiculo, transaction);

                        var idVeiculo = resultQuery.Read<int>().First();

                        if (veiculo.Arquivos.Count > 0 && idVeiculo != 0)
                        {
                            var result = await SalvarArquivosCarro(idVeiculo, veiculo.Arquivos.ToList(), conexao);

                            if (result)
                            {
                                await transaction.CommitAsync();

                                return true;
                            }
                        }

                    }
                    await transaction.RollbackAsync();

                    return false;
                }
            }
        }

        public async Task<PaginacaoList> GetAllVeiculos(PaginacaoList paginacao)
        {
            paginacao.Veiculos = new List<Veiculo>();

            using (var conexao = _conexaoDb.GetConnectionDb())
            {
                paginacao.TotalItems = await conexao.QueryFirstAsync<int>("SELECT COUNT(id) FROM tbl_veiculo");

                paginacao.TotalPaginas = (int)Math.Ceiling((decimal)paginacao.TotalItems / paginacao.ItemsPorPagina);

                paginacao.BuscarItemsApartir = (paginacao.PaginaAtual - 1) * paginacao.ItemsPorPagina;

                var query = @"SELECT v.id as Id, v.nome_proprietario as NomeProprietario,
                                      v.modelo_veiculo as ModeloVeiculo, v.fabricante_veiculo as FabricanteVeiculo,
                                      v.ano_veiculo as AnoVeiculo, v.cor_veiculo as CorVeiculo,
                                      v.estado as Estado, v.cidade as Cidade, v.informacoes_gerais as InformacoesGerais,
                                      au.id as Id,au.legenda as Legenda, au.nome_arquivo as NomeArquivo,au.id_veiculo as IdVeiculo
                                          FROM (SELECT * FROM tbl_veiculo LIMIT @limit,@offset) v
                                          INNER JOIN tbl_arquivo_upload au
                                          ON v.id = au.id_veiculo;";

                await conexao.QueryAsync<Veiculo, Arquivo, Veiculo>(sql: query, param: new { limit = paginacao.BuscarItemsApartir, offset = paginacao.ItemsPorPagina },
                                        map: (veic, arq) =>
                                        {
                                            Veiculo veiculo = paginacao.Veiculos.FirstOrDefault(x => x.Id == veic.Id);

                                            if (veiculo is null)
                                            {
                                                veiculo = new Veiculo(veic.Id, veic.NomeProprietario, veic.ModeloVeiculo, veic.FabricanteVeiculo,
                                                             veic.AnoVeiculo, veic.CorVeiculo, veic.Estado, veic.Cidade, veic.InformacoesGerais);

                                                paginacao.Veiculos.Add(veiculo);
                                            }

                                            if (arq is not null)
                                            {
                                                veiculo.AddArquivos(new Arquivo(arq.Id, arq.Legenda, arq.NomeArquivo, arq.IdVeiculo));
                                            }
                                            return veiculo;
                                        });
            }
            return paginacao;
        }


        public async Task<Veiculo> GetVeiculoById(int id)
        {
            Veiculo veiculo = null;

            using (var conexao = _conexaoDb.GetConnectionDb())
            {
                var query = @"SELECT v.id as Id, v.nome_proprietario as NomeProprietario,
	                                 v.modelo_veiculo as ModeloVeiculo, v.fabricante_veiculo as FabricanteVeiculo,
	                                 v.ano_veiculo as AnoVeiculo, v.cor_veiculo as CorVeiculo,
	                                 v.estado as Estado, v.cidade as Cidade, v.informacoes_gerais as InformacoesGerais,
	                                 au.id as Id, au.legenda as Legenda, au.nome_arquivo as NomeArquivo, au.id_veiculo as IdVeiculo
                                     FROM tbl_veiculo v
                                     INNER JOIN tbl_arquivo_upload au
                                     ON v.id = au.id_veiculo  WHERE v.id = @Id;";

                await conexao.QueryAsync<Veiculo, Arquivo, Veiculo>(sql: query, param: new { id = id },
                   map: (veic, arq) =>
                   {
                       if (veiculo is null)
                       {
                           veiculo = new Veiculo(veic.Id, veic.NomeProprietario, veic.ModeloVeiculo, veic.FabricanteVeiculo,
                                                 veic.AnoVeiculo, veic.CorVeiculo, veic.Estado, veic.Cidade, veic.InformacoesGerais);
                       }

                       if (arq is not null)
                       {
                           veiculo.AddArquivos(new Arquivo(arq.Id, arq.Legenda, arq.NomeArquivo, arq.IdVeiculo));
                       }

                       return veiculo;
                   });

            }
            return veiculo;
        }

        public async Task<ICollection<Veiculo>> GetUltimosVeiculosCadastrados(int quantidade)
        {
            var veiculos = new List<Veiculo>();

            using (var conexao = _conexaoDb.GetConnectionDb())
            {
                var query = @"SELECT v.id as Id, v.nome_proprietario as NomeProprietario,
                                     v.modelo_veiculo as ModeloVeiculo, v.fabricante_veiculo as FabricanteVeiculo,
	                                 v.ano_veiculo as AnoVeiculo, v.cor_veiculo as CorVeiculo,
	                                 v.estado as Estado, v.cidade as Cidade, v.informacoes_gerais as InformacoesGerais,
	                                 au.id as Id,au.legenda as Legenda, au.nome_arquivo as NomeArquivo,au.id_veiculo as IdVeiculo
                                     FROM(SELECT * FROM tbl_veiculo ORDER BY id DESC LIMIT @quantidade) v
                                     INNER JOIN tbl_arquivo_upload au
                                     ON v.id = au.id_veiculo; ";

                await conexao.QueryAsync<Veiculo, Arquivo, Veiculo>(sql: query, param: new { quantidade = quantidade },
                        map: (veic, arq) =>
                         {
                             var veiculo = veiculos.FirstOrDefault(x => x.Id == veic.Id);

                             if (veiculo is null)
                             {
                                 veiculo = new Veiculo(veic.Id, veic.NomeProprietario, veic.ModeloVeiculo, veic.FabricanteVeiculo, veic.AnoVeiculo,
                                                       veic.CorVeiculo, veic.Estado, veic.Cidade, veic.InformacoesGerais);
                                 veiculos.Add(veiculo);
                             }

                             if (arq is not null)
                             {
                                 veiculo.AddArquivos(new Arquivo(arq.Id, arq.Legenda, arq.NomeArquivo, arq.IdVeiculo));
                             }

                             return veic;
                         });
            }
            return veiculos;
        }

        public async Task<PaginacaoList> GetVeiculoByModelo(PaginacaoList paginacao, string modelo)
        {
            paginacao.Veiculos = new List<Veiculo>();

            using (var conexao = _conexaoDb.GetConnectionDb())
            {
                paginacao.TotalItems = await conexao.QueryFirstAsync<int>("SELECT COUNT(id) FROM tbl_veiculo WHERE modelo_veiculo LIKE CONCAT('%',@modelo,'%');",
                                                                            param: new { modelo = modelo });

                paginacao.TotalPaginas = (int)Math.Ceiling((decimal)paginacao.TotalItems / paginacao.ItemsPorPagina);

                paginacao.BuscarItemsApartir = (paginacao.PaginaAtual - 1) * paginacao.ItemsPorPagina;

                var query = @"SELECT v.id as Id, v.nome_proprietario as NomeProprietario,
                                     v.modelo_veiculo as ModeloVeiculo, v.fabricante_veiculo as FabricanteVeiculo,
                                     v.ano_veiculo as AnoVeiculo, v.cor_veiculo as CorVeiculo,
                                     v.estado as Estado, v.cidade as Cidade, v.informacoes_gerais as InformacoesGerais,
                                     au.id as Id,au.legenda as Legenda, au.nome_arquivo as NomeArquivo,au.id_veiculo as IdVeiculo
                                     FROM (SELECT * FROM tbl_veiculo WHERE modelo_veiculo LIKE CONCAT('%',@modelo,'%') LIMIT @limit,@offset) v
                                     INNER JOIN tbl_arquivo_upload au
                                     ON v.id = au.id_veiculo;";

                await conexao.QueryAsync<Veiculo, Arquivo, Veiculo>(sql: query, param: new { modelo = modelo, limit = paginacao.BuscarItemsApartir, offset = paginacao.ItemsPorPagina },
                    map: (veic, arqu) =>
                    {
                        var veiculo = paginacao.Veiculos.FirstOrDefault(x => x.Id == veic.Id);

                        if (veiculo is null)
                        {
                            veiculo = new Veiculo(veic.Id, veic.NomeProprietario, veic.ModeloVeiculo, veic.FabricanteVeiculo,
                                                  veic.AnoVeiculo, veic.CorVeiculo, veic.Estado, veic.Cidade, veic.InformacoesGerais);
                            paginacao.Veiculos.Add(veiculo);
                        }

                        if (arqu is not null)
                        {
                            veiculo.AddArquivos(new Arquivo(arqu.Id, arqu.Legenda, arqu.NomeArquivo, arqu.IdVeiculo));
                        }

                        return veiculo;
                    });

            }
            return paginacao;
        }

        private async Task<bool> SalvarArquivosCarro(int idVeiculo, ICollection<Arquivo> arquivos, DbConnection conexao)
        {
            var query = @"INSERT INTO tbl_arquivo_upload (legenda,nome_arquivo,id_veiculo) VALUES (@Legenda,@NomeArquivo,@IdVeiculo);";

            foreach (var arquivo in arquivos)
            {
                arquivo.UpdateEntidadeArquivo(new Arquivo(arquivo.Id, arquivo.Legenda, arquivo.NomeArquivo, idVeiculo));

                if (!arquivo.EhValido)
                    return false;

                await conexao.ExecuteAsync(query, arquivo);
            }
            return true;
        }

    }
}
