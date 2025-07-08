//// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
using System;
class Hello
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello World.!");
        Console.WriteLine(args.Length);
        //Console.WriteLine(args[0]);
        Console.ReadKey();

        //int grade = 90;
        //int* ptr = &grade;

        //Console.WriteLine("Original Grade: " + grade);
        //Console.WriteLine("Memory Address: " + (ulong)ptr);

        //*ptr = 95; // Modifying value using pointer
        //Console.WriteLine("Updated Grade: " + grade);
    }
}