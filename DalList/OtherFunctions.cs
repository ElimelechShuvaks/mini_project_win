using DO;
namespace Dal;

internal class OtherFunctions
{
    internal static void exceptionNotFound(string entityName, int id = 0)
    {
        throw new EntityNotExistException($"{entityName} with id: {id} doesn't exsist in data source");
    }

    internal static void exceptionFound(string entityName, int id)
    {
        throw new IdExistException($"{entityName} with id: {id} already exsist in data source");
    }
}
