using System.Collections.Generic;
using System.Linq;

namespace LinearPartitioning
{
	public static class LinearPartition_By_FloatDDA
	{
		/// <summary>
		/// Perform linear partitioning using a floating-point-based
		/// digital differential analyzer (DDA).  Runs in O(n) time.
		/// </summary>
		public static List<List<RecordInfo>> DoIt(IReadOnlyList<RecordInfo> records, int numPartitions)
		{
			int total = records.Sum(r => r.Size);
			double targetSize = (double)total / numPartitions;

			List<List<RecordInfo>> result = new List<List<RecordInfo>>();

			List<RecordInfo> currentPartition = new List<RecordInfo>();
			double currentSum = 0;
			double currentActualSum = 0;

			foreach (RecordInfo record in records)
			{
				int size = record.Size;
				if (currentSum + size / 2 < targetSize
					|| result.Count >= numPartitions - 1	// Or-clause is only needed for rounding errors
					|| currentPartition.Count == 0)			// Every row must have at least one item
				{
					currentSum += size;
					currentActualSum += size;
					currentPartition.Add(record);
				}
				else
				{
					double error = targetSize - currentSum;	// Amount we *should* have distributed to previous line but couldn't.
					result.Add(currentPartition);
					currentPartition = new List<RecordInfo>();
					currentPartition.Add(record);
					currentSum = size - error;
					currentActualSum = size;
				}
			}

			if (currentPartition.Count != 0)
				result.Add(currentPartition);

			return result;
		}
	}
}