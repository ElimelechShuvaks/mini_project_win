
using System.Runtime.Serialization;

namespace BO;

// i make a class that all BL exceptions inherit from him, and ve inherit from the basic exception class
// it makes easier the code in the try/catch.

public class BlExceptions : Exception
{
    public BlExceptions(string message) : base(message) { }
    public BlExceptions(string message, Exception innerException) : base(message, innerException) { }
}

public class EntityNotExistException : BlExceptions
{
    public EntityNotExistException(string message) : base(message) { }
    public EntityNotExistException(string message, DO.EntityNotExistException exception) : base(message, exception) { }
}


public class IdExistException : BlExceptions
{
    public IdExistException(string message) : base(message) { }
    public IdExistException(string message, DO.IdExistException exception) : base(message, exception) { }
}

public class DalConfigException : BlExceptions
{
    public DalConfigException(string message) : base(message) { }
    public DalConfigException(string message, Exception inner) : base(message, inner) { }  
}

public class IdNotValidException : BlExceptions
{
    public IdNotValidException(string message) : base(message) { }
}

public class CanNotRemoveProductException : BlExceptions
{
    public CanNotRemoveProductException(string message) : base(message) { }
}

public class NotExsitInStockException : BlExceptions
{
    public NotExsitInStockException(string message) : base(message) { }
}

public class StatusErrorException : BlExceptions
{
    public StatusErrorException(string message) : base(message) { }
}

public class EmptyCartException : BlExceptions
{
    public EmptyCartException(string message) : base(message) { }
}

public class PriceNotValidException : BlExceptions
{
    public PriceNotValidException(string message) : base(message) { }
}

public class NameNotValidException : BlExceptions
{
    public NameNotValidException(string message) : base(message) { }
}

public class InStockNotValidException : BlExceptions
{
    public InStockNotValidException(string message) : base(message) { }
}

public class NegativeAmountExeption : BlExceptions
{
    public NegativeAmountExeption(string? message) : base(message) { }

    public NegativeAmountExeption(string? message, Exception? innerException) : base(message, innerException) { }
}

public class EmailNotValidException : BlExceptions
{
    public EmailNotValidException(string message) : base(message) { }
}

public class AddressNotValidException : BlExceptions
{
    public AddressNotValidException(string message) : base(message) { }
}