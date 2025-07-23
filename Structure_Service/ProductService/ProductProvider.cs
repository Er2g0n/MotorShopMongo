

using MongoDB.Driver;
using Structure_Context;
using Structure_Core.BaseClass;
using Structure_Core.ProductManagement;
using Structure_Interface.IBaseServices;

namespace Structure_Service.ProductService;
public class ProductProvider : ICRUD_Service<Product, string>
{
    private readonly MongoDBContext _mongoDBContext;
    private readonly IMongoCollection<Product> _collection;
    public ProductProvider(MongoDBContext mongoDBContext)
    {
        _mongoDBContext = mongoDBContext;
        _collection = _mongoDBContext.GetCollection<Product>("Product");
    }
    public async Task<ResultService<IEnumerable<Product>>> GetAll()
    {
        var products = await _collection.Find(_ => true).ToListAsync();
        return new ResultService<IEnumerable<Product>>
        { Code = "200", Message = "Success", Data = products };
    }
    public Task<ResultService<Product>> Get(string ID)
    {
        throw new NotImplementedException();
    }

    public Task<ResultService<Product>> Create(Product entity)
    {
        throw new NotImplementedException();
    }

    public Task<ResultService<Product>> Delete(string ID)
    {
        throw new NotImplementedException();
    }

    public Task<ResultService<Product>> Update(Product entity)
    {
        throw new NotImplementedException();
    }
}
