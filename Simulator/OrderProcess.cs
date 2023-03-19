using BlApi;
using BO;

namespace Simulator;

public static class Simulator
{
    static IBl? bl = Factory.get();

    /// <summary>
    /// a random variable to use when it's needed.
    /// </summary>
    public static readonly Random random = new Random(DateTime.Now.Millisecond);

    private const int c_timeSleep = 1000;

    private static volatile bool stopSimulation;

    private static event Action? s_stopSimulation;

    public static event Action? s_StopSimulation
    {
        add => s_stopSimulation += value;
        remove => s_StopSimulation -= value;
    }

    private static event Action<int, object?>? s_updateSimulation;

    public static event Action<int, object>? s_UpdateSimulation
    {
        add => s_updateSimulation += value;
        remove => s_updateSimulation -= value;
    }

    public static void StartSimulator()
    {
        Thread thread = new Thread(simulatorAction);
        thread.Start();
    }

    private static void simulatorAction(object? obj)
    {
        while (!stopSimulation)
        {
            try
            {
                int? orderId = bl!.Order.nextOrderSending();

                if (orderId is null)
                    Thread.Sleep(c_timeSleep);

                else
                {
                    int _orderId = orderId ?? throw new InvalidCastException("");

                    Order order = bl.Order.GetDetailsOrder(_orderId);

                    int nextOrderStatus = (int)order.Status! + 1;
                    int TreatmentTime = random.Next(3, 7);
                    OrderProcess orderProcess = new OrderProcess
                    {
                        CurrentOrder = order,
                        NextOrderStatus =
                        (BO.OrderStatus)nextOrderStatus,
                        EndTreatment = DateTime.Now + new TimeSpan(0, 0, TreatmentTime)
                    };

                    s_updateSimulation?.Invoke(1, orderProcess);

                    Thread.Sleep(TreatmentTime * 1000);

                    switch (orderProcess.CurrentOrder.Status)
                    {
                        case OrderStatus.Shipied:
                            bl.Order.OrderDeliveryUpdate(orderProcess.CurrentOrder.Id);
                            break;

                        default:
                            bl.Order.OrderShippingUpdate(orderProcess.CurrentOrder.Id);
                            break;
                    }
                }
                Thread.Sleep(c_timeSleep);
            }
            catch (Exception) { }
        }
    }

    public static void StopSimulator()
    {
        stopSimulation = true;
        s_stopSimulation?.Invoke();
    }
}

/// <summary>
/// help class for the simulator.
/// </summary>
public class OrderProcess
{
    public BO.Order? CurrentOrder { get; set; }

    public DateTime? CurrentTime { get; set; } = DateTime.Now;

    public BO.OrderStatus? NextOrderStatus { get; set; }

    public DateTime? EndTreatment { get; set; }
}