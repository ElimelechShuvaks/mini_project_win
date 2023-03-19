
namespace DalApi;

public interface ICrud<T> where T : struct
{
    int Add(T t);

    void Delete(int id);

    void Update(T t);

    T Get(Func<T?, bool>? func);

    IEnumerable<T?> GetList(Func<T?, bool>? func = null);
}
