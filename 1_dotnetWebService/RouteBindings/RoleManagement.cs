using dotnetWebService.Model;
using System.Collections.Generic;
using System;
using dotnetWebService.RoleManagement.Service;
using System.Linq;
using dotnetWebService.Core.Service;
using dotnetWebService.Core.DB;

namespace dotnetWebService.RouteBindings
{
    public class RoleManagement
    {
        
        public static List<Role> GetRoleList()
        {
            List<Role> objList = null;

            try
            {
                objList = new RoleService().GetRoleList();
            }
            catch (Exception ex)
            {
                FileWriter.WriteToFile(ex.ToString());
                Console.WriteLine(ex.ToString());
            }
            return objList;
        }

       
        public static Role GetRoleDetailsByID(int role_id)
        {
            Role objRoleDetails = null;
            RoleService roleService = new RoleService();

            try
            {
                objRoleDetails = roleService.GetRoleDetailsByID(role_id).FirstOrDefault();
                objRoleDetails.role_permissions = roleService.GetRolePermissionList(role_id);

            }
            catch (Exception ex)
            {
                FileWriter.WriteToFile(ex.ToString());
                Console.WriteLine(ex.ToString());
            }
            return objRoleDetails;
        }

        public static Role AddUpdateRoleInfo(Role role)
        {
            try
            {
                role = new RoleService().AddUpdateRoleInfo(role);
            }
            catch (Exception ex)
            {
                return role;
            }
            return role;
        }

      
        public static int DeleteRoleInfo(Role Role)
        {
            int result = 0;

            try
            {
                result = new RoleService().DeleteRoleInfo(Role);
            }
            catch (Exception ex)
            {
                return 0;

            }
            return result;
        }

        #region Role Permission Info
        ///<summary>
        /// This method is used to get the RolePermission list
        ///</summary>
        ///<returns></returns>
        public static List<RolePermission> GetRolePermissionList(int role_id)
        {
            List<RolePermission> objList = null;

            try
            {
                objList = new RolesService().GetRolePermissionList(role_id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objList;
        }

        ///<summary>
        /// This method is used to Save Role Permission Info
        ///</summary>
        ///<param name="rolePermission"></param>
        ///<returns></returns>
        public static int SaveRolePermissionInfo(RolePermission rolePermission)
        {
            int result = 0;

            try
            {
                result = new RoleService().SaveRolePermissionInfo(rolePermission);
            }
            catch (Exception ex)
            {
                return 0;
            }
            return result;
        }
        #endregion

        public static int UpdateUser(User user)
        {
            int result = 0;

            try
            {
                result = new UserService().UpdateUser(user);
            }
            catch (Exception ex)
            {
                return 0;
            }
            return result;
        }

        public static List<User> GetUserList()
        {
            List<User> result = null;

            try
            {
                result = new UserService().GetUserList();
            }
            catch (Exception ex)
            {
                return result;
            }
            return result;
        }

        public static int DeleteUser(string User_id)
        {
            int result = 0;
            try
            {
             result = new UserService().DeleteUser(User_id);
            }
            catch (Exception ex)
            {
                return result;
            }
            return result;
        }
    }
}
