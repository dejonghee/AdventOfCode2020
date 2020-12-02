using AdventOfCode.Core;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;

namespace AdventOfCode2020.Benchmarks
{
    [MemoryDiagnoser]
    [NativeMemoryProfiler]
    public class Day1Benchmark
    {
        private readonly Day1.Part2 _part2 = new Day1.Part2();

        [Benchmark]
        public void BenchmarkNaive() => _part2.Solution();

        [Benchmark()]
        public void BenchmarkHashset() => _part2.Solution();
    }
}
