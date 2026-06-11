namespace Simulation;

public class Item
{
    public int ID { get; }
    public string Category { get; }
    public Item(string category, int id)
    {
        ID = id;
        Category = category;
    }
}