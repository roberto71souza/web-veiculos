using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVeiculos.Core.DTO_s;
using WebVeiculos.Core.Helper;
using WebVeiculos.Core.Services.Contratos;
using WebVeiculos.Models.Entities;
using WebVeiculos.Models.Entities.Paginacao;
using WebVeiculos.Models.Repositories.Contratos;

namespace WebVeiculos.Core.Services.Implementacao
{
    public class VeiculoService : IVeiculoService
    {
        private IVeiculoRepository _veiculoRepository { get; set; }
        private IMapper _mapper { get; set; }
        private Util _util { get; set; }

        public VeiculoService(IVeiculoRepository repository, IMapper mapper, Util util)
        {
            _veiculoRepository = repository;
            _mapper = mapper;
            _util = util;
        }

        public async Task<(bool, VeiculoDto)> CreateVeiculoService(List<IFormFile> arquivos, VeiculoDto veiculoDto)
        {
            try
            {
                var veiculoMap = _mapper.Map<Veiculo>(veiculoDto);

                veiculoMap.AddArquivos(arquivos);

                var result = await _veiculoRepository.CreateVeiculo(veiculoMap);

                if (result)
                {
                    await _util.SalvarImagem(arquivos, veiculoMap.Arquivos.ToList());
                }

                _mapper.Map(veiculoMap, veiculoDto);

                return (result, veiculoDto);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PaginacaoListDto> GetAllVeiculosService(PaginacaoListDto paginacao)
        {
            try
            {
                var paginacaoMap = _mapper.Map<PaginacaoList>(paginacao);

                var veiculosPaginResult = await _veiculoRepository.GetAllVeiculos(paginacaoMap);

                var VeiculosPaginDto = _mapper.Map<PaginacaoListDto>(veiculosPaginResult);

                return VeiculosPaginDto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<VeiculoDto> GetVeiculoByIdService(int id)
        {
            try
            {
                var veiculo = await _veiculoRepository.GetVeiculoById(id);

                var veiculoMapper = _mapper.Map<VeiculoDto>(veiculo);

                return veiculoMapper;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ICollection<VeiculoDto>> GetUltimosVeiculosCadastradosService(int quantidade)
        {
            try
            {
                var veiculosResult = await _veiculoRepository.GetUltimosVeiculosCadastrados(quantidade);

                var veiculosMap = _mapper.Map<ICollection<VeiculoDto>>(veiculosResult);

                return veiculosMap;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PaginacaoListDto> GetVeiculoByModeloService(PaginacaoListDto paginacao, string modelo)
        {
            try
            {
                var paginacaoMap = _mapper.Map<PaginacaoList>(paginacao);

                var veiculosPaginResult = await _veiculoRepository.GetVeiculoByModelo(paginacaoMap, modelo);

                var VeiculosPaginDto = _mapper.Map<PaginacaoListDto>(veiculosPaginResult);

                return VeiculosPaginDto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
