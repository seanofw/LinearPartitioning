using System;
using System.Collections.Generic;
using System.Linq;

namespace LinearPartitioning
{
	public static class LinearPartition_By_Skiena
	{
		/// <summary>
		/// Perform linear partitioning using Steven Skiena's dynamic-programming solution.
		/// Runs in O(k * n^2) time.
		/// </summary>
		public static List<List<RecordInfo>> DoIt(IReadOnlyList<RecordInfo> records, int k)
		{
			int n = records.Count;

			if (k < 0)
				return new List<List<RecordInfo>> { new List<RecordInfo>() };
			if (k >= n)
				return records.Select(r => new List<RecordInfo> { r }).ToList();

			int[,] m = new int[n, k];
			int[,] d = new int[n, k];

			for (int i = 1; i < n; i++)
				m[i, 0] = records[i].Size + m[i - 1, 0];
			for (int j = 0; j < k; j++)
				m[0, j] = records[0].Size;

			for (int i = 1; i < n; i++)
			{
				for (int j = 1; j < k; j++)
				{
					m[i, j] = int.MaxValue;

					for (int x = 0; x < i; x++)
					{
						int cost = Math.Max(m[x, j - 1], m[i, 0] - m[x, 0]);
						if (m[i, j] > cost)
						{
							m[i, j] = cost;
							d[i - 1, j - 1] = x;
						}
					}
				}
			}

			List<List<RecordInfo>> result = new List<List<RecordInfo>>();
			ReconstructPartition(result, records, d, n - 1, k - 2);
			return result;
		}

		private static void ReconstructPartition(List<List<RecordInfo>> result, IReadOnlyList<RecordInfo> records,
			int[,] d, int n, int k)
		{
			// Iterative version:  Avoids stack depth, but then we have to reverse the result.
			while (k >= 0)
			{
				result.Add(new List<RecordInfo>(records.Skip(d[n - 1, k] + 1).Take(n - d[n - 1, k])));
				n = d[n - 1, k];
				k--;
			}
			result.Add(new List<RecordInfo>(records.Take(n + 1)));
			result.Reverse();
		}
	}
}
