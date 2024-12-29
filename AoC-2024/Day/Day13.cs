using System;
using System.Globalization;
using System.Reflection.PortableExecutable;
using System.Text;

namespace AoC.Day
{
    public class Day13 {

        private static int costA = 3;
        private static int costB = 1;
        
        public static void Run(string file) {
            Console.WriteLine("Day 13: Claw Contraption" + Environment.NewLine);

            List<ClawMachine> list = new List<ClawMachine>();

            string[] lines = File.ReadAllLines(file);
            int machineNum = 0;
            int winnable1 = 0;
            int winnable2 = 0;
            long winnableCost1 = 0;
            long winnableCost2 = 0;
            for (int i = 0; i < lines.Length; i += 4) {
                machineNum++;
                ClawMachine machine = new ClawMachine(machineNum, lines[i], lines[i + 1], lines[i + 2]);
                list.Add(machine);
            }

            //Part 1
            Console.WriteLine("== Part 1 ==\r\n");
            foreach (ClawMachine machine in list) {
                machine.WeLoveMath();
                if (machine.WonPrize) {
                    winnable1++;
                    winnableCost1 += machine.TokensCost;
                }
            }

            //Part 2
            Console.WriteLine("\r\n-Recalibrating-");
            foreach (ClawMachine machine in list) {
                machine.Reset();
                machine.Prize_X += 10000000000000;
                machine.Prize_Y += 10000000000000;
            }
            Console.WriteLine("\r\n== Part 2 ==\r\n");
            foreach (ClawMachine machine in list) {
                machine.WeLoveMath();
                if (machine.WonPrize) {
                    winnable2++;
                    winnableCost2 += machine.TokensCost;
                }
            }

            Console.WriteLine("\r\n---End---\r\n");

            Console.WriteLine("Part 1: {0} winnable with {1} tokens.", winnable1, winnableCost1);
            //Answer: 27105
            Console.WriteLine("-After calibration-");
            Console.WriteLine("Part 2: {0} winnable with {1} tokens.", winnable2, winnableCost2);
            //Answer: 101726882250942
        }

        private class ClawMachine {
            public int ID;
            public int ButtonA_X;
            public int ButtonA_Y;
            public int ButtonB_X;
            public int ButtonB_Y;

            public long Prize_X;
            public long Prize_Y;

            public bool WonPrize;
            public long TokensCost;

            public ClawMachine(int num, string buttonA, string buttonB, string prize) {
                ID = num;

                int Aplus1 = buttonA.IndexOf("+") + 1;
                int Acomma = buttonA.IndexOf(",");
                int Aplus2 = buttonA.LastIndexOf("+") + 1;
                ButtonA_X = int.Parse(buttonA.Substring(Aplus1, Acomma - Aplus1));
                ButtonA_Y = int.Parse(buttonA.Substring(Aplus2));

                int Bplus1 = buttonB.IndexOf("+") + 1;
                int Bcomma = buttonB.IndexOf(",");
                int Bplus2 = buttonB.LastIndexOf("+") + 1;
                ButtonB_X = int.Parse(buttonB.Substring(Bplus1, Bcomma - Bplus1));
                ButtonB_Y = int.Parse(buttonB.Substring(Bplus2));

                int Pequals1 = prize.IndexOf("=") + 1;
                int Pcomma = prize.IndexOf(",");
                int Pequals2 = prize.LastIndexOf("=") + 1;
                Prize_X = int.Parse(prize.Substring(Pequals1, Pcomma - Pequals1));
                Prize_Y = int.Parse(prize.Substring(Pequals2));
            }

            public void Reset() {
                WonPrize = false;
                TokensCost = 0;
            }

            public void WeLoveMath() {
                long buttonA_buttonB = (ButtonA_X * ButtonB_Y) - (ButtonA_Y * ButtonB_X);
                long prize_buttonB = (Prize_X * ButtonB_Y) - (Prize_Y * ButtonB_X);
                long buttonA_prize = (ButtonA_X * Prize_Y) - (ButtonA_Y * Prize_X);

                long aPresses = prize_buttonB / buttonA_buttonB;
                long bPresses = buttonA_prize / buttonA_buttonB;

                long reachX = ButtonA_X * aPresses + ButtonB_X * bPresses;
                long reachY = ButtonA_Y * aPresses + ButtonB_Y * bPresses;

                if (reachX == Prize_X && reachY == Prize_Y) {
                    WonPrize = true;
                    TokensCost = (aPresses * costA) + (bPresses * costB);
                    Console.WriteLine("#{0}: A button {1} times and B button {2} times, cost {3}.", ID, aPresses, bPresses, TokensCost);
                } else {
                    Console.WriteLine("#{0}: No win :(", ID);
                }
            }
        }
    }
}
