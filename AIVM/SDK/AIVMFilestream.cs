using System.Collections.Generic;
using System.IO;

namespace AIVM.SDK
{
    public class AIVMFilestream
    {
        public static char splitline = (char)0xFF;
        public static char endline = (char)0xFE;

        public static void WriteInstructionsToFile(List<Instruction> instructions, string path)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                foreach (Instruction instruction in instructions)
                {
                    writer.Write((int)instruction.Opcode);
                    writer.Write(splitline);
                    writer.Write(instruction.OperandInt);
                    writer.Write(splitline);
                    writer.Write(instruction.OperandString);
                    writer.Write(endline);
                }
            }
        }

        public static List<Instruction> ParseFile(string path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                List<Instruction> instructions = new List<Instruction>();

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(endline);

                    foreach (string part in parts)
                    {
                        if (part.Trim() == "") continue;

                        string[] values = part.Split(splitline);

                        Opcode opcode = (Opcode)int.Parse(values[0]);
                        int operandInt = int.Parse(values[1]);
                        string operandString = values[2];

                        Instruction instruction = new Instruction()
                        {
                            Opcode = opcode,
                            OperandInt = operandInt,
                            OperandString = operandString
                        };

                        instructions.Add(instruction);
                    }
                }

                return instructions;
            }
        }
    }
}
