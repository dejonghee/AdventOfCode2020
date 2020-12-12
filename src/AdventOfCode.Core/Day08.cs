using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Core
{
    public class Day08
    {
        public class Part1 : IProblem<int>
        {
            public async Task<int> SolveAsync(IAsyncEnumerable<string> input)
            {
                var tmp = new LinkedList<(string operation, int argument)>();
                await foreach (var instruction in input)
                {
                    var parts = instruction.Split(' ');
                    var operation = parts[0];
                    var argument = int.Parse(parts[1]);
                    tmp.AddLast((operation, argument));
                }
                var parsedInput = tmp.ToArray();

                var seenPointers = new HashSet<int>();
                var accumulator = 0;
                var pointer = 0;
                while (pointer >= 0 && pointer < parsedInput.Length)
                {
                    if (!seenPointers.Add(pointer))
                    {
                        break;
                    }

                    var (operation, argument) = parsedInput[pointer];
                    if (string.Equals(operation, "nop"))
                    {
                        pointer++;
                    }
                    else if (string.Equals(operation, "acc"))
                    {
                        accumulator += argument;
                        pointer++;
                    }
                    else if (string.Equals(operation, "jmp"))
                    {
                        pointer += argument;
                    }
                    else
                    {
                        throw new Exception($"Unsupported instruction: {operation} {argument}");
                    }
                }

                return accumulator;
            }
        }

        public class Part2 : IProblem<int>
        {
            public async Task<int> SolveAsync(IAsyncEnumerable<string> input)
            {
                var tmp = new LinkedList<(string operation, int argument)>();
                await foreach (var instruction in input)
                {
                    var parts = instruction.Split(' ');
                    var operation = parts[0];
                    var argument = int.Parse(parts[1]);
                    tmp.AddLast((operation, argument));
                }
                var parsedInput = tmp.ToArray();

                var seenPointers = new HashSet<int>();
                var accumulator = 0;
                var pointer = 0;

                var codeChanged = false;
                var seenPointersCopy = new HashSet<int>();
                var accumulatorCopy = 0;
                var pointerCopy = 0;

                while (pointer >= 0 && pointer < parsedInput.Length)
                {
                    var alreadyExplored = false;

                    if (!seenPointers.Add(pointer))
                    {
                        // Reset to point where we changed the instruction.
                        seenPointers = new HashSet<int>(seenPointersCopy);
                        accumulator = accumulatorCopy;
                        pointer = pointerCopy;
                        codeChanged = false;
                        alreadyExplored = true;
                    }

                    var (operation, argument) = parsedInput[pointer];
                    if (string.Equals(operation, "nop"))
                    {
                        if (alreadyExplored == true || codeChanged == true)
                        {
                            pointer++;
                        }
                        else
                        {
                            if (argument == 0)
                            {
                                // Avoid infinit loop by doing jmp 0.
                                pointer++;
                            }
                            else
                            {
                                seenPointersCopy = new HashSet<int>(seenPointers);
                                accumulatorCopy = accumulator;
                                pointerCopy = pointer;
                                codeChanged = true;

                                // Execute nop as jmp.
                                pointer += argument;
                            }
                        }
                    }
                    else if (string.Equals(operation, "acc"))
                    {
                        accumulator += argument;
                        pointer++;
                    }
                    else if (string.Equals(operation, "jmp"))
                    {
                        if (alreadyExplored == true || codeChanged == true)
                        {
                            pointer += argument;
                        }
                        else
                        {
                            seenPointersCopy = new HashSet<int>(seenPointers);
                            accumulatorCopy = accumulator;
                            pointerCopy = pointer;
                            codeChanged = true;

                            // Execute jmp as nop.
                            pointer++;
                        }
                    }
                    else
                    {
                        throw new Exception($"Unsupported instruction: {operation} {argument}");
                    }
                }

                return accumulator;
            }
        }
    }
}
