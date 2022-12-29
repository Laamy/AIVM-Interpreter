using System.Collections.Generic;

namespace AIVM.SDK
{
    public class AIVM // examples
    {
        public static List<Instruction> stringSizeExample = new List<Instruction>()
            {
                // Load the string "hello" into memory at location 0(OperandInt)
                new Instruction() { Opcode = Opcode.LOAD_STRING, OperandInt = 0, OperandString = "hello" },

                // Get the size of the string at memory location 0(OperandInt)
                new Instruction() { Opcode = Opcode.SIZE_STRING, OperandInt = 0 },

                // Print the size to output stream
                new Instruction() { Opcode = Opcode.PRINT_INT },
                
                // Halt the interpreter
                new Instruction() { Opcode = Opcode.HALT }
            };

        public static List<Instruction> inputPromtThenGreetExample = new List<Instruction>()
            {
                // Load the string "Hello " into memory at location 0(OperandInt)
                new Instruction() { Opcode = Opcode.LOAD_STRING, OperandInt = 0, OperandString = "Hello " },

                // Input a string and store it in memory starting at location 6
                new Instruction() { Opcode = Opcode.INPUT_STRING, OperandInt = 6 },

                // Concatenate the strings stored at locations 0 and 6 and store the result in memory at location 0
                new Instruction() { Opcode = Opcode.CONCAT_STRINGS, OperandInt = 0 },

                // Print the concatenated string stored at location 0
                new Instruction() { Opcode = Opcode.PRINT_CONCAT_STRINGS, OperandInt = 0 },

                // Halt the interpreter
                new Instruction() { Opcode = Opcode.HALT }
            };
    }
}
