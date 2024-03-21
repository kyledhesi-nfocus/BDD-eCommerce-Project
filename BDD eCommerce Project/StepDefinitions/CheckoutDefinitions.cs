using BDD_eCommerce_Project.Support;
using BDD_eCommerce_Project.Support.PageObjects;
using NUnit.Framework;
using OpenQA.Selenium;
using TechTalk.SpecFlow.Assist;
using TechTalk.SpecFlow.Infrastructure;

namespace BDD_eCommerce_Project.StepDefinitions {
    [Binding]
    public class CheckoutDefinitions {

        private readonly ScenarioContext _scenarioContext;
        private IWebDriver driver;
        private string screenshotFilePath;
        private readonly ISpecFlowOutputHelper _specFlowOutputHelper;

        /* Steps definitions for ordering an item */
        public CheckoutDefinitions(ScenarioContext scenarioContext, ISpecFlowOutputHelper specFlowOutputHelper) {
            _scenarioContext = scenarioContext;
            _specFlowOutputHelper = specFlowOutputHelper;
            this.driver = (IWebDriver)_scenarioContext["myDriver"];
            this.screenshotFilePath = (string)_scenarioContext["screenshotFilePath"];     
        }

        /* Background Step - Add an item to the cart */
        [Given(@"I add a product to the cart")]
        public void AddAProductToTheCart() {
            Navigation navigation = new(driver);
            navigation.ClickLink(Navigation.Link.Shop);     // navigate to the 'Shop' page
            
            Shop shop = new(driver);
           _specFlowOutputHelper.WriteLine("Successfully entered shop");
            
            try {
                shop.AddProductToCart("");      //  call 'AddProductToCart' to add a product to the cart (default item: Belt)
                _specFlowOutputHelper.WriteLine($"Successfully added item to the cart");
            } catch {
                Assert.Fail($"Unsuccessfully added item to the cart");      // Assert.Fail if an exception occurs during the attempt to add a product
            }
            
            navigation.ClickLink(Navigation.Link.Cart);     // navigate to the 'Cart' page
            _specFlowOutputHelper.WriteLine("Successfully entered cart");
        }

        /* Continue to the checkout page */
        [Given(@"I proceed to checkout")]
        public void ProceedToCheckout() {
            Cart cart = new(driver);
            cart.ClickCheckoutButton();    // navigate to the 'Checkout' page
            _specFlowOutputHelper.WriteLine("Successfully entered checkout");
        }

        /* Enter the billing details from feature file */
        [Given(@"I enter my billing details")]
        public void EnterMyBillingDetails(Table billingDetailsTable) {
            Checkout checkout = new(driver);
            BillingDetails billingDetails = billingDetailsTable.CreateInstance<BillingDetails>();   // converts the table of billing details into an instance of BillingDetails
            try {
                Assert.That(checkout.EnterBillingDetails(billingDetails), "Unsuccessfully entered billing details into input fields");      // checks if 'EnterBillingDetails' successfully inputs all data into fields  
                _specFlowOutputHelper.WriteLine("Successfully entered billing details");
            } catch (AssertionException) {
                HelperLibrary.TakeScreenshot(driver, screenshotFilePath + "Billing Details Exception.jpg");
                _specFlowOutputHelper.AddAttachment(screenshotFilePath + "Billing Details Exception.jpg");
                throw;
            }
        }

        /* Select the payment method from feature file */
        [Given(@"I select the payment method '(.*)'")]
        public void SelectThePaymentMethod(string paymentMethod) {
            Checkout checkout = new(driver);
            checkout.ClickPaymentMethod(paymentMethod);     // select the payment method
            _specFlowOutputHelper.WriteLine($"Successfully clicked payment method: {paymentMethod}");
        }

        /* Complete the checkout */
        [When(@"I place the order")]
        public void PlaceTheOrder(){
            Checkout checkout = new(driver);
            checkout.ClickPlaceOrderButton();  // click the 'Place order' button
            _specFlowOutputHelper.WriteLine("Successfully clicked Place order button");
        }

        /* Wait for the order confirmation page */
        [Then(@"I should see the Order recieved page")]
        public void SeeTheOrderRecievedPage() {
            OrderReceived orderReceived = new(driver);
            try {
                _scenarioContext["confirmationOrderNumber"] = orderReceived.GetOrderNumber();   // store 'GetOrderNumber' value into _scenarioContext
                _specFlowOutputHelper.WriteLine($"Successfully placed order with order number:{orderReceived.GetOrderNumber()} - Attaching Order Confirmation screenshot to report");
                HelperLibrary.TakeScreenshot(driver, screenshotFilePath + "Order Confirmation.jpg");
                _specFlowOutputHelper.AddAttachment(screenshotFilePath + "Order Confirmation.jpg");
            } catch(Exception) {
                HelperLibrary.TakeScreenshot(driver, screenshotFilePath + "Order Confirmation Exception.jpg");
                _specFlowOutputHelper.AddAttachment(screenshotFilePath + "Order Confirmation Exception.jpg");
                Assert.Fail("Unsuccessfully placed order - Attaching screenshot to report");    // Assert.Fail if order confirmation page does not appear
            }
        }

        /* Checks if Order Number is displayed on the 'Orders' page */
        [Then(@"the order number should appear on the Orders page")]
        public void OrderNumberShouldAppearOnTheOrdersPage() {
            Navigation navigation = new(driver);
            navigation.ClickLink(Navigation.Link.MyAccount);    // naviagate to the 'My account' page

            MyAccountDashboard myAccountDashboard = new(driver);
            _specFlowOutputHelper.WriteLine("Successfully entered My account");

            myAccountDashboard.ClickViewOrdersLink();    // navigate to the 'Orders' page
            MyAccountOrders myAccountOrders = new(driver);

            _specFlowOutputHelper.WriteLine("Successfully entered Orders page");

            string MostRecentOrderNumber = myAccountOrders.GetMostRecentOrderID();  // get most recent Order ID from 'Orders' page - call 'GetMostRecentOrderID'
            string confirmationOrderNumber = (string)_scenarioContext["confirmationOrderNumber"];
            try {
                Assert.That(confirmationOrderNumber, Is.EqualTo(MostRecentOrderNumber));    // checks if the Order Number displayed from 'Order Recieved' page matches with the most recent Order Number on 'Orders' page 
                _specFlowOutputHelper.WriteLine($"Successfully displayed latest order: Order number displayed {MostRecentOrderNumber} | Expected order number: {confirmationOrderNumber} - Attaching Orders screenshot to report");
            } catch (AssertionException) {
                _specFlowOutputHelper.WriteLine($"Unsuccessfully displayed latest order\nOrder number displayed {MostRecentOrderNumber} | Expected order number: {confirmationOrderNumber}");
            }

            HelperLibrary.TakeScreenshot(driver, screenshotFilePath + "My Account - All Orders Page.jpg");
            _specFlowOutputHelper.AddAttachment(screenshotFilePath + "My Account - All Orders Page.jpg");

        }
    }
}
