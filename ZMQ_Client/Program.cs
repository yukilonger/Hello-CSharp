// See https://aka.ms/new-console-template for more information
using NetMQ;
using NetMQ.Sockets;
using ZMQ_Client;

Console.WriteLine("Hello, World!");


/* tcp */
if (false)
{
    using (var client = new RequestSocket())
    {
        client.Connect("tcp://localhost:5555");
        while (true)
        {
            var key = Console.ReadKey().Key.ToString();
            client.SendFrame(key);
            var message = client.ReceiveFrameString();
            Console.WriteLine(message);
        }
    }
}


/* inproc */
if (true)
{
    new Mainm();
}

NetMQConfig.Cleanup();