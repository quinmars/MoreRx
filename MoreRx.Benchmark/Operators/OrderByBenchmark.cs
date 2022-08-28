using System.Reactive.Linq;
using BenchmarkDotNet.Attributes;

namespace MoreRx.Benchmark
{
    public class OrderByBenchmark
    {
        private static readonly int[] s_steps = new int[] { 1, 10, 100, 1_000 };

        private int _store;

        [Benchmark]
        [ArgumentsSource(nameof(RandomNumbers))]
        public void Sort(int[] values)
        {
            values
                .ToObservable()
                .OrderBy(x => x % 30)
                .ThenBy(x => x)
                .Subscribe(v => Volatile.Write(ref _store, v));
        }

        public static IEnumerable<int[]> RandomNumbers()
        {
            var rnd = new Random(0);
            return s_steps
                .Select(n => Enumerable.Range(0, n).Select(_ => rnd.Next()).ToArray())
                .ToArray();
        }
    }
}
