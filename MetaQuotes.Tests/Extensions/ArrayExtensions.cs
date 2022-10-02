using MetaQuotes.Geobase;

namespace MetaQuotes.Tests.Extensions;

public static class ArrayExtensions
{
    public static  void Copy(this Array sourceArray, Array destinationArray, Offset.Slice slice)
    {
        Array.Copy(sourceArray, 0, destinationArray, slice.Start, Math.Min(slice.Length, sourceArray.Length));
    }
    
    public static T[] ExpandIfNeeded<T>(this T[] arr, int checkSize )
    {
        var temp = arr.Length == 0 ? new T[1] : arr; 
        if (arr.Length <= checkSize)
        {
            var size = temp.Length * 2;
            while (size < checkSize)
            {
                size *= 2;
            }
            var expandedDb = new T[size];
            Array.Copy(arr, expandedDb, arr.Length);
            return expandedDb;
        }

        return arr;
    }
}