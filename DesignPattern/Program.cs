// See https://aka.ms/new-console-template for more information
using DesignPattern;

Console.WriteLine("Hello, World!");

var singletonInstance = Singleton.Instance();
Console.WriteLine(singletonInstance.ToString());