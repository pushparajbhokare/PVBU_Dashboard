using App.Configurations;
using dotnetWebService.Model;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using TNT.Core.DAL;


namespace dotnetWebService.Core.DB
{
    public class UsersDAL : HelperDAL
    {
        #region Variable
        ///<summary>
        ///Specify the Database variable
        ///<summary>
        Database objDB;
        static string ConnectionString;


        //runTimeConfiguration configname = new runTimeConfiguration();
        ///<summary>
        ///Specify the static variable
        ///<summary>


        #endregion

        #region Constructor
        ///<summary>
        ///This constructor is used to get the connectionstring from the config file
        ///<summary>
        public UsersDAL()
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
        public List<User> GetUserDetailsByUsername(string username)
        {
            List<User> objUser = null;
            objDB = new SqlDatabase(ConnectionString);

            using (SqlConnection objConn = new SqlConnection(ConnectionString))
            {
                using (DbCommand objCmd = objDB.GetStoredProcCommand("GetUserDetailsByUserName"))
                {
                    objConn.Open();
                    objCmd.Connection = objConn;

                    try
                    {
                        objDB.AddInParameter(objCmd, "@p_username", DbType.String, username);

                        using (DataTable dataTable = objDB.ExecuteDataSet(objCmd).Tables[0])
                        {
                            objUser = ConvertTo<User>(dataTable);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    objConn.Close();
                }
            }

            return objUser;
        }

        #endregion


        #region Add Or Delete User
        public int AddUser(User user)
        {
            int result = 0;
            objDB = new SqlDatabase(ConnectionString);

            using (DbConnection objConn = objDB.CreateConnection())
            {
                using (DbCommand objCmd = objDB.GetStoredProcCommand("AddUser"))
                {
                    objConn.Open();
                    objCmd.Connection = objConn;

                    objDB.AddInParameter(objCmd, "@p_username", DbType.String, user.username);
                    objDB.AddInParameter(objCmd, "@p_full_name", DbType.String, user.full_name);
                    objDB.AddInParameter(objCmd, "@p_email", DbType.String, user.email);
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

        public int UpdateUser(User user)
        {
            int result = 0;
            objDB = new SqlDatabase(ConnectionString);

            using (DbConnection objConn = objDB.CreateConnection())
            {
                using (DbCommand objCmd = objDB.GetStoredProcCommand("UpdateUser"))
                {
                    objConn.Open();
                    objCmd.Connection = objConn;
                    objDB.AddInParameter(objCmd, "@p_id", DbType.Int32, user.id);
                    objDB.AddInParameter(objCmd, "@p_role_id", DbType.Int32, user.role_id);
                    objDB.AddInParameter(objCmd, "@p_plant_location", DbType.String, user.plant_location);
                    objDB.AddInParameter(objCmd, "@p_area", DbType.String, user.area);
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

        public int DeleteUser(string User_id)
        {
            int result = 0;
            objDB = new SqlDatabase(ConnectionString);

            using (DbConnection objConn = objDB.CreateConnection())
            {
                using (DbCommand objCmd = objDB.GetSqlStringCommand("DELETE FROM USERS WHERE USERNAME = @p_User_id"))
                {
                    objConn.Open();
                    objCmd.Connection = objConn;

                    objDB.AddInParameter(objCmd, "@p_User_id", DbType.String, User_id);


                    try
                    {

                        result = objDB.ExecuteNonQuery(objCmd);
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

        public List<User> GetUserList()
        {
            List<User> objList = null;
            objDB = new SqlDatabase(ConnectionString);
            // string Query = "SELECT US.ID AS ID, US.username, US.plant_location, US.area, RL.role_name, RL.role_id FROM users AS US INNER JOIN roles RL ON US.role_id = RL.role_id ORDER BY id";
            string Query = "SELECT US.ID AS ID, US.username, US.plant_location, US.area, RL.role_name, RL.role_id , US.full_name FROM users AS US INNER JOIN roles RL ON US.role_id = RL.role_id ORDER BY id";
            using (SqlConnection objConn = new SqlConnection(ConnectionString))
            {
                using (DbCommand objCmd = objDB.GetSqlStringCommand(Query))
                {
                    objConn.Open();
                    objCmd.Connection = objConn;

                    try
                    {
                        using (DataTable dataTable = objDB.ExecuteDataSet(objCmd).Tables[0])
                        {
                            objList = ConvertTo<User>(dataTable);
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
