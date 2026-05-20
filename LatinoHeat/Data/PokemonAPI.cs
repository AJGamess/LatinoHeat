using LatinoHeat.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace LatinoHeat.Data
{
    public class PokemonAPI
    {
        private readonly HttpClient _client;
        // Constructor to initialize HttpClient with the base address of the PokeAPI
        public PokemonAPI(HttpClient client)
        {
            _client = client;
            if (_client.BaseAddress == null)
            {
                _client.BaseAddress = new Uri("https://pokeapi.co/api/v2/");
            }
        }
        // Method to fetch Pokemon data by name or ID
        public async Task<Pokemon?> GetPokemon(string nameOrId)
        {
            try
            {
                var identifier = nameOrId.Trim().ToLowerInvariant();

                var node = await _client.GetFromJsonAsync<JsonNode>($"pokemon/{identifier}");
                if (node == null) return null;

                // Extract the relevant data from the JSON response
                return new Pokemon
                {
                    Id = (int)node["id"],
                    Name = (string)node["name"],
                    // Get the image URL from the sprites section
                    ImageUrl = (string)node["sprites"]["front_default"],
                    Type = string.Join("/", node["types"].AsArray()
                        .Select(t => (string)t["type"]["name"]))
                };
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }
    }
}


