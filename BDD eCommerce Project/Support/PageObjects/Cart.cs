using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BDD_eCommerce_Project.Support.HelperLibrary;

namespace BDD_eCommerce_Project.Support.PageObjects {
    internal class Cart {

        private IWebDriver _driver;
        public Cart(IWebDriver driver) {
            this._driver = driver;
        }

        public IWebElement CouponCodeInput => WaitForElement(_driver, 5, By.Id("coupon_code"));
        public IWebElement CheckoutButton => WaitForElement(_driver, 2, By.CssSelector("#post-5 .cart-collaterals .wc-proceed-to-checkout"));
        public IWebElement RemoveItemButton => WaitForElement(_driver, 2, By.CssSelector(".remove"));
        public IWebElement RemoveCoupon => WaitForElement(_driver, 2, By.CssSelector(".woocommerce-remove-coupon"));
        public string CouponAlert => WaitForElement(_driver, 2, By.CssSelector("#post-5 .woocommerce-notices-wrapper")).Text;
        public string OriginalPrice => WaitForElement(_driver, 2, By.CssSelector("#post-5 .cart-collaterals .cart-subtotal span")).Text;
        public string ShippingPrice => WaitForElement(_driver, 2, By.CssSelector("#post-5 .cart-collaterals .woocommerce-shipping-totals.shipping span")).Text;
        public string TotalPrice => WaitForElement(_driver, 2, (By.CssSelector("#post-5 .cart-collaterals .order-total span"))).Text;


        public void EnterCouponCode(string coupon) {
            CouponCodeInput.Clear();
            CouponCodeInput.SendKeys(coupon);
            CouponCodeInput.SendKeys(Keys.Enter);
        }

        public decimal GetOriginalPrice() {
            return ToDecimal(OriginalPrice);
        }

        public decimal GetReducedAmount() {
            try {
                WaitForElement(_driver, 2, By.CssSelector($"#post-5 .cart-collaterals .cart-discount.coupon-edgewords span"));
                string ReducedAmount = _driver.FindElement(By.CssSelector($"#post-5 .cart-collaterals .cart-discount.coupon-edgewords span")).Text;
                return ToDecimal(ReducedAmount);
            } catch {
                WaitForElement(_driver, 2, By.CssSelector($"#post-5 .cart-collaterals .cart-discount.coupon-nfocus span"));
                string ReducedAmount = _driver.FindElement(By.CssSelector($"#post-5 .cart-collaterals .cart-discount.coupon-nfocus span")).Text;
                return ToDecimal(ReducedAmount);
            }
        }

        public decimal GetShippingPrice() {
            return ToDecimal(ShippingPrice);
        }

        public decimal GetTotalPrice() {
            return ToDecimal(TotalPrice);
        }

        public void RemoveCouponAndItem() {
            try {
                RemoveCoupon.Click();
            } catch {

            }
            WaitForElementDisabled(_driver, 2, By.CssSelector("blockUI.blockOverlay"));
            RemoveItemButton.Click();
        }
        public void Checkout() {
            CheckoutButton.Click();
        }

        public string WaitForAlert() {
            return CouponAlert; // Wait for coupon applied
        }
    }
}
