using System;

namespace LinearPartitioning
{
	public class RecordInfo
	{
		public string Name { get; init; } = string.Empty;
		public int Size { get; init; }

		public string Duration
		{
			get
			{
				// Turn TotalMinutes into a string of the form "MM:SS" for readability.
				return $"{Size / 60}:{Size % 60:D2}";
			}

			init
			{
				// Turn a string of the form "MM:SS" into Size.
				string[] pieces = value.Split(':');
				if (pieces.Length != 2)
					throw new ArgumentException("Duration should be in the form MM:SS");
				Size = int.Parse(pieces[0]) * 60 + int.Parse(pieces[1]);
			}
		}

		public override string ToString()
			=> $"{Duration} {Name}";
	}
}
