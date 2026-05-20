using Microsoft.AspNetCore.Mvc;
using LatinoHeat.Data;
using LatinoHeat.Models;
using System;
using System.Threading.Tasks;

namespace LatinoHeat.Controllers
{
    public class PokemonController : Controller
    {
        private readonly PokemonAPI _pokemonApi;

        //Injecting API class via the constructor
        public PokemonController(PokemonAPI pokemonApi)
        {
            _pokemonApi = pokemonApi;
        }

        // This action will handle hitting https://localhost:XXXX/Pokemon
        public async Task<IActionResult> Index()
        {
            var randomId = new Random().Next(1, 1026).ToString();

            // Fire off the API requests in parallel
            var firstTask = _pokemonApi.GetPokemon("flareon");
            var secondTask = _pokemonApi.GetPokemon("oshawott");
            var randomTask = _pokemonApi.GetPokemon(randomId);

            await Task.WhenAll(firstTask, secondTask, randomTask);

            var viewModel = new PokemonViewModel
            {
                FirstPokemon = await firstTask,
                SecondPokemon = await secondTask,
                RandomPokemon = await randomTask
            };
  
            return View(viewModel);
        }
    }
}