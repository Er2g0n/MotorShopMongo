using MongoDB.Driver;
using Structure_Context;
using Structure_Core.BaseClass;
using Structure_Core.TransactionManagement;
using Structure_Interface.IBaseServices;


namespace Structure_Service.TransactionManagement;
public class OrderProvider : ICRUD_Service<Order, string>
{
    private readonly IMongoCollection<Order> _collection;
    private readonly MongoDBContext _mongoContext;

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
            return new ResultService<Order> { Data = null, Code = "404", Message = "Order not found" };

        return new ResultService<Order> { Data = order, Code = "200", Message = "Success" };
    }


    public async Task<ResultService<Order>> Create(Order entity)
    {
        entity.ID = null;
        entity.CreatedDate = DateTime.Now;
        entity.UpdatedDate = DateTime.Now;
        if(entity.Items != null && entity.Items.Count > 0)
        {
            foreach (var item in entity.Items)
            {
                item.ID = null; // Đặt ID mới cho từng OrderItem
                item.CreatedDate = DateTime.Now;
                item.UpdatedDate = DateTime.Now;
            }
        }
        await _collection.InsertOneAsync(entity);
        return new ResultService<Order> { Data = entity, Code = "200", Message = "Created successfully" };
    }

    public async Task<ResultService<Order>> Update(Order entity)
    {
        var existingOrder = await _collection.Find(x => x.ID == entity.ID).FirstOrDefaultAsync();
        if (existingOrder == null)
            return new ResultService<Order> { Data = null, Code = "404", Message = "Order not found" };

        // Giữ nguyên CreatedBy & CreatedDate
        entity.CreatedBy = existingOrder.CreatedBy;
        entity.CreatedDate = existingOrder.CreatedDate;

        // Cập nhật UpdatedDate
        entity.UpdatedDate = DateTime.Now;
        await _collection.ReplaceOneAsync(x => x.ID == entity.ID, entity);

        return new ResultService<Order> { Data = entity, Code = "200", Message = "Updated successfully" };
    }
    public async Task<ResultService<Order>> Delete(string ID)
    {
        var result = await _collection.DeleteOneAsync(x => x.ID == ID);
        if (result.DeletedCount == 0)
            return new ResultService<Order> { Data = null, Code = "404", Message = "Order not found" };

        return new ResultService<Order> { Data = null, Code = "200", Message = "Deleted successfully" };
    }

}
