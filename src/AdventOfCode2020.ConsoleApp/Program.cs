using System;
using AdventOfCode.Core;
using AdventOfCode.Core.Utils;

namespace AdventOfCode2020.ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Day 1
            //var inputDay1 = Day1.GetData();

            // Day 1 - Part 1
            //(var x, var y, var result) = new Day1.Part1().Solve(inputDay1);
            //Console.WriteLine($"{x} * {y} = {result}");

            // Day 1 - Part 2
            //(var x, var y, var z, var result) = new Day1.Part2().Solve(inputDay1);
            //Console.WriteLine($"{x} * {y} * {z} = {result}");

            // Day 2
            //var inputDay2 = Day2.GetData();

            // Day 2 - Part 1
            //var validPart1 = new Day2.Part1().Solve(inputDay2);
            //Console.WriteLine($"Valid: {validPart1}");

            // Day 2 - Part 2
            //var validPart2 = new Day2.Part2().Solve(inputDay2);
            //Console.WriteLine($"Valid: {validPart2}");

            // Day 3
            //var inputDay3 = Day3.GetData();

            // Day 3 - Part 1
            //var validPart1 = new Day3.Part1().Solve(inputDay3);
            //Console.WriteLine($"Count: {validPart1}");

            // Day 3 - Part 2
            //var validPart2 = new Day3.Part2().Solve(inputDay3);
            //Console.WriteLine($"Count: {validPart2}");

            // Day 4 - Part 1
            //using(var dataReader = new DataReader("InputDay4.txt"))
            //{
            //    var input = dataReader.GetData();
            //    var solution = new Day4.Part1().Solve(input);
            //    Console.WriteLine($"Valid: {solution}");
            //}

            // Day 5 - Part 1
            //using (var dataReader = new DataReader("InputDay5.txt"))
            //{
            //    var input = dataReader.GetData();
            //    var solution = new Day5.Part1().Solve(input);
            //    Console.WriteLine($"Max: {solution}");
            //}

            // Day 5 - Part 6
            using (var dataReader = new DataReader("InputDay5.txt"))
            {
                var input = dataReader.GetData();
                var solution = new Day5.Part2().Solve(input);
                Console.WriteLine($"Max: {solution}");
            }
        }
    }
}
