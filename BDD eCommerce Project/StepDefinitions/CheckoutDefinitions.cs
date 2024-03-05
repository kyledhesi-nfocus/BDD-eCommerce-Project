using BDD_eCommerce_Project.Support;
using BDD_eCommerce_Project.Support.PageObjects;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        private readonly BillingDetails billingDetails;
        private readonly Checkout checkout;
        private readonly OrderReceived orderReceived;
        private readonly MyAccountDashboard myAccountDashboard;
        private readonly MyAccountOrders myAccountOrders;
        private string orderNumber;


        public CheckoutDefinitions(ScenarioContext scenarioContext) {
            _scenarioContext = scenarioContext;

            this.driver = (IWebDriver)_scenarioContext["myDriver"];
            this.navigation = new(driver);
            this.shop = new(driver);
            this.cart = new(driver);
            this.checkout = new(driver);
            this.billingDetails = new(firstName: "King", lastName: "Charles", streetName: "Buckingham Palace Road", city: "London", postcode: "SW1A 1AA", phoneNumber: "0798347190321", email: "example@email.co.uk");
            this.myAccountDashboard = new(driver);
            this.myAccountOrders = new(driver);
            this.orderReceived = new(driver);  
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

        [When(@"I enter my billing details")]
        public void WhenIEnterMyBillingDetails() {
            checkout.EnterBillingDetails(billingDetails);
        }

        [When(@"I click the Place order button")]
        public void WhenIClickThePlaceOrderButton(){
            checkout.PlaceOrder();
        }

        [Then(@"I should see the Order recieved page")]
        public void ThenIShouldSeeTheOrderRecievedPage() {
            Assert.That(orderReceived.OrderRecieved(), "Order has not been placed"); 
            orderNumber = orderReceived.GetOrderNumber();
        }

        [Then(@"the order number should appear on the Orders page")]
        public void ThenTheOrderNumberShouldAppearOnTheOrdersPage() {
            navigation.ClickLink(Navigation.Link.MyAccount);
            myAccountDashboard.ViewOrders();

            string OrderID = myAccountOrders.GetOrderID();

            try {
                Assert.That(orderNumber, Is.EqualTo(OrderID.TrimStart('#')).IgnoreCase);
                Console.WriteLine("Successfully displayed latest order");
                TestContext.WriteLine($"Order number displayed {orderNumber}, Most recent order on My account {OrderID}");
                myAccountOrders.Dashboard();
            } catch {
                Assert.Fail("Unsuccessfully displayed latest order");
            }   
        }
    }
}
