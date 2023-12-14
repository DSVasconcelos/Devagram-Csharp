using Devagram.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Security.Policy;

namespace Devagram.Services
{
    public class CosmicService
    {
        public string EnviarImagem(ImagemDto imagemDto) 
        {
            Stream imagem;
            
            imagem = imagemDto.Imagem.OpenReadStream();

            var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "hWWee6dPicHQpAeYRCRFodSAbGLr9MxkRZtBQhgjkR9ieWcF9d");

            var request = new HttpRequestMessage(HttpMethod.Post, "file");

            var conteudo = new MultipartFormDataContent {
                {new StreamContent(imagem), "media", imagemDto.Nome }
            };

             request.Content = conteudo;

            var retornoReq = client.PostAsync("https://workers.cosmicjs.com/v3/buckets/devagram-fotos/media", request.Content).Result;

            var urlRetorno = retornoReq.Content.ReadFromJsonAsync<CosmicRespostaDto>();

            return urlRetorno.Result.media.url;

        }

    }

}
