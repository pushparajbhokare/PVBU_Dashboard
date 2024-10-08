using dotnetWebService.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
using TNT.Core.DAL;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using dotnetWebService.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
using TNT.Core.DAL;
using Microsoft.AspNetCore.Http;


namespace dotnetWebService.Core.DB
{
    public class RolesDAL : HelperDAL
    {
        #region Variable
        ///<summary>
        ///Specify the Database variable
        ///<summary>
        Database objDB;

        ///<summary>
        ///Specify the static variable
        ///<summary>
        static string ConnectionString;
        #endregion

        #region Constructor
        ///<summary>
        ///This constructor is used to get the connectionstring from the config file
        ///<summary>
        public RolesDAL()
        {
            ConnectionString = DBConnection.GetConnectionString();

        }
        #endregion

        #region Database Method


        ///<summary>
        /// This method is used to Get Role Permisions Details By Role_id
        ///<summary>
        ///<param name="role_id"></param>
        ///<returns></returns>
       
        public List<RolePermission> GetRolePermissionList(int role_id)
        {
            
            List<RolePermission> objList = null;
            objDB = new SqlDatabase(ConnectionString);

            using (SqlConnection objConn = new SqlConnection(ConnectionString))
            {
                using (DbCommand objCmd = objDB.GetStoredProcCommand("GetRolePermissionList"))
                {
                    objConn.Open();
                    objCmd.Connection = objConn;

                    try
                    {
                        objDB.AddInParameter(objCmd, "@p_role_id", DbType.Int32, role_id);

                        using (DataTable dataTable = objDB.ExecuteDataSet(objCmd).Tables[0])
                        {
                            objList = ConvertTo<RolePermission>(dataTable);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    objConn.Close();
                }
            }

            return objList;
        }
              
        #endregion
    }
}
