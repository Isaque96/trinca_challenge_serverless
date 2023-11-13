using System.Text.Json;

namespace Churrascada.Extensions;

public static class StreamExtensions
{
    public static async Task<T?> ToObjectAsync<T>(this Stream stream)
    {
        var stringValue = await new StreamReader(stream).ReadToEndAsync();

        return JsonSerializer.Deserialize<T>(stringValue);
    }
}