using dotnetWebService.Core.DB;
using dotnetWebService.Model;
using System.Collections.Generic;

namespace dotnetWebService.Core.Service
{
    public  class RolesService
    {
        private readonly  PermissionsDAL rolesDAL;
        public RolesService()
        {
            rolesDAL = new PermissionsDAL();
        }
        public List<RolePermission> GetRolePermissionList(int role_id)
        {
            return rolesDAL.GetRolePermissionList(role_id);
        }
    }
}
