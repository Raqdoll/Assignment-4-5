// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;

// namespace Assignment2
// {
//     public class InMemoryRepository : IRepository
//     {
//         public List<Player> playerList = new List<Player>();

//         public async Task<Player> CreatePlayer(Player player)
//         {
//             playerList.Add(player);
//             return player;
//         }

//         public async Task<Player> DeletePlayer(Guid id)
//         {
//             foreach(var playervar in playerList){
//                 if(playervar.Id == id){
//                     playerList.Remove(playervar);
//                     return playervar;
//                 }
//             }
//             return null;
//         }

//         public async Task<Player> GetPlayer(Guid id)
//         {
//             foreach(var playervar in playerList){
//                 if(playervar.Id == id){
//                     return playervar;
//                 }
//             }
//             return null;
//         }

//         public async Task<Player[]> GetAllPlayers()
//         {
//             return playerList.ToArray();
//         }

//         public async Task<Player> ModifyPlayer(Guid id, Player player)
//         {
//             foreach(var playervar in playerList){
//                 if(playervar.Id == id){
//                     playervar.Score = player.Score;
//                     return playervar;
//                 }
//             }
//             return null;
//         }
//          public List<Item> itemList = new List<Item>();

//         public async Task<Item> CreateItem(Item item)
//         {
//             itemList.Add(item);
//             return item;
//         }

//         public async Task<Item> DeleteItem(Guid id)
//         {
//             foreach(var itemvar in itemList){
//                 if(itemvar.Id == id){
//                     itemList.Remove(itemvar);
//                     return itemvar;
//                 }
//             }
//             return null;
//         }

//         public async Task<Item> GetItem(Guid id)
//         {
//             foreach(var itemvar in itemList){
//                 if(itemvar.Id == id){
//                     return itemvar;
//                 }
//             }
//             return null;
//         }

//         public async Task<Item[]> GetAllItems()
//         {
//             return itemList.ToArray();
//         }

//         public async Task<Item> ModifyItem(Guid id, ModifiedItem item)
//         {
//             foreach(var itemvar in itemList){
//                 if(itemvar.Id == id){
//                     itemvar.Level = item.Level;
//                     return itemvar;
//                 }
//             }
//             return null;
//         }
//     }
// }