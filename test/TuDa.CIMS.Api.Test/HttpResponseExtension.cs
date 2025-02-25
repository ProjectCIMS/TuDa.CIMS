using Microsoft.AspNetCore.Mvc;

namespace TuDa.CIMS.Api.Test;

public static class HttpResponseExtension
{
    public static async Task ShouldBeSuccessAsync(this HttpResponseMessage response)
    {
        response
            .IsSuccessStatusCode.Should()
            .BeTrue(
                response.IsSuccessStatusCode
                    ? string.Empty
                    : await response.Content.ReadAsStringAsync()
            );
    }
}
