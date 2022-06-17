using BenchmarkDotNet.Attributes;
using Super.Paula;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Benchmarks
{
    [MemoryDiagnoser]
    public class TypeNameConverterBenchmarks
    {
        //[Params(5, 20, 100)]
        //public int Count { get; set; }

        [Benchmark]
        public void ToKebabCaseOriginal() => _ = TypeNameConverter.ToKebabCase(typeof(TypeNameConverterBenchmarks));

        [Benchmark]
        public void ToKebabCaseNew() => _ = TypeNameConverterNew.ToKebabCase(typeof(TypeNameConverterBenchmarks));
    }
}
