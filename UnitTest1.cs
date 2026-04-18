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



     [Test]
    public async Task Login_DogruUrun_SepeteEkleme_BasariliOlmali()
    {
        await _loginPage.LoginAsync("standard_user", "secret_sauce");
        await Expect(Page).ToHaveURLAsync("https://www.saucedemo.com/inventory.html");
    
        // Sepete ekle
        await Page.ClickAsync("button[id='add-to-cart-sauce-labs-backpack']");
    
        // Sepet ikonuna tıkla
        await Page.ClickAsync(".shopping_cart_link");
    
        // Sepet sayfasında mıyız?
        await Expect(Page).ToHaveURLAsync("https://www.saucedemo.com/cart.html");
    
        // Ürün sepette var mı?
        var urunAdi = await Page.Locator(".inventory_item_name").TextContentAsync();
        Assert.That(urunAdi, Is.EqualTo("Sauce Labs Backpack"));
         
    }

    
     [Test]
    public async Task Login_DogruUrun_SepeteEkleme_CheckOUT_BasariliOlmali()
    {
        await _loginPage.LoginAsync("standard_user", "secret_sauce");
        await Expect(Page).ToHaveURLAsync("https://www.saucedemo.com/inventory.html");
    
        // Sepete ekle
        await Page.ClickAsync("button[id='add-to-cart-sauce-labs-backpack']");
    
        // Sepet ikonuna tıkla
        await Page.ClickAsync(".shopping_cart_link");
    
        // Sepet sayfasında mıyız?
        await Expect(Page).ToHaveURLAsync("https://www.saucedemo.com/cart.html");
    
        // Ürün sepette var mı?
        var urunAdi = await Page.Locator(".inventory_item_name").TextContentAsync();
        Assert.That(urunAdi, Is.EqualTo("Sauce Labs Backpack"));
         
        // Checkout butonuna tıkla
        await Page.ClickAsync("button[id='checkout']");
        await Expect(Page).ToHaveURLAsync("https://www.saucedemo.com/checkout-step-one.html");
        await Page.FillAsync("#first-name", "John");
        await Page.FillAsync("#last-name", "Doe");
        await Page.FillAsync("#postal-code", "12345");
        await Page.ClickAsync("input[id='continue']");
        await Expect(Page).ToHaveURLAsync("https://www.saucedemo.com/checkout-step-two.html");

    }

    [Test]
    public async Task Checkout_page_YeniTest()
    {
    var checkoutPage = new CheckoutPage(Page);
    
    // Önce login ol
    await _loginPage.LoginAsync("standard_user", "secret_sauce");
    
    // Sonra checkout adımları
    await checkoutPage.CheckoutAddToCartAsync();
    await checkoutPage.CheckOutAsync("John", "Doe", "12345");
    
    // URL kontrolü
    await Expect(Page).ToHaveURLAsync("https://www.saucedemo.com/checkout-step-two.html");
    }

    [Test]
    public async Task Price_Low_To_High()
    {

        await _loginPage.LoginAsync("standard_user", "secret_sauce");
        // Sırala dropdown'ını tıkla
        await Page.SelectOptionAsync(".product_sort_container", "lohi");
        // İlk ürünün fiyatını al
        var ilkUrun = await Page.Locator(".inventory_item_name").First.TextContentAsync();
        Assert.That(ilkUrun, Is.EqualTo("Sauce Labs Onesie"));
    }


    [Test]
    public async Task Login_BosKullaniciAdi_HataMesajiGelmeli()
    {
    // Boş username, boş password
    await _loginPage.LoginAsync("", "");
    var hata = await _loginPage.HataMesajiAsync();
    Assert.That(hata, Does.Contain("Username is required"));
    }

    [Test]
    public async Task Login_BosSifre_HataMesajiGelmeli()
    {
    // Sadece şifre boş
    await _loginPage.LoginAsync("standard_user", "");
    var hata = await _loginPage.HataMesajiAsync();
    Assert.That(hata, Does.Contain("Password is required"));
    }


    [Test]
    public async Task Login_CokUzunKullaniciAdi_HataMesajiGelmeli()
    {
    // 500 karakterlik kullanıcı adı
    var uzunInput = new string('a', 500);
    await _loginPage.LoginAsync(uzunInput, "secret_sauce");
    var hata = await _loginPage.HataMesajiAsync();
    Assert.That(hata, Does.Contain("Username and password do not match"));
    }

    
    [Test]
    public async Task Login_OzelKarakter_HataMesajiGelmeli()
    {
    // SQL injection denemesi gibi özel karakterler
    await _loginPage.LoginAsync("' OR '1'='1", "' OR '1'='1");
    var hata = await _loginPage.HataMesajiAsync();
    Assert.That(hata, Does.Contain("Username and password do not match"));
    }


}