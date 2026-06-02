using System.IO.Pipelines;

namespace FloodFill;

class Program
{
    static void DeterminateColor(int value)
    {
        switch (value)
        {
            case 0:
                Console.BackgroundColor = ConsoleColor.Black;
                break;
            case 1:
                Console.BackgroundColor = ConsoleColor.Blue;
                break;
            case 2:
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                break;
            case 3:
                Console.BackgroundColor = ConsoleColor.Cyan;
                break;
            case 4:
                Console.BackgroundColor = ConsoleColor.Red;
                break;
            case 5:
                Console.BackgroundColor = ConsoleColor.Magenta;
                break;
            case 6:
                Console.BackgroundColor = ConsoleColor.Yellow;
                break;
            case 7:
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                break;
            case 8:
                Console.BackgroundColor = ConsoleColor.DarkRed;
                break;
            case 9:
                Console.BackgroundColor = ConsoleColor.DarkGray;
                break;
            default:
                Console.ResetColor();
                break;
        }
    }
    static void PrintMatrix(int[,] matrix, bool colorMode = false)
    {
        if (matrix == null || matrix.GetLength(0) == 0 || matrix.GetLength(1) == 0)
            return;
        
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);
        
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (colorMode)
                {
                    DeterminateColor(matrix[i, j]);
                    Console.Write("   ");
                    continue;
                }
                Console.Write(matrix[i, j]);
            }
            Console.ResetColor();
            Console.WriteLine();
        }
        Console.ResetColor();
    }

    static void FillRecursive(int[,] matrix, int x, int y, int valueToFill, int value)
    {
        if (x >= matrix.GetLength(1) || x < 0  ||
                y >= matrix.GetLength(0) || y < 0  ||
                    matrix[y, x] != valueToFill || valueToFill == value)
            return;

        matrix[y, x] = value;
        FillRecursive(matrix, x + 1, y, valueToFill, value);
        FillRecursive(matrix, x - 1, y, valueToFill, value);
        FillRecursive(matrix, x, y + 1, valueToFill, value);
        FillRecursive(matrix, x, y - 1, valueToFill, value);
    }
    
    static void FloodFillRecursive(int[,] matrix, (int, int) coordinates, int value)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);
        
        bool validX = coordinates.Item1 <= rows ? (coordinates.Item1 >= 0 ? true : false) : false;
        bool validY = coordinates.Item2 <= rows ? (coordinates.Item2 >= 0 ? true : false) : false;
        
        if (cols == 0 || rows == 0 || !validX || !validY)
            return;
        
        int valueToFill = matrix[coordinates.Item2, coordinates.Item1];

        FillRecursive(matrix, coordinates.Item1, coordinates.Item2, valueToFill, value);
    }

    static void FloodFillInterative(int[,] matrix, (int, int) coordinates, int value)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);
        
        bool validX = coordinates.Item1 <= rows ? (coordinates.Item1 >= 0 ? true : false) : false;
        bool validY = coordinates.Item2 <= rows ? (coordinates.Item2 >= 0 ? true : false) : false;
        
        if (cols == 0 || rows == 0 || !validX || !validY)
            return;
        
        Stack<(int, int)> stack = new Stack<(int, int)>();
        int valueToFill = matrix[coordinates.Item2, coordinates.Item1];
         
        stack.Push((coordinates.Item1, coordinates.Item2));
        while (stack.Count > 0)
        {
            (int, int) coords = stack.Pop();
            
            if (coords.Item1 < 0 ||  coords.Item1 >= cols || coords.Item2 < 0 || coords.Item2 >= rows)
                continue;
            
            if (matrix[coords.Item2, coords.Item1] != valueToFill)
                continue;
            
            matrix[coords.Item2, coords.Item1] = value;
            
            stack.Push((coords.Item1 + 1, coords.Item2));
            stack.Push((coords.Item1 - 1, coords.Item2));
            stack.Push((coords.Item1, coords.Item2 + 1));
            stack.Push((coords.Item1, coords.Item2 - 1));
        }
    } 
    
    static void Main()
    {
        int[,] matrix = new int[15, 20]
        {
            { 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2 },
            { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 2, 3, 3, 3, 3, 2 },
            { 1, 0, 5, 5, 5, 0, 1, 0, 0, 0, 0, 0, 0, 0, 2, 3, 0, 0, 3, 2 },
            { 1, 0, 5, 0, 5, 0, 1, 0, 0, 0, 0, 0, 0, 0, 2, 3, 0, 0, 3, 2 },
            { 1, 0, 5, 5, 5, 0, 1, 0, 0, 0, 0, 0, 0, 0, 2, 3, 3, 3, 3, 2 },
            { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2 },
            { 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 4, 4, 4, 4, 0, 0 },
            { 7, 7, 7, 7, 0, 0, 8, 8, 8, 8, 0, 0, 0, 4, 6, 6, 6, 4, 0, 0 },
            { 7, 0, 0, 7, 0, 0, 8, 0, 0, 8, 0, 0, 0, 4, 6, 0, 6, 4, 0, 0 },
            { 7, 0, 0, 7, 0, 0, 8, 0, 0, 8, 0, 0, 0, 4, 6, 6, 6, 4, 0, 0 },
            { 7, 7, 7, 7, 0, 0, 8, 8, 8, 8, 0, 0, 0, 4, 4, 4, 4, 4, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
        };
        (int x, int y) coordinates = (7, 2);
        
        Console.WriteLine("=========================================================");
        Console.WriteLine("                  STAGE 1: BEFORE FILL                   ");
        Console.WriteLine("=========================================================");
        PrintMatrix(matrix, true);
        
        FloodFillRecursive(matrix, coordinates, 9);
        // FloodFillInterative(matrix, coordinates, 9);

        Console.WriteLine("=========================================================");
        Console.WriteLine("                  STAGE 2: AFTER FILL                    ");
        Console.WriteLine("=========================================================");
        PrintMatrix(matrix, true);
        Console.WriteLine("=========================================================");
    }
}
