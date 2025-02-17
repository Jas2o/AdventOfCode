using System.Text;

namespace AoC.Day {
    public class Day15 {
        public static void Run(string file) {
            Console.WriteLine("Day 15: Warehouse Woes" + Environment.NewLine);

            List<WHObject> listWarehouse1 = new List<WHObject>();
            List<WHObject> listWarehouse2 = new List<WHObject>();
            WHObject robot1 = null;
            WHObject robot2 = null;
            (int maxX, int maxY) = (0, 0);

            string[] lines = File.ReadAllLines(file);
            StringBuilder sbPath = new StringBuilder();
            bool mode = false;
            for(int y = 0; y < lines.Length; y++) {
                if (lines[y].Length == 0) {
                    (maxX, maxY) = (lines[y - 1].Length, y);
                    mode = true;
                    continue;
                }

                if (mode) {
                    sbPath.Append(lines[y]);
                } else {
                    for(int x = 0; x < lines[y].Length; x++) {
                        char c = lines[y][x];
                        WH type = WH.None;
                        if (c == '.') continue;
                        else if (c == '#') type = WH.Wall;
                        else if (c == 'O') type = WH.Box;
                        else if (c == '@') type = WH.Robot;
                        else throw new Exception();

                        WHObject obj = new WHObject(x, y, type);
                        listWarehouse1.Add(obj);

                        WHObject obj2 = new WHObject(x * 2, y, type);
                        if (c == 'O') obj2.Type = WH.BoxWide;
                        if (c != '@') obj2.X2 = x * 2 + 1;
                        listWarehouse2.Add(obj2);

                        if (type == WH.Robot) {
                            robot1 = obj;
                            robot2 = obj2;
                        }
                    }
                }
            }

            string path = sbPath.ToString();

            Console.WriteLine("Initial state:");
            DrawMapV2(listWarehouse1, maxX, maxY);
            Console.WriteLine();

            for (int i = 0; i < path.Length; i++) {
                SimulateV2(listWarehouse1, robot1, path[i]);
                //Console.WriteLine("Move {0}:", path[i]);
                //DrawMapV2(listWarehouse1, maxX, maxY);
                //Console.WriteLine();
            }

            Console.WriteLine("After:");
            DrawMapV2(listWarehouse1, maxX, maxY);
            Console.WriteLine();

            int partA = 0;
            foreach(WHObject obj in listWarehouse1) {
                if (obj.Type == WH.Box)
                    partA += 100 * obj.Y + obj.X1;
            }

            Console.WriteLine("Wide:");
            DrawMapV2(listWarehouse2, maxX*2, maxY);
            Console.WriteLine();

            for (int i = 0; i < path.Length; i++) {
                SimulateV2(listWarehouse2, robot2, path[i]);
                //Console.WriteLine("Move {0}:", path[i]);
                //DrawMapV2(listWarehouse2, maxX*2, maxY);
                //Console.WriteLine();
            }

            Console.WriteLine("Wide after:");
            DrawMapV2(listWarehouse2, maxX * 2, maxY);
            Console.WriteLine();

            int partB = 0;
            foreach (WHObject obj in listWarehouse2) {
                if (obj.Type == WH.BoxWide)
                    partB += 100 * obj.Y + obj.X1;
            }

            if (false) {
                // This is a simulator you can control with arrow keys!
                Console.Clear();
                Console.WriteLine("\x1b[3J");
                DrawMapV2(listWarehouse2, maxX*2, maxY);

                bool loop = true;
                while (loop) {
                    ConsoleKeyInfo key = Console.ReadKey(intercept: true);

                    if (key.Key == ConsoleKey.Escape)
                        loop = false;
                    else if (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.W)
                        SimulateV2(listWarehouse2, robot2, '^');
                    else if (key.Key == ConsoleKey.LeftArrow || key.Key == ConsoleKey.A)
                        SimulateV2(listWarehouse2, robot2, '<');
                    else if (key.Key == ConsoleKey.RightArrow || key.Key == ConsoleKey.D)
                        SimulateV2(listWarehouse2, robot2, '>');
                    else if (key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.S)
                        SimulateV2(listWarehouse2, robot2, 'v');

                    Console.Clear();
                    Console.WriteLine("\x1b[3J");
                    DrawMapV2(listWarehouse2, maxX*2, maxY);
                }
            }

            Console.WriteLine("Part 1: " + partA);
            //Answer: 1509074
            Console.WriteLine("Part 2: " + partB);
            //Answer: 1521453
        }

        private static void DrawMapV2(List<WHObject> list, int maxX, int maxY) {
            for (int y = 0; y < maxY; y++) {
                for (int x = 0; x < maxX; x++) {
                    WHObject obj1 = list.Find(o => o.Y == y && o.X1 == x);
                    WHObject obj2 = list.Find(o => o.Y == y && o.X2 == x);
                    char c = '?';
                    if (obj1 == null && obj2 == null) {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        c = '.';
                        //c = ' ';
                    } else if(obj1 != null) {
                        if(obj1.Type == WH.Robot)
                            Console.BackgroundColor = ConsoleColor.Red;
                        c = obj1.Symbol();
                    } else {
                        if (obj2.Type == WH.Wall)
                            c = '#';
                        else if (obj2.Type == WH.BoxWide)
                            c = ']';
                    }

                    if(c == 'O' || c == '[' || c == ']')
                        Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(c);
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }

        private static void SimulateV2(List<WHObject> list, WHObject robot, char direction) {
            (int diffX, int diffY) = (0, 0);
            if (direction == '^') diffY--;
            else if (direction == '>') diffX++;
            else if (direction == 'v') diffY++;
            else if (direction == '<') diffX--;

            (int lookX, int lookY) = (robot.X1 + diffX, robot.Y + diffY);
            WHObject objLook = list.Find(o => o.Y == lookY && (o.X1 == lookX || o.X2 == lookX));
            //Check if we can move into a free spot.
            if(objLook == null) {
                robot.X2 = robot.X1 += diffX;
                robot.Y += diffY;
            } else {
                //Check the obstruction and if moveable.
                if (objLook.Type == WH.Wall)
                    return;

                if (objLook.Type == WH.Box) {
                    List<WHMove> listMoves = new List<WHMove>();
                    PushBox(list, listMoves, objLook, diffX, diffY);
                    if (listMoves.Any(m => !m.Valid))
                        return;
                    foreach (WHMove move in listMoves) {
                        move.Obj.X1 += move.DiffX;
                        move.Obj.X2 += move.DiffX;
                        move.Obj.Y += move.DiffY;
                    }
                } else if (objLook.Type == WH.BoxWide) {
                    List<WHMove> listMoves = new List<WHMove>();
                    PushBoxWide(list, listMoves, objLook, diffX, diffY);
                    if (listMoves.Any(m => !m.Valid))
                        return;
                    foreach (WHMove move in listMoves) {
                        move.Obj.X1 += move.DiffX;
                        move.Obj.X2 += move.DiffX;
                        move.Obj.Y += move.DiffY;
                    }
                }
            }

            //Recheck and move if able to
            objLook = list.Find(o => o.Y == lookY && (o.X1 == lookX || o.X2 == lookX));
            if (objLook == null) {
                robot.X2 = robot.X1 += diffX;
                robot.Y += diffY;
            }
        }

        private enum WH {
            None = 0,
            Robot = 1,
            Box = 2,
            BoxWide = 3,
            Wall = 8
        }

        private static void PushBox(List<WHObject> list, List<WHMove> listMoves, WHObject objLook, int diffX, int diffY) {
            WHMove move = new WHMove(objLook, diffX, diffY);
            listMoves.Add(move);

            (int afterX, int afterY) = (objLook.X1 + diffX, objLook.Y + diffY);
            WHObject objAfter = list.Find(o => o.Y == afterY && (o.X1 == afterX));
            if (objAfter == null)
                move.Valid = true;
            else if (objAfter.Type == WH.Wall)
                move.Valid = false;
            else if (objAfter.Type == WH.Box) {
                PushBox(list, listMoves, objAfter, diffX, diffY);
                move.Valid = true;
            } else
                throw new Exception();
        }
        private static void PushBoxWide(List<WHObject> list, List<WHMove> listMoves, WHObject objLook, int diffX, int diffY) {
            (int afterX1, int afterX2, int afterY) = (objLook.X1, objLook.X2, objLook.Y); 

            while (true) {
                //A loop is needed because we might still be looking at the same object unless I ever decide to fix my code.

                (afterX1, afterX2, afterY) = (afterX1 + diffX, afterX2 + diffX, afterY + diffY);
                WHObject objAfter = list.Find(o => o.Y == afterY && (o.X1 == afterX1 || o.X2 == afterX1));
                WHObject objAfter2 = null;
                if (diffX != 0)
                    objAfter2 = list.Find(o => o.Y == afterY && (o.X2 == afterX2 || o.X2 == afterX2));
                else
                    objAfter2 = list.Find(o => o.Y == afterY && (o.X1 == afterX2 || o.X1 == afterX2));

                if (objAfter == objLook || objAfter2 == objLook)
                    continue;

                //Definitely do not want to double move something.
                if (listMoves.Any(m => m.Obj == objLook))
                    return;
                WHMove move = new WHMove(objLook, diffX, diffY);
                listMoves.Add(move);

                if (objAfter == null && objAfter2 == null) {
                    move.Valid = true;
                    return;
                } else {
                    if(diffY == 0) {
                        if (objAfter == null && objAfter2 == null) {
                            move.Valid = true;
                            return;
                        }
                    }

                    if (objAfter != null) {
                        if (objAfter.Type == WH.Wall) {
                            move.Valid = false;
                            return;
                        } else if (objAfter.Type == WH.BoxWide) {
                            PushBoxWide(list, listMoves, objAfter, diffX, diffY);
                        }
                    }
                    if (objAfter2 != null) {
                        if (objAfter2.Type == WH.Wall) {
                            move.Valid = false;
                            return;
                        } else if (objAfter2.Type == WH.BoxWide) {
                            PushBoxWide(list, listMoves,objAfter2, diffX, diffY);
                        }
                    }

                    move.Valid = true;
                    return;
                }
            }
        }

        private class WHObject {
            public int X1;
            public int X2;
            public int Y;
            public WH Type;

            public WHObject(int x, int y, WH type) {
                X1 = x;
                Y = y;
                Type = type;

                if (Type == WH.BoxWide) {
                    X2 = X1 + 1;
                } else {
                    X2 = X1;
                }
            }

            public override string ToString() {
                return string.Format("{0} at x{1} y{2}", Type.ToString(), X1, Y);
            }

            public char Symbol() {
                switch (Type) {
                    case WH.None: return '.';
                    case WH.Robot: return '@';
                    case WH.Box: return 'O';
                    case WH.BoxWide: return '[';
                    case WH.Wall: return '#';
                    default:
                        return '?';
                }
            }
        }

        private class WHMove {
            public WHObject Obj;
            public int DiffX;
            public int DiffY;
            public bool Valid;

            public WHMove(WHObject obj, int diffX, int diffY) {
                Obj = obj;
                DiffX = diffX;
                DiffY = diffY;
                Valid = false;
            }
        }
    }
}
