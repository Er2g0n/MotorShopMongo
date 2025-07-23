using MongoDB.Bson.Serialization.Attributes;

namespace Structure_Core.ProductManagement;
public class Product : BaseClass.BaseClass
{
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    [BsonIgnore] // Bỏ qua khi serialize/deserialize
    public decimal Total => Quantity * UnitPrice;

}
