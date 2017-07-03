using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using System.Collections.Generic;
using System.Web.Mvc;
using SportsStore.WebUI.Models;
using SportsStore.WebUI.HtmlHelpers;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CanPaginate()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>() {
                new Product(){ ProductID=1, Name="Football",Price=25},
                new Product(){ ProductID=2,Name="Surf board",Price=179},
                new Product(){ ProductID=3, Name="Running shoes",Price=95},
                new Product(){ ProductID=4,Name="BasketBall",Price=18},
                new Product(){ ProductID=5, Name="Skin",Price=29},
            });
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;

            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual("BasketBall", prodArray[0].Name);
            Assert.AreEqual("Skin", prodArray[1].Name);
        }

        [TestMethod]
        public void CanSendPaginationViewModel()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>() {
                new Product(){ ProductID=1, Name="Football",Price=25},
                new Product(){ ProductID=2,Name="Surf board",Price=179},
                new Product(){ ProductID=3, Name="Running shoes",Price=95},
                new Product(){ ProductID=4,Name="BasketBall",Price=18},
                new Product(){ ProductID=5, Name="Skin",Price=29},
            });
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;
            PagingInfo pagingInfo = result.PagingInfo;

            Assert.AreEqual(2, pagingInfo.CurrentPage);
            Assert.AreEqual(3, pagingInfo.ItemsPerPage);
            Assert.AreEqual(5, pagingInfo.TotalItems);
            Assert.AreEqual(2, pagingInfo.TotalPages);
        }


        [TestMethod]
        public void CanGeneratePageLinks()
        {
            HtmlHelper myHelper = null;
            PagingInfo pagingInfo = new PagingInfo()
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10,
            };
            Func<int, string> pageUrlDelegate = i => "Page" + i;
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }

        [TestMethod]
        public void CanFilterProducts()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>() {
                new Product(){ ProductID=1, Name="Football",Price=25,Category="Cat1"},
                new Product(){ ProductID=2,Name="Surf board",Price=179,Category="Cat2"},
                new Product(){ ProductID=3, Name="Running shoes",Price=95,Category="Cat1"},
                new Product(){ ProductID=4,Name="BasketBall",Price=18,Category="Cat2"},
                new Product(){ ProductID=5, Name="Skin",Price=29,Category="Cat3"},
            });
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            ProductsListViewModel result = (ProductsListViewModel)controller.List("Cat2", 1).Model;

            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length == 2, "length err");
            Assert.AreEqual("Surf board", prodArray[0].Name);
            Assert.AreEqual("BasketBall", prodArray[1].Name);
            Assert.IsTrue(prodArray[0].Category == "Cat2", "Index 0 err");
            Assert.IsTrue(prodArray[1].Category == "Cat2", "Index 1 err");
        }

        [TestMethod]
        public void GenerateCategorySpecificProductCount()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>() {
                new Product(){ ProductID=1, Name="Football",Price=25,Category="Cat1"},
                new Product(){ ProductID=2,Name="Surf board",Price=179,Category="Cat2"},
                new Product(){ ProductID=3, Name="Running shoes",Price=95,Category="Cat1"},
                new Product(){ ProductID=4,Name="BasketBall",Price=18,Category="Cat2"},
                new Product(){ ProductID=5, Name="Skin",Price=29,Category="Cat3"},
            });
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            int result1 = ((ProductsListViewModel)controller.List("Cat1").Model).PagingInfo.TotalItems;
            int result2 = ((ProductsListViewModel)controller.List("Cat2").Model).PagingInfo.TotalItems;
            int result3 = ((ProductsListViewModel)controller.List("Cat3").Model).PagingInfo.TotalItems;
            int resultAll = ((ProductsListViewModel)controller.List(null).Model).PagingInfo.TotalItems;

            ProductsListViewModel result = (ProductsListViewModel)controller.List("Cat2", 1).Model;


            Assert.AreEqual(2, result1);
            Assert.AreEqual(2, result2);
            Assert.AreEqual(1, result3);
            Assert.AreEqual(5, resultAll);

        }

        [TestMethod]
        public void CanCreateCategories()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>() {
                new Product(){ ProductID=1, Name="Football",Price=25,Category="Apples"},
                new Product(){ ProductID=2,Name="Surf board",Price=179,Category="Apples"},
                new Product(){ ProductID=3, Name="Running shoes",Price=95,Category="Plums"},
                new Product(){ ProductID=4,Name="BasketBall",Price=18,Category="Oranges"}
            });
            NavController controller = new NavController(mock.Object);

            string[] result = ((IEnumerable<string>)controller.Menu().Model).ToArray();

            Assert.IsTrue(result.Length == 3, "length err");
            Assert.AreEqual("Apples", result[0]);
            Assert.AreEqual("Oranges", result[1]);
            Assert.AreEqual("Plums", result[2]);
        }


        [TestMethod]
        public void IndicatesSelectedCategory()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>() {
                new Product(){ ProductID=1, Name="Football",Price=25,Category="Apples"},
                new Product(){ ProductID=2,Name="Surf board",Price=179,Category="Apples"},
                new Product(){ ProductID=3, Name="Running shoes",Price=95,Category="Plums"},
                new Product(){ ProductID=4,Name="BasketBall",Price=18,Category="Oranges"}
            });
            NavController controller = new NavController(mock.Object);
            string categoryToSelect = "Apples";
            string result = controller.Menu(categoryToSelect).ViewBag.SelectedCategory;

            Assert.AreEqual(categoryToSelect, result);
        }

    }
}
