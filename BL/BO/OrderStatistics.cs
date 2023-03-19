using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;


/// <summary>
/// I added this class in order to use it in the order class where I will make groups in each group an amount from each order status,
/// confirmed, sent, and arrived
/// </summary>
public class OrderStatistics
{

    public int CountSumStatus { get; set; }

    public OrderStatus? Status { get; set; }

    public override string ToString()
    {
        return $@"
count Sum Status {CountSumStatus}
Order status: {Status}
";
    }
}
