using System;
using HemtentaTdd2017;
using HemtentaTdd2017.webshop;
using System.Linq;
using Moq;
using NUnit.Framework;

namespace UnitTest
{
    /* Vilka metoder och properties behöver testas?
        AddProduct()
        RemoveProduct()
        Checkout()

     Ska några exceptions kastas?
        ArgumentNullException (Om produkten är null)
        ProductValueIsNegativeOrZeroException (Om produktens pris är 0 eller mindre)
        InsufficientFundsException (Om ens balans är mindre än vad det kostar att checka ut)
        
     Vilka är domänerna för IWebshop och IBasket?
        IWebshop
            Basket: Kan vara null eller ett tillåtet object av IBasket
            Checkout: Kan bara vara void

        IBasket
            AddProduct och RemoveProduct kan bara vara void
            TotalCost: decimaltal, MaxValue, MinValue, MinusOne, Zero, One */

    [TestFixture]
    public class WebShopTest
    {
        private decimal productPrice = 20;
        private int numberOfProducts = 5;
        
        Mock<IBilling> bill;
        Product product;
        IWebshop webshop;
        IBasket basket;

        [SetUp]
        public void Setup()
        {
            bill = new Mock<IBilling>();
            product = new Product { Price = productPrice };
            basket = new Basket();
            webshop = new Webshop(basket);
        }

        [Test]
        public void AddProduct_Success()
        {
            basket.AddProduct(product, numberOfProducts);
            Assert.AreEqual(productPrice * numberOfProducts, basket.TotalCost);
        }

        [Test]
        public void AddProduct_ProductAmountIsNegative_Failed()
        {
            Assert.Throws<ProductValueIsNegativeOrZeroException>(() => basket.AddProduct(product, -1));
        }

        [Test]
        public void AddProduct_IncorrectValuesOnPrice_Failed()
        {
            //Anledning till att jag skrev 3 testfall i 1 testfall
            //är att jag fick error när jag försökte göra ett TestCase
            //med dessa decimal värdena
            product.Price = decimal.MinValue;
            Assert.Throws<ProductValueIsNegativeOrZeroException>(() => basket.AddProduct(product, numberOfProducts));

            product.Price = decimal.MinusOne;
            Assert.Throws<ProductValueIsNegativeOrZeroException>(() => basket.AddProduct(product, numberOfProducts));

            product.Price = decimal.Zero;
            Assert.Throws<ProductValueIsNegativeOrZeroException>(() => basket.AddProduct(product, numberOfProducts));
        }

        [Test]
        public void AddProduct_ProductIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => basket.AddProduct(null, numberOfProducts));
        }

        [Test]
        public void RemoveAllProducts_Success()
        {
            basket.AddProduct(product, numberOfProducts);
            basket.RemoveProduct(product, numberOfProducts);

            Assert.AreEqual(0, basket.TotalCost);
        }

        [Test]
        public void RemoveProducts_ExceptOne_Success()
        {
            basket.AddProduct(product, numberOfProducts);
            basket.RemoveProduct(product, numberOfProducts - 1);

            Assert.AreEqual(productPrice, basket.TotalCost);
        }

        [Test]
        public void RemoveProduct_ProductIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => basket.RemoveProduct(null, numberOfProducts));
        }

        [Test]
        public void Checkout_Success()
        {
            basket.AddProduct(product, numberOfProducts);
            bill.SetupGet(x => x.Balance).Returns(basket.TotalCost);
            webshop.Checkout(bill.Object);
            bill.Verify(x => x.Pay(basket.TotalCost), Times.Once);
        }

        [Test]
        public void Checkout_InsufficientFunds_Throws()
        {
            basket.AddProduct(product, numberOfProducts);
            bill.SetupGet(x => x.Balance).Returns(basket.TotalCost - 1);

            Assert.Throws<InsufficientFundsException>(() => webshop.Checkout(bill.Object));
        }

        [Test]
        public void Checkout_BillingIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => webshop.Checkout(null));
        }
    }
}
