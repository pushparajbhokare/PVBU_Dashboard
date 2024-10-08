using dotnetWebService.Model;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using System;
using TNT.Core.DAL;
using dotnetWebService.Core.DB;

namespace dotnetWebService.RoleManagement.DB
{
    public class RoleDal : HelperDAL
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
        public RoleDal()
        {
            ConnectionString = DBConnection.GetConnectionString();
        }
        #endregion

        #region Database Method
        #region Role Info
        /// <summary>
        /// This method is used to Get Role List
        /// </summary>
        /// <returns></returns>
        public List<Role> GetRoleList()
        {
            List<Role> objList = null;
            objDB = new SqlDatabase(ConnectionString);

            using (SqlConnection objConn = new SqlConnection(ConnectionString))
            {
                using (DbCommand objCmd = objDB.GetStoredProcCommand("GetRoleList"))
                {
                    objConn.Open();
                    objCmd.Connection = objConn;

                    try
                    {
                        using (DataTable dataTable = objDB.ExecuteDataSet(objCmd).Tables[0])
                        {
                            objList = ConvertTo<Role>(dataTable);
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

        ///<summary>
        /// This method is used to Get Role Details By role_id
        ///<summary>
        ///<param name="role_id"></param>
        ///<returns></returns>
        public List<Role> GetRoleDetailsByID(int role_id)
        {
            List<Role> objList = null;
            objDB = new SqlDatabase(ConnectionString);

            using (SqlConnection objConn = new SqlConnection(ConnectionString))
            {
                using (DbCommand objCmd = objDB.GetStoredProcCommand("GetRoleDetailsByID"))
                {
                    objConn.Open();
                    objCmd.Connection = objConn;

                    try
                    {
                        objDB.AddInParameter(objCmd, "@p_role_id", DbType.Int32, role_id);

                        using (DataTable dataTable = objDB.ExecuteDataSet(objCmd).Tables[0])
                        {
                            objList = ConvertTo<Role>(dataTable);
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

        ///<summary>
        /// This method is used to Add Update Role Info
        ///<summary>
        ///<param name="role"></param>
        ///<returns></returns>
        public Role AddUpdateRoleInfo(Role role)
        {
            objDB = new SqlDatabase(ConnectionString);

            using (SqlConnection objConn = new SqlConnection(ConnectionString))
            {
                using (DbCommand objCmd = objDB.GetStoredProcCommand("AddUpdateRoleDetails"))
                {
                    objConn.Open();
                    objCmd.Connection = objConn;

                    objDB.AddParameter(objCmd, "@p_role_id", DbType.Int32, ParameterDirection.InputOutput, "p_role_id", DataRowVersion.Default, role.role_id);
                    objDB.AddInParameter(objCmd, "@p_role_code", DbType.String, role.role_code);
                    objDB.AddInParameter(objCmd, "@p_role_name", DbType.String, role.role_name);
                    objDB.AddInParameter(objCmd, "@p_landing_page", DbType.String, role.landing_page);
                    objDB.AddInParameter(objCmd, "@p_type", DbType.Int32, role.type);
                    objDB.AddOutParameter(objCmd, "@p_status", DbType.Int16, 0);

                    try
                    {
                        objDB.ExecuteNonQuery(objCmd);
                        role.status = Convert.ToInt32(objDB.GetParameterValue(objCmd, "@p_status"));
                        role.role_id = Convert.ToInt32(objDB.GetParameterValue(objCmd, "@p_role_id"));
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    objConn.Close();
                }
            }

            return role;
        }

        ///<summary>
        /// This method is used to Delete Role Info
        ///<summary>
        ///<param name="role"></param>
        ///<returns></returns>
        public int DeleteRoleInfo(Role role)
        {
            int result = 0;
            objDB = new SqlDatabase(ConnectionString);

            using (SqlConnection objConn = new SqlConnection(ConnectionString))
            {
                using (DbCommand objCmd = objDB.GetStoredProcCommand("DeleteRole"))
                {
                    objConn.Open();
                    objCmd.Connection = objConn;

                    objDB.AddInParameter(objCmd, "@p_role_id", DbType.Int32, role.role_id);
                    objDB.AddOutParameter(objCmd, "@p_status", DbType.Int16, 0);

                    try
                    {
                        objDB.ExecuteNonQuery(objCmd);
                        result = Convert.ToInt32(objDB.GetParameterValue(objCmd, "@p_status"));
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    objConn.Close();
                }
            }

            return result;
        }
        #endregion

        #region Role Permission Info  
        ///<summary>
        /// This method is used to Save Role Permission Info
        ///<summary>
        ///<param name="rolePermission"></param>
        ///<returns></returns>
        public int SaveRolePermissionInfo(RolePermission rolePermission)
        {
            int result = 0;
            objDB = new SqlDatabase(ConnectionString);

            using (DbConnection objConn = objDB.CreateConnection())
            {
                using (DbCommand objCmd = objDB.GetStoredProcCommand("SaveRolePermissionInfo"))
                {
                    objConn.Open();
                    objCmd.Connection = objConn;

                    objDB.AddInParameter(objCmd, "@p_rp_id", DbType.Int32, rolePermission.rp_id);
                    objDB.AddInParameter(objCmd, "@p_role_id", DbType.Int32, rolePermission.role_id);
                    objDB.AddInParameter(objCmd, "@p_pr_id", DbType.Int32, rolePermission.pr_id);
                    objDB.AddInParameter(objCmd, "@p_pr_granted", DbType.Boolean, rolePermission.pr_granted);
                    objDB.AddInParameter(objCmd, "@p_type", DbType.Int32, rolePermission.type);
                    objDB.AddOutParameter(objCmd, "@p_status", DbType.Int16, 0);

                    try
                        {
                        objDB.ExecuteNonQuery(objCmd);
                        result = Convert.ToInt32(objDB.GetParameterValue(objCmd, "@p_status"));
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    objConn.Close();
                }
            }

            return result;
        }
        #endregion
        #endregion

       
    }
}
