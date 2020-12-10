using System;
using System.Threading.Tasks;
using AdventOfCode.Core;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.ConsoleApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            int? day = 10; // null; //

            if (day == null || day == 1)
            {
                await SolveAsync<Day1.Part1, long>(solution => $"Answer = {solution}", s => s == 927684);
                await SolveAsync<Day1.Part2, long>(solution => $"Answer = {solution}", s => s == 292093004);
            }

            if (day == null || day == 2)
            {
                await SolveAsync<Day2.Part1, int>(solution => $"Valid = {solution}", s => s == 536);
                await SolveAsync<Day2.Part2, int>(solution => $"Valid = {solution}", s => s == 558);
            }

            if (day == null || day == 3)
            {
                await SolveAsync<Day3.Part1, long>(solution => $"Count = {solution}", s => s == 220);
                await SolveAsync<Day3.Part2, long>(solution => $"Count = {solution}", s => s == 2138320800);
            }

            if (day == null || day == 4)
            {
                await SolveAsync<Day4.Part1, int>(solution => $"# Valid = {solution}", s => s == 233);
                await SolveAsync<Day4.Part2, int>(solution => $"# Valid = {solution}", s => s == 111);
            }

            if (day == null || day == 5)
            {
                await SolveAsync<Day5.Part1, int>(solution => $"Max SeatId = {solution}", s => s == 980);
                await SolveAsync<Day5.Part2, int>(solution => $"SeatId = {solution}", s => s == 607);
            }

            if (day == null || day == 6)
            {
                await SolveAsync<Day6.Part1, long>(solution => $"Sum = {solution}", s => s == 6335);
                await SolveAsync<Day6.Part2, long>(solution => $"Sum = {solution}", s => s == 3392);
            }

            if (day == null || day == 7)
            {
                //await SolveAsync<Day7.Part1, int>(solution => $"Combinations = {solution}", s => s == 226);
                await SolveAsync<Day7.Part2, long>(solution => $"Combinations = {solution}");
            }

            if (day == null || day == 8)
            {
                await SolveAsync<Day8.Part1, int>(solution => $"Accumulator = {solution}", s => s == 1814);
                await SolveAsync<Day8.Part2, int>(solution => $"Accumulator = {solution}", s => s == 1056);
            }

            if (day == null || day == 9)
            {
                await SolveAsync<Day9.Part1, long>(solution => $"First invalid number = {solution}", s => s == 258585477);
                await SolveAsync<Day9.Part2, long>(solution => $"Encryption weakness = {solution}", s => s == 36981213);
            }

            if (day == null || day == 10)
            {
                //await SolveAsync<Day10.Part1, long>(solution => $"Answer = {solution}", s => s == 1848);
                await SolveAsync<Day10.Part2, long>(solution => $"Answer = {solution}", s => s == 8099130339328);
            }
        }

        private static async Task SolveAsync<TProblem, TSolution>(Func<TSolution, string> printer, Func<TSolution, bool> assert = null)
            where TProblem : IProblem<TSolution>, new()
        {
            var problemType = typeof(TProblem);
            Console.WriteLine($"Trying to solve {problemType.Name} of {problemType.ReflectedType.Name}...");

            var dayId = problemType.ReflectedType.Name.Substring(3);
            var inputFile = $"InputDay{dayId}.txt";

            using (var dataReader = new DataReader(inputFile))
            {
                var input = dataReader.GetDataAsync();
                var problem = new TProblem();
                var solution = await problem.SolveAsync(input);

                Console.WriteLine($"Solution:  {printer(solution)}");

                if (assert != null && !assert(solution))
                    Console.WriteLine($"Solution is invalid!");

                Console.WriteLine("");
            }
        }
    }
}
