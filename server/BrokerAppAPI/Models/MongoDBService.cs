using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.Extensions.Options;
using BrokerAppAPI.Models;

namespace BrokerAppAPI.Models
{
    public class MongoDBService
    {
        //private readonly IMongoCollection<Users> _playlistCollection;

        public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            //_playlistCollection = database.GetCollection<Users>(mongoDBSettings.Value.CollectionName);
        }

        //public async Task<List<Playlist>> GetAsync() { }
        //public async Task CreateAsync(Playlist playlist) { }
        public async Task AddToPlaylistAsync(string id, string movieId) { }
        public async Task DeleteAsync(string id) { }

    }
}
