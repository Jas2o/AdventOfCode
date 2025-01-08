namespace AoC.Shared {
    public static class Permutations {

        //https://chadgolden.com/blog/finding-all-the-permutations-of-an-array-in-c-sharp
        public static List<IList<T>> Get<T>(T[] input) {
            var list = new List<IList<T>>();
            return Recursive(input, 0, input.Length - 1, list);
        }

        private static List<IList<T>> Recursive<T>(T[] input, int start, int end, List<IList<T>> list) {
            if (start == end) {
                // We have one of our possible n! solutions,
                // add it to the list.
                list.Add(new List<T>(input));
            } else {
                for (var i = start; i <= end; i++) {
                    Swap(ref input[start], ref input[i]);
                    Recursive(input, start + 1, end, list);
                    Swap(ref input[start], ref input[i]);
                }
            }

            return list;
        }

        private static void Swap<T>(ref T a, ref T b) {
            var temp = a;
            a = b;
            b = temp;
        }

    }
}
