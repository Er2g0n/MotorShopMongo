using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structure_Context;
public class MongoDBContext
{
    private readonly IMongoDatabase _database;
    private readonly IConfiguration _configuration;

    public MongoDBContext(IConfiguration configuration)
    {
        _configuration = configuration;
        var connectionString = _configuration.GetConnectionString("MongoDb"); // Sử dụng key "MongoDb" từ appsettings.json
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase("ShopMongo"); // Database cố định là "ShopMongo"
    }

    public IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        return _database.GetCollection<T>(collectionName);
    }
}