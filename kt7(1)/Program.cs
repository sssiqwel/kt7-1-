using System;

public interface IConverter<in T, out U>
{
    U Convert(T value);
}

public class StringToIntConverter : IConverter<string, int>
{
    public int Convert(string value)
    {
        if (int.TryParse(value, out int result))
        {
            return result;
        }
        return 0;
    }
}

public class ObjectToStringConverter : IConverter<object, string>
{
    public string Convert(object value)
    {
        return value?.ToString() ?? "null";
    }
}

public class DetailedObjectToStringConverter : IConverter<object, string>
{
    public string Convert(object value)
    {
        if (value == null)
            return "NULL";

        return $"[{value.GetType().Name}: {value}]";
    }
}


public class ObjectToIntConverter : IConverter<object, int>
{
    public int Convert(object value)
    {
        if (value == null) return 0;

        if (int.TryParse(value.ToString(), out int result))
        {
            return result;
        }
        return 0;
    }
}

class Program
{
    public static U[] ConvertArray<T, U>(T[] array, IConverter<T, U> converter)
    {
        U[] result = new U[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            result[i] = converter.Convert(array[i]);
        }
        return result;
    }

    static void Main(string[] args)
    {

        Console.WriteLine("1. Контрвариантность:");
        string[] stringArray = { "123", "456", "789" };
        var stringToIntConverter = new StringToIntConverter();
        int[] intArray = ConvertArray(stringArray, stringToIntConverter);
        Console.WriteLine($"Результат: [{string.Join(", ", intArray)}]");


        Console.WriteLine("\n2. Ковариантность:");
        object[] objectArray = { 42, "Hello", 3.14, null };


        IConverter<object, object> objectToObjectConverter = new ObjectToStringConverter();
        object[] convertedObjects = ConvertArray(objectArray, objectToObjectConverter);
        Console.WriteLine($"Результат: [{string.Join(", ", convertedObjects)}]");

        Console.WriteLine("\n3. ObjectToIntConverter:");
        object[] mixedObjects = { "100", 200, "300", null };
        var objectToIntConverter = new ObjectToIntConverter();
        int[] numbersFromObjects = ConvertArray(mixedObjects, objectToIntConverter);
        Console.WriteLine($"Результат: [{string.Join(", ", numbersFromObjects)}]");


        Console.WriteLine("\n4. Разные конвертеры:");
        object[] testArray = { 100, "Test", DateTime.Now, 2.71m, null };

        var simpleConverter = new ObjectToStringConverter();
        string[] simpleResult = ConvertArray(testArray, simpleConverter);
        Console.WriteLine($"Простой: [{string.Join(", ", simpleResult)}]");

        var detailedConverter = new DetailedObjectToStringConverter();
        string[] detailedResult = ConvertArray(testArray, detailedConverter);
        Console.WriteLine($"Детальный: [{string.Join(", ", detailedResult)}]");
    }
}