using Microsoft.Playwright;

namespace PlaywrightTestleri;

public class LoginPage
{
    private readonly IPage _page;

    public LoginPage(IPage page)
    {
        _page = page;
    }

    public async Task GotoAsync()
    {
        await _page.GotoAsync("https://www.saucedemo.com");
    }

    public async Task LoginAsync(string username, string password)
    {
        await _page.FillAsync("#user-name", username);
        await _page.FillAsync("#password", password);
        await _page.ClickAsync("#login-button");
    }

    public async Task<string?> HataMesajiAsync()
    {
        return await _page.Locator("[data-test='error']").TextContentAsync();
    }
}