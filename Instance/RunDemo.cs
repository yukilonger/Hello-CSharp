using System;
namespace Instance
{
	public class RunDemo
	{
        private readonly DemoService demoService = DemoService.Instance;

        public RunDemo()
		{
			demoService.ToString();
		}
	}
}

