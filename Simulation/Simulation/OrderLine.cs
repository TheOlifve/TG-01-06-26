namespace Simulation;

public class OrderLine
{
    private Queue<Item> _items = new Queue<Item>();
    public int LineCapacity { get; set; }

    public Status GetStatus()
    {
        if (_items.Count == 0)
            return Status.Empty;
        return Status.InProgress;
    }

    public bool HasAvailableSlots()
    {
        if (_items.Count >= LineCapacity) return false;
        return true;
    }

    public bool HasItem()
    {
        return _items.Count > 0;
    }

    public void AddItem(Item item)
    {
        if (item == null)
            return;
        _items.Enqueue(item);
    }

    public Item GetItem()
    {
        return _items.Dequeue();
    }

    public OrderLine(int lineCapacity)
    {
        LineCapacity = lineCapacity;
    }
}