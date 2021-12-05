using System.Collections.Generic;
using System.Linq;

namespace LinearPartitioning
{
	public static class LinearPartition_By_CStyle
	{
		/// <summary>
		/// This uses the same technique as LinearPartition_By_IntDDA, but the core
		/// mechanics uses optimized, pure-integer, C-style code to demonstrate that
		/// this solution does not require fancy modern-language features to work well.
		/// </summary>
		public static List<List<RecordInfo>> DoIt(IReadOnlyList<RecordInfo> records, int numPartitions)
		{
			int[] values = records.Select(r => r.Size).ToArray();
			int[] partitions = new int[numPartitions];

			LinearPartition(values, values.Length, partitions, numPartitions);

			List<List<RecordInfo>> result = new List<List<RecordInfo>>();
			for (int i = 0; i < numPartitions; i++)
			{
				int start = partitions[i];
				int end = i < numPartitions - 1 ? partitions[i + 1] : values.Length;
				List<RecordInfo> partition = new List<RecordInfo>(records.Skip(start).Take(end - start));
				result.Add(partition);
			}

			return result;
		}

		/*
		 * Generate linear partitions of the numbers in 'values' of count 'numValues',
		 * and write the start indices of each partition to 'partitions'.
		 *
		 * This uses a DDA approach to slice up the values so that there are
		 * 'numPartitions' partitions of nearly equal size; the result is locally optimal
		 * (but not necessarily globally optimal), and requires only two total passes
		 * over the data, and requires only seven total integer local variables as storage
		 * and only addition, subtraction, multiplication, and if-statements, which means
		 * this compiles to very efficient assembly language and can run well even on
		 * *extremely* resource-constrained hardware.
		 */
		private static void LinearPartition(int[] values, int numValues, int[] partitions, int numPartitions)
		{
			int i;
			int size, targetSize;
			int currentSum;
			int partitionIndex;
			bool isFirst;

			targetSize = 0;
			for (i = 0; i < numValues; i++)
				targetSize += values[i];

			targetSize *= 2;

			currentSum = 0;
			partitionIndex = 0;
			isFirst = true;

			partitions[partitionIndex++] = 0;

			for (i = 0; i < numValues; i++)
			{
				size = values[i] * numPartitions;
				if (currentSum + size < targetSize || isFirst)
				{
					currentSum += size * 2;
					isFirst = false;
				}
				else
				{
					partitions[partitionIndex++] = i;
					currentSum = size * 2 - (targetSize - currentSum);
					isFirst = true;
				}
			}
		}
	}
}
