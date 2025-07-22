using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Structure_Core.TransactionManagement;
public class Order : BaseClass.BaseClass
{
    public string OrderNumber { get; set; } = string.Empty;
    public string CustomerID { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public List<OrderItem> Items { get; set; } = new(); // ✅ Chuỗi các dữ liệu
    public decimal TotalAmount { get; set; }
}
