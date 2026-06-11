namespace Simulation;

public enum Status
{
    Complete = 0,
    InProgress = 1,
    Empty = 2,
    Finished = 3,
    Passed = 4,
    Failed = 5
}

public class QualityChecker
{
    private int _seed;
    private int _processTicks;
    
    private int _minQualityCheckTicks;
    private int _maxQualityCheckTicks;
    private int _qualityPassPercentage;
    
    private Item _item = null;
    
    public Status CurrentStatus { get; private set; } = Status.Empty;
    
    Random _random;

    public QualityChecker(int minQualityCheckTicks, int maxQualityCheckTicks, int qualityPassPercentage, int seed)
    {
        _minQualityCheckTicks = minQualityCheckTicks;
        _maxQualityCheckTicks = maxQualityCheckTicks;
        _qualityPassPercentage = qualityPassPercentage;
        _seed = seed;
        _random = new Random(_seed);
    }


    public Item PeekItem()
    {
        return _item;
    }
    
    public void AddItem(Item item)
    {
        _processTicks = _random.Next((int)_minQualityCheckTicks, (int)_maxQualityCheckTicks + 1);
        _item = item;
        CurrentStatus = Status.InProgress;
    }

    public Item PopItem()
    {
        Item item = _item;
        
        _item = null;
        CurrentStatus = Status.Empty;
        return item;
    }

    public void OnTick()
    {
        _processTicks--;
        if (_processTicks <= 0 && CurrentStatus == Status.InProgress)
        {
            CurrentStatus = _random.Next(0, 100) < _qualityPassPercentage
                ? Status.Passed
                : Status.Failed;
        }
    }
}