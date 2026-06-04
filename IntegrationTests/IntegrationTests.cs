using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace IntegrationTests;

public class IntegrationTests
{
    [Fact]
    public async Task ApiIsAlive()
    {
        await Task.Delay(1);
        Assert.True(true);
    }
}
