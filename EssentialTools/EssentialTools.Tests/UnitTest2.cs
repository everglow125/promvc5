using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EssentialTools.Models;
using System.Linq;
using Moq;
namespace EssentialTools.Tests
{
    [TestClass]
    public class UnitTest2
    {
        private Product[] products =
        {
            new Product{ Name="Kayak",Category="Watersports",Price=275M},
            new Product{ Name="Lifejacket",Category="Watersports",Price=48.95M},
            new Product{ Name="Soccer ball",Category="Soccer",Price=19.5M},
            new Product{ Name="Corner flag",Category="Soccer",Price=34.95M}
        };
        [TestMethod]
        public void Sum_Products_Correctly()
        {
            Mock<IDiscountHelper> mock = new Mock<IDiscountHelper>();
            mock.Setup(m => m.ApplyDiscount(It.IsAny<decimal>()))
                .Returns<decimal>(total => total * 0.9M);
            var target = new LinqValueCalculator(mock.Object);

            //   var discounter = new MinimumDiscountHelper();
            //   var target = new LinqValueCalculator(discounter);
            var goalTotal = products.Sum(x => x.Price) * 0.9M;
            var result = target.ValueProducts(products);
            Assert.AreEqual(goalTotal, result);
        }

        private Product[] createProduct(decimal value)
        {
            return new[] { new Product { Price = value } };
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Pass_Through_Variable_Discounts()
        {
            Mock<IDiscountHelper> mock = new Mock<IDiscountHelper>();
            mock.Setup(x => x.ApplyDiscount(It.IsAny<decimal>()))
                .Returns<decimal>(total => total);
            mock.Setup(x => x.ApplyDiscount(It.Is<decimal>(v => v == 0)))
                .Throws<System.ArgumentOutOfRangeException>();
            mock.Setup(x => x.ApplyDiscount(It.Is<decimal>(v => v > 100)))
                .Returns<decimal>(total => total * 0.9M);
            mock.Setup(x => x.ApplyDiscount(It.IsInRange<decimal>(10, 100, Range.Inclusive)))
                 .Returns<decimal>(total => total - 5M);

            var target = new LinqValueCalculator(mock.Object);


            decimal TenDollarDiscount = target.ValueProducts(createProduct(10));
            decimal HundredDollarDiscount = target.ValueProducts(createProduct(100));
            decimal FiftyDollarDiscount = target.ValueProducts(createProduct(50));
            decimal FiveDollarDiscount = target.ValueProducts(createProduct(5));
            decimal FiveHundredDollarDiscount = target.ValueProducts(createProduct(500));

            Assert.AreEqual(5, FiveDollarDiscount, "$5 Fail");
            Assert.AreEqual(5, TenDollarDiscount, "$10 Fail");
            Assert.AreEqual(45, FiftyDollarDiscount, "$50 Fail");
            Assert.AreEqual(95, HundredDollarDiscount, "$100 Fail");
            Assert.AreEqual(450, FiveHundredDollarDiscount, "$500 Fail");
            target.ValueProducts(createProduct(0));

        }
    }
}
