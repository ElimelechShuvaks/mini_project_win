
namespace DO;

public class IdNotExistException : Exception
{
    public IdNotExistException(string message) : base(message) { }
}

public class IdExistException : Exception
{
    public IdExistException(string message) : base(message) { }
}

public class EntityNotExistException : Exception
{
    public EntityNotExistException(string message) : base(message) { }
}

[Serializable]
public class DalConfigException : Exception
{
    public DalConfigException(string message) : base(message) { }
    public DalConfigException(string message, Exception ex) : base(message, ex) { }
}