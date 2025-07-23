using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Structure_Context;
using Structure_Core.BaseClass;
using Structure_Core.Extensions;
using Structure_Core.ProductManagement;
using Structure_Core.TransactionManagement;
using Structure_Core.UserManagement;
using Structure_Interface.IBaseServices;
using Structure_Interface.IUserService;


namespace Structure_Service.UserService;

public class UserProvider : ICRUD_Service<User, string>, IUserProvider
{
    private readonly IMongoCollection<User> _userCollection;
    private readonly IMongoCollection<Order> _orderCollection;
    private readonly IMongoCollection<Product> _productCollection;
    private readonly MongoDBContext _mongoDBContext;
    public UserProvider(MongoDBContext mongoDBContext)
    {
        _mongoDBContext = mongoDBContext;
        _userCollection = _mongoDBContext.GetCollection<User>("User");
        _orderCollection = _mongoDBContext.GetCollection<Order>("Order");
        _productCollection = _mongoDBContext.GetCollection<Product>("Product");
    }
    public async Task<ResultService<IEnumerable<User>>> GetAll()
    {
        var customers = await _userCollection.Find(_ => true).ToListAsync();

        return new ResultService<IEnumerable<User>> { Data = customers, Code = "0", Message = "Success" };
    }
    public async Task<ResultService<User>> Get(string ID)
    {
        var user = await _userCollection.Find(x => x.ID == ID).FirstOrDefaultAsync();
        if (user == null)
            return new ResultService<User> { Data = null, Code = "1", Message = "User not found" };

        return new ResultService<User> { Data = user, Code = "0", Message = "Success" };
    }

    public async Task<ResultService<User>> Create(User entity)
    {
        entity.ID = null;
        entity.CreatedDate = DateTime.Now;
        entity.UpdatedDate = DateTime.Now;
        await _userCollection.InsertOneAsync(entity);
        return new ResultService<User> { Data = entity, Code = "0", Message = "Success" };
    }

    public async Task<ResultService<User>> Update(User entity)
    {
        //Lấy dữ liệu gốc từ DB
        var existingUser = await _userCollection.Find(x => x.ID == entity.ID).FirstOrDefaultAsync();
        if (existingUser == null)
            return new ResultService<User> { Data = null, Code = "1", Message = "User not found" };

        // Giữ nguyên CreatedBy & CreatedDate
        entity.CreatedBy = existingUser.CreatedBy;
        entity.CreatedDate = existingUser.CreatedDate;


        // Cập nhật UpdatedDate
        entity.UpdatedDate = DateTime.Now;
        await _userCollection.ReplaceOneAsync(x => x.ID == entity.ID, entity);

        return new ResultService<User> { Data = entity, Code = "0", Message = "Success" };

    }

    public async Task<ResultService<User>> Delete(string ID)
    {
        var result = await _userCollection.DeleteOneAsync(x => x.ID == ID);
        if (result.DeletedCount == 0)   
            return new ResultService<User> { Data = null, Code = "1", Message = "User not found" };

        return new ResultService<User> { Data = null, Code = "0", Message = "Success" };
    }

    public async Task<ResultService<IEnumerable<User>>> MongoSearch(string keyword)
    {
        List<User> result;

        if (string.IsNullOrWhiteSpace(keyword))
        {
            result = await _userCollection.Find(_ => true).ToListAsync();
        }
        else
        {
            var filterBuilder = Builders<User>.Filter;

            var filter = filterBuilder.Or(
                filterBuilder.Regex(u => u.FirstName, new BsonRegularExpression($"^{keyword}", "i")),
                filterBuilder.Regex(u => u.LastName, new BsonRegularExpression($"^{keyword}", "i")),
                filterBuilder.Regex(u => u.Email, new BsonRegularExpression($"^{keyword}", "i"))
            );

            result = await _userCollection.Find(filter).ToListAsync();
        }

        return new ResultService<IEnumerable<User>>
        {
            Data = result,
            Code = "0",
            Message = "Success"
        };
    }

    public async Task<ResultService<IEnumerable<User>>> LinqSearch(string keyword, string orderByDescending = null)
    {
        var users = _userCollection.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            users = users.Search(keyword);
        }
        users = users.Sort(orderByDescending); // gọi extension method sort

        var results = await users.ToListAsync();

        return new ResultService<IEnumerable<User>>
        {
            Data = results,
            Code = "0",
            Message = "Success"
        };
    }
    public async Task<ResultService<IEnumerable<User>>> FilterDateRange(DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _userCollection.AsQueryable();
        query = query.FilterByDateRange(startDate, endDate);
        var results = await query.ToListAsync();

        return new ResultService<IEnumerable<User>>
        {
            Data = results,
            Code = "0",
            Message = "Filtered by date range"
        };
    }

    public async Task<ResultService<IEnumerable<User>>> Search(string? keyword = null, string? orderBy = null, DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _userCollection.AsQueryable();
        // Apply search
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Search(keyword);
        }

        // Apply date range filter
        query = query.FilterByDateRange(startDate, endDate);

        // Apply sorting
        query = query.Sort(orderBy); var results = await query.ToListAsync();

        return new ResultService<IEnumerable<User>>
        {
            Data = results,
            Code = "0",
            Message = "Filtered by key word, orderBy, Date"
        };
    }

    public async Task<ResultService<IEnumerable<User>>> FilterUsersByProduct(string productId)
    {
        ResultService<IEnumerable<User>> result = new();

        if (string.IsNullOrWhiteSpace(productId))
        {
            result.Code = "-1";
            result.Message = "Product ID must not be empty.";
            result.Data = null;
            return result;
        }

        try
        {
            var orders = await _orderCollection
                .Find(o => o.Items.Any(item => item.ProductID == productId))
                .ToListAsync();

            if (orders == null || !orders.Any())
            {
                result.Code = "404";
                result.Message = "No orders found for the given product ID.";
                result.Data = null;
                return result;
            }

            var userIds = orders.Select(o => o.CustomerID).Distinct().ToList();

            if (!userIds.Any())
            {
                result.Code = "404";
                result.Message = "No customers found for the given product ID.";
                result.Data = null;
                return result;
            }

            var users = await _userCollection.Find(u => userIds.Contains(u.ID)).ToListAsync();

            if (users == null || !users.Any())
            {
                result.Code = "404";
                result.Message = "Users linked to orders not found.";
                result.Data = null;
                return result;
            }

            result.Code = "0";
            result.Message = "Filtered users by product ";
            result.Data = users;
            return result;
        }
        catch (MongoException mex)
        {
            result.Code = "2";
            result.Message = $"MongoDB error: {mex.Message}";
            result.Data = null;
            return result;
        }
        catch (Exception ex)
        {
            result.Code = "999";
            result.Message = $"Unexpected error: {ex.Message}";
            result.Data = null;
            return result;
        }
    }

}
