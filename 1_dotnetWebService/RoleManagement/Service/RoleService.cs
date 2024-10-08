using dotnetWebService.Core.DB;
using dotnetWebService.Model;
using dotnetWebService.RoleManagement.DB;
using System.Collections.Generic;
using dotnetWebService.RoleManagement.DB;
using dotnetWebService.Model;

namespace dotnetWebService.RoleManagement.Service
{
    public class RoleService
    {
        private readonly RoleDal roleDAL;
        private readonly PermissionsDAL permissionsDAL;
        public RoleService()
        {
            roleDAL = new RoleDal();
            permissionsDAL = new PermissionsDAL();
        }

        public List<Role> GetRoleList()
        {
            return roleDAL.GetRoleList();
        }
        public List<Role> GetRoleDetailsByID(int role_id)
        {
            return roleDAL.GetRoleDetailsByID(role_id);
        }

        public List<RolePermission> GetRolePermissionList(int role_id)
        {
            return permissionsDAL.GetRolePermissionList(role_id);
        }

        public Role AddUpdateRoleInfo(Role role)
        {
            return roleDAL.AddUpdateRoleInfo(role);
        }

        public int DeleteRoleInfo(Role role)
        {
            return roleDAL.DeleteRoleInfo(role);
        }

        public int SaveRolePermissionInfo(RolePermission rolePermission)
        {
            return roleDAL.SaveRolePermissionInfo(rolePermission);
        }

    }
}
