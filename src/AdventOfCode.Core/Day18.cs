using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Core
{
    public class Day18 : Day<long, long>
    {
        protected override IProblem<long> GetPart1() => new Part1();
        protected override IProblem<long> GetPart2() => new Part2();

        private class Part1 : IProblem<long>
        {
            public virtual async Task<long> SolveAsync(IAsyncEnumerable<string> input)
            {
                var result = 0L;

                var operatorAndParenthesisStack = new Stack<string>();
                var operandStack = new Stack<long>();

                await foreach (var expression in input)
                {
                    // Surround ( and ) with spaces to make parsing more easy.
                    var parts = expression
                        .Replace(")", " ) ")
                        .Replace("(", " ( ")
                        .Split(" ", StringSplitOptions.RemoveEmptyEntries);

                    foreach (var part in parts)
                    {
                        if (IsOperator(part))
                        {
                            operatorAndParenthesisStack.Push(part);
                        }
                        else if (IsParenthesis(part))
                        {
                            if (part == "(")
                            {
                                operatorAndParenthesisStack.Push(part);
                            }
                            else
                            {
                                HandleClosingParenthesis(operatorAndParenthesisStack, operandStack);
                            }
                        }
                        else if (long.TryParse(part, out var number))
                        {
                            HandleNumber(number, operatorAndParenthesisStack, operandStack);
                        }
                        else
                        {
                            throw new Exception("Unknown input: " + part);
                        }
                    }

                    while (operatorAndParenthesisStack.Count > 0)
                    {
                        var item = operatorAndParenthesisStack.Pop();
                        if (IsOperator(item))
                        {
                            var b = operandStack.Pop();
                            var a = operandStack.Pop();
                            var o = item;
                            operandStack.Push(Calculate(a, b, o));
                        }
                    }

                    result += operandStack.Pop();
                }

                return result;
            }

            protected virtual void HandleClosingParenthesis(Stack<string> operatorAndParenthesisStack, Stack<long> operandStack)
            {
                while (operatorAndParenthesisStack.Count > 0 && !IsParenthesis(operatorAndParenthesisStack.Peek()))
                {
                    var b = operandStack.Pop();
                    var a = operandStack.Pop();
                    var o = operatorAndParenthesisStack.Pop();
                    operandStack.Push(Calculate(a, b, o));
                }

                if (operatorAndParenthesisStack.Count > 0)
                    operatorAndParenthesisStack.Pop();

                while (operatorAndParenthesisStack.Count > 0 && !IsParenthesis(operatorAndParenthesisStack.Peek()))
                {
                    var b = operandStack.Pop();
                    var a = operandStack.Pop();
                    var o = operatorAndParenthesisStack.Pop();
                    operandStack.Push(Calculate(a, b, o));
                }
            }

            protected virtual void HandleNumber(long number, Stack<string> operatorAndParenthesisStack, Stack<long> operandStack)
            {
                if (operatorAndParenthesisStack.Count > 0 && IsOperator(operatorAndParenthesisStack.Peek()))
                {
                    var a = operandStack.Pop();
                    var o = operatorAndParenthesisStack.Pop();
                    operandStack.Push(Calculate(a, number, o));
                }
                else
                {
                    operandStack.Push(number);
                }
            }

            protected static bool IsOperator(string str)
            {
                return str switch
                {
                    "+" => true,
                    "*" => true,
                    _ => false
                };
            }

            protected static bool IsParenthesis(string str)
            {
                return str switch
                {
                    "(" => true,
                    ")" => true,
                    _ => false
                };
            }

            protected static long Calculate(long a, long b, string operantor)
            {
                return operantor switch
                {
                    "+" => a + b,
                    "*" => a * b,
                    _ => throw new Exception($"Operator {operantor} is not supported.")
                };
            }
        }

        private class Part2 : Part1
        {
            protected override void HandleClosingParenthesis(Stack<string> operatorAndParenthesisStack, Stack<long> operandStack)
            {
                while (operatorAndParenthesisStack.Count > 0 && !IsParenthesis(operatorAndParenthesisStack.Peek()))
                {
                    var b = operandStack.Pop();
                    var a = operandStack.Pop();
                    var o = operatorAndParenthesisStack.Pop();
                    operandStack.Push(Calculate(a, b, o));
                }

                if (operatorAndParenthesisStack.Count > 0 && IsParenthesis(operatorAndParenthesisStack.Peek()))
                    operatorAndParenthesisStack.Pop();

                while (operatorAndParenthesisStack.Count > 0 && operatorAndParenthesisStack.Peek() == "+")
                {
                    var b = operandStack.Pop();
                    var a = operandStack.Pop();
                    var o = operatorAndParenthesisStack.Pop();
                    operandStack.Push(Calculate(a, b, o));
                }
            }

            protected override void HandleNumber(long number, Stack<string> operatorAndParenthesisStack, Stack<long> operandStack)
            {
                if (operatorAndParenthesisStack.Count > 0 && operatorAndParenthesisStack.Peek() == "+")
                {
                    var a = operandStack.Pop();
                    var o = operatorAndParenthesisStack.Pop();
                    operandStack.Push(Calculate(a, number, o));
                }
                else
                {
                    operandStack.Push(number);
                }
            }
        }
    }
}
