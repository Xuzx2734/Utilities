using System;
using System.Collections.Generic;
using System.Diagnostics;

/// <summary>
/// Summary description for Class1
/// </summary>
public static class LinqExtension
{

    public static IEnumerable<T> DebugLinq<T>(this IEnumerable<T> enumerable, string tabName, Func<T, string> printMethod)
    {
        int count = 0;
        foreach (var item in enumerable)
        {
            if (printMethod != null)
            {
                Debug.WriteLine($"{tabName}|item {count} = {printMethod(item)}");
            }
            count++;
            yield return item;
        }
        //Debug.WriteLine($"{tabName} | count = {count}");

        //yield return enumerable;
    }


}
