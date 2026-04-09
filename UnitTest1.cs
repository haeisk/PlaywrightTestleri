using Microsoft.Playwright.NUnit;
using Microsoft.Playwright;

namespace PlaywrightTestleri;

public class Tests : PageTest
{
    private LoginPage _loginPage;

    [SetUp]
    public async Task Setup()
    {
        _loginPage = new LoginPage(Page);
        await _loginPage.GotoAsync();
    }

    [Test]
    public async Task AnaSayfa_Baslik_DogruOlmali()
    {
        await Expect(Page).ToHaveTitleAsync("Swag Labs");
    }

    [Test]
    public async Task Login_DogruBilgilerle_BasariliOlmali()
    {
        await _loginPage.LoginAsync("standard_user", "secret_sauce");
        await Expect(Page).ToHaveURLAsync("https://www.saucedemo.com/inventory.html");
    }

    [Test]
    public async Task Login_YanlisKullanici_HataMesajiGelmeli()
    {
        await _loginPage.LoginAsync("yanlis_kullanici", "yanlis_sifre");
        var hata = await _loginPage.HataMesajiAsync();
        Assert.That(hata, Does.Contain("Username and password do not match"));
    }


     [Test]
    public async Task Login_DogruUrun_BasariliOlmali()
    {
        await _loginPage.LoginAsync("standard_user", "secret_sauce");
        await Expect(Page).ToHaveURLAsync("https://www.saucedemo.com/inventory.html");
          var urunAdi = await Page.Locator("div.inventory_item_name").First.TextContentAsync();
          Assert.That(urunAdi, Is.EqualTo("Sauce Labs Backpack"));
    }

    [Test]
    public async Task Login_HataAninda_ScreenshotAlmali()
    {
    await _loginPage.LoginAsync("yanlis_kullanici", "yanlis_sifre");
    
    // Screenshot al
    await Page.ScreenshotAsync(new PageScreenshotOptions
    {
        Path = "hata_screenshot.png"
    });

    var hata = await _loginPage.HataMesajiAsync();
    Assert.That(hata, Does.Contain("Username and password do not match"));
    }


}