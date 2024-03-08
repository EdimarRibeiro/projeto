using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProjectManager.Domain.Models;

namespace ProjectManager.Web.Rotinas
{
    public class ConsultaResponsavel
    {
        private static HttpClient client = new HttpClient();
        private static string url = "https://randomuser.me/api/";

        public async Task<ResponseRandomUser> Consultar()
        {
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var responseResult = response.Content.ReadAsStringAsync().Result;
                JObject tmpRetorno = JObject.Parse(responseResult);
                JsonSerializer serializer = new JsonSerializer();
                return (ResponseRandomUser)serializer.Deserialize(new JTokenReader(tmpRetorno), typeof(ResponseRandomUser));
            }
            return null;
        }
    }
}
