using System;
using System.Collections.Generic;
using System.Text;

namespace metaheuristic
{
    class Common
    {

		public static void Merge(StringBuilder code1, StringBuilder code2)
		{
			int position = Overlap(code1, code2);

			if (position == -1)
				code1.Append(code2);
			if (position >= 0)
			{
				if (position + code2.Length > code1.Length)
				{
					code1.Length = position;
					code1.Append(code2);
				}
			}
		}

		public static void Merge(StringBuilder code1, String code2)
		{
			int position = Overlap(code1, new StringBuilder(code2));

			if (position == -1)
				code1.Append(code2);
			if (position >= 0)
			{
				if (position + code2.Length > code1.Length)
				{
					code1.Length = position;
					code1.Append(code2);
				}
			}
		}

		public static int Overlap(StringBuilder s1, StringBuilder s2)
		{
			Boolean success = true;
			int min = Math.Min(s1.Length, s2.Length);
			int start = Math.Max(s1.Length - s2.Length, 0);
			for (int i = start; i < s1.Length; i++)
			{
				success = true;
				for (int j = 0; j < s1.Length - i; j++)
				{
					if (s1[j + i] != s2[j])
					{
						success = false;
						break;
					}
				}
				if (success)
					return i;
			}
			return -1;
		}
	}
}
