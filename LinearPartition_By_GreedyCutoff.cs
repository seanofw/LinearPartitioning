using System.Collections.Generic;
using System.Linq;

namespace LinearPartitioning
{
	public static class LinearPartition_By_GreedyCutoff
	{
		/// <summary>
		/// Perform linear partitioning by using a greedy cutoff.  Runs in O(n) time,
		/// but may generate too many partitions, and the last partition is likely to
		/// be too small, and the mean is likely to be way off.
		/// </summary>
		public static List<List<RecordInfo>> DoIt(IReadOnlyList<RecordInfo> records, int numPartitions)
		{
			int total = records.Sum(r => r.Size);
			double targetSize = (double)total / numPartitions;

			List<List<RecordInfo>> result = new List<List<RecordInfo>>();

			List<RecordInfo> currentPartition = new List<RecordInfo>();
			int currentSum = 0;

			foreach (RecordInfo record in records)
			{
				if (currentSum + record.Size < targetSize
					|| currentPartition.Count == 0)
				{
					currentPartition.Add(record);
					currentSum += record.Size;
				}
				else
				{
					result.Add(currentPartition);
					currentPartition = new List<RecordInfo> { record };
					currentSum = record.Size;
				}
			}

			if (currentPartition.Count != 0)
				result.Add(currentPartition);

			return result;
		}
	}
}
