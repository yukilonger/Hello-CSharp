// See https://aka.ms/new-console-template for more information
using NetMQ;
using NetMQ.Sockets;

Console.WriteLine("Hello, World!");


using (var server = new ResponseSocket())
{
    server.Bind("tcp://*:5555");
    Console.WriteLine("Listening");
    while (true)
    {
        var message = server.ReceiveFrameString();
        Console.WriteLine("Client message {0}", message);
        Thread.Sleep(1);
        server.SendFrame($"Server Time {DateTime.Now} for client message {message}");
    }
}


