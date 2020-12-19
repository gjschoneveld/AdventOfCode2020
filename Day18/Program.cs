using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day18
{
    class Program
    {
        enum TokenType
        {
            Number,
            Other
        }

        class Token
        {
            public TokenType Type { get; set; }
            public long Value { get; set; }

            public static Token Parse(char c)
            {
                if (char.IsDigit(c))
                {
                    return new Token
                    {
                        Type = TokenType.Number,
                        Value = c - '0'
                    };
                }

                return new Token
                {
                    Type = TokenType.Other,
                    Value = c
                };
            }
        }

        static long EvaluateExpression(long left, long right, long? oper)
        {
            return oper switch
            {
                '+' => left + right,
                '*' => left * right,
                _ => right
            };
        }

        static long EvaluateFormula1(List<Token> tokens)
        {
            long value = 0;
            long? oper = null;

            var stack = new Stack<(long value, long? oper)>();

            foreach (var token in tokens)
            {
                if (token.Type == TokenType.Number)
                {
                    value = EvaluateExpression(value, token.Value, oper);
                }
                else if (token.Value == '+' || token.Value == '*')
                {
                    oper = token.Value;
                }
                else if (token.Value == '(')
                {
                    // push current calculation
                    stack.Push((value, oper));

                    // start new calculation
                    value = 0;
                    oper = null;
                }
                else if (token.Value == ')')
                {
                    // pop previous calculation
                    var pop = stack.Pop();

                    // use value between brackets as right argument in previous calculation
                    value = EvaluateExpression(pop.value, value, pop.oper);
                }
            }

            return value;
        }

        static void Print(List<Token> tokens)
        {
            foreach (var token in tokens)
            {
                if (token.Type == TokenType.Number)
                {
                    Console.Write(token.Value);
                }
                else
                {
                    Console.Write((char)token.Value);
                }
            }

            Console.WriteLine();
        }

        static void ReplaceRangeByValue(List<Token> tokens, int index, int count, long value)
        {
            tokens.RemoveRange(index + 1, count - 1);

            tokens[index] = new Token
            {
                Type = TokenType.Number,
                Value = value
            };
        }

        static void EvaluateAndReplaceExpression(List<Token> tokens, int operPosition)
        {
            var oper = tokens[operPosition];
 
            var left = tokens[operPosition - 1];
            var right = tokens[operPosition + 1];

            var result = EvaluateExpression(left.Value, right.Value, oper.Value);

            ReplaceRangeByValue(tokens, operPosition - 1, 3, result);
        }

        static long EvaluateFormula2(List<Token> tokens)
        {
            while (tokens.Count > 1)
            {
                var open = tokens.FindLastIndex(t => t.Type == TokenType.Other && t.Value == '(');
                var plus = tokens.FindIndex(t => t.Type == TokenType.Other && t.Value == '+');
                var star = tokens.FindIndex(t => t.Type == TokenType.Other && t.Value == '*');

                if (open != -1)
                {
                    var close = tokens.FindIndex(open, t => t.Type == TokenType.Other && t.Value == ')');

                    var subFormula = tokens.GetRange(open + 1, close - open - 1);
                    var subResult = EvaluateFormula2(subFormula);

                    ReplaceRangeByValue(tokens, open, close - open + 1, subResult);
                }
                else if (plus != -1)
                {
                    EvaluateAndReplaceExpression(tokens, plus);
                }
                else if (star != -1)
                {
                    EvaluateAndReplaceExpression(tokens, star);
                }
            }

            return tokens[0].Value;
        }

        static List<Token> Tokenize(string formula)
        {
            return formula.Where(c => c != ' ').Select(Token.Parse).ToList();
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");
            var tokenized = input.Select(Tokenize).ToList();

            var answer1 = tokenized.Select(EvaluateFormula1).Sum();

            Console.WriteLine($"Answer 1: {answer1}");


            var answer2 = tokenized.Select(EvaluateFormula2).Sum();

            Console.WriteLine($"Answer 2: {answer2}");
        }
    }
}
