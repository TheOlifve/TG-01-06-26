using System.Runtime.InteropServices.ComTypes;

namespace Simulation;

public class Machine
{
    private Item _producedItem;
        
    private Item _example;

    private int _machineAProductionInterval;

    private int _timer = 0;
    
    private int _cnt = 0;
    
    public Status CurrentStatus { get; private set; } = Status.Empty;
    
    public Machine(Item example, int machineAProductionInterval, int cnt = 100)
    {
        _example = example;
        _machineAProductionInterval = machineAProductionInterval;
        _timer = _machineAProductionInterval;
        _cnt = cnt;
    }
    
    public Item TakeProducedItem()
    {
        Item item = _producedItem;
        _producedItem = null;
        CurrentStatus = Status.Empty;
        return item;
    }
    
    public void OnTick(int ID)
    {
        if (_cnt <= 0 || CurrentStatus == Status.Finished)
        {
            CurrentStatus = Status.Finished;
            return;
        }

        _timer--;

        if (_timer > 0)
        {
            CurrentStatus = Status.InProgress;
            return;
        }
        
        if (CurrentStatus == Status.Complete)
            return;
        else
        {
            CurrentStatus = Status.Complete;
            if (_producedItem == null)
            {
                _timer = _machineAProductionInterval;
                _cnt--;
                _producedItem = new Item(_example.Category, ID);
            }
        }
    }
}