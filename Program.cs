using System;
using System.Collections.Generic;
using System.Linq;

namespace LinearPartitioning
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			const int NumPartitions = 10;

			List<List<RecordInfo>> result0 = LinearPartition_By_IdenticalCount.DoIt(SampleData.Records, NumPartitions);
			Console.WriteLine($"Linear partition by identical count:\r\n{Analyze(result0, SampleData.Records, NumPartitions)}");

			List<List<RecordInfo>> result1 = LinearPartition_By_GreedyCutoff.DoIt(SampleData.Records, NumPartitions);
			Console.WriteLine($"Linear partition by greedy cutoff:\r\n{Analyze(result1, SampleData.Records, NumPartitions)}");

			List<List<RecordInfo>> result2 = LinearPartition_By_FloatDDA.DoIt(SampleData.Records, NumPartitions);
			Console.WriteLine($"Linear partition by float DDA:\r\n{Analyze(result2, SampleData.Records, NumPartitions)}");

			List<List<RecordInfo>> result3 = LinearPartition_By_IntDDA.DoIt(SampleData.Records, NumPartitions);
			Console.WriteLine($"Linear partition by integer DDA:\r\n{Analyze(result3, SampleData.Records, NumPartitions)}");

			List<List<RecordInfo>> result4 = LinearPartition_By_IntDDA.DoIt(SampleData.Records, NumPartitions);
			Console.WriteLine($"Linear partition by integer DDA, C-style:\r\n{Analyze(result4, SampleData.Records, NumPartitions)}");

			List<List<RecordInfo>> result5 = LinearPartition_By_Skiena.DoIt(SampleData.Records, NumPartitions);
			Console.WriteLine($"Linear partition by Skiena's dynamic programming algorithm:\r\n{Analyze(result5, SampleData.Records, NumPartitions)}");
		}

		private static string Analyze(List<List<RecordInfo>> result,
			IReadOnlyList<RecordInfo> originalData, int numExpectedPartitions)
		{
			int numActualPartitions = result.Count;

			int actualTotal = result.SelectMany(r => r).Sum(r => r.Size);
			int expectedTotal = originalData.Sum(r => r.Size);

			List<int> partitionSizes = new List<int>();

			double idealMean = (double)expectedTotal / numExpectedPartitions;

			double actualMean = 0.0;
			foreach (List<RecordInfo> partition in result)
			{
				int partitionSize = partition.Sum(r => r.Size);
				partitionSizes.Add(partitionSize);
				actualMean += partitionSize;
			}
			actualMean /= numActualPartitions;

			double maxDiff = 0;
			double actualVariance = 0.0;
			foreach (int partitionSize in partitionSizes)
			{
				double diff = partitionSize - actualMean;
				actualVariance += diff * diff;
				maxDiff = Math.Max(Math.Abs(partitionSize - actualMean), maxDiff);
			}
			actualVariance /= numActualPartitions;

			double actualStdDev = Math.Sqrt(actualVariance);

			int min = partitionSizes.Min(), max = partitionSizes.Max();

			return $"    [ {string.Join(", ", partitionSizes.Select(p => p.ToString()))} ]\r\n"
				+ $"    Partitions:{numActualPartitions} (vs {numExpectedPartitions})  Total:{actualTotal}  Mean:{actualMean:N2} (vs {idealMean:N2})  StdDev:{actualStdDev:N2}  Min:{min}  Max:{max}  Worst:±{maxDiff:N2}\r\n";
		}
	}
}
