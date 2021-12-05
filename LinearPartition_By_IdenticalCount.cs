using System.Collections.Generic;
using System.Linq;

namespace LinearPartitioning
{
	public static class LinearPartition_By_IdenticalCount
	{
		/// <summary>
		/// Attempt to perform linear partitioning by simply putting an identical
		/// count of records into each partition (or as close to identical count as
		/// possible, since the number of partitions may not evenly divide into the
		/// number of records).  This is far from ideal, but it makes for a good
		/// baseline measurement.
		/// </summary>
		public static List<List<RecordInfo>> DoIt(IReadOnlyList<RecordInfo> records, int numPartitions)
		{
			double targetSize = (double)records.Count / numPartitions;

			List<List<RecordInfo>> result = new List<List<RecordInfo>>();

			for (int i = 0; i < numPartitions; i++)
			{
				int start = (int)(i * targetSize + 0.5f);
				int end = (int)((i + 1) * targetSize + 0.5f);

				List<RecordInfo> currentList = new List<RecordInfo>();
				currentList.AddRange(records.Skip(start).Take(end - start));
				result.Add(currentList);
			}

			return result;
		}
	}
}
