using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;
//using game_server.Players;
using Assignment2;
using System.Linq;

namespace game_server.Repositories
{
    public class MongoDbRepository : IRepository
    {
            
        private readonly IMongoCollection<Player> _collection;
        private readonly IMongoCollection<BsonDocument> _bsonDocumentCollection;

        public MongoDbRepository()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            IMongoDatabase database = mongoClient.GetDatabase("Game");
            _collection = database.GetCollection<Player>("players");
            _bsonDocumentCollection = database.GetCollection<BsonDocument>("players");
        }

        public async Task<Player> CreatePlayer(Player player)
        {
            await _collection.InsertOneAsync(player);
            return player;
        }

        public async Task<Player[]> GetAllPlayers()
        {
            List<Player> players = await _collection.Find(new BsonDocument()).ToListAsync();
            return players.ToArray();
        }

        public Task<Player[]> GetPlayersMinScore(int score) // 1
        {
            var builder = Builders<Player>.Filter;
            var filter = builder.Gte("Score", score);
            List<Player> _players = _collection.Find(filter).ToListAsync().Result;
            return Task.FromResult(_players.ToArray());
        }
        public Task<Player[]> GetAllPlayersWithItem(itemTypes weapon) // 4
        {
            var builder = Builders<Player>.Filter;
            var filter = builder.ElemMatch(e=> e.itemList, i => i.Type == weapon);
            List<Player> _players = _collection.Find(filter).ToListAsync().Result;
            return Task.FromResult(_players.ToArray());
        }

        public async Task<Player> GetPlayer(Guid playerId)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq("_id", playerId);
            return await _collection.Find(filter).FirstAsync();
        }

        public async Task<Player> ModifyPlayer(Guid id, Player player)
        {
            var filter = Builders<Player>.Filter.Eq("_id", player.Id);
            await _collection.ReplaceOneAsync(filter, player);
            return player;
        }

        public async Task<Player> IncrementPlayerScore(Guid id, int added) // 7
        {
            var filter = Builders<Player>.Filter.Eq("_id", id);
            var addScore = Builders<Player>.Update.Inc(p => p.Score, added);
            var op = new FindOneAndUpdateOptions<Player>()
            {
                ReturnDocument = ReturnDocument.After
            };
            Player player = await _collection.FindOneAndUpdateAsync(filter, addScore, op);
            return player;
        }

        public async Task<Player> DeletePlayer(Guid playerId)
        {
            var filter = Builders<Player>.Filter.Eq("id", playerId);
            _collection.DeleteOne(filter);
            return null;
        }

        public async Task<Item> CreateItem(Guid playerId, Item item)
        {
            var temp = await GetPlayer(playerId);
            temp.itemList.Add(item);
            await ModifyPlayer(playerId, temp);
            return item;
        }

        public async Task<Item> GetItem(Guid playerId, Guid itemId)
        {
            var temp = GetPlayer(playerId);
            foreach(var itemvar in temp.Result.itemList){
                if(itemvar.Id == itemId){
                    return itemvar;
                }
            }
            return null;
        }

        public async Task<Item[]> GetAllItems(Guid playerId)
        {
            return GetPlayer(playerId).Result.itemList.ToArray();
        }

        public async Task<Item> ModifyItem(Guid playerId, Item item)
        {
            var temp = await GetPlayer(playerId);
            foreach(var itemvar in temp.itemList){
                if(itemvar.Id == item.Id){
                    temp.itemList.Remove(itemvar);
                    temp.itemList.Add(item);
                    var filter = Builders<Player>.Filter.Eq("_id", playerId);
                    await _collection.FindOneAndReplaceAsync(filter, temp);
                    return item;
                }
            }
            return null;
        }

        public async Task<Item> DeleteItem(Guid playerId, Guid itemId)
        {
            Player temp = await GetPlayer(playerId);
            foreach(var itemvar in temp.itemList){
                if(itemvar.Id == itemId){
                    temp.itemList.Remove(itemvar);
                    var filter = Builders<Player>.Filter.Eq("_id", playerId);
                    await _collection.FindOneAndReplaceAsync(filter, temp);
                    return itemvar;
                }
            }
            return null;
        }

        public async Task<int> GetMostCommonLevel(){ // 11
            var temp = _collection.Aggregate()
                .Project(f=> new {Level = f.Level})
                .Group(f=>f.Level, f => new {Level = f.Key, Count = f.Sum(u=>1)})
                .SortByDescending(f => f.Count)
                .Limit(3);
            var lista = await temp.ToListAsync();
            return lista[0].Level;
        
        }
    }
}