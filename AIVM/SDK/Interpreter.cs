using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIVM.SDK
{
    public struct Instruction
    {
        public Opcode Opcode;

        public int OperandInt;
        public string OperandString;
    }

    public class State
    {
        public List<int> Memory { get; set; }

        public int PC { get; set; }
    }

    public enum Opcode
    {
        LOAD, STORE,
        ADD, SUB,
        JUMP,
        JZ, JNZ,
        LOAD_STRING, PRINT_STRING,
        CONCAT_STRINGS, PRINT_CONCAT_STRINGS,
        INPUT_STRING,
        PRINT_INT,
        SIZE_STRING,
        HALT
    }

    public class Interpreter
    {
        private State _state;

        private List<Instruction> _instructions;

        private int _accumulator;

        public Interpreter(List<Instruction> instructions, List<int> memory)
        {
            _instructions = instructions;
            _state = new State()
            {
                Memory = memory,
                PC = 0
            };
            _accumulator = 0;
        }

        private void ConcatStrings(Instruction instruction)
        {
            StringBuilder sb1 = new StringBuilder();
            int i = instruction.OperandInt;
            while (i < _state.Memory.Count && i >= 0 && _state.Memory[i] != 0)
            {
                sb1.Append((char)_state.Memory[i]);
                i++;
            }

            int length1 = sb1.Length;

            StringBuilder sb2 = new StringBuilder();
            i = instruction.OperandInt + length1 + 1;
            while (i < _state.Memory.Count && i >= 0 && _state.Memory[i] != 0)
            {
                sb2.Append((char)_state.Memory[i]);
                i++;
            }

            int length2 = sb2.Length;

            StringBuilder sb3 = new StringBuilder();
            sb3.Append(sb1.ToString());
            sb3.Append(sb2.ToString());

            int length3 = sb3.Length;

            if (instruction.OperandInt >= 0 && instruction.OperandInt + length3 <= _state.Memory.Count)
            {
                for (int _i = 0; _i < length3; _i++)
                {
                    _state.Memory[instruction.OperandInt + _i] = (int)sb3[_i];
                }
            }
            else
            {
                Console.WriteLine("ERROR: Invalid memory location for concatenated string: " + instruction.OperandInt);
            }
        }
        private void PrintConcatStrings(Instruction instruction)
        {
            if (instruction.OperandInt >= 0 && instruction.OperandInt < _state.Memory.Count)
            {
                StringBuilder sb = new StringBuilder();
                int i = instruction.OperandInt;
                while (i < _state.Memory.Count && _state.Memory[i] != 0)
                {
                    sb.Append((char)_state.Memory[i]);
                    i++;
                }
                Console.WriteLine(sb.ToString());
            }
            else
            {
                Console.WriteLine("ERROR: Invalid memory location for string: " + instruction.OperandInt);
            }
        }

        private void InputString(Instruction instruction)
        {
            string input = Console.ReadLine();

            int length = input.Length;

            if (instruction.OperandInt >= 0 && instruction.OperandInt + input.Length <= _state.Memory.Count)
            {
                for (int i = 0; i < length; i++)
                {
                    _state.Memory[instruction.OperandInt + i] = (int)input[i];
                }
            }
            else
            {
                Console.WriteLine("ERROR: Invalid memory location for input string: " + instruction.OperandInt);
            }
        }

        public int GetStringSize(string input)
        {
            return Encoding.UTF8.GetByteCount(input);
        }

        private void SizeString(Instruction instruction)
        {
            if (instruction.OperandInt >= 0 && instruction.OperandInt < _state.Memory.Count)
            {
                int size = 0;
                int i = instruction.OperandInt;
                while (i < _state.Memory.Count && _state.Memory[i] != 0)
                {
                    size++;
                    i++;
                }

                _accumulator = size;
            }
            else
            {
                Console.WriteLine("ERROR: Invalid memory location for string: " + instruction.OperandInt);
            }
        }

        public void Execute()
        {
            while (true)
            {
                Instruction instruction = _instructions[_state.PC];

                switch (instruction.Opcode)
                {
                    case Opcode.LOAD:
                        _accumulator = _state.Memory[instruction.OperandInt];
                        break;

                    case Opcode.STORE:
                        _state.Memory[instruction.OperandInt] = _accumulator;
                        break;

                    case Opcode.ADD:
                        _accumulator += _state.Memory[instruction.OperandInt];
                        break;

                    case Opcode.SUB:
                        _accumulator -= _state.Memory[instruction.OperandInt];
                        break;

                    case Opcode.JUMP:
                        _state.PC = instruction.OperandInt;
                        break;

                    case Opcode.JZ:
                        if (_accumulator == 0)
                            _state.PC = instruction.OperandInt;
                        break;

                    case Opcode.JNZ:
                        if (_accumulator != 0)
                            _state.PC = instruction.OperandInt;
                        break;

                    case Opcode.LOAD_STRING:
                        if (instruction.OperandInt >= 0)
                        {
                            int neededCapacity = instruction.OperandInt + instruction.OperandString.Length;
                            if (_state.Memory.Count < neededCapacity)
                            {
                                int numElementsToAdd = neededCapacity - _state.Memory.Count;
                                _state.Memory.AddRange(Enumerable.Repeat(0, numElementsToAdd));
                            }

                            for (int _i = 0; _i < instruction.OperandString.Length; _i++)
                            {
                                _state.Memory[instruction.OperandInt + _i] = (int)instruction.OperandString[_i];
                            }
                        }
                        else
                        {
                            Console.WriteLine("ERROR: Invalid memory location for string: " + instruction.OperandInt);
                        }
                        break;

                    case Opcode.PRINT_STRING:
                        if (instruction.OperandInt >= 0 && instruction.OperandInt < _state.Memory.Count)
                        {
                            StringBuilder sb = new StringBuilder();
                            int i = instruction.OperandInt;
                            while (i < _state.Memory.Count && _state.Memory[i] != 0)
                            {
                                sb.Append((char)_state.Memory[i]);
                                i++;
                            }
                            Console.WriteLine(sb.ToString());
                        }
                        else
                        {
                            Console.WriteLine("ERROR: Invalid memory location for string: " + instruction.OperandInt);
                        }
                        break;

                    case Opcode.PRINT_CONCAT_STRINGS:
                        PrintConcatStrings(instruction);
                        break;

                    case Opcode.PRINT_INT:
                        Console.WriteLine(_accumulator);
                        break;

                    case Opcode.CONCAT_STRINGS:
                        ConcatStrings(instruction);
                        break;

                    case Opcode.INPUT_STRING:
                        InputString(instruction);
                        break;

                    case Opcode.SIZE_STRING:
                        SizeString(instruction);
                        break;

                    case Opcode.HALT:
                        Console.WriteLine("HALT");
                        return;

                    default:
                        Console.WriteLine("Invalid opcode: " + instruction.Opcode);
                        break;
                }

                _state.PC++;
            }
        }
    }
}
