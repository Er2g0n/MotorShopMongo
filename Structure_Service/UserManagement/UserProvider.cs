using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Structure_Context;
using Structure_Core.BaseClass;
using Structure_Core.Extensions;
using Structure_Core.TransactionManagement;
using Structure_Core.UserManagement;
using Structure_Interface.IBaseServices;
using Structure_Interface.IUserManagement;


namespace Structure_Service.UserManagement;

public class UserProvider : ICRUD_Service<User, string>, IUserProvider
{
    private readonly IMongoCollection<User> _collection;
    private readonly MongoDBContext _mongoDBContext;
    public UserProvider(MongoDBContext mongoDBContext)
    {
        _mongoDBContext = mongoDBContext;
        _collection = _mongoDBContext.GetCollection<User>("User");
    }
    public async Task<ResultService<IEnumerable<User>>> GetAll()
    {
        var customers = await _collection.Find(_ => true).ToListAsync();

        return new ResultService<IEnumerable<User>> { Data = customers, Code = "200", Message = "Success" };
    }
    public async Task<ResultService<User>> Create(User entity)
    {
        entity.ID = null;
        entity.CreatedDate = DateTime.Now;
        entity.UpdatedDate = DateTime.Now;
        await _collection.InsertOneAsync(entity);
        return new ResultService<User> { Data = entity, Code = "200", Message = "Created successfully" };
    }
    public async Task<ResultService<User>> Get(string ID)
    {
        var user = await _collection.Find(x => x.ID == ID).FirstOrDefaultAsync();
        if (user == null)
            return new ResultService<User> { Data = null, Code = "404", Message = "User not found" };

        return new ResultService<User> { Data = user, Code = "200", Message = "Success" };
    }

    public async Task<ResultService<User>> Update(User entity)
    {
        //Lấy dữ liệu gốc từ DB
        var existingUser = await _collection.Find(x => x.ID == entity.ID).FirstOrDefaultAsync();
        if (existingUser == null)
            return new ResultService<User> { Data = null, Code = "404", Message = "User not found" };

        // Giữ nguyên CreatedBy & CreatedDate
        entity.CreatedBy = existingUser.CreatedBy;
        entity.CreatedDate = existingUser.CreatedDate;


        // Cập nhật UpdatedDate
        entity.UpdatedDate = DateTime.Now;
        await _collection.ReplaceOneAsync(x => x.ID == entity.ID, entity);

        return new ResultService<User> { Data = entity, Code = "200", Message = "Updated successfully" };

    }

    public async Task<ResultService<User>> Delete(string ID)
    {
        var result = await _collection.DeleteOneAsync(x => x.ID == ID);
        if (result.DeletedCount == 0)
            return new ResultService<User> { Data = null, Code = "404", Message = "User not found" };

        return new ResultService<User> { Data = null, Code = "200", Message = "Deleted successfully" };
    }

    public async Task<ResultService<IEnumerable<User>>> MongoSearch(string keyword)
    {
        List<User> result;

        if (string.IsNullOrWhiteSpace(keyword))
        {
            result = await _collection.Find(_ => true).ToListAsync();
        }
        else
        {
            var filterBuilder = Builders<User>.Filter;

            var filter = filterBuilder.Or(
                filterBuilder.Regex(u => u.FirstName, new BsonRegularExpression($"^{keyword}", "i")),
                filterBuilder.Regex(u => u.LastName, new BsonRegularExpression($"^{keyword}", "i")),
                filterBuilder.Regex(u => u.Email, new BsonRegularExpression($"^{keyword}", "i"))
            );

            result = await _collection.Find(filter).ToListAsync();
        }

        return new ResultService<IEnumerable<User>>
        {
            Data = result,
            Code = "200",
            Message = "Filtered success"
        };
    }

    public async Task<ResultService<IEnumerable<User>>> LinqSearch(string keyword,string orderByDescending = null)
    {
        var users = _collection.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            users = users.Search(keyword);
        }
        users = users.Sort(orderByDescending); // gọi extension method sort

        var results = await users.ToListAsync();

        return new ResultService<IEnumerable<User>>
        {
            Data = results,
            Code = "200",
            Message = "Search completed"
        };
    }
    public async Task<ResultService<IEnumerable<User>>> FilterDateRange(DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _collection.AsQueryable();
        query = query.FilterByDateRange(startDate, endDate);
        var results = await query.ToListAsync();

        return new ResultService<IEnumerable<User>>
        {
            Data = results,
            Code = "200",
            Message = "Filtered by date range"
        };
    }

    public async Task<ResultService<IEnumerable<User>>> Search(string? keyword = null, string? orderBy = null, DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _collection.AsQueryable();
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
            Code = "200",
            Message = "Filtered by date range"
        };
    }
}
