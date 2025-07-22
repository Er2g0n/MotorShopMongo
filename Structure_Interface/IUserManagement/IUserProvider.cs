using Structure_Core.BaseClass;
using Structure_Core.User;


namespace Structure_Interface.IUserManagement;
public interface IUserProvider
{
    Task<ResultService<IEnumerable<User>>> MongoSearch(string keyword);
    Task<ResultService<IEnumerable<User>>> LinqSearch(string keyword,string orderByOrderByDescending);
}
