namespace Minesweeper
{
    internal static class Helper
    {
        internal static int Clamp(int min, int value, int max)
        {
            if (value < min)
            {
                return min;
            }
            if (value > max)
            {
                return max;
            }
            return value;
        }

        internal static string GetPaddedNumber(int value, int maxValue)
        {
            int paddingSpaceCount = GetDigitCount(maxValue) - GetDigitCount(value);
            return new string(' ', paddingSpaceCount) + value.ToString();
        }

        internal static int GetDigitCount(int value)
        {
            return value.ToString().Length;
        }

        /// <summary>
        /// Get the specified significant digit. For e.g. the value 789, 
        /// 7 has significance 2,
        /// 8 has significance 1, and
        /// 9 has significance 0.
        /// </summary>
        /// <returns>The significant digit. If the significance is out of range, a padding space.</returns>
        internal static string GetSignificantDigit(int value, int significance)
        {
            string valueString = value.ToString();
            if (significance >= 0 && significance < valueString.Length)
            {
                return valueString.Reverse().ElementAt(significance).ToString();
            }
            return " ";
        }
    }
}
