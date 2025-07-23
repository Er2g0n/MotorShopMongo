using Structure_Core.BaseClass;
using Structure_Core.UserManagement;


namespace Structure_Interface.IUserService;
public interface IUserProvider
{
    Task<ResultService<IEnumerable<User>>> MongoSearch(string keyword);
    Task<ResultService<IEnumerable<User>>> LinqSearch(string keyword, string orderByOrderByDescending);
    Task<ResultService<IEnumerable<User>>> FilterDateRange(DateTime? startDate = null, DateTime? endDate = null);
    Task<ResultService<IEnumerable<User>>> Search(string? keyword = null, string? orderBy = null, DateTime? startDate = null, DateTime? endDate = null);
    Task<ResultService<IEnumerable<User>>> FilterUsersByProduct(string productID);
}
