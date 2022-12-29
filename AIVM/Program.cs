using AIVM.SDK;

using System;
using System.Collections.Generic;
using System.Linq;

namespace AIVM
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var instructions = SDK.AIVM.inputPromtThenGreetExample;

            var memory = new List<int>(Enumerable.Repeat(0, 5242880)); // 5mb of ram

            var interpreter = new Interpreter(instructions, memory);
            interpreter.Execute();
            Console.ReadKey();
        }
    }
}
