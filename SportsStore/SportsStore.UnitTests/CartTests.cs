using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Entities;
using System.Linq;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void CanAddNewLines()
        {
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            Cart target = new Cart();
            target.AddItem(p1, 1);
            target.AddItem(p2, 2);
            CartLine[] results = target.Lines.ToArray();

            Assert.AreEqual(2, results.Length);
            Assert.AreEqual(p1, results[0].Product);
            Assert.AreEqual(p2, results[1].Product);
        }

        [TestMethod]
        public void CanAddQuantityForExistingLines()
        {
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            Cart target = new Cart();
            target.AddItem(p1, 1);
            target.AddItem(p2, 2);
            target.AddItem(p1, 10);
            CartLine[] results = target.Lines.OrderBy(x => x.Product.ProductID).ToArray();

            Assert.AreEqual(2, results.Length);
            Assert.AreEqual(11, results[0].Quantity);
            Assert.AreEqual(2, results[1].Quantity);
        }

        [TestMethod]
        public void CanRemoveLines()
        {
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            Product p3 = new Product { ProductID = 3, Name = "P3" };
            Cart target = new Cart();
            target.AddItem(p1, 1);
            target.AddItem(p2, 3);
            target.AddItem(p3, 5);
            target.AddItem(p2, 1);

            target.RemoveLine(p2);

            var results = target.Lines;

            Assert.AreEqual(0, results.Where(x => x.Product == p2).Count());
            Assert.AreEqual(2, results.Count());

        }


        [TestMethod]
        public void CalculateCartTotal()
        {
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };
            Cart target = new Cart();
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 3);
            var result = target.ComputeTotalValue();

            Assert.AreEqual(450M, result);
        }

        [TestMethod]
        public void CanClearCartLines()
        {
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };
            Cart target = new Cart();
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 3);
            target.Clear();

            Assert.AreEqual(0, target.Lines.Count());
        }
    }
}
