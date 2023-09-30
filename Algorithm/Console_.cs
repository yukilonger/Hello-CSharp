using System;
namespace Algorithm
{
	public class Console_<T>
	{
		public Console_()
		{
		}

		public static void WriteLine(IEnumerable<T> ls)
		{
			Console.WriteLine(string.Join(',', ls));
		}
	}
}

