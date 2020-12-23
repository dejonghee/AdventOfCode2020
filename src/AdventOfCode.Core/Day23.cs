using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Core
{
    public class Day23 : Day<string, string>
    {
        protected override IProblem<string> GetPart1() => new Part1();

        protected override IProblem<string> GetPart2() => new Part2();

        private class Part1 : IProblem<string>
        {
            public virtual async Task<string> SolveAsync(IAsyncEnumerable<string> input)
            {
                var cupByLabel = await SolveProblemAsync(input, 100);

                return GenerateResult(cupByLabel);
            }

            protected async Task<Cup[]> SolveProblemAsync(IAsyncEnumerable<string> input, int iterations)
            {
                var (firstCup, minLabel, maxLabel) = await ParseInputAsync(input);

                var cupsByLabel = GetCupsByLabel(firstCup, maxLabel);

                var currentCup = firstCup;
                for (var iteration = 0; iteration < iterations; iteration++)
                {
                    // Crab picks up the three cups that are immediately clockwise of the current cup.
                    var cup1 = currentCup.Next;
                    var cup2 = cup1.Next;
                    var cup3 = cup2.Next;
                    Cup.PickUp(cup1, cup2, cup3);

                    // The crab selects a destination cup.
                    Cup destination;
                    var destinationLabel = currentCup.Label - 1;
                    do
                    {
                        if (destinationLabel < minLabel)
                            destinationLabel = maxLabel;

                        destination = cupsByLabel[destinationLabel];
                        destinationLabel--;
                    }
                    while (destination.WasPickedUp);

                    // The crab places the cups it just picked up so that they are immediately clockwise of the destination cup.
                    // They keep the same order as when they were picked up.
                    Cup.Move(destination, cup1, cup2, cup3);
                    Cup.PutDown(cup1, cup2, cup3);

                    // The crab selects a new current cup:
                    // the cup which is immediately clockwise of the current cup.
                    currentCup = currentCup.Next;
                }

                return cupsByLabel;
            }

            protected async virtual Task<(Cup start, int minLabel, int maxLabel)> ParseInputAsync(IAsyncEnumerable<string> input)
            {
                var enumerator = input.GetAsyncEnumerator();
                await enumerator.MoveNextAsync();

                var numbers = enumerator.Current.Select(c => int.Parse($"{c}")).ToArray();

                var first = new Cup(numbers[0]);

                var previous = first;
                for (var index = 1; index < numbers.Length; index++)
                {
                    var current = new Cup(numbers[index]);

                    // Set 2 way link.
                    previous.Next = current;
                    current.Previous = previous;

                    previous = current;
                }

                // Close the loop (first - last).
                previous.Next = first;
                first.Previous = previous;

                return (first, numbers.Min(), numbers.Max());
            }

            private Cup[] GetCupsByLabel(Cup first, int maxLabel)
            {
                var cupsByLabel = new Cup[maxLabel + 1];
                var current = first;

                do
                {
                    cupsByLabel[current.Label] = current;
                    current = current.Next;
                } while (current.Label != first.Label);

                return cupsByLabel;
            }

            private string GenerateResult(Cup[] cups)
            {
                var sb = new StringBuilder();

                var cup = cups[1].Next;
                do
                {
                    sb.Append(cup.Label);
                    cup = cup.Next;
                } while (cup.Label != 1);

                return sb.ToString();
            }
        }

        private class Part2 : Part1
        {
            public override async Task<string> SolveAsync(IAsyncEnumerable<string> input)
            {
                var cups = await SolveProblemAsync(input, 10000000);

                return $"{(long)cups[1].Next.Label * (long)cups[1].Next.Next.Label}";
            }

            protected override async Task<(Cup start, int minLabel, int maxLabel)> ParseInputAsync(IAsyncEnumerable<string> input)
            {
                const int count = 1000000;
                var (firstCup, minLabel, maxLabel) = await base.ParseInputAsync(input);

                var previous = firstCup.Previous;
                for (var index = maxLabel + 1; index <= count; index++)
                {
                    var current = new Cup(index);

                    // Set 2 way link.
                    previous.Next = current;
                    current.Previous = previous;

                    previous = current;
                }

                // Close the loop (first - last).
                previous.Next = firstCup;
                firstCup.Previous = previous;

                return (firstCup, minLabel, count);
            }
        }

        private class Cup
        {
            public Cup Previous { get; set; }

            public Cup Next { get; set; }

            public bool WasPickedUp { get; private set; }

            public int Label { get; }

            public Cup(int value)
            {
                Label = value;
            }

            public void PickUp()
            {
                WasPickedUp = true;
            }

            public void PutDown()
            {
                WasPickedUp = false;
            }

            public static void PickUp(params Cup[] cups)
            {
                foreach (var cup in cups)
                    cup.PickUp();
            }

            public static void PutDown(params Cup[] cups)
            {
                foreach (var cup in cups)
                    cup.PutDown();
            }

            public static void Move(Cup destination, params Cup[] cups)
            {
                var destinationNext = destination.Next;

                var cupsPrevious = cups[0].Previous;
                var cupsNext = cups[cups.Length - 1].Next;

                cupsPrevious.Next = cupsNext;
                cupsNext.Previous = cupsPrevious;

                destination.Next = cups[0];
                cups[0].Previous = destination;

                cups[cups.Length - 1].Next = destinationNext;
                destinationNext.Previous = cups[cups.Length - 1];
            }
        }
    }
}
