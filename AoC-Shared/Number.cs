﻿using System.Collections;

namespace AoC.Shared {
    public static class Number {

        public static string IntToString(int value, int toBase) {
            //https://stackoverflow.com/questions/923771/quickest-way-to-convert-a-base-10-number-to-any-base-in-net
            string result = string.Empty;
            do {
                result = "0123456789ABCDEF"[value % toBase] + result;
                value /= toBase;
            }
            while (value > 0);

            return result;
        }

        public static long GetIntFromBitArray(BitArray bitArray) {
            var array = new byte[8];
            bitArray.CopyTo(array, 0);
            return BitConverter.ToInt64(array, 0);
        }
    }
}
