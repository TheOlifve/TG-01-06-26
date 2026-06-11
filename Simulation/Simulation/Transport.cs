namespace Simulation;

public class Transport
{
    private int _transportCapacityPerArrival;

    public Transport(int transportCapacityPerArrival)
    {
        _transportCapacityPerArrival = transportCapacityPerArrival;
    }
    
    private bool TryTransferCategory(Warehouse from, Warehouse to, string category)
    {
        Item item;
        
        if (!from.TryPopItem(category, out item))
            return false;

        if (!to.HasEmptySlotForItem(item))
        {
            from.TryToAddItem(item);
            Console.WriteLine(
                $"Transfer skipped: Stock for category '{item.Category}' is full.");
            return false;
        }

        to.TryToAddItem(item);
        return true;
    }

    public void Transfer(Warehouse from, Warehouse to)
    {
        int typeA = 0;
        int typeB = 0;
        int typeC = 0;
        
        for (int loaded = 0; loaded < _transportCapacityPerArrival; loaded++)
        {
            bool transferred = false;

            if (TryTransferCategory(from, to, "A"))
            {
                typeA++;
                transferred = true;
            }
            else if (TryTransferCategory(from, to, "B"))
            {
                typeB++;
                transferred = true;
            }
            else if (TryTransferCategory(from, to, "C"))
            {
                typeC++;
                transferred = true;
            }

            if (!transferred)
                break;
        }
        if (typeA + typeB + typeC != 0)
            Console.WriteLine($"Transferred TypeA[{typeA}] - TypeB[{typeB}] - TypeC[{typeC}] to Stock");
    }
}