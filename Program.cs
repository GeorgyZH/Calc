﻿using System.Text.RegularExpressions;

namespace Calc
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string? expression = Console.ReadLine();

            LinkedList<string> res = new();

            Stack<char> stack = new();

            string[] input = Regex.Split(expression, @"(\D)");

            Dictionary<char, int> priority = new Dictionary<char, int>();
            priority['('] = 0;
            priority['^'] = 1;
            priority['*'] = 2;
            priority['/'] = 2;
            priority['+'] = 3;
            priority['-'] = 3;

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
            Stack<double> doubleStack = new();

            foreach (var item in res)
            {
                if (double.TryParse(item, out double value))
                {
                    doubleStack.Push(value);
                }
                else
                {
                    double op1 = doubleStack.Pop();
                    double op2;
                    if (doubleStack.Count == 0)
                        op2 = 0;
                    else
                        op2 = doubleStack.Pop();

                    switch (item)
                    {
                        case "+":
                            doubleStack.Push(op2 + op1);
                            break;
                        case "*":
                            doubleStack.Push(op2 * op1);
                            break;
                        case "/":
                            doubleStack.Push(op2 / op1);
                            break;
                        case "-":
                            doubleStack.Push(op2 - op1);
                            break;
                        case "^":
                            doubleStack.Push(Math.Pow(op2, op1));
                            break;
                        default:
                            throw new InvalidOperationException();
                    }
                    Console.WriteLine($"{op2} {item} {op1} = {doubleStack.Peek()}");
                }
            }
            Console.WriteLine(doubleStack.Pop());
        }
    }
}