using System;
namespace Console1
{
	public class DependA
	{
		private readonly IDependB b;

        public DependA(IDependB _b)
		{
			b = _b;
		}

		public void Run()
		{
			b.Run();
		}
	}
}

