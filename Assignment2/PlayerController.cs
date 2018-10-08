using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assignment2
{
    [Route("api/players")]
    [ApiController]
    [Authorize(Policy = "BasicClients")]
    public class PlayerController : Controller
    {
        PlayerProcessor processor;
        public PlayerController(PlayerProcessor proc){
            processor = proc;
        }
        [HttpGet("{id}")]
            public Task<Player> GetPlayer(Guid id){
            return processor.GetPlayer(id);
        }
        [HttpGet]
        public Task<Player[]> GetAllPlayers(int? minScore){

            if (minScore.HasValue){
                return processor.GetPlayersMinScore((int)minScore);
            }
            return processor.GetAllPlayers();
        }
        [HttpGet("{weapon}")]
        public Task<Player[]> GetAllPlayersWithItem([FromQuery]itemTypes weapon){

            return processor.GetAllPlayersWithItem(weapon);
        }
        [HttpPost]
        public Task<Player> CreatePlayer([FromBody]NewPlayer player){
            return processor.CreatePlayer(player);
        }
        [HttpPut("{id}")]
        public Task<Player> ModifyPlayer(Guid id, [FromBody]ModifiedPlayer player){
            return processor.ModifyPlayer(id,player);
        }
        [HttpDelete("{id}")]
        [Authorize (Policy = "AdminOnly")]
        public Task<Player> DeletePlayer(Guid id){
            return processor.DeletePlayer(id);
        }
        [HttpGet("Lvl")]
        public async Task<int> GetMostCommonLevel(){
            return await processor.GetMostCommonLevel();
        }
        [HttpPut("{score}")]
        public Task<Player> IncrementPlayerScore([FromBody]Guid id, int added){
        return processor.IncrementPlayerScore(id, added);
        }
    }
}