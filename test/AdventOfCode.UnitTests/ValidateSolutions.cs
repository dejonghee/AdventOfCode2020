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

        private static async Task Validate<TDay, TSolutionPart1, TSolutionPart2>(TSolutionPart1 solutionPart1, TSolutionPart2 solutionPart2)
            where TDay : IDay<TSolutionPart1, TSolutionPart2>, new()
        {
            var day = new TDay();
            var inputFile = $"Input/{typeof(TDay).Name}.txt";

            if (solutionPart1 != null)
            {
                using (var dataReader = new DataReader(inputFile))
                {
                    var input = dataReader.GetDataAsync();
                    var solution = await day.SolvePart1Async(input);

                    Assert.AreEqual(solutionPart1, solution);
                }
            }

            if (solutionPart2 != null)
            {
                using (var dataReader = new DataReader(inputFile))
                {
                    var input = dataReader.GetDataAsync();
                    var solution = await day.SolvePart2Async(input);

                    Assert.AreEqual(solutionPart2, solution);
                }
            }
        }
    }
}
