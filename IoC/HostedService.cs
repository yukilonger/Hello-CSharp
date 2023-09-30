using System;
using Microsoft.Extensions.Hosting;

namespace Console1
{
	public class HostedService: IHostedService
    {
		private readonly DependA a;

		public HostedService(DependA _a)
		{
			a = _a;
		}

        public Task StartAsync(CancellationToken cancellationToken)
        {
            a.Run();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}

