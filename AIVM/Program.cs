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
            //AIVMFilestream.WriteInstructionsToFile(SDK.AIVM.stringSizeExample, "Examples\\StringSizeExample.aivm");
            //AIVMFilestream.WriteInstructionsToFile(SDK.AIVM.inputPromtThenGreetExample, "Examples\\InputPromtThenGreetExample.aivm");

            var instructions = AIVMFilestream.ParseFile("Examples\\StringSizeExample.aivm");

            var memory = new List<int>(Enumerable.Repeat(0, 5242880)); // 5mb of ram

            var interpreter = new Interpreter(instructions, memory);
            interpreter.Execute();
            Console.ReadKey();
        }
    }
}
