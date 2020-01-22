using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using LoginServer.Models;
using System.Data.SqlClient;
using System.Diagnostics;
using Newtonsoft.Json;


namespace LoginServer.Controllers
{
    [EnableCorsAttribute("*", "*", "*")]
    public class LoginController : ApiController
    {
        public string connectionString = @"Data Source=DESKTOP-VAB8R5D;Initial Catalog=login;Integrated Security=True";

        [HttpPost]
        public string HandleLogindata([FromBody]Account account)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_Account_Login", sqlConnection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.Add("@Username", System.Data.SqlDbType.VarChar).Value = account.Username;
                cmd.Parameters.Add("@Password", System.Data.SqlDbType.VarChar).Value = account.Password;
                sqlConnection.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if ((bool)dr["IsExist"])
                    {
                        return "Đăng nhập thành công";
                    }
                }
                sqlConnection.Close();
            }
            
            return "Đăng nhập thất bại";

        }
    }
}
