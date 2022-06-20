using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebVeiculos.Core.DTO_s;

namespace WebVeiculos.WebApi.Helper
{
    public class Util
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        public Util(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        public List<ArquivoDto> CriarListaArquivos(IFormFileCollection files, VeiculoDto veiculo)
        {
            var arquivos = new List<ArquivoDto>();

            foreach (var file in files)
            {
                Random randNum = new Random();
                var arquivo = new ArquivoDto();
                
                var extensao = Path.GetExtension(file.FileName);
                var modelo = veiculo.ModeloVeiculo.Split();
                var proprietario = veiculo.NomeProprietario.Split();
                var random = randNum.Next(500);

                arquivo.Legenda = veiculo.ModeloVeiculo;

                arquivo.NomeArquivo = $"{modelo[0].ToLower()}-{proprietario[0].ToLower()}-{random}{extensao}";

                arquivos.Add(arquivo);
            }

            return arquivos;
        }

        public bool ExtensaoValida(IFormFileCollection files)
        {
            foreach (var file in files)
            {
                var extensao = Path.GetExtension(file.FileName);
                var extensoesValidas = new string[] { ".jpg", ".jpeg", ".png" };

                if (!extensoesValidas.Contains(extensao.ToLower()))
                {
                    return false;
                }
            }
            return true;
        }

        public async Task SalvarImagem(IFormFileCollection files, List<ArquivoDto> arquivos)
        {
            for (int i = 0; i < files.Count; i++)
            {
                var imagemPath = Path.Combine(_hostEnvironment.ContentRootPath, $"Recursos/Imagens", arquivos[i].NomeArquivo);

                using (var upload = new FileStream(imagemPath, FileMode.Create))
                {
                    await files[i].CopyToAsync(upload);
                }
            }

        }
    }
}
