using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebVeiculos.Core.DTO_s;
using WebVeiculos.Models.Entities;

namespace WebVeiculos.Core
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Veiculo, VeiculoDto>().ReverseMap();
            CreateMap<Arquivo, ArquivoDto>().ReverseMap();
        }
    }
}
