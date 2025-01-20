namespace AoC.Day {
    public class Day13
    {
        public static void Run(string file) {
            Console.WriteLine("Day 13: Mine Cart Madness" + Environment.NewLine);

            Dictionary<(int, int), char> nodes = new Dictionary<(int, int), char>();
            List<Cart> carts = new List<Cart>();

            string[] lines = File.ReadAllLines(file);
            for (int y = 0; y < lines.Length; y++) {
                for (int x = 0; x < lines[y].Length; x++) {
                    char c = lines[y][x];
                    if (c == ' ')
                        continue;

                    if(c == '^' || c == 'v') {
                        carts.Add(new Cart(x, y, c));
                        c = '|';
                    } else if (c == '<' || c == '>') {
                        carts.Add(new Cart(x, y, c));
                        c = '-';
                    }

                    nodes.Add((x, y), c);
                }
            }

            bool draw = lines.Length < 20;

            DrawMap(nodes, carts);

            string partA = string.Empty;
            string partB = string.Empty;
            List<Cart> crashed = new List<Cart>();
            while (true) {
                //Tick
                IOrderedEnumerable<Cart> sortedCarts = carts.OrderBy(c => c.Y).ThenBy(c => c.X);
                foreach(Cart cart in sortedCarts) {
                    switch(cart.Dir) {
                        case Direction.Up: cart.Y--; break;
                        case Direction.Down: cart.Y++; break;
                        case Direction.Left: cart.X--; break;
                        case Direction.Right: cart.X++; break;
                    }

                    char current = nodes[(cart.X, cart.Y)];
                    if(current == '/') {
                        switch (cart.Dir) {
                            case Direction.Up: cart.Dir = Direction.Right; break;
                            case Direction.Down: cart.Dir = Direction.Left; break;
                            case Direction.Left: cart.Dir = Direction.Down; break;
                            case Direction.Right: cart.Dir = Direction.Up; break;
                        }
                    } else if(current == '\\') {
                        switch (cart.Dir) {
                            case Direction.Up: cart.Dir = Direction.Left; break;
                            case Direction.Down: cart.Dir = Direction.Right; break;
                            case Direction.Left: cart.Dir = Direction.Up; break;
                            case Direction.Right: cart.Dir = Direction.Down; break;
                        }
                    } else if(current == '+') {
                        if(cart.Intersection == 0) {
                            //Left
                            switch (cart.Dir) {
                                case Direction.Up: cart.Dir = Direction.Left; break;
                                case Direction.Down: cart.Dir = Direction.Right; break;
                                case Direction.Left: cart.Dir = Direction.Down; break;
                                case Direction.Right: cart.Dir = Direction.Up; break;
                            }
                            cart.Intersection = 1;
                        } else if (cart.Intersection == 1) {
                            //Straight
                            cart.Intersection = 2;
                        } else if (cart.Intersection == 2) {
                            //Right
                            switch (cart.Dir) {
                                case Direction.Up: cart.Dir = Direction.Right; break;
                                case Direction.Down: cart.Dir = Direction.Left; break;
                                case Direction.Left: cart.Dir = Direction.Up; break;
                                case Direction.Right: cart.Dir = Direction.Down; break;
                            }
                            cart.Intersection = 0;
                        }
                    }

                    //Collision check
                    List<Cart> matches = carts.FindAll(c => c.X == cart.X && c.Y == cart.Y && c.Dir != cart.Dir);
                    if (matches.Count > 0) {
                        if (partA == string.Empty) {
                            //First
                            partA = string.Format("{0},{1}", cart.X, cart.Y);
                        }
                        crashed.Add(cart);
                        crashed.AddRange(matches);
                    }
                }

                if (draw)
                    DrawMap(nodes, carts);

                foreach (Cart c in crashed)
                    carts.Remove(c);
                crashed.Clear();

                if (carts.Count < 2) {
                    if (!draw)
                        DrawMap(nodes, carts);

                    if (carts.Count == 1)
                        partB = string.Format("{0},{1}", carts[0].X, carts[0].Y);
                    break;
                }
            }

            Console.WriteLine("Part 1: " + partA);
            //Answer: 48,20
            Console.WriteLine("Part 2: " + partB);
            //Answer: 59,64
        }

        private static void DrawMap(Dictionary<(int, int), char> nodes, List<Cart> carts) {
            int minY = nodes.Min(n => n.Key.Item2);
            int minX = nodes.Min(n => n.Key.Item1);
            int maxY = nodes.Max(n => n.Key.Item2);
            int maxX = nodes.Max(n => n.Key.Item1);

            for (int y = minY; y <= maxY; y++) {
                for (int x = minX; x <= maxX; x++) {
                    if (nodes.ContainsKey((x, y))) {
                        int num = carts.FindAll(c => c.X == x && c.Y == y).Count();
                        if (num == 1)
                            Console.BackgroundColor = ConsoleColor.DarkGreen;
                        else if (num > 1)
                            Console.BackgroundColor = ConsoleColor.Red;

                        Console.Write(nodes[(x, y)]);

                        if (num != 0)
                            Console.ResetColor();
                    } else
                        Console.Write(" ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private enum Direction {
            None, Up, Right, Down, Left, Imaginary
        }

        private class Cart {
            public int X;
            public int Y;
            public Direction Dir;
            public int Intersection;

            public Cart(int x, int y, char c) {
                X = x;
                Y = y;
                switch (c) {
                    case '<': Dir = Direction.Left; break;
                    case '>': Dir = Direction.Right; break;
                    case 'v': Dir = Direction.Down; break;
                    case '^': Dir = Direction.Up; break;
                }
                Intersection = 0;
            }
        }
    }
}
