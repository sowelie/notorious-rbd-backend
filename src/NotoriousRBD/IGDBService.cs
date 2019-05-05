using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using NotoriousRBD.Models;

namespace NotoriousRBD {
    class IGDBService {
        private static readonly string BASE_URL = "https://api-v3.igdb.com";
        private static readonly HttpClient client = new HttpClient();
        private string apiKey = "1690e23e3abe29a9c7942895aae874d9";
        private ILambdaContext context = null;

        public IGDBService(ILambdaContext context) {
            this.context = context;
        }

        public async Task<List<Game>> GetGames(string search) {
            return await postList<Game>("games", search == null ? "fields name;" : $"search \"{search}\"; limit 50; fields name, first_release_date, involved_companies, game_modes, genres, platforms, player_perspectives, themes;");
        }fields

        private async Task<List<T>> postList<T>(string url, string body) {
            var serializer = new DataContractJsonSerializer(typeof(List<T>));
            var message = new HttpRequestMessage(HttpMethod.Post, $"{BASE_URL}/{url}") {
                Content = new StringContent(body),
                Headers = {
                    { "user-key", apiKey }
                }
            };

            var streamTask = client.SendAsync(message);            
            return serializer.ReadObject(await (await streamTask).Content.ReadAsStreamAsync()) as List<T>;
        }
    }
}