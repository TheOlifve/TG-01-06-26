namespace Brackets;

class Program
{
    static bool ValidateBrackets(string input)
    {
        if (string.IsNullOrEmpty(input) || input.Length % 2 != 0)
            return false;
        
        Stack<char> stack = new Stack<char>();
        Dictionary<char, char> dict = new Dictionary<char, char>
        {
            {'{', '}'}, {'[', ']'}, {'(', ')'}
        };
        
        foreach (char c in input)
        {
            if (dict.ContainsKey(c))
            {
                stack.Push(dict[c]);
            }
            else if (dict.ContainsValue(c))
            {
                if (stack.Count != 0 && stack.Pop() != c)
                    return false;
            }
            else
            {
                Console.WriteLine("Invalid input");
                return false;
            }
        }
        return (stack.Count == 0);
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