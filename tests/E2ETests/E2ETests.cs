using Microsoft.Playwright;
using Xunit;

namespace E2ETests;

public class E2ETests : IAsyncLifetime
{
    private IPlaywright _playwright;
    private IBrowser _browser;
    private IPage _page;

    public async Task InitializeAsync()
    {
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions 
        { 
            Headless = false
        });
        _page = await _browser.NewPageAsync();
    }

    [Fact]
    public async Task SiteLoadsSuccessfully()
    {
        await _page.GotoAsync("http://localhost:5071");
        await _page.WaitForSelectorAsync(".card");
        var hasButton = await _page.Locator("button:has-text('Забронировать')").CountAsync();
        Assert.True(hasButton > 0, "Кнопка 'Забронировать' не найдена");
    }

    public async Task DisposeAsync()
    {
        await _browser.CloseAsync();
        _playwright?.Dispose();
    }
}
