using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace WebVeiculos.Core.DTO_s
{
    public class VeiculoIFormDto
    {
        public List<IFormFile> FormFile { get; set; }
        public VeiculoDto Veiculo { get; set; }
    }

}
