using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.PagesModel.AddEditUser;
using BPIWebApplication.Shared.PagesModel.AddEditProject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BPIWebApplication.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagementController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _conString;

        public ManagementController(IConfiguration configuration)
        {
            _configuration = configuration;
            _conString = _configuration.GetValue<string>("ConnectionStrings:ProcedureConnection");
        }

        // decode encode

        private static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        // get DA

        internal DataTable getAllBisnisUnitData()
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
                    command.CommandText = "[getAllBisnisUnitData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(dt);

                }
                catch (SqlException ex)
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

        internal DataTable getAllDepartmentData()
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
                    command.CommandText = "[getAllDepartmentData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(dt);

                }
                catch (SqlException ex)
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

        internal DataTable getAllUserAdminData()
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
                    command.CommandText = "[getAllUserAdminData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(dt);

                }
                catch (SqlException ex)
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

        internal DataTable getAllProjectData()
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
                    command.CommandText = "[getAllProjectData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(dt);

                }
                catch (SqlException ex)
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

        // create DA

        internal void createNewDepartmentData(QueryModel<Department> data)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[createDepartmentData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@DepartmentID", data.Data.DepartmentID.ToUpper());
                    command.Parameters.AddWithValue("@DepartmentName", data.Data.DepartmentName.ToUpper());
                    command.Parameters.AddWithValue("@DepartmentLabel", data.Data.DepartmentLabel.ToUpper());
                    command.Parameters.AddWithValue("@BisnisUnitID", data.Data.BisnisUnitID);
                    command.Parameters.AddWithValue("@AuditUser", data.userEmail);
                    command.Parameters.AddWithValue("@AuditAction", data.userAction);
                    command.Parameters.AddWithValue("@AuditActionDate", data.userActionDate);

                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        internal void createNewUserData(QueryModel<UserAdmin> data)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[createUserAdminData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@UserID", data.Data.UserID);
                    command.Parameters.AddWithValue("@UserEmail", data.Data.UserEmail);
                    command.Parameters.AddWithValue("@UserRole", data.Data.UserRole);
                    command.Parameters.AddWithValue("@AuditUser", data.userEmail);
                    command.Parameters.AddWithValue("@AuditAction", data.userAction);
                    command.Parameters.AddWithValue("@AuditActionDate", data.userActionDate);

                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        internal void createProjectData(QueryModel<Project> data)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[createProjectData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ProjectName", data.Data.ProjectName);
                    command.Parameters.AddWithValue("@ProjectStatus", data.Data.ProjectStatus);
                    command.Parameters.AddWithValue("@ProjectNote", data.Data.ProjectNote);
                    command.Parameters.AddWithValue("@AuditUser", data.userEmail);
                    command.Parameters.AddWithValue("@AuditAction", data.userAction);
                    command.Parameters.AddWithValue("@AuditActionDate", data.userActionDate);

                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        // edit DA

        internal void editDepartmentData(QueryModel<Department> data)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[editDepartmentData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@DepartmentID", data.Data.DepartmentID);
                    command.Parameters.AddWithValue("@DepartmentName", data.Data.DepartmentName);
                    command.Parameters.AddWithValue("@DepartmentLabel", data.Data.DepartmentLabel);
                    command.Parameters.AddWithValue("@BisnisUnitID", data.Data.BisnisUnitID);
                    command.Parameters.AddWithValue("@AuditUser", data.userEmail);
                    command.Parameters.AddWithValue("@AuditAction", data.userAction);
                    command.Parameters.AddWithValue("@AuditActionDate", data.userActionDate);

                    command.ExecuteNonQuery();

                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }

        }

        internal void editUserData(QueryModel<UserAdmin> data)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[editUserAdminData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@UserID", data.Data.UserID);
                    command.Parameters.AddWithValue("@UserEmail", data.Data.UserEmail);
                    command.Parameters.AddWithValue("@UserRole", data.Data.UserRole);
                    command.Parameters.AddWithValue("@AuditUser", data.userEmail);
                    command.Parameters.AddWithValue("@AuditAction", data.userAction);
                    command.Parameters.AddWithValue("@AuditActionDate", data.userActionDate);

                    command.ExecuteNonQuery();

                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }

        }

        internal void editProjectData(QueryModel<Project> data)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[editProjectData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ProjectName", data.Data.ProjectName);
                    command.Parameters.AddWithValue("@ProjectStatus", data.Data.ProjectStatus);
                    command.Parameters.AddWithValue("@ProjectNote", data.Data.ProjectNote);
                    command.Parameters.AddWithValue("@AuditUser", data.userEmail);
                    command.Parameters.AddWithValue("@AuditAction", data.userAction);
                    command.Parameters.AddWithValue("@AuditActionDate", data.userActionDate);

                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        // is check exist DA

        internal bool isDepartmentDataExist(string deptID)
        {
            var conBool = false;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[isDepartmentPresent]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@DepartmentID", deptID);
                    var data = command.ExecuteScalar();
                    conBool = Convert.ToBoolean(data);

                    /*
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    */
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return conBool;
        }

        internal bool isUserAdminDataExist(string userEmail)
        {
            var conBool = false;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[isUserAdminPresent]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@UserEmail", userEmail);
                    var data = command.ExecuteScalar();
                    conBool = Convert.ToBoolean(data);

                    /*
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    */
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return conBool;
        }

        internal bool isProjectDataExist(string projectName)
        {
            var conBool = false;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[isProjectPresent]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ProjectName", projectName);
                    var data = command.ExecuteScalar();
                    conBool = Convert.ToBoolean(data);

                    /*
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    */
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return conBool;
        }

        // http get

        [HttpGet("getAllBisnisUnitData")]
        public async Task<IActionResult> getAllBisnisUnitDataTable()
        {
            ResultModel<List<BisnisUnit>> res = new ResultModel<List<BisnisUnit>>();
            List<BisnisUnit> bisnisUnit = new List<BisnisUnit>();
            DataTable dtBisnisUnit = new DataTable("BisnisUnitData");
            IActionResult actionResult = null;

            try
            {
                dtBisnisUnit = getAllBisnisUnitData();

                if (dtBisnisUnit.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtBisnisUnit.Rows)
                    {
                        BisnisUnit temp = new BisnisUnit();

                        temp.BisnisUnitID = dt["bisnisUnitID"].ToString();
                        temp.BisnisUnitName = dt["bisnisUnitName"].ToString();
                        temp.BisnisUnitLabel = dt["bisnisUnitLabel"].ToString();

                        bisnisUnit.Add(temp);
                    }

                    res.Data = bisnisUnit;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        [HttpGet("getAllDepartmentData")]
        public async Task<IActionResult> getAllDepartmentDataTable()
        {
            ResultModel<List<Department>> res = new ResultModel<List<Department>>();
            List<Department> department = new List<Department>();
            DataTable dtDepartment = new DataTable("DepartmentData");
            IActionResult actionResult = null;

            try
            {
                dtDepartment = getAllDepartmentData();

                if (dtDepartment.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtDepartment.Rows)
                    {
                        Department temp = new Department();

                        temp.DepartmentID = dt["departmentID"].ToString();
                        temp.DepartmentName = dt["departmentName"].ToString();
                        temp.DepartmentLabel = dt["departmentLabel"].ToString();
                        temp.BisnisUnitID = dt["bisnisUnitID"].ToString();

                        department.Add(temp);
                    }

                    res.Data = department;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        [HttpGet("getAllUserAdminData")]
        public async Task<IActionResult> getAllUserAdminDataTable()
        {
            ResultModel<List<UserAdmin>> res = new ResultModel<List<UserAdmin>>();
            List<UserAdmin> userAdmin = new List<UserAdmin>();
            DataTable dtUserAdmin = new DataTable("UserAdminData");
            IActionResult actionResult = null;

            try
            {
                dtUserAdmin = getAllUserAdminData();

                if (dtUserAdmin.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtUserAdmin.Rows)
                    {
                        UserAdmin temp = new UserAdmin();

                        temp.UserID = dt["userID"].ToString();
                        temp.UserEmail = dt["userEmail"].ToString();
                        temp.UserRole = dt["userRole"].ToString();

                        userAdmin.Add(temp);
                    }

                    res.Data = userAdmin;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        [HttpGet("getAllProjectData")]
        public async Task<IActionResult> getAllProjectDataTable()
        {
            ResultModel<List<Project>> res = new ResultModel<List<Project>>();
            List<Project> project = new List<Project>();
            DataTable dtProject = new DataTable("ProjectData");
            IActionResult actionResult = null;

            try
            {
                dtProject = getAllProjectData();

                if (dtProject.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtProject.Rows)
                    {
                        Project temp = new Project();

                        temp.ProjectName = dt["projectName"].ToString();
                        temp.ProjectStatus = dt["projectStatus"].ToString();
                        temp.ProjectNote = dt["projectNote"].ToString();

                        project.Add(temp);
                    }

                    res.Data = project;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        // http get (check exist)

        [HttpGet("isDepartmentDataPresent/{DeptID}")]
        public async Task<IActionResult> isDepartmentDataPresentInTable(string DeptID)
        {
            ResultModel<string> res = new ResultModel<string>();
            bool present = false;
            IActionResult actionResult = null;

            string temp = Base64Decode(DeptID);

            try
            {
                present = isDepartmentDataExist(temp);

                if (present)
                {
                    res.Data = DeptID;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = DeptID;
                    res.isSuccess = false;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Success Fetch - Procedure Not Found";

                    actionResult = Ok(res);
                    //throw new Exception($"{}")
                }

            }
            catch (Exception ex)
            {
                res.Data = "";
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        [HttpGet("isUserAdminPresent/{userEmail}")]
        public async Task<IActionResult> isUserAdminDataPresentInTable(string userEmail)
        {
            ResultModel<string> res = new ResultModel<string>();
            bool present = false;
            IActionResult actionResult = null;

            string temp = Base64Decode(userEmail);

            try
            {
                present = isUserAdminDataExist(temp);

                if (present)
                {
                    res.Data = userEmail;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = userEmail;
                    res.isSuccess = false;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Success Fetch - Procedure Not Found";

                    actionResult = Ok(res);
                    //throw new Exception($"{}")
                }

            }
            catch (Exception ex)
            {
                res.Data = "";
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        [HttpGet("isProjectPresent/{projectNo}")]
        public async Task<IActionResult> isProjectDataPresentInTable(string projectNo)
        {
            ResultModel<string> res = new ResultModel<string>();
            bool present = false;
            IActionResult actionResult = null;

            string temp = Base64Decode(projectNo);

            try
            {
                present = isProjectDataExist(temp);

                if (present)
                {
                    res.Data = projectNo;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = projectNo;
                    res.isSuccess = false;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Success Fetch - Procedure Not Found";

                    actionResult = Ok(res);
                    //throw new Exception($"{}")
                }

            }
            catch (Exception ex)
            {
                res.Data = "";
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        // http post (create)

        [HttpPost("createNewDepartmentData")]
        public async Task<IActionResult> createNewDepartmentDataTable(QueryModel<Department> data)
        {
            ResultModel<QueryModel<Department>> res = new ResultModel<QueryModel<Department>>();
            IActionResult actionResult = null;

            // create procedure data to db
            try
            {
                createNewDepartmentData(data);

                res.Data = data;
                res.isSuccess = true;
                res.ErrorCode = "00";
                res.ErrorMessage = "";

                actionResult = Ok(res);

            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        [HttpPost("createNewUserAdminData")]
        public async Task<IActionResult> createNewUserAdminDataTable(QueryModel<UserAdmin> data)
        {
            ResultModel<QueryModel<UserAdmin>> res = new ResultModel<QueryModel<UserAdmin>>();
            IActionResult actionResult = null;

            // create procedure data to db
            try
            {
                createNewUserData(data);

                res.Data = data;
                res.isSuccess = true;
                res.ErrorCode = "00";
                res.ErrorMessage = "";

                actionResult = Ok(res);

            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        [HttpPost("createNewProjectData")]
        public async Task<IActionResult> createProjectDataTable(QueryModel<Project> data)
        {
            ResultModel<QueryModel<Project>> res = new ResultModel<QueryModel<Project>>();
            IActionResult actionResult = null;

            // create procedure data to db
            try
            {
                createProjectData(data);

                res.Data = data;
                res.isSuccess = true;
                res.ErrorCode = "00";
                res.ErrorMessage = "";

                actionResult = Ok(res);

            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        // http post (edit)

        [HttpPost("editDepartmentData")]
        public async Task<IActionResult> editDepartmentDataTable(QueryModel<Department> data)
        {
            ResultModel<QueryModel<Department>> res = new ResultModel<QueryModel<Department>>();
            IActionResult actionResult = null;

            // create procedure data to db
            try
            {
                editDepartmentData(data);

                res.Data = data;
                res.isSuccess = true;
                res.ErrorCode = "00";
                res.ErrorMessage = "";

                actionResult = Ok(res);

            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        [HttpPost("editUserAdminData")]
        public async Task<IActionResult> editUserAdminDataTable(QueryModel<UserAdmin> data)
        {
            ResultModel<QueryModel<UserAdmin>> res = new ResultModel<QueryModel<UserAdmin>>();
            IActionResult actionResult = null;

            // create procedure data to db
            try
            {
                editUserData(data);

                res.Data = data;
                res.isSuccess = true;
                res.ErrorCode = "00";
                res.ErrorMessage = "";

                actionResult = Ok(res);

            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        [HttpPost("editProjectData")]
        public async Task<IActionResult> editProjectDataTable(QueryModel<Project> data)
        {
            ResultModel<QueryModel<Project>> res = new ResultModel<QueryModel<Project>>();
            IActionResult actionResult = null;

            // create procedure data to db
            try
            {
                editProjectData(data);

                res.Data = data;
                res.isSuccess = true;
                res.ErrorCode = "00";
                res.ErrorMessage = "";

                actionResult = Ok(res);

            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

    }
}
