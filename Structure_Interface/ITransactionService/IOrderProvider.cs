using Structure_Core.BaseClass;
using Structure_Core.TransactionManagement;


namespace Structure_Interface.ITransactionService;
public interface IOrderProvider
{
    Task<ResultService<IEnumerable<Order>>> Search(string? keyword = null, string? orderBy = null, DateTime? startDate = null, DateTime? endDate = null);
}
