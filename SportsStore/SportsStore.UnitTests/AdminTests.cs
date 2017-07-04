using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Entities;
using System.Linq;
using Moq;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Models;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using System.Collections.Generic;
namespace SportsStore.UnitTests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void IndexContainsAllProducts()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product(){ ProductID=1, Name="P1",Price=25,Category="Apples"},
                new Product(){ ProductID=2, Name="P2",Price=25,Category="Apples"},
                new Product(){ ProductID=3, Name="P3",Price=25,Category="Apples"}
            });
            AdminController target = new AdminController(mock.Object);
            Product[] result = ((IEnumerable<Product>)target.Index().ViewData.Model).ToArray();

            Assert.AreEqual(3, result.Length);
            Assert.AreEqual("P1", result[0].Name);
            Assert.AreEqual("P2", result[1].Name);
            Assert.AreEqual("P3", result[2].Name);
        }

        [TestMethod]
        public void CanEditProduct()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product(){ ProductID=1, Name="P1",Price=25,Category="Apples"},
                new Product(){ ProductID=2, Name="P2",Price=25,Category="Apples"},
                new Product(){ ProductID=3, Name="P3",Price=25,Category="Apples"}
            });
            AdminController target = new AdminController(mock.Object);

            Product p1 = target.Edit(1).ViewData.Model as Product;
            Product p2 = target.Edit(2).ViewData.Model as Product;
            Product p3 = target.Edit(3).ViewData.Model as Product;


            Assert.AreEqual(1, p1.ProductID);
            Assert.AreEqual(2, p2.ProductID);
            Assert.AreEqual(3, p3.ProductID);
        }

        [TestMethod]
        public void CannotEditNonexistentProduct()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product(){ ProductID=1, Name="P1",Price=25,Category="Apples"},
                new Product(){ ProductID=2, Name="P2",Price=25,Category="Apples"},
                new Product(){ ProductID=3, Name="P3",Price=25,Category="Apples"}
            });
            AdminController target = new AdminController(mock.Object);

            Product result = target.Edit(4).ViewData.Model as Product;

            Assert.IsNull(result);

        }

        [TestMethod]
        public void CanSaveValidChanges()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            AdminController target = new AdminController(mock.Object);
            Product product = new Product { Name = "Test" };
            ActionResult result = target.Edit(product);
            mock.Verify(m => m.SaveProduct(product));//验证该方法是否被调用
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));

        }


        [TestMethod]
        public void CannotSaveInvalidChanges()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            AdminController target = new AdminController(mock.Object);
            Product product = new Product { Name = "Test" };
            target.ModelState.AddModelError("error", "error");
            ActionResult result = target.Edit(product);
            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never());//验证该方法从未被调用
            Assert.IsInstanceOfType(result, typeof(ViewResult));

        }


        [TestMethod]
        public void CanDeleteValidProducts()
        {
            Product prod = new Product { ProductID = 2, Name = "Test" };
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product(){ ProductID=1, Name="P1",Price=25,Category="Apples"},
                prod,
                new Product(){ ProductID=3, Name="P3",Price=25,Category="Apples"}
            });
            AdminController target = new AdminController(mock.Object);
            target.Delete(prod.ProductID);

            mock.Verify(m => m.DeleteProduct(prod.ProductID));
        }
    }
}
