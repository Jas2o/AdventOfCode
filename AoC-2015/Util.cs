namespace AoC {
    public class Util {

        //https://stackoverflow.com/questions/923771/quickest-way-to-convert-a-base-10-number-to-any-base-in-net
        public static string Int32ToString(int value, int toBase) {
            string result = string.Empty;
            do {
                result = "0123456789ABCDEF"[value % toBase] + result;
                value /= toBase;
            }
            while (value > 0);

            return result;
        }

    }
}
