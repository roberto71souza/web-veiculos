using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebVeiculos.Core.DTO_s;
using WebVeiculos.Core.Services.Contratos;
using WebVeiculos.Models.Entities;
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

        public async Task<ICollection<VeiculoDto>> GetAllVeiculosService()
        {
            try
            {
                var veiculos = await _veiculoRepository.GetAllVeiculos();

                var veiculosMapper = _mapper.Map<ICollection<VeiculoDto>>(veiculos);

                return veiculosMapper;
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

        public async Task<ICollection<VeiculoDto>> GetVeiculoByModeloService(string modelo)
        {
            try
            {
                var veiculos = await _veiculoRepository.GetVeiculoByModelo(modelo);

                var veiculosMapper = _mapper.Map<ICollection<VeiculoDto>>(veiculos);

                return veiculosMapper;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
