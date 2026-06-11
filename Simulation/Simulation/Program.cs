namespace Simulation;

class Program
{
    static void Main(string[] args)
    {
        Simulator simulator = new Simulator();
        
        simulator.InitSimulator();
        simulator.Simulate();
    }
}