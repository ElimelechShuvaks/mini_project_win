using OtherFunction;

namespace BO;

/// <summary>
/// class of OrderTracking
/// </summary>
public class OrderTracking
{
    /// <summary>
    /// id of Order Tracking
    /// </summary>
    public int OrderId { get; set; }
    /// <summary>
    /// status of Order Tracking
    /// </summary>
    public OrderStatus? Status { get; set; }

    /// <summary>
    /// List of DateTime and Description.
    /// </summary>
    public List<Tuple<DateTime?, string?>>? TuplesOfDateAndDescription { get; set; }

    public override string ToString()
    {
        return $@"
Order id: {OrderId}
Status: {Status}
Order Tracking: 
{TuplesOfDateAndDescription?.GetToStrings()}
";
    }
}
