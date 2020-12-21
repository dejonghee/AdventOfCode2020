using System.Collections.Generic;
using System.Threading.Tasks;
using AdventOfCode.Core;
using AdventOfCode.Core.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode.UnitTests
{
    [TestClass]
    public class ValidateSolutions
    {
        [TestMethod]
        public async Task Day01()
        {
            await Validate<Day01, long, long>(927684, 292093004);
        }

        [TestMethod]
        public async Task Day02()
        {
            await Validate<Day02, int, int>(536, 558);
        }

        [TestMethod]
        public async Task Day03()
        {
            await Validate<Day03, long, long>(220, 2138320800);
        }

        [TestMethod]
        public async Task Day04()
        {
            await Validate<Day04, int, int>(233, 111);
        }

        [TestMethod]
        public async Task Day05()
        {
            await Validate<Day05, int, int>(980, 607);
        }

        [TestMethod]
        public async Task Day06()
        {
            await Validate<Day06, int, int>(6335, 3392);
        }

        [TestMethod]
        public async Task Day07()
        {
            await Validate<Day07, int, int>(226, 9569);
        }

        [TestMethod]
        public async Task Day08()
        {
            await Validate<Day08, int, int>(1814, 1056);
        }

        [TestMethod]
        public async Task Day09()
        {
            await Validate<Day09, long, long>(258585477, 36981213);
        }

        [TestMethod]
        public async Task Day10()
        {
            await Validate<Day10, int, long>(1848, 8099130339328);
        }

        [TestMethod]
        public async Task Day11()
        {
            await Validate<Day11, int, int>(2344, 2076);
        }

        [TestMethod]
        public async Task Day12()
        {
            await Validate<Day12, int, int>(2057, 71504);
        }

        [TestMethod]
        public async Task Day13()
        {
            await Validate<Day13, decimal, decimal>(2845, 487905974205117);
        }

        [TestMethod]
        public async Task Day14()
        {
            await Validate<Day14, long, long>(11179633149677, 4822600194774);
        }

        [TestMethod]
        public async Task Day15()
        {
            await Validate<Day15, int, int>(517, 1047739);
        }

        [TestMethod]
        public async Task Day16()
        {
            await Validate<Day16, long, long>(29878, 855438643439);
        }

        [TestMethod, Ignore("No solution yet :(")]
        public async Task Day17()
        {
            //await Validate<Day17, int, int>(-1, -1);
            await ValidatePart1<Day17, int, int>(-1);
            //await ValidatePart2<Day17, int, int>(-1);
        }

        [TestMethod]
        public async Task Day18()
        {
            await Validate<Day18, long, long>(14006719520523, 545115449981968);
        }

        [TestMethod]
        public async Task Day19()
        {
            await Validate<Day19, int, int>(102, 318);
        }

        [TestMethod]
        public async Task Day20()
        {
            //await Validate<Day20, long, long>(23497974998093, -1);
            await ValidatePart1<Day20, long, long>(23497974998093);
            //await ValidatePart2<Day20, long, long>(-1);
        }

        [TestMethod]
        public async Task Day21()
        {
            //await Validate<Day21, long, long>(23497974998093, -1);
            await ValidatePart1<Day21, int, int>(-1);
            //await ValidatePart2<Day21, long, long>(-1);
        }

        #region Helpers

        private static async Task Validate<TDay, TSolutionPart1, TSolutionPart2>(TSolutionPart1 solutionPart1, TSolutionPart2 solutionPart2)
        where TDay : class, IDay<TSolutionPart1, TSolutionPart2>, new()
        {
            var day = new TDay();

            await ValidatePart1<TDay, TSolutionPart1, TSolutionPart2>(solutionPart1, day: day);
            await ValidatePart2<TDay, TSolutionPart1, TSolutionPart2>(solutionPart2, day: day);
        }

        private static async Task ValidatePart1<TDay, TSolutionPart1, TSolutionPart2>(TSolutionPart1 solutionPart1, IAsyncEnumerable<string> input = null, TDay day = null)
            where TDay : class, IDay<TSolutionPart1, TSolutionPart2>, new()
        {
            day ??= new TDay();

            if (input == null)
            {
                var inputFile = $"Input/{typeof(TDay).Name}.txt";
                using (var dataReader = new DataReader(inputFile))
                {
                    input = dataReader.GetDataAsync();
                    var solution = await day.SolvePart1Async(input);
                    Assert.AreEqual(solutionPart1, solution);
                }
            }
            else
            {
                var solution = await day.SolvePart1Async(input);
                Assert.AreEqual(solutionPart1, solution);
            }
        }

        private static async Task ValidatePart2<TDay, TSolutionPart1, TSolutionPart2>(TSolutionPart2 solutionPart2, IAsyncEnumerable<string> input = null, TDay day = null)
            where TDay : class, IDay<TSolutionPart1, TSolutionPart2>, new()
        {
            day ??= new TDay();

            if (input == null)
            {
                var inputFile = $"Input/{typeof(TDay).Name}.txt";
                using (var dataReader = new DataReader(inputFile))
                {
                    input = dataReader.GetDataAsync();
                    var solution = await day.SolvePart2Async(input);
                    Assert.AreEqual(solutionPart2, solution);
                }
            }
            else
            {
                var solution = await day.SolvePart2Async(input);
                Assert.AreEqual(solutionPart2, solution);
            }
        }

        /// <summary>
        /// Helper method to create test input for our problems.
        /// </summary>
        private static async IAsyncEnumerable<string> ToAsyncEnumerable(params string[] input)
        {
            foreach (var str in input)
            {
                yield return str;
            }

            // To avoid warning about async method being sync.
            await Task.CompletedTask;
        }

        #endregion
    }
}
