using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BuzzBotTwo.External.SoftResIt.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BuzzBotTwo.External.SoftResIt
{
    public class SoftResClient : ISoftResClient
    {
        private readonly HttpClient _client;
        private const string ApiBaseAddress = @"https://softres.it/api";

        public SoftResClient()
        {
            _client = new HttpClient();
        }
        public async Task<RaidModel> CreateRaid(SoftResRaidBuilderExpression builderExpression)
        {
            var builder = new SoftResRaidBuilder();
            builderExpression(builder);
            var requestBody = builder.Build();
            return await Post<RaidModel>("raid/create", requestBody);
        }

        private async Task<T> Post<T>(string route, object requestPayload)
        {
            var requestBody = JsonConvert.SerializeObject(requestPayload, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()

            });
            var rawResponse =
                await _client.PostAsync($"{ApiBaseAddress}/{route}", new StringContent(requestBody, Encoding.UTF8, "application/json"));
            if (!rawResponse.IsSuccessStatusCode)
            {
                return default(T);
            }

            var rawResponseJson = await rawResponse.Content.ReadAsStringAsync();
            var response =
                JsonConvert.DeserializeObject<T>(rawResponseJson);
            return response;
        }
    }
}