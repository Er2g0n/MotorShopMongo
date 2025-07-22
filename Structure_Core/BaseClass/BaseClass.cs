using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;


namespace Structure_Core.BaseClass;
public abstract class BaseClass
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ID { get; set; } = ObjectId.GenerateNewId().ToString(); // ✅ thêm default
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public string UpdatedBy { get; set; }
    public DateTime UpdatedDate { get; set; } = DateTime.Now;
}
