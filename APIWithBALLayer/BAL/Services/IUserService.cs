using BAL.Models;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BAL.Services
{
    public interface IUserService
    {
        Task<bool> RegisterUser(UserModel user);
        //Task<DataTable> GetUsers();
        Task<List<UserModel>> GetUsers();
        Task<bool> DeleteUser(int id);
        Task<bool> UpdateUser(UserModel user);
        Task<bool> LoginUser(UserLoginModel userLogin);
         
    }    
}
