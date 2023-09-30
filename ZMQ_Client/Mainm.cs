using System;
using NetMQ;
using NetMQ.Sockets;

namespace ZMQ_Client
{
	public class Mainm
	{
		public Mainm()
		{
			using (var runtime = new NetMQRuntime())
			{
				runtime.Run(ServerAsync(), ClientAsync());
			}
		}

		public async Task ServerAsync()
		{
			using(var server = new RouterSocket("inproc://async"))
			{
				while (true)
				{
					var (routingKey, more) = await server.ReceiveRoutingKeyAsync();
					var (message, _) = await server.ReceiveFrameStringAsync();
					Console.WriteLine("{0} {1} {2}", routingKey, more, message);
					await Task.Delay(1);
					server.SendMoreFrame(routingKey);
					server.SendFrame($"Server Time {DateTime.Now}");
				}
			}
		}

		public async Task ClientAsync()
		{
			using(var client = new DealerSocket("inproc://async"))
			{
				while(true)
				{
					var key = Console.ReadKey().Key.ToString();
					client.SendFrame(key);
					var (message, more) = await client.ReceiveFrameStringAsync();
					Console.WriteLine("{0} {1}", message, more);
					await Task.Delay(1);
				}
			}
		}
	}
}

