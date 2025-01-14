using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TuDa.CIMS.Api.Test;

public static class JsonExtension
{
    /// <summary>
    /// TODO: This is a temporary solution to <see cref="System.Text.Json"/> not respecting any JsonConfiguration.
    /// This does the same a <see cref="System.Net.Http.Json.HttpContentJsonExtensions.ReadFromJsonAsync"/> with enums as strings.
    /// </summary>
    public static async Task<TValue?> FromJsonAsync<TValue>(this HttpContent httpContent)
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new JsonStringEnumConverter());
        options.PropertyNameCaseInsensitive = true;

        return await httpContent.ReadFromJsonAsync<TValue>(options);
    }
}
