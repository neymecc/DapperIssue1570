using BenchmarkDotNet.Running;

namespace DapperIssue1570
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Bench>();
        }
    }
}