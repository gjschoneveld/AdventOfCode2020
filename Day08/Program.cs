using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day08
{
    abstract class Instruction
    {
        public abstract int Execute(Processor processor);

        public static Instruction Parse(string line)
        {
            var parts = line.Split(' ');

            switch (parts[0])
            {
                case "acc":
                    return new Acc { Argument = int.Parse(parts[1]) };
                case "jmp":
                    return new Jmp { Argument = int.Parse(parts[1]) };
                case "nop":
                    return new Nop();
                default:
                    throw new Exception("Opcode not recognized");
            }
        }
    }

    class Acc : Instruction
    {
        public int Argument { get; set; }

        public override int Execute(Processor processor)
        {
            processor.Accumulator += Argument;

            return 1;
        }
    }

    class Jmp : Instruction
    {
        public int Argument { get; set; }

        public override int Execute(Processor processor)
        {
            return Argument;
        }
    }

    class Nop : Instruction
    {
        public override int Execute(Processor processor)
        {
            return 1;
        }
    }

    class Processor
    {
        public List<Instruction> Program { get; set; }

        public int ProgramCounter { get; set; }
        public int Accumulator { get; set; }

        public void Step()
        {
            ProgramCounter += Program[ProgramCounter].Execute(this);
        }

        public bool IsDone => ProgramCounter == Program.Count;

    }

    class Program
    {
        static bool HasValidProgram(Processor processor)
        {
            var locationsSeen = new HashSet<int>();

            while (!processor.IsDone && !locationsSeen.Contains(processor.ProgramCounter))
            {
                locationsSeen.Add(processor.ProgramCounter);

                processor.Step();
            }

            return processor.IsDone;
        }

        static string ChangeLine(string line)
        {
            if (line.StartsWith("nop"))
            {
                return line.Replace("nop", "jmp");
            }

            return line.Replace("jmp", "nop");
        }

        static Processor FindValidProgram(string[] input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                input[i] = ChangeLine(input[i]);

                var program = input.Select(Instruction.Parse).ToList();

                var processor = new Processor { Program = program };

                if (HasValidProgram(processor))
                {
                    return processor;
                }

                input[i] = ChangeLine(input[i]);
            }

            return null;
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            var program = input.Select(Instruction.Parse).ToList();
            var processor = new Processor { Program = program };

            HasValidProgram(processor);

            var answer1 = processor.Accumulator;
            Console.WriteLine($"Answer 1: {answer1}");

            var valid = FindValidProgram(input);

            var answer2 = valid.Accumulator;
            Console.WriteLine($"Answer 2: {answer2}");
        }
    }
}
