using OpenQA.Selenium;
using static BDD_eCommerce_Project.Support.HelperLibrary;

namespace BDD_eCommerce_Project.Support.PageObjects {
    internal class Cart {

        private IWebDriver _driver;
        public Cart(IWebDriver driver) {
            this._driver = driver;
        }

        public IWebElement CartTitleHeader => WaitForElement(_driver, 3, By.CssSelector("#post-5 > header > h1"));
        public IWebElement CouponCodeInput => WaitForElement(_driver, 3, By.Id("coupon_code"));
        public IWebElement CheckoutButton => WaitForElement(_driver, 3, By.CssSelector("#post-5 .cart-collaterals .wc-proceed-to-checkout"));
        public IWebElement RemoveItemButton => WaitForElement(_driver, 3, By.CssSelector(".remove"));
        public IWebElement RemoveCouponLink => WaitForElement(_driver, 3, By.CssSelector(".woocommerce-remove-coupon"));
        public IWebElement EdgewordsCoupon => WaitForElement(_driver, 3, By.CssSelector($"#post-5 .cart-collaterals .cart-discount.coupon-edgewords span"));
        public IWebElement nFocusCoupon => WaitForElement(_driver, 3, By.CssSelector($"#post-5 .cart-collaterals .cart-discount.coupon-nfocus span"));
        public string CouponAlert => WaitForElement(_driver, 3, By.CssSelector("#post-5 .woocommerce-notices-wrapper")).Text;
        public string OriginalPrice => WaitForElement(_driver, 3, By.CssSelector("#post-5 .cart-collaterals .cart-subtotal span")).Text;
        public string ShippingPrice => WaitForElement(_driver, 3, By.CssSelector("#post-5 .cart-collaterals .woocommerce-shipping-totals.shipping span")).Text;
        public string TotalPrice => WaitForElement(_driver, 3, (By.CssSelector("#post-5 .cart-collaterals .order-total span"))).Text;
        public void EnterCouponCode(string coupon) {
          CouponCodeInput.Clear();
          CouponCodeInput.SendKeys(coupon);
          CouponCodeInput.SendKeys(Keys.Enter);
        }

        public decimal GetOriginalPrice() {
            return ToDecimal(OriginalPrice);
        }

        public decimal GetReducedAmount(string coupon) {
            string ReducedAmount = WaitForElement(_driver, 3, By.CssSelector($"#post-5 .cart-collaterals .cart-discount.coupon-{coupon} span")).Text;
            return ToDecimal(ReducedAmount);
        }
        public decimal GetShippingPrice() {
            return ToDecimal(ShippingPrice);
        }

        public decimal GetTotalPrice() {
            return ToDecimal(TotalPrice);
        }

        public void RemoveCouponFromCart() {
            WaitForElementToBeClickable(_driver, 2, By.CssSelector(".woocommerce-remove-coupon"));
            RemoveCouponLink.Click();      
        }

        public void RemoveItemFromCart() {
            WaitForElementToBeClickable(_driver, 2, By.CssSelector(".remove"));
            RemoveItemButton.Click();
        }
        public void ClickCheckoutButton() {
            CheckoutButton.Click();
        }

        public string WaitForAlert() {
            return CouponAlert;
        }

        public void ClearCart() {
            while (true) {
                try {
                    RemoveCouponFromCart(); 
                } catch {
                    break; // Exit the loop for removing coupons
                }
            }

            while (true) {
                try {
                    RemoveItemFromCart();
                }
                catch {
                    break; // Exit the loop for removing items
                }
            }
        }
    }
}