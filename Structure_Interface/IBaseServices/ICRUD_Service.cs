using Structure_Core.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structure_Interface.IBaseServices;
public interface ICRUD_Service<T, U>
{
    Task<ResultService<IEnumerable<T>>> GetAll();
    Task<ResultService<T>> Get(U ID);
    Task<ResultService<T>> Create(T entity);
    Task<ResultService<T>> Update(T entity);
    Task<ResultService<T>> Delete(U ID);
}
