using Microsoft.AspNetCore.Http;
using System;

namespace WebVeiculos.Core.DTO_s
{
    public class VeiculoIFormDto
    {
        public IFormFileCollection FormFile { get; set; }
        public VeiculoDto Veiculo { get; set; }
    }

}
