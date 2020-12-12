using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdventOfCode.Core.Utils
{
    public interface IProblem<TSolution>
    {
        Task<TSolution> SolveAsync(IAsyncEnumerable<string> input);
    }
}
