using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PartyInvites.Models
{
    public class UserInfo
    {
        public string LoginName { get; set; }
    }
    public interface IUserRepository
    {
        void Add(UserInfo newUser);
        UserInfo FetchByLoginName(string loginName);
        void SubmitChanges();

    }
    public class DefaultUserRepository : IUserRepository
    {
        public void Add(UserInfo user)
        {
        }
        public UserInfo FetchByLoginName(string loginName)
        {
            return new UserInfo() { LoginName = loginName };
        }
        public void SubmitChanges() { }
    }
}