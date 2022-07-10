using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebVeiculos.Core.DTO_s;
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

        public VeiculoService(IVeiculoRepository repository, IMapper mapper)
        {
            _veiculoRepository = repository;
            _mapper = mapper;
        }

        public async Task<bool> CreateVeiculoService(VeiculoDto veiculo)
        {
            try
            {
                var veiculoMapper = _mapper.Map<Veiculo>(veiculo);

                var result = await _veiculoRepository.CreateVeiculo(veiculoMapper);

                return result;
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
