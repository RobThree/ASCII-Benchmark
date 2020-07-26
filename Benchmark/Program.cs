using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ASCIITest
{
    class Program
    {
        // Inspired by https://lemire.me/blog/2020/07/21/avoid-character-by-character-processing-when-performance-matters/

        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            var iterations = 10;
            var methods = new (string, Action<string>)[] {
                ("Regex", (s) => IsAscii_Regex(s)),
                ("Branchy1", (s) => IsAscii_Branchy1(s)),
                ("Branchy2", (s) => IsAscii_Branchy2(s)),
                ("Branchless", (s) => IsAscii_BranchLess(s)),
                ("Hybrid", (s) => IsAscii_Hybrid(s))
            };

            // Bench all files
            foreach (var f in Directory.GetFiles("Testfiles"))
            {
                Console.WriteLine($"Benching {Path.GetFileName(f)}");

                // Read file into memory
                var strings = ReadFile(f).ToArray();
                PrintFileStats(strings);

                // Benchmark
                Console.WriteLine("Measuring methods... please be patient...");
                var results = methods.Select(m => new { Name = m.Item1, Results = Benchmark(strings, m.Item2, iterations) });

                // Print results
                Console.WriteLine(string.Join("\r\n", results.Select(
                    r => $"{r.Name,-15}\tAvg: {r.Results.Average(r => r.TotalSeconds):N4}s\tMin: {r.Results.Min().TotalSeconds:N4}s\tMax: {r.Results.Max().TotalSeconds:N4}s\t{strings.Length / r.Results.Average(r => r.TotalSeconds),12:N0} strings/sec")
                ));
                Console.WriteLine();
            }
        }

        private static void PrintFileStats(string[] strings)
        {
            Console.WriteLine($"\tLines           : {strings.Length:N0}");
            Console.WriteLine($"\tAvg. length     : {strings.Select(s => s.Length).Average():N2}");
            Console.WriteLine($"\tMax. length     : {strings.Select(s => s.Length).Max():N0}");
            Console.WriteLine($"\tNon-Ascii lines : {strings.Where(s => !IsAscii_Hybrid(s)).Count() / (double)strings.Length:P2}%");
        }

        private static IEnumerable<string> ReadFile(string file)
        {
            using (var fs = File.OpenRead(file))
            using (var gz = new GZipStream(fs, CompressionMode.Decompress))
            using (var sr = new StreamReader(gz))
                while (!sr.EndOfStream)
                    yield return sr.ReadLine();
        }

        private static List<TimeSpan> Benchmark(string[] strings, Action<string> method, int rounds, int warmupRounds = 3)
        {
            //Warmup
            for (int i = 0; i < warmupRounds; i++)
                Parallel.For(0, strings.Length, i => method(strings[i]));

            var results = new List<TimeSpan>();
            // Start benchmark
            for (int i = 0; i < rounds; i++)
            {
                var sw = Stopwatch.StartNew();
                Parallel.For(0, strings.Length, i => method(strings[i]));
                results.Add(sw.Elapsed);
            }
            // Return results
            return results;
        }

        private static readonly Regex nonasciifilter = new Regex("[^\x00-\x7F]", RegexOptions.Compiled);
        private static bool IsAscii_Regex(string value) => !nonasciifilter.IsMatch(value);

        private static bool IsAscii_Branchy1(string value)
        {
            var b = Encoding.Default.GetBytes(value);
            for (var i = 0; i < b.Length; i++)
            {
                if (b[i] >= 128)            // Only difference with IsAscii_Branchy2
                    return false;
            }
            return true;
        }

        private static bool IsAscii_Branchy2(string value)
        {
            var b = Encoding.Default.GetBytes(value);
            for (var i = 0; i < b.Length; i++)
            {
                if ((b[i] & 0x80) != 0)     // Only difference with IsAscii_Branchy1
                    return false;
            }
            return true;
        }

        private static bool IsAscii_BranchLess(string value)
        {
            var b = Encoding.Default.GetBytes(value);
            ulong running = 0;
            int i = 0;
            for (; i + 8 <= b.Length; i += 8)
                running |= BitConverter.ToUInt64(b, i);
            for (; i < b.Length; i++)
                running |= b[i];
            return (running & 0x8080808080808080) == 0;
        }

        private static bool IsAscii_Hybrid(string value)
        {
            var b = Encoding.Default.GetBytes(value);
            int i = 0;
            for (; i + 8 <= b.Length; i += 8)
            {
                var payload = BitConverter.ToUInt64(b, i);
                if ((payload & 0x8080808080808080) != 0) return false;
            }
            for (; i < b.Length; i++)
                if ((b[i] & 0x80) != 0) return false;
            return true;
        }
    }
}
