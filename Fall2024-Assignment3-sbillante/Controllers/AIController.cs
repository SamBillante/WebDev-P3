using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Fall2024_Assignment3_sbillante.Controllers
{
    public class AIController() : Controller
    {
        public async Task<string> CallChatGPT(string prompt, IConfiguration config)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("api-key", config["OpenAI-Key"]);

                var requestBody = new
                {
                    messages = new[]
                    {
                        new { role = "system", content = "you generate realistic fake reviews for movies. delimit each review with a | character to signify a new review is starting. Do NOT use numbers."},
                        new { role = "user", content = prompt }
                    },
                    max_tokens = 950
                };

                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"{config["OpenAI-Endpoint"]}", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    return result.choices[0].message.content;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Request to Azure OpenAI failed: {errorContent}");
                }
            }
        }
    }
}
