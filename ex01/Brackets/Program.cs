namespace Brackets;

class Program
{
    static bool Check(char c, char top)
    {
        if (top == '(' && c != ')')
            return false;
        if (top == '[' && c != ']')
            return false;
        if (top == '{' && c != '}')
            return false;
        return true;
    }
    
    static bool ValidateBrackets(string input)
    {
        if (string.IsNullOrEmpty(input) || input.Length % 2 != 0)
            return false;
        
        char[] stack = new char[input.Length];
        int top = 0;
        
        foreach (char c in input)
        {
            if (c == '(' || c == '[' || c == '{')
            {
                stack[top] = c;
                top++;
                continue;
            }
            if (top == 0 || !Check(c, stack[--top]))
                return false;
        }
        return (top == 0);
    }
    
    static void Main(string[] args)
    {
        while (true)
        {
            Console.Write("> ");
            Console.WriteLine(ValidateBrackets(Console.ReadLine()));
        }
    }
}