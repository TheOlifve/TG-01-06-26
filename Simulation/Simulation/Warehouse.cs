namespace Simulation;

public class Warehouse
{
    private int _storageCapacityPerType;
    
    private Queue<Item> _storageA = new Queue<Item>();
    private Queue<Item> _storageB = new Queue<Item>();
    private Queue<Item> _storageC = new Queue<Item>();

    public int GetAItemCount()
    {
        return _storageA.Count;
    }
    
    public int GetBItemCount()
    {
        return _storageB.Count;
    }
    
    public int GetCItemCount()
    {
        return _storageC.Count;
    }
    
    
    public int GetTotalItemCount()
    {
        return _storageA.Count + _storageB.Count + _storageC.Count;
    }
    
    public Status GetStatus()
    {
        if (_storageA.Count == 0
            && _storageB.Count == 0
            && _storageC.Count == 0)
            return Status.Empty;
        return Status.InProgress;
    }

    public Warehouse(int storageCapacityPerType)
    {
        _storageCapacityPerType = storageCapacityPerType;
    }

    public bool TryToAddItem(Item item)
    {
        if (item == null)
            return false;
        switch (item.Category)
        {
            case "A" :
                _storageA.Enqueue(item);
                return true;
            case "B":
                _storageB.Enqueue(item);
                return true;
            case "C":
                _storageC.Enqueue(item);
                return true;
        }
        return false;
    }

    public bool TryPopItem(string category, out Item item)
    {
        if (category == "A" && _storageA.Count != 0)
        {
            item = _storageA.Dequeue();
            return true;
        }

        if (category == "B" && _storageB.Count != 0)
        {
            item = _storageB.Dequeue();
            return true;
        }

        if (category == "C" && _storageC.Count != 0)
        {
            item = _storageC.Dequeue();
            return true;
        }
        item = null;
        return false;
    }

    public int StorageCountPerType(string type)
    {
        switch (type)
        {
            case "A":
                return _storageA.Count;
            case "B":
                return _storageB.Count;
            case "C":
                return _storageC.Count;
        }
        return 0;
    }

    public bool HasEmptySlotForItem(Item item)
    {
        if (item == null)
            return false;
        switch (item.Category)
        {
            case "A":
                return _storageA.Count < 50;
            case "B":
                return _storageB.Count < 50;
            case "C":
                return _storageC.Count < 50;
        }
        return false;
    }
}