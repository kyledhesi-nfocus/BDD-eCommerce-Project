using BDD_eCommerce_Project.Support;
using BDD_eCommerce_Project.Support.PageObjects;
using NUnit.Framework;
using OpenQA.Selenium;
using TechTalk.SpecFlow.Assist;
using TechTalk.SpecFlow.Infrastructure;
using Allure.Net.Commons;

namespace BDD_eCommerce_Project.StepDefinitions {
    [Binding]
    public class CheckoutDefinitions {
        private readonly ScenarioContext _scenarioContext;
        private IWebDriver _driver;
        private string _screenshotFilePath;
        private readonly ISpecFlowOutputHelper _specFlowOutputHelper;

        /* Steps definitions for ordering an item */
        public CheckoutDefinitions(ScenarioContext scenarioContext, ISpecFlowOutputHelper specFlowOutputHelper) {
            _scenarioContext = scenarioContext;
            _specFlowOutputHelper = specFlowOutputHelper;
            this._driver = (IWebDriver)_scenarioContext["myDriver"];
            this._screenshotFilePath = (string)_scenarioContext["screenshotFilePath"];     
        }

        /* Background Step - Add an item to the cart */
        [Given(@"I add a product to the cart")]
        public void AddAProductToTheCart() {
            Navigation navigation = new(_driver);
            navigation.ClickLink(Navigation.Link.Shop);     // navigate to the 'Shop' page
            Shop shop = new(_driver);
            try {
                shop.AddProductToCart("");      //  call 'AddProductToCart' to add a product to the cart (default item: Belt)
            } catch {
                Assert.Fail($"Unsuccessfully added item to the cart");      // Assert.Fail if an exception occurs during the attempt to add a product
            }
            navigation.ClickLink(Navigation.Link.Cart);     // navigate to the 'Cart' page
        }

        /* Continue to the checkout page */
        [Given(@"I proceed to checkout")]
        public void ProceedToCheckout() {
            Cart cart = new(_driver);
            cart.ClickCheckoutButton();    // navigate to the 'Checkout' page
        }

        /* Enter the billing details from feature file */
        [Given(@"I enter my billing details")]
        public void EnterMyBillingDetails(Table billingDetailsTable) {
            Checkout checkout = new(_driver);
            BillingDetails billingDetails = billingDetailsTable.CreateInstance<BillingDetails>();   // converts the table of billing details into an instance of BillingDetails
            try {
                Assert.That(checkout.EnterBillingDetails(billingDetails), "Unsuccessfully entered billing details into input fields");      // checks if 'EnterBillingDetails' successfully inputs all data into fields  
            } catch (AssertionException) {
                HelperLibrary.TakeScreenshot(_driver, _screenshotFilePath + "Billing Details Exception.jpg");
                _specFlowOutputHelper.AddAttachment(_screenshotFilePath + "Billing Details Exception.jpg");
                AllureApi.AddAttachment(_screenshotFilePath + "Billing Details Exception.jpg");
                throw;
            }
        }

        /* Select the payment method from feature file */
        [Given(@"I select the payment method '(.*)'")]
        public void SelectThePaymentMethod(string paymentMethod) {
            Checkout checkout = new(_driver);
            checkout.ClickPaymentMethod(paymentMethod);     // select the payment method
            _specFlowOutputHelper.WriteLine($"Successfully selected payment method: {paymentMethod}");
        }

        /* Complete the checkout */
        [When(@"I place the order")]
        public void PlaceTheOrder(){
            Checkout checkout = new(_driver);
            checkout.ClickPlaceOrderButton();  // click the 'Place order' button
        }

        /* Wait for the order confirmation page */
        [Then(@"I should see the Order recieved page")]
        public void SeeTheOrderRecievedPage() {
            OrderReceived orderReceived = new(_driver);
            try {
                _scenarioContext["confirmationOrderNumber"] = orderReceived.GetOrderNumber();   // store 'GetOrderNumber' value into _scenarioContext
                _specFlowOutputHelper.WriteLine($"Successfully placed the order - order number:{orderReceived.GetOrderNumber()}");
            } catch(Exception) {
                Assert.Fail("Unsuccessfully placed the order");    // Assert.Fail if order confirmation page does not appear
            } finally {
                HelperLibrary.TakeScreenshot(_driver, _screenshotFilePath + "Order Confirmation Page.jpg");
                _specFlowOutputHelper.AddAttachment(_screenshotFilePath + "Order Confirmation Page.jpg");
                AllureApi.AddAttachment(_screenshotFilePath + "Order Confirmation Page.jpg");
            }
        }

        /* Checks if Order Number is displayed on the 'Orders' page */
        [Then(@"the order number should appear on the Orders page")]
        public void OrderNumberShouldAppearOnTheOrdersPage() {
            Navigation navigation = new(_driver);
            navigation.ClickLink(Navigation.Link.MyAccount);    // naviagate to the 'My account' page
            MyAccountDashboard myAccountDashboard = new(_driver);
            myAccountDashboard.ClickViewOrdersLink();    // navigate to the 'Orders' page
            MyAccountOrders myAccountOrders = new(_driver);
            string MostRecentOrderNumber = myAccountOrders.GetMostRecentOrderID();  // get most recent Order ID from 'Orders' page - call 'GetMostRecentOrderID'
            string confirmationOrderNumber = (string)_scenarioContext["confirmationOrderNumber"];
            try {
                Assert.That(confirmationOrderNumber, Is.EqualTo(MostRecentOrderNumber), "Unsuccessfully displayed latest order");    // checks if the Order Number displayed from 'Order Recieved' page matches with the most recent Order Number on 'Orders' page 
                _specFlowOutputHelper.WriteLine($"Successfully displayed latest order");
            } finally {
                HelperLibrary.TakeScreenshot(_driver, _screenshotFilePath + "My Account - All Orders Page.jpg");
                _specFlowOutputHelper.AddAttachment(_screenshotFilePath + "My Account - All Orders Page.jpg");
                AllureApi.AddAttachment(_screenshotFilePath + "My Account - All Orders Page.jpg");
            }
        }
    }
}
