using System.Collections.Generic;

namespace Utils {
   public static class ArrayUtils {
      public static T Random<T>(this T[] array) => array[UnityEngine.Random.Range(0, array.Length)];
      public static T RandomOrDefault<T>(this T[] array, T defaultValue = default) => array == null || array.Length == 0 ? defaultValue : array.Random();

      public static T Random<T>(this IReadOnlyList<T> array) => array[UnityEngine.Random.Range(0, array.Count)];
      public static T RandomOrDefault<T>(this IReadOnlyList<T> array, T defaultValue = default) => array == null || array.Count == 0 ? defaultValue : array.Random();
   }
}