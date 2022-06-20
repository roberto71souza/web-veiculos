using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebVeiculos.Models.Entities;
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
            using (var conexao = _conexaoDb.ConnectionDapper)
            {
                if (veiculo.EhValido)
                {
                    var query = @"INSERT INTO tbl_veiculo(nome_proprietario,modelo_veiculo,fabricante_veiculo,ano_veiculo,cor_veiculo,estado,cidade,informacoes_gerais)
                              VALUES(@NomeProprietario,@ModeloVeiculo,@FabricanteVeiculo,@AnoVeiculo,@CorVeiculo,@Estado,@Cidade,@InformacoesGerais);
                              SELECT LAST_INSERT_ID();";

                    var resultQuery = await conexao.QueryMultipleAsync(query, veiculo);

                    var idVeiculo = resultQuery.Read<int>().First();

                    if (veiculo.Arquivos.Count() > 0 && idVeiculo != 0)
                    {
                        SalvarArquivosCarro(idVeiculo, veiculo.Arquivos);
                    }

                    if (idVeiculo != 0)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public async Task<ICollection<Veiculo>> GetAllVeiculos()
        {
            var veiculos = new List<Veiculo>();

            using (var conexao = _conexaoDb.ConnectionDapper)
            {
                var query = @"SELECT v.id as Id, v.nome_proprietario as NomeProprietario,
	                                 v.modelo_veiculo as ModeloVeiculo, v.fabricante_veiculo as FabricanteVeiculo,
	                                 v.ano_veiculo as AnoVeiculo, v.cor_veiculo as CorVeiculo,
	                                 v.estado as Estado, v.cidade as Cidade, v.informacoes_gerais as InformacoesGerais,
	                                 au.id as Id,au.legenda as Legenda, au.nome_arquivo as NomeArquivo,au.id_veiculo as IdVeiculo
                                     FROM tbl_veiculo v
                                     INNER JOIN tbl_arquivo_upload au
                                     ON v.id = au.id_veiculo;";

                await conexao.QueryAsync<Veiculo, Arquivo, Veiculo>(sql: query,
                                        map: (veic, arq) =>
                                        {
                                            Veiculo veiculo = veiculos.FirstOrDefault(x => x.Id == veic.Id);

                                            if (veiculo is null)
                                            {
                                                veiculo = new Veiculo(veic.Id, veic.NomeProprietario, veic.ModeloVeiculo, veic.FabricanteVeiculo,
                                                             veic.AnoVeiculo, veic.CorVeiculo, veic.Estado, veic.Cidade, veic.InformacoesGerais);

                                                veiculos.Add(veiculo);
                                            }

                                            if (veiculo.Arquivos is null)
                                            {
                                                veiculo.Arquivos = new List<Arquivo>();

                                            }

                                            if (arq is not null)
                                            {
                                                veiculo.Arquivos.Add(new Arquivo(arq.Id, arq.Legenda, arq.NomeArquivo, arq.IdVeiculo));
                                            }
                                            return veiculo;
                                        });
            }
            return veiculos;
        }


        public async Task<Veiculo> GetVeiculoById(int id)
        {
            Veiculo veiculo = null;

            using (var conexao = _conexaoDb.ConnectionDapper)
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

                       if (veiculo.Arquivos is null)
                       {
                           veiculo.Arquivos = new List<Arquivo>();
                       }

                       if (arq is not null)
                       {
                           veiculo.Arquivos.Add(new Arquivo(arq.Id, arq.Legenda, arq.NomeArquivo, arq.IdVeiculo));
                       }

                       return veiculo;
                   });

            }
            return veiculo;
        }

        public async Task<ICollection<Veiculo>> GetVeiculoByModelo(string modelo)
        {
            var veiculos = new List<Veiculo>();

            using (var conexao = _conexaoDb.ConnectionDapper)
            {

                var query = @"SELECT v.id as Id, v.nome_proprietario as NomeProprietario,
	                                 v.modelo_veiculo as ModeloVeiculo, v.fabricante_veiculo as FabricanteVeiculo,
	                                 v.ano_veiculo as AnoVeiculo, v.cor_veiculo as CorVeiculo,
	                                 v.estado as Estado, v.cidade as Cidade, v.informacoes_gerais as InformacoesGerais,
	                                 au.id as Id, au.legenda as Legenda, au.nome_arquivo as NomeArquivo, au.id_veiculo as IdVeiculo
                                     FROM tbl_veiculo v
                                     INNER JOIN tbl_arquivo_upload au
                                     ON v.id = au.id_veiculo
                                     WHERE v.modelo_veiculo LIKE CONCAT('%',@modelo,'%');";

                await conexao.QueryAsync<Veiculo, Arquivo, Veiculo>(sql: query, param: new { modelo = modelo },
                    map: (veic, arqu) =>
                    {
                        var veiculo = veiculos.FirstOrDefault(x => x.Id == veic.Id);

                        if (veiculo is null)
                        {
                            veiculo = new Veiculo(veic.Id, veic.NomeProprietario, veic.ModeloVeiculo, veic.FabricanteVeiculo,
                                                  veic.AnoVeiculo, veic.CorVeiculo, veic.Estado, veic.Cidade, veic.InformacoesGerais);
                            veiculos.Add(veiculo);
                        }

                        if (veiculo.Arquivos is null)
                        {
                            veiculo.Arquivos = new List<Arquivo>();
                        }

                        if (arqu is not null)
                        {
                            veiculo.Arquivos.Add(new Arquivo(arqu.Id, arqu.Legenda, arqu.NomeArquivo, arqu.IdVeiculo));
                        }

                        return veiculo;
                    });

            }
            return veiculos;
        }

        private void SalvarArquivosCarro(int idVeiculo, ICollection<Arquivo> arquivos)
        {
            using (var conexao = _conexaoDb.ConnectionDapper)
            {
                var query = @"INSERT INTO tbl_arquivo_upload (legenda,nome_arquivo,id_veiculo) VALUES (@Legenda,@NomeArquivo,@IdVeiculo);";

                foreach (var arquivo in arquivos)
                {
                    arquivo.UpdateEntidadeArquivo(new Arquivo(arquivo.Id, arquivo.Legenda, arquivo.NomeArquivo, idVeiculo));

                    if (arquivo.EhValido)
                    {
                        conexao.Execute(query, arquivo);
                    }
                }
            }
        }

    }
}
