using Microsoft.Playwright;

namespace PlaywrightTestleri;

public class CheckoutPage
{
    private readonly IPage _page;

    public CheckoutPage(IPage page)
    {
        _page = page;
    }

    public async Task GotoAsync()
    {
        await _page.GotoAsync("https://www.saucedemo.com/checkout-step-one.html");
    }

    public async Task CheckoutAddToCartAsync()
    {
        await _page.ClickAsync("button[id='add-to-cart-sauce-labs-backpack']");
        await _page.ClickAsync(".shopping_cart_link");
        await _page.ClickAsync("button[id='checkout']");
    }

    public async Task CheckOutAsync(string firstName, string lastName, string postalCode)
    {
        await _page.FillAsync("#first-name", firstName);
        await _page.FillAsync("#last-name", lastName);
        await _page.FillAsync("#postal-code", postalCode);
        await _page.ClickAsync("input[id='continue']");
        
    }

    public async Task<string?> HataMesajiAsync()
    {
        return await _page.Locator("[data-test='error']").TextContentAsync();
    }
}