using BAL.Configurations;
using BAL.Models;
using Common.Utilities;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using ZstdSharp.Unsafe;


namespace BAL.Services
{
   
   public class UserService : AppDbContext, IUserService
        {


        public async Task<bool> DeleteUser(int id)
        {
            try
            {
                OpenContext();
                _sqlCommand.Add_Parameter_WithValue("prm_id", id);

               bool result = await _sqlCommand.Execute_Query("DeleteUser", CommandType.StoredProcedure);
                return result;
                //_sqlCommand.Execute_Query("delete from user_details where user_id = prm_id; ", CommandType.Text);

            }
            catch (Exception ex) 
            {
                throw ex;
            }
            finally
            {
                CloseContext();
            }
        }

        //public async Task<DataTable> GetUsers()
        //{
        //    try
        //    {
        //        OpenContext();
        //        var result = await _sqlCommand.Select_Table("getAllUsers", CommandType.StoredProcedure);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        CloseContext();
        //    }
        //}

        //public async Task<DataTable> GetUsers()
        //{
        //    try
        //    {
        //        OpenContext();
        //        var result = await _sqlCommand.Select_Table("getAllUsers", CommandType.StoredProcedure);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        CloseContext();
        //    }
        //}

        public async Task<bool> LoginUser(UserLoginModel userLogin)
        {
            try
            {
                OpenContext();
                _sqlCommand.Clear_CommandParameter();
                _sqlCommand.Add_Parameter_WithValue("@mobile", userLogin.mobile);
                _sqlCommand.Add_Parameter_WithValue("@password",userLogin.password);

               var result = await _sqlCommand.Select_Table("SELECT mobile, password FROM user_details WHERE mobile = @mobile AND password = @password; " , CommandType.Text);
   
                if(result.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                CloseContext();
            }
        }



        public async Task<bool> RegisterUser(UserModel user)
            {
            try
            { 
                 OpenContext();
                _sqlCommand.Clear_CommandParameter();

                _sqlCommand.Add_Parameter_WithValue("prm_name", user.name);
                _sqlCommand.Add_Parameter_WithValue("prm_mobile", user.mobile);
                _sqlCommand.Add_Parameter_WithValue("prm_email", user.email);
                _sqlCommand.Add_Parameter_WithValue("prm_password", user.password);

                bool result = await _sqlCommand.Execute_Query("registeruser", CommandType.StoredProcedure);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
      
              finally
                {
                CloseContext();
                }
            }

        public async Task<bool> UpdateUser(UserModel user)
        {
            try
            {
                OpenContext();
                _sqlCommand.Add_Parameter_WithValue("prm_id", user.user_id);
                _sqlCommand.Add_Parameter_WithValue("prm_name", user.name);
                _sqlCommand.Add_Parameter_WithValue("prm_mobile", user.mobile);
                _sqlCommand.Add_Parameter_WithValue("prm_email", user.email);

                return await _sqlCommand.Execute_Query("UpdateAllUser", CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseContext();
            }
        }

        //public async Task List<User> GetUsers()
        //{
        //    try
        //    {
        //        OpenContext();

        //    return await _sqlCommand.Select_Table("getAllUsers", CommandType.StoredProcedure);
                 
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        CloseContext();
        //    }
        //}

        public async Task<List<UserModel>> GetUsers()
        {
            try
            {
                OpenContext();

                  DataTable result = await _sqlCommand.Select_Table("getAllUsers", CommandType.StoredProcedure);
                List<UserModel> users = DataTableVsListOfType.ConvertDataTableToList<UserModel>(result);

                List<UserLoginModel> login = new List<UserLoginModel>();
                for(int i = 0; i< result.Rows.Count; i++)
                {
                    UserLoginModel loginData = new UserLoginModel();
                    loginData.mobile = result.Rows[i]["mobile"].ToString();
                    loginData.password = result.Rows[i]["password"].ToString();
                    login.Add(loginData);
                }
                var resultData = login;               
                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseContext();
            }
        }





        

         
    }
    }

