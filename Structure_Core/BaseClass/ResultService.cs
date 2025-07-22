
namespace Structure_Core.BaseClass;

public class ResultService<T>
{
    public string Message { get; set; }
    public string Code { get; set; }
    public T Data { get; set; }
}

