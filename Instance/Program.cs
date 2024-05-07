// See https://aka.ms/new-console-template for more information
using Instance;

Console.WriteLine("Hello, World!");

RunDemo runDemo = new RunDemo();
RunDemo runDemo2 = new RunDemo();
RunDemo runDemo3 = new RunDemo();

var result = DemoService.Instance;
Console.Write(result.Count);