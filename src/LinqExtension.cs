using System;

/// <summary>
/// Summary description for Class1
/// </summary>
public static class LinqExtension
{
	
    public static IEquatable<T> LogLinq<T>(this IEquatable<T> enumerable, string tabName, Func<T,string> printMethod)
    {
        int count = 0;
        foreach(var item in enumerable)
        {
            Debug.WriteLine($"{tabName}|item {count} = {printMethod(item)}");
            count++;
            yield return item;
        }
        Debug.WriteLine($"{logName} | count = {count}");

        return enumerable;
    }
   

}
