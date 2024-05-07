using System;
namespace Instance
{
	public class DemoService
	{
		// 单例模式
		public static DemoService Instance { get; } = new DemoService();
		public DemoService() { }

        public int Count = 0;

        public override string ToString()
        {
            Count++;
            return $"Demo Service: {Count}";
        }
    }
}

