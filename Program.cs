using System.Text.RegularExpressions;

namespace Calc
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //ввод выражения
            string? expression = Console.ReadLine();

            LinkedList<string> res = new();

            Stack<char> stack = new();
            //разбиение его на токены
            string[] input = Regex.Split(expression, @"(\D)");

            Dictionary<char, int> priority = new Dictionary<char, int>();
            priority['('] = 0;
            priority['^'] = 1;
            priority['*'] = 2;
            priority['/'] = 2;
            priority['+'] = 3;
            priority['-'] = 3;

            //заполнение стека операциями и числами
            foreach (var item in input)
            {
                if (item == "")
                    continue;
                if (int.TryParse(item, out int r))
                {
                    res.AddLast(item);
                }
                else if (char.TryParse(item, out char ch) && priority.ContainsKey(ch))
                {
                    if (stack.Count != 0 && stack.Peek() != '(' && (priority[ch] >= priority[stack.Peek()]))
                    {
                        while (stack.Count != 0 && priority[stack.Peek()] <= priority[ch] && stack.Peek() != '(')
                            res.AddLast(stack.Pop().ToString());
                    }

                    stack.Push(ch);
                }
                else if (ch == '(')
                    stack.Push('(');
                else if (ch == ')')
                {
                    while (stack.Peek() != '(')
                    {
                        res.AddLast(stack.Pop().ToString());
                    }
                    stack.Pop();
                }

            }
            while (stack.Count != 0)
            {
                res.AddLast(stack.Pop().ToString());

            }
            foreach (var item in res)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();
            //стек для подсчета обратной польской записи
            Stack<double> doubleStack = new();

            var operationts = new Dictionary<char, Func<double, double, double>>();
            operationts.Add('+',(x,y) => x+y);
            operationts.Add('*',(x,y) => x*y);
            operationts.Add('-',(x,y) => y-x);
            operationts.Add('/', (x, y) => y / x);
            operationts.Add('^', (x, y) => Math.Pow(y, x));


            foreach (var item in res)
            {
                if (double.TryParse(item, out double value))
                {
                    doubleStack.Push(value);
                }
                else if (operationts.ContainsKey(char.Parse(item)))
                {
                    double op1 = doubleStack.Pop();
                    double op2;
                    if (doubleStack.Count == 0)
                        op2 = 0;
                    else
                        op2 = doubleStack.Pop();
                    doubleStack.Push(operationts[char.Parse(item)](op1, op2));
                }
                else
                {
                    throw new ArgumentException();
                    //double op1 = doubleStack.Pop();
                    //double op2;
                    //if (doubleStack.Count == 0)
                    //    op2 = 0;
                    //else
                    //    op2 = doubleStack.Pop();

                    //switch (item)
                    //{
                    //    case "+":
                    //        doubleStack.Push(op2 + op1);
                    //        break;
                    //    case "*":
                    //        doubleStack.Push(op2 * op1);
                    //        break;
                    //    case "/":
                    //        doubleStack.Push(op2 / op1);
                    //        break;
                    //    case "-":
                    //        doubleStack.Push(op2 - op1);
                    //        break;
                    //    case "^":
                    //        doubleStack.Push(Math.Pow(op2, op1));
                    //        break;
                    //    default:
                    //        throw new InvalidOperationException();
                    //}
                    //Console.WriteLine($"{op2} {item} {op1} = {doubleStack.Peek()}");
                }
            }
            Console.WriteLine(doubleStack.Pop());

        }
    }
}