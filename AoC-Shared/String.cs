namespace AoC.Shared {
    public static class StringExtensions {
        public static string ReplaceFirst(this string original, string find, string replace) {
            int position = original.IndexOf(find);
            if (position < 0) {
                return original;
            }
            original = original.Substring(0, position) + replace + original.Substring(position + find.Length);
            return original;
        }
    }
}
