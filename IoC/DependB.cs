using System;
namespace Console1
{
	public class DependB:IDependB
	{
		public DependB()
		{
		}

        public void Run()
        {
            Console.WriteLine("DependB");
        }
    }
}

