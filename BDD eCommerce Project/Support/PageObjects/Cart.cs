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

        public IWebElement CouponCodeInput => _driver.FindElement(By.Id("coupon_code"));
        public IWebElement CheckoutButton => _driver.FindElement(By.CssSelector("#post-5 .cart-collaterals .wc-proceed-to-checkout"));
        public IWebElement RemoveItemButton => _driver.FindElement(By.CssSelector(".remove"));
        public IWebElement RemoveCoupon => _driver.FindElement(By.CssSelector(".woocommerce-remove-coupon"));
        public string CouponAlert => _driver.FindElement(By.CssSelector("#post-5 .woocommerce-notices-wrapper")).Text;
        public string OriginalPrice => _driver.FindElement(By.CssSelector("#post-5 .cart-collaterals .cart-subtotal span")).Text;
        public string ShippingPrice => _driver.FindElement(By.CssSelector("#post-5 .cart-collaterals .woocommerce-shipping-totals.shipping span")).Text;
        public string TotalPrice => _driver.FindElement(By.CssSelector("#post-5 .cart-collaterals .order-total span")).Text;


        public void EnterCouponCode(string coupon) {
            WaitForElement(_driver, 5, By.Id("coupon_code"));
            CouponCodeInput.Clear();
            CouponCodeInput.SendKeys(coupon);
            CouponCodeInput.SendKeys(Keys.Enter);
        }

        public decimal GetOriginalPrice() {
            WaitForElement(_driver, 2, By.CssSelector("#post-5 .cart-collaterals .cart-subtotal span"));
            return ToDecimal(OriginalPrice);
        }

        public decimal GetReducedAmount(string coupon) {
            WaitForElement(_driver, 2, By.CssSelector($"#post-5 .cart-collaterals .cart-discount.coupon-{coupon} span"));
            string ReducedAmount = _driver.FindElement(By.CssSelector($"#post-5 .cart-collaterals .cart-discount.coupon-{coupon} span")).Text;
            
            return ToDecimal(ReducedAmount);
        }

        public decimal GetShippingPrice() {
            WaitForElement(_driver, 2, (By.XPath("//*[@id=\"shipping_method\"]/li/label/span")));
            return ToDecimal(ShippingPrice);
        }

        public decimal GetTotalPrice() {
            WaitForElement(_driver, 2, (By.CssSelector("#post-5 .cart-collaterals .order-total span")));
            return ToDecimal(TotalPrice);
        }

        public void RemoveCouponAndItem() {
            try {
                WaitForElement(_driver, 2, By.CssSelector(".woocommerce-remove-coupon"));
                RemoveCoupon.Click();
            } catch {
               
            }
            WaitForElement(_driver, 2, By.CssSelector(".remove"));
            WaitForElementDisabled(_driver, 2, By.CssSelector("blockUI.blockOverlay"));
            RemoveItemButton.Click();
        }
        public void Checkout() {
            WaitForElement(_driver, 2, By.CssSelector("#post-5 .cart-collaterals .wc-proceed-to-checkout"));
            CheckoutButton.Click();
        }

        public void WaitForAlert() {
            WaitForElement(_driver, 2, By.CssSelector("#post-5 .woocommerce-notices-wrapper")); // Wait for coupon applied
        }
    }
}
