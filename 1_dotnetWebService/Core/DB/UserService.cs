using dotnetWebService.Model;
using dotnetWebService.RoleManagement.DB;
using System.Collections.Generic;

namespace dotnetWebService.Core.DB
{
    public class UserService
    {
        private readonly UsersDAL userDAL;
        public UserService()
        {
            userDAL = new UsersDAL();
        }

        public int AddUser(User user)
        {
            return userDAL.AddUser(user);
        }

        public int UpdateUser(User user)
        {
            return userDAL.UpdateUser(user);
        }

        public int DeleteUser(string User_id)
        {
            return userDAL.DeleteUser(User_id);
        }

        public List<User> GetUserList()
        {
            return userDAL.GetUserList();
        }
    }
}
