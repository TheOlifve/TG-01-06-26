namespace Simulation;

public class Simulator
{
    private int _seed = 42;
    
    private int _totalSimulationTicks = 1000;
    private int _startItemId = 100;
    
    private int _orderLineCapacity = 5;
    
    private int _storageCapacityPerType = 50;
    private int _stockCapacityPerType = 200;
    
    private int _minQualityCheckTicks = 1;
    private int _maxQualityCheckTicks = 3;
    private int _qualityPassPercentage = 80;
    
    private int _transportArrivalInterval = 4;

    private int _transportCapacityPerArrival = 6;

    private int _machineAProductionInterval = 1;
    private int _machineBProductionInterval = 2;
    private int _machineCProductionInterval = 3;

    private int _machineATotalItems = 35;
    private int _machineBTotalItems = 18;
    private int _machineCTotalItems = 12;

    private int _ticks = 1;

    private Machine _machineA;
    private Machine _machineB;
    private Machine _machineC;
    
    private OrderLine _orderLine;
    
    private QualityChecker _qualityChecker;
    
    private Warehouse _warehouse;
    private Warehouse _stock;
    
    private Transport _transport;

    private bool CheckSimulationEnd()
    {
        if (_machineA.CurrentStatus == Status.Finished
            && _machineB.CurrentStatus == Status.Finished
            && _machineC.CurrentStatus == Status.Finished
            && _orderLine.GetStatus() == Status.Empty
            && _warehouse.GetStatus() == Status.Empty
            && _qualityChecker.CurrentStatus == Status.Empty)
            return true;
        return false;
    }
    
    public void Simulate()
    {
        Item item;
        int qcPassed = 0;
        int qcFailed = 0;
        int orderLineBlockedTicks = 0;
        while (_ticks < _totalSimulationTicks && !CheckSimulationEnd())
        {
            Console.WriteLine($"\n=== Tick [{_ticks}] ===");
            _machineC.OnTick(_startItemId);
            if (_machineC.CurrentStatus == Status.Complete)
            {
                if (_orderLine.HasAvailableSlots())
                {
                    item = _machineC.TakeProducedItem();
                    _startItemId++;
                    _orderLine.AddItem(item);
                    Console.WriteLine($"Machine C produced item [{item.Category}|{item.ID}] and put it on the order line.");
                }
                else
                {
                    Console.WriteLine("Machine C production blocked - OrderLine is full");
                    orderLineBlockedTicks++;
                }
            }
            else if (_machineC.CurrentStatus == Status.InProgress)
                Console.WriteLine($"Machine C production in progress");

            _machineB.OnTick(_startItemId);
            if (_machineB.CurrentStatus == Status.Complete)
            {
                if (_orderLine.HasAvailableSlots())
                {
                    item = _machineB.TakeProducedItem();
                    _startItemId++;
                    _orderLine.AddItem(item);
                    Console.WriteLine($"Machine B produced item [{item.Category}|{item.ID}] and put it on the order line.");
                }
                else
                {
                    Console.WriteLine("Machine B production blocked - OrderLine is full");
                    orderLineBlockedTicks++;
                }
            }
            else if  (_machineB.CurrentStatus == Status.InProgress)
                Console.WriteLine($"Machine B production in progress");

            _machineA.OnTick(_startItemId);
            if (_machineA.CurrentStatus == Status.Complete)
            {
                if (_orderLine.HasAvailableSlots())
                {
                    item = _machineA.TakeProducedItem();
                    _startItemId++;
                    _orderLine.AddItem(item);
                    if (item != null)
                        Console.WriteLine($"Machine A produced item [{item.Category}|{item.ID}] and put it on the order line.");
                }
                else
                {
                    Console.WriteLine("Machine A production blocked - OrderLine is full");
                    orderLineBlockedTicks++;
                }
            }
            else if (_machineA.CurrentStatus == Status.InProgress)
                Console.WriteLine($"Machine A production in progress");
            
            if (_qualityChecker.CurrentStatus == Status.Empty && _orderLine.HasItem())
            {
                item = _orderLine.GetItem();
                
                _qualityChecker.AddItem(item);
                
                if (item != null)
                    Console.WriteLine($"Item [{item.Category}|{item.ID}] moved from OrderLine to QualityChecker");
            }
            else if (_qualityChecker.CurrentStatus == Status.InProgress)
            {
                Console.WriteLine($"Quality check in progress for item [{_qualityChecker.PeekItem().Category}|{_qualityChecker.PeekItem().ID}]");

                _qualityChecker.OnTick();
            }
            else if (_qualityChecker.CurrentStatus == Status.Passed)
            {
                item = _qualityChecker.PeekItem();
                if (_warehouse.HasEmptySlotForItem(item))
                {
                    Console.WriteLine($"Quality check completed for item [{item.Category}|{item.ID}] : PASS");
                    _qualityChecker.PopItem();
                    _warehouse.TryToAddItem(item);
                    qcPassed++;
                }
                else
                    Console.WriteLine($"Quality check can't be completed. Warehouse is full.");
            }
            else if (_qualityChecker.CurrentStatus == Status.Failed)
            {
                _qualityChecker.PopItem();
                qcFailed++;
                Console.WriteLine($"Quality check completed : FAIL");
            }
            
            //TRansport logic
            if (_ticks % _transportArrivalInterval == 0)
                _transport.Transfer(_warehouse, _stock);
            
            _ticks++;
        }
        Console.WriteLine("\n\n================ FINAL SIMULATION REPORT ================\n");

        Console.WriteLine("📦 MACHINE INPUT CONFIGURATION:");
        Console.WriteLine($"Machine A planned items: {_machineATotalItems}");
        Console.WriteLine($"Machine B planned items: {_machineBTotalItems}");
        Console.WriteLine($"Machine C planned items: {_machineCTotalItems}");

        Console.WriteLine("\n🏭 MACHINE OUTPUT:");
        Console.WriteLine($"Machine A produced: {_machineATotalItems}");
        Console.WriteLine($"Machine B produced: {_machineBTotalItems}");
        Console.WriteLine($"Machine C produced: {_machineCTotalItems}");
        Console.WriteLine($"TOTAL PRODUCED: {_machineATotalItems + _machineBTotalItems + _machineCTotalItems}");

        Console.WriteLine("\n✅ QUALITY CONTROL:");
        Console.WriteLine($"Total checked: {_machineATotalItems + _machineBTotalItems + _machineCTotalItems}");
        Console.WriteLine($"Passed: {qcPassed}");
        Console.WriteLine($"Failed: {qcFailed}");

        Console.WriteLine("\n🏬 STORAGE:");
        Console.WriteLine($"Warehouse items stored: {_warehouse.GetTotalItemCount()} |" +
                          $"A[{_warehouse.GetAItemCount()}] | B[{_warehouse.GetBItemCount()}] | C[{_warehouse.GetCItemCount()}]");
        Console.WriteLine($"Stock items received: {_stock.GetTotalItemCount()} |" +
                          $"A[{_stock.GetAItemCount()}] | B[{_stock.GetBItemCount()}] | C[{_stock.GetCItemCount()}]");

        Console.WriteLine("\n⚠️ SYSTEM EVENTS:");
        Console.WriteLine($"OrderLine blocked ticks: {orderLineBlockedTicks}");

        Console.WriteLine("\n========================================================\n");
    }

    public void InitSimulator()
    {
        int mode = ReadInt("Enter 0 for default mode or 1 for manual mode", 0);
        if (mode == 1)
        {
            _totalSimulationTicks = ReadInt("Total simulation ticks", _totalSimulationTicks);

            _startItemId = ReadInt("Start item ID", _startItemId);

            _orderLineCapacity = ReadInt("Order line capacity", _orderLineCapacity);

            _storageCapacityPerType = ReadInt("Storage capacity per type", _storageCapacityPerType);

            _stockCapacityPerType = ReadInt("Stock capacity per type", _stockCapacityPerType);

            _minQualityCheckTicks = ReadInt("Minimum quality check ticks", _minQualityCheckTicks);

            _maxQualityCheckTicks = ReadInt("Maximum quality check ticks", _maxQualityCheckTicks);

            _qualityPassPercentage = ReadInt("Quality pass percentage", _qualityPassPercentage);

            _seed = ReadInt("Random seed", _seed);

            _transportArrivalInterval = ReadInt("Transport arrival interval (ticks)", _transportArrivalInterval);

            _transportCapacityPerArrival = ReadInt("Transport capacity per arrival", _transportCapacityPerArrival);

            _machineAProductionInterval = ReadInt("Machine A production interval (ticks)", _machineAProductionInterval);

            _machineBProductionInterval = ReadInt("Machine B production interval (ticks)", _machineBProductionInterval);

            _machineCProductionInterval = ReadInt("Machine C production interval (ticks)", _machineCProductionInterval);

            _machineATotalItems = ReadInt("Machine A total items to produce", _machineATotalItems);

            _machineBTotalItems = ReadInt("Machine B total items to produce", _machineBTotalItems);

            _machineCTotalItems = ReadInt("Machine C total items to produce", _machineCTotalItems);
        }
        
        _machineA = new Machine(new Item("A", 0), _machineAProductionInterval,_machineATotalItems);
        _machineB = new Machine(new Item("B", 0), _machineBProductionInterval,_machineBTotalItems);
        _machineC = new Machine(new Item("C", 0), _machineCProductionInterval,_machineCTotalItems);
        _orderLine = new OrderLine(_orderLineCapacity);
        _qualityChecker = new QualityChecker(_minQualityCheckTicks, _maxQualityCheckTicks, _qualityPassPercentage, _seed);
        _warehouse = new Warehouse(_storageCapacityPerType);
        _stock = new Warehouse(_stockCapacityPerType);
        _transport = new Transport(_transportCapacityPerArrival);
    }
    
    private static int ReadInt(string prompt, int defaultValue)
    {
        Console.Write($"{prompt} [default {defaultValue}]: ");
        int value;

        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
            return defaultValue;

        int.TryParse(input, out value);

        return value < 0 ? defaultValue : value;
    }
}