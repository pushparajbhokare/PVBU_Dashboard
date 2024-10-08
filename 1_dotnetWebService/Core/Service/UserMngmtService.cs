using dotnetWebService.Core.DB;
using dotnetWebService.Model;
using System.Linq;

namespace dotnetWebService.Core.Service
{
    public class UserMngmtService
    {
        //private readonly UserMngmtDB userMngmtDB;
        private readonly UsersDAL userMngmtDAL;
        public UserMngmtService()
        {
            userMngmtDAL = new UsersDAL();
        }

        public User GetUserDetailsByUsername(string username)
        {
            return userMngmtDAL.GetUserDetailsByUsername(username).FirstOrDefault();
        }
    }
}
