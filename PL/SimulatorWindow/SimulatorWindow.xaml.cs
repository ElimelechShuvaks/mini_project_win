using BlApi;
using Simulator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;

namespace PL.SimulatorWindow;

/// <summary>
/// Interaction logic for SimulatorWindow.xaml
/// </summary>
public partial class SimulatorWindow : Window
{
    IBl? bl = Factory.get();

    public string Clock
    {
        get { return (string)GetValue(ClockProperty); }
        set { SetValue(ClockProperty, value); }
    }

    public static readonly DependencyProperty ClockProperty =
        DependencyProperty.Register("Clock", typeof(string), typeof(SimulatorWindow));

    public OrderProcess? OrderProcess
    {
        get { return (OrderProcess?)GetValue(orderProcessProperty); }
        set { SetValue(orderProcessProperty, value); }
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty orderProcessProperty =
        DependencyProperty.Register("OrderProcess", typeof(OrderProcess), typeof(SimulatorWindow));



    private BackgroundWorker backgroundWorker;

    private const int c_timeSleep = 1000;

    private Dictionary<int, Action<object?>> _actions;

    public SimulatorWindow()
    {
        Clock = DateTime.Now.ToLongTimeString();

        InitializeComponent();

        _actions = new Dictionary<int, Action<object?>>();

        backgroundWorker = new BackgroundWorker
        {
            WorkerReportsProgress = true,
            WorkerSupportsCancellation = true
        };

        backgroundWorker.DoWork += BackgroundWorker_DoWork;
        backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
        backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;

        backgroundWorker.RunWorkerAsync();
    }

    private void BackgroundWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
    {

    }

    private void BackgroundWorker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
    {
        if (_actions.ContainsKey(e.ProgressPercentage))
            _actions[e.ProgressPercentage]?.Invoke(e.UserState);
    }

    private void BackgroundWorker_DoWork(object? sender, DoWorkEventArgs e)
    {
        Simulator.Simulator.s_StopSimulation += () => backgroundWorker.CancelAsync();

        Simulator.Simulator.s_UpdateSimulation += onProgressChanged;

        Simulator.Simulator.StartSimulator();

        addAction(0, updateClock);
        addAction(1, updateOrder);

        while (!backgroundWorker.CancellationPending)
        {
            Thread.Sleep(c_timeSleep);
            onProgressChanged(0, DateTime.Now);
        }
    }

    private void updateClock(object? obj)
    {
        DateTime? dateTime = (DateTime?)obj;
        if (dateTime is not null)
        {
            Clock = dateTime.Value.ToLongTimeString();
        }
    }


    private void updateOrder(object? obj)
    {
        if (obj is OrderProcess orderProcess)
            OrderProcess = orderProcess;
        else
            OrderProcess = null;
    }

    private void onProgressChanged(int progressPercentage, object? userState) =>
        backgroundWorker.ReportProgress(progressPercentage, userState);

    private void addAction(int key, Action<object?> action)
       => _actions.Add(key, action);

    private void stopButton_Click(object sender, RoutedEventArgs e)
    {
        Simulator.Simulator.StopSimulator();
        Close();
    }
}
