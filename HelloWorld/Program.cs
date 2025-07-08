//// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
using HelloWorld;
using System;
using System.Drawing;
class Hello
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello World.!");
        Console.WriteLine(args.Length);
        //Console.WriteLine(args[0]);
        //Console.ReadLine();

        //int grade = 90;
        //int* ptr = &grade;

        //Console.WriteLine("Original Grade: " + grade);
        //Console.WriteLine("Memory Address: " + (ulong)ptr);

        //*ptr = 95; // Modifying value using pointer
        //Console.WriteLine("Updated Grade: " + grade);


        AccessSpecifiers @as = new AccessSpecifiers();
        @as.length = 4.5;
        @as.width = 3.5;
        @as.Display();
        Console.ReadLine();
    }
}