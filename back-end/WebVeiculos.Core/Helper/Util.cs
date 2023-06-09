using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WebVeiculos.Models.Entities;

namespace WebVeiculos.Core.Helper
{
    public class Util
    {
        public async Task SalvarImagem(List<IFormFile> files, List<Arquivo> arquivos)
        {
            for (int i = 0; i < files.Count; i++)
            {
                var imagemPath = Path.Combine(Environment.CurrentDirectory, "Arquivos", "Imagens", arquivos[i].NomeArquivo);

                using (var upload = new FileStream(imagemPath, FileMode.Create, FileAccess.Write))
                {
                    await files[i].CopyToAsync(upload);
                }
            }

        }
    }
}
