using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace AoC.Day
{
    public class Day12
    {
        //Dumb JSON "parsing" without using a library.

        public static void Run(string file) {
            Console.WriteLine("Day 12: JSAbacusFramework.io" + Environment.NewLine);

            bool verbose = true;

            string input = File.ReadAllText(file);

            int indent = 0;
            StringBuilder sbTxt = new StringBuilder();
            for (int i = 0; i < input.Length; i++) {
                char c = input[i];

                if (c == '[') {
                    indent++;
                } else if (c == ']') {
                    sbTxt.AppendLine();
                    for (int t = 0; t < indent-1; t++) {
                        sbTxt.Append("  ");
                    }
                } else if (c == '{') {
                    indent++;
                } else if (c == '}') {
                    indent--;
                    sbTxt.AppendLine();
                    for (int t = 0; t < indent; t++) {
                        sbTxt.Append("  ");
                    }
                }

                sbTxt.Append(c);

                if (c == ':') {
                    sbTxt.Append(' ');
                } else if (c == '[' || c == '{' || c == ',') {
                    sbTxt.AppendLine();
                    for (int t = 0; t < indent; t++) {
                        sbTxt.Append("  ");
                    }
                } else if (c == ']') {
                    indent--;
                }
            }

            int part1sum = 0;
            char[] ignore = ['[', ']', '{', '}'];
            Stack<bool> stack = new Stack<bool>();
            Stack<int> stackSkip = new Stack<int>();

            string[] lines = sbTxt.ToString().Split("\r\n");
            StringBuilder sbTxtDown = new StringBuilder();
            foreach (string line in lines) {
                bool skip = false;

                if (line.Contains(']')) {
                    if (stackSkip.Contains(stack.Count))
                        stackSkip.Pop();
                    stack.Pop();
                } else if (line.Contains('}')) {
                    if (stackSkip.Contains(stack.Count))
                        stackSkip.Pop();
                    stack.Pop();
                }

                if (stackSkip.Count > 0) {
                    if(stack.Count == stackSkip.Peek())
                        Console.ForegroundColor = ConsoleColor.Cyan;
                    else
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    skip = true;
                } else {
                    skip = false;
                }
                if (ignore.Any(line.Contains)) {
                    //Down here to keep the colour changes
                    skip = false;
                }

                bool colour = line.Contains("red");
                if (colour) {
                    if (!stackSkip.Contains(stack.Count)) {
                        bool peek = stack.Peek();
                        if (peek) {
                            stackSkip.Push(stack.Count);
                            Console.ForegroundColor = ConsoleColor.Red;
                        } else {
                            continue; //Required
                        }
                    }
                }

                if (line.Contains('[')) {
                    stack.Push(false);
                } else if (line.Contains('{')) {
                    stack.Push(true);
                } else if (line.Contains('}') || line.Contains(']')) {
                    skip = false;
                } else {
                    string filtered = line.Substring(line.LastIndexOf(' ') + 1).Replace(",", "");
                    if (!filtered.Contains('"')) {
                        int num = int.Parse(filtered);
                        part1sum += num;
                    }
                }

                if (!skip) {
                    sbTxtDown.Append(line);
                    sbTxtDown.AppendLine();
                }
                if(verbose) {
                    Console.Write(line);
                    Console.WriteLine();
                }

                Console.ResetColor();
            }

            verbose = false;

            //Reverse it and look up
            lines = sbTxtDown.ToString().Trim().Split("\r\n").Reverse().ToArray();
            StringBuilder sbTxtUp = new StringBuilder();
            foreach (string line in lines) {
                if (line.Contains('[')) {
                    if (stackSkip.Contains(stack.Count))
                        stackSkip.Pop();
                    stack.Pop();
                } else if (line.Contains('{')) {
                    if (stackSkip.Contains(stack.Count))
                        stackSkip.Pop();
                    stack.Pop();
                }

                bool skip = (stackSkip.Count > 0);

                bool colour = line.Contains("red");
                if (colour) {
                    //if (stack.Peek()) {
                        stackSkip.Push(stack.Count);
                    //} else {
                        //continue; //This never happens?
                    //}
                }

                if (line.Contains(']')) {
                    stack.Push(false);
                } else if (line.Contains('}')) {
                    stack.Push(true);
                } else if (line.Contains('{') || line.Contains('[')) {
                }

                if (!skip) {
                    sbTxtUp.Append(line);
                    sbTxtUp.AppendLine();
                }

                Console.ResetColor();
            }

            //Re-reverse it and output
            int part2sum = 0;
            lines = sbTxtUp.ToString().Trim().Split("\r\n").Reverse().ToArray();
            foreach (string line in lines) {
                if (line.Contains(']')) {
                    stack.Push(false);
                } else if (line.Contains('}')) {
                    stack.Push(true);
                } else if (line.Contains('{') || line.Contains('[')) {
                } else {
                    string filtered = line.Substring(line.LastIndexOf(' ') + 1).Replace(",", "");
                    if (!filtered.Contains('"')) {
                        int num = int.Parse(filtered);
                        part2sum += num;
                    }
                }

                if(verbose)
                    Console.WriteLine(line);
            }
            //Console.WriteLine(sbTxtUp.ToString());

            Console.WriteLine();
            Console.WriteLine("Part 1: " + part1sum);
            //Answer: 156366
            Console.WriteLine("Part 2: " + part2sum);
            //Answer: 96852
        }

        private static int SbToInt(ref StringBuilder sb) {
            if (sb.Length == 0)
                return 0;

            int number = int.Parse(sb.ToString());
            sb.Clear();
            return number;
        }
    }
}
