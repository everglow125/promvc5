using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Models;
using SportsStore.WebUI.Infrastructure.Abstract;
using System.Web.Mvc;

namespace SportsStore.UnitTests
{
    /// <summary>
    /// AdminSecurityTests 的摘要说明
    /// </summary>
    [TestClass]
    public class AdminSecurityTests
    {
        [TestMethod]
        public void CanLoginWithValidCredentials()
        {
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("admin", "secret")).Returns(true);

            LoginViewModel model = new LoginViewModel
            {
                Username = "admin",
                Password = "secret"
            };

            AccountController target = new AccountController(mock.Object);

            ActionResult result = target.Login(model, "/MyUrl");

            Assert.AreEqual("/MyUrl", ((RedirectResult)result).Url);
            Assert.IsInstanceOfType(result, typeof(RedirectResult));
        }

        [TestMethod]
        public void CannotLoginWithInvalidCredentials()
        {
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("badUser", "badPass")).Returns(false);

            LoginViewModel model = new LoginViewModel
            {
                Username = "badUser",
                Password = "badPass"
            };

            AccountController target = new AccountController(mock.Object);

            ActionResult result = target.Login(model, "/MyUrl");

            Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}
