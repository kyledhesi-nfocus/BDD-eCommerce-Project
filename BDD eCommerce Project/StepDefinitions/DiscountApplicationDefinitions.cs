using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using BDD_eCommerce_Project.Support.PageObjects;

namespace BDD_eCommerce_Project.StepDefinitions {
    [Binding]
    public class DiscountApplicationDefinitions { 
        
        private readonly ScenarioContext _scenarioContext;
        private IWebDriver driver;

        private string baseUrl;
        private string username;
        private string password;

        private readonly LoginMyAccount loginMyAccount;
        private readonly Navigation navigation;
        private readonly Shop shop;
        private readonly Cart cart;


        public DiscountApplicationDefinitions(ScenarioContext scenarioContext) {
            _scenarioContext = scenarioContext;
            
            this.driver = (IWebDriver)_scenarioContext["myDriver"];
            this.username = (string)_scenarioContext["username"];
            this.password = (string)_scenarioContext["password"];
            this.baseUrl = (string)_scenarioContext["baseURL"];

            loginMyAccount = new(driver);
            navigation = new(driver);
            shop = new(driver);
            cart = new(driver);
        }

        [Given(@"I am on the login page")]
        public void GivenIAmOnTheLoginPage() {
           driver.Url = baseUrl;
        }

        [When(@"I enter my email and password")]
        public void WhenIEnterMyEmailAndPassword() {
            loginMyAccount.EnterUsernameAndPassword(username, password);
        }

        [When(@"I click the Login button or press ENTER")]
        public void WhenIClickTheLoginButtonOrPressENTER() {
            loginMyAccount.DismissBanner();
            loginMyAccount.ClickLoginButton();
        }

        [Then(@"I should have successfully logged in")]
        public void GivenIShouldHaveSuccessfullyLoggedIn() {
            Assert.That(driver.FindElement(By.LinkText("Log out")).Displayed);
            Console.WriteLine("Successfully logged in");
        }

        [Given(@"I am on the shop page")]
        public void GivenIAmOnTheShopPage() {
            navigation.ClickLink(Navigation.Link.Shop);
        }

        [When(@"I add an item to the cart")]
        public void WhenIAddAnItemToTheCart() {
            shop.AddToCart(Shop.Product.Belt);
        }

        [When(@"I view the cart")]
        public void WhenIViewTheCart() {
            navigation.ClickLink(Navigation.Link.Cart);
        }

        [When(@"I apply the coupon '(.*)'")]
        public void WhenIApplyTheCoupon(string coupon) {
            cart.EnterCouponCode(coupon);

            cart.WaitForAlert();

            if(cart.CouponAlert.Contains("does not exist!")) {
                cart.RemoveCouponAndItem();

                Assert.Fail("Unsuccessfully applied the coupon");
                Console.WriteLine(cart.CouponAlert);
            }
        }

        [Then(@"the coupon '(.*)' should be applied to the subtotal")]
        public void ThenTheCouponShouldBeAppliedToTheSubtotal(string coupon) {  
            
            decimal originalPrice = cart.GetOriginalPrice(); // Get orignal price
            decimal reducedAmount = 0; // Initalise reducedAmount
            reducedAmount += cart.GetReducedAmount(coupon); // Get reduced amount + update variable
            decimal shippingPrice = cart.GetShippingPrice(); // Get shipping price
            decimal totalPrice = cart.GetTotalPrice(); // Get total price

            decimal couponDiscount = 0M;
            if(coupon == "edgewords") {
                couponDiscount += 0.10M;
            } else if (coupon == "nfocus") {
                couponDiscount += 0.25M;
            }

            decimal discountAmount = originalPrice * couponDiscount; // Value to be compared with reducedAmount
            decimal total = (originalPrice - reducedAmount) + shippingPrice; // Value to be compared with totalPrice

            try {
                Assert.That(discountAmount, Is.EqualTo(reducedAmount));
                Console.WriteLine($"Successfully reduced {couponDiscount*100}% from original price");
                TestContext.WriteLine($"Subtotal price displayed {originalPrice}, Reduced amount displayed {reducedAmount}");
            } catch {
                cart.RemoveCouponAndItem();
                Assert.Fail($"Unsuccessfully reduced {couponDiscount*100}% from original price");
            }

            try {
                Assert.That(total, Is.EqualTo(totalPrice));
                Console.WriteLine("Successfully calculated total after coupon & shippping");
                TestContext.WriteLine($"Total price displayed {totalPrice}, Original price displayed {originalPrice} - reduced amount displayed {reducedAmount} + shipping cost {shippingPrice}");
            } catch {
                Assert.Fail("Unsuccessfully calculated total after coupon & shipping");
            }

            cart.RemoveCouponAndItem();
            Console.WriteLine("Successfully removed items");

            navigation.ClickLink(Navigation.Link.MyAccount);
            Console.WriteLine("Successfully entered My account");
        }
    }
}
