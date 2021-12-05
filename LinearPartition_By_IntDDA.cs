using System.Collections.Generic;
using System.Linq;

namespace LinearPartitioning
{
	public static class LinearPartition_By_IntDDA
	{
		/// <summary>
		/// Perform linear partitioning using a pure-integer scaled
		/// digital differential analyzer (DDA).  Runs in O(n) time.
		/// </summary>
		public static List<List<RecordInfo>> DoIt(IReadOnlyList<RecordInfo> records, int numPartitions)
		{
			// 'targetSize' and 'size' and 'currentSum' and 'currentActualSum' are typed
			// as 'int' here, but really they just need to be any type large enough to
			// handle the total sum of the record sizes times 2.

			int total = records.Sum(r => r.Size);
			int targetSize = total * 2;

			List<List<RecordInfo>> result = new List<List<RecordInfo>>();

			List<RecordInfo> currentPartition = new List<RecordInfo>();
			int currentSum = 0;
			int currentActualSum = 0;

			foreach (RecordInfo record in records)
			{
				int size = record.Size * numPartitions;
				if (currentSum + size < targetSize
					|| currentPartition.Count == 0)				// Every row must have at least one item
				{
					size *= 2;
					currentSum += size;
					currentActualSum += size;
					currentPartition.Add(record);
				}
				else
				{
					size *= 2;
					int error = targetSize - currentSum;
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