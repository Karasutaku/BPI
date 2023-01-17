using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using BPIWebApplication.Shared.MainModel.Login;

namespace BPIWebApplication.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _configuration;
        private readonly string _conString;

        public LoginController(HttpClient http, IConfiguration configuration)
        {
            _http = http;
            _configuration = configuration;
            _conString = _configuration.GetValue<string>("ConnectionStrings:ProcedureConnection");
        }

        // check exist DA

        internal DataTable getUserAdminbyEmail(string email)
        {
            DataTable dt = new DataTable("Data");

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[getUserAdminbyUserEmail]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@userEmail", email);

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(dt);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return dt;
        }

        // http post

        [HttpPost("Authenticate")]
        public async Task<IActionResult> checkLoginUser (LoginUser data)
        {
            LoginResponseBody res = new LoginResponseBody();

            _http.BaseAddress = new Uri("http://sandbox.mitra10.com//");
            var result = await _http.PostAsJsonAsync<LoginUser>("api/login", data);

            var jsonBody = await result.Content.ReadFromJsonAsync<LoginResponseBody>();

            if (jsonBody.statusCode != "00")
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("IsAdmin")]
        public async Task<IActionResult> isUserAdmin (LoginUser data)
        {
            //ResultModel<ActiveUser<LoginUser>> res = new ResultModel<ActiveUser<LoginUser>>();
            //ActiveUser<LoginUser> activeUser = new ActiveUser<LoginUser>();
            //DataTable dtUser = new DataTable("DataUser");
            //IActionResult actionResult = null;

            //try
            //{
            //    dtUser = getUserAdminbyEmail(data.userName);

            //    if (dtUser.Rows.Count > 0)
            //    {
            //        ActiveUser<LoginUser> temp = new ActiveUser<LoginUser>();

            //        // passing data admin user or guest user data
            //        foreach (DataRow dt in dtUser.Rows)
            //        {
            //            activeUser.Name = dt["userID"].ToString();
            //            activeUser.UserLogin = new LoginUser();
            //            activeUser.UserLogin.userName = dt["userEmail"].ToString();
            //            activeUser.UserLogin.password = "";
            //            activeUser.role = dt["userRole"].ToString();
            //        }

            //        res.Data = activeUser;
            //        res.isSuccess = true;
            //        res.ErrorCode = "00";
            //        res.ErrorMessage = "OK";

            //        actionResult = Ok(res);
            //    } else
            //    {
            //        // passing normal user data
            //        activeUser.Name = data.userName;
            //        activeUser.UserLogin = new LoginUser();
            //        activeUser.UserLogin.userName = data.userName;
            //        activeUser.UserLogin.password = "";
            //        activeUser.role = "user";

            //        res.Data = activeUser;
            //        res.isSuccess = true;
            //        res.ErrorCode = "00";
            //        res.ErrorMessage = "OK";

            //        actionResult = Ok(res);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    res.Data = activeUser;
            //    res.isSuccess = false;
            //    res.ErrorCode = "99";
            //    res.ErrorMessage = ex.Message;

            //    actionResult = BadRequest(res);
            //}

            //return actionResult;
            return null;
        }


    }
}
