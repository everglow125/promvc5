using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PartyInvites.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PartyInvites.Models;
namespace PartyInvites.Controllers.Tests
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void ChangeLoginNameTest()
        {
            UserInfo user = new UserInfo() { LoginName = "Bob" };
            FakeRepository repositoryParam = new FakeRepository();
            repositoryParam.Add(user);
            HomeController target = new HomeController(repositoryParam);
            string oldLoginParam = user.LoginName;
            string newLoginParam = "Joe";
            target.ChangeLoginName(oldLoginParam, newLoginParam);
            Assert.AreEqual(newLoginParam, user.LoginName);
            Assert.IsTrue(repositoryParam.DidSubmitChanges);

            var list = new List<TempClass>() { 
                new TempClass(){ Name="test1",Value="value1"},
                new TempClass(){ Name="test2",Value="value2"},
                new TempClass(){ Name="test3",Value="value3"},
                new TempClass(){ Name="test1",Value="value4"},
                new TempClass(){ Name="test2",Value="value5"},
                new TempClass(){ Name="test3",Value="value6"},
                new TempClass(){ Name="test1",Value="value7"},
            };

            var temp = TMatch(list, "test1");
            Func<TempClass, string, bool> ttt = (x, y) => x.Name == y;
        }

        public IEnumerable<TempClass> TMatch(IEnumerable<TempClass> source, string name)
        {
            foreach (var item in source)
            {
                if (item.Name == name)
                    yield return item;
            }
        }

        public class TempClass
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        class FakeRepository : IUserRepository
        {
            public List<UserInfo> Users = new List<UserInfo>();
            public bool DidSubmitChanges = false;
            public void Add(UserInfo user)
            {
                Users.Add(user);
            }

            public UserInfo FetchByLoginName(string loginName)
            {
                return Users.First(x => x.LoginName == loginName);

            }
            public void SubmitChanges()
            {
                DidSubmitChanges = true;
            }
        }
    }
}
