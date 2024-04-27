using System;
using UnityEngine;


namespace KTools
{
    /// <summary>
    /// A set of Function that extends standard cs primitives
    /// </summary>
    public static class Extensions
    {

        /// <summary>
        /// Floor to 1/10 values
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static int FloorTen(this int i)
        {
            return ((int)Mathf.Floor(i / 10f)) * 10;
        }
     
        /// <summary>
        /// Used for Enum and return the next value in the Enumeration
        /// </summary>
        /// <typeparam name="T">The enum type</typeparam>
        /// <param name="src">the source enum</param>
        /// <returns>the next value in the enumaration list</returns>
        public static T Next<T>(this T src) where T : struct
        {
            if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

            T[] Arr = (T[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf<T>(Arr, src) + 1;
            return (Arr.Length == j) ? Arr[0] : Arr[j];
        }
        private static IFormatProvider inv
                    = System.Globalization.CultureInfo.InvariantCulture.NumberFormat;
        
        /// <summary>
        /// return a formated  String using an invariant correction
        /// </summary>
        /// <typeparam name="T">a primitive to convert</typeparam>
        /// <param name="obj">the value</param>
        /// <param name="format"a format ex : "N2" (optional)</param>
        /// <returns></returns>
        public static string ToStringInvariant<T>(this T obj, string format = null)
        {
            return (format == null) ? System.FormattableString.Invariant($"{obj}")
                                    : String.Format(inv, $"{{0:{format}}}", obj);
        }

        /// <summary>
        /// Clamp the value between min and max (inclusive)
        /// </summary>
        /// <typeparam name="T">a primitive to clamp</typeparam>
        /// <param name="value">the value</param>
        /// <param name="min">the minimum value</param>
        /// <param name="max">the maximum value</param>
        /// <returns>clamped value</returns>
        public static T Clamp<T>(T value, T min, T max)
            where T : System.IComparable<T> {
            T result = value;
            if (value.CompareTo(max) > 0)
                result = max;
            if (value.CompareTo(min) < 0)
                result = min;
            return result;
        }
   
    }
}
