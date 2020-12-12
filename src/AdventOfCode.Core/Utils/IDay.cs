using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdventOfCode.Core.Utils
{
    public interface IDay<TSolutionPart1, TSolutionPart2>
    {
        Task<TSolutionPart1> SolvePart1Async(IAsyncEnumerable<string> input);
        Task<TSolutionPart2> SolvePart2Async(IAsyncEnumerable<string> input);
    }

    public abstract class Day<TSolutionPart1, TSolutionPart2> : IDay<TSolutionPart1, TSolutionPart2>
    {
        public async Task<TSolutionPart1> SolvePart1Async(IAsyncEnumerable<string> input)
        {
            var part1 = GetPart1();
            return await part1.SolveAsync(input);
        }

        public async Task<TSolutionPart2> SolvePart2Async(IAsyncEnumerable<string> input)
        {
            var part2 = GetPart2();
            return await part2.SolveAsync(input);
        }

        protected abstract IProblem<TSolutionPart1> GetPart1();
        protected abstract IProblem<TSolutionPart2> GetPart2();
    }
}
