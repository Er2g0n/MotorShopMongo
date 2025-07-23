using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Structure_Context;
using Structure_Core.BaseClass;
using Structure_Core.Extensions;
using Structure_Core.TransactionManagement;
using Structure_Core.UserManagement;
using Structure_Interface.IBaseServices;
using Structure_Interface.ITransactionService;


namespace Structure_Service.TransactionService;
public class OrderProvider : ICRUD_Service<Order, string>, IOrderProvider
{
    private readonly MongoDBContext _mongoContext;
    private readonly IMongoCollection<Order> _collection;

    public OrderProvider(MongoDBContext mongoContext)
    {
        _mongoContext = mongoContext;
        _collection = _mongoContext.GetCollection<Order>("Order");
    }
    public async Task<ResultService<IEnumerable<Order>>> GetAll()
    {
        var orders = await _collection.Find(_ => true).ToListAsync();
        return new ResultService<IEnumerable<Order>> { Data = orders, Code = "200", Message = "Success" };
    }
    public async Task<ResultService<Order>> Get(string ID)
    {
        var order = await _collection.Find(x => x.ID == ID).FirstOrDefaultAsync();
        if (order == null)
            return new ResultService<Order> { Data = null, Code = "1", Message = "Order not found" };

        return new ResultService<Order> { Data = order, Code = "0", Message = "Success" };
    }


    public async Task<ResultService<Order>> Create(Order entity)
    {
        entity.ID = null;
        entity.CreatedDate = DateTime.Now;
        entity.UpdatedDate = DateTime.Now;
        if (entity.Items != null && entity.Items.Count > 0)
        {
            foreach (var item in entity.Items)
            {
                item.ID = null; // Đặt ID mới cho từng OrderItem
                item.CreatedDate = DateTime.Now;
                item.UpdatedDate = DateTime.Now;
            }
        }
        await _collection.InsertOneAsync(entity);
        return new ResultService<Order> { Data = entity, Code = "0", Message = "Success" };
    }

    public async Task<ResultService<Order>> Update(Order entity)
    {
        var existingOrder = await _collection.Find(x => x.ID == entity.ID).FirstOrDefaultAsync();
        if (existingOrder == null)
            return new ResultService<Order> { Data = null, Code = "1", Message = "Order not found" };

        // Giữ nguyên CreatedBy & CreatedDate
        entity.CreatedBy = existingOrder.CreatedBy;
        entity.CreatedDate = existingOrder.CreatedDate;

        // Cập nhật UpdatedDate
        entity.UpdatedDate = DateTime.Now;
        await _collection.ReplaceOneAsync(x => x.ID == entity.ID, entity);

        return new ResultService<Order> { Data = entity, Code = "0", Message = "Success" };
    }
    public async Task<ResultService<Order>> Delete(string ID)
    {
        var result = await _collection.DeleteOneAsync(x => x.ID == ID);
        if (result.DeletedCount == 0)
            return new ResultService<Order> { Data = null, Code = "1", Message = "Order not found" };

        return new ResultService<Order> { Data = null, Code = "0", Message = "Success" };
    }
    public async Task<ResultService<IEnumerable<Order>>> Search(string? keyword = null, string? orderBy = null, DateTime? startDate = null, DateTime? endDate = null)
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

        return new ResultService<IEnumerable<Order>>
        {
            Data = results,
            Code = "0",
            Message = "Success"
        };
    }
}
