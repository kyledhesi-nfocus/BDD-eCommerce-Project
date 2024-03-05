using BDD_eCommerce_Project.Support.PageObjects;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_eCommerce_Project.StepDefinitions {
    [Binding]
    public class CheckoutDefinitions {

        private readonly ScenarioContext _scenarioContext;
        private IWebDriver driver;

        private readonly Navigation navigation;
        private readonly Shop shop;
        private readonly Cart cart;

        public CheckoutDefinitions(ScenarioContext scenarioContext) {
            _scenarioContext = scenarioContext;

            this.driver = (IWebDriver)_scenarioContext["myDriver"];
            this.navigation = new(driver);
            this.shop = new(driver);
            this.cart = new(driver);
        }

  
        [Given(@"I have at least one product in my cart")]
        public void GivenIHaveAtLeastOneProductInMyCart() {
            navigation.ClickLink(Navigation.Link.Shop);
            shop.AddToCart(Shop.Product.Belt);
        }

        [Given(@"I am on the cart page")]
        public void GivenIAmOnTheCartPage() {
            navigation.ClickLink(Navigation.Link.Cart);
        }

        [When(@"I click the Proceed to checkout button")]
        public void WhenIClickTheProceedToCheckoutButton() {
            cart.Checkout();
        }




    }
}
