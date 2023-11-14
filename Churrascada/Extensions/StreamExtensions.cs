using System.Text.Json;
using Newtonsoft.Json;

namespace Churrascada.Extensions;

public static class StreamExtensions
{
    public static async Task<T?> ToObjectAsync<T>(this Stream stream)
    {
        var stringValue = await new StreamReader(stream).ReadToEndAsync();

        return JsonConvert.DeserializeObject<T>(stringValue);
    }
}