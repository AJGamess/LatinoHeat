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

        // Constructor to inject the PokemonAPI class
        public PokemonController(PokemonAPI pokemonApi)
        {
            _pokemonApi = pokemonApi;
        }

        // Action method to fetch and display Pokemon data
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