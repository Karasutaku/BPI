using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using BPIDA.Models.DbModel;
using BPIDA.Models.MainModel.Procedure;
using BPIDA.Models.MainModel.Procedure.Filter;
using BPIDA.Models.MainModel.Procedure.Report;
using BPIDA.Models.MainModel.Company;
using BPIDA.Models.PagesModel.AddEditProject;
using BPIDA.Models.MainModel;
using BPIDA.Models.MainModel.PettyCash;
using System.Security.Principal;
using System;
using BPIDA.Models.MainModel.Mailing;
using Microsoft.IdentityModel.Tokens;
using BPIDA.Models.MainModel.CashierLogbook;
using System.Reflection;
using BPIDA.Models.MainModel.Standarizations;

namespace BPIDA.Controllers
{
    /// <summary>
    /// BPI BASE CONTROLLER
    /// </summary>


    [Route("api/DA/BPIBase")]
    [ApiController]
    public class BPIBaseController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _conString;

        public BPIBaseController(IConfiguration configuration)
        {
            _configuration = configuration;
            _conString = _configuration.GetValue<string>("ConnectionStrings:Bpi");
        }

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

        internal DataTable getAllBisnisUnitData(string loc)
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
                    command.Parameters.AddWithValue("@LocationID", loc);

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

        internal DataTable getAllDepartmentData(string loc)
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
                    command.Parameters.AddWithValue("@LocationID", loc);

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

        [HttpGet("getAllBisnisUnitData/{param}")]
        public async Task<IActionResult> getAllBisnisUnitDataTable(string param)
        {
            ResultModel<List<BisnisUnit>> res = new ResultModel<List<BisnisUnit>>();
            List<BisnisUnit> bisnisUnit = new List<BisnisUnit>();
            DataTable dtBisnisUnit = new DataTable("BisnisUnitData");
            IActionResult actionResult = null;

            try
            {
                dtBisnisUnit = getAllBisnisUnitData(Base64Decode(param));

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

        [HttpGet("getAllDepartmentData/{param}")]
        public async Task<IActionResult> getAllDepartmentDataTable(string param)
        {
            ResultModel<List<Department>> res = new ResultModel<List<Department>>();
            List<Department> department = new List<Department>();
            DataTable dtDepartment = new DataTable("DepartmentData");
            IActionResult actionResult = null;

            try
            {
                dtDepartment = getAllDepartmentData(Base64Decode(param));

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
                else
                {
                    res.Data = null;
                    res.isSuccess = false;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "No Data";

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

    /// <summary>
    /// SOP PROCEDURE CONTROLLER
    /// </summary>

    [Route("api/DA/Procedure")]
    [ApiController]
    public class ProcedureController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _conString;
        private readonly string _uploadPath, _archivePath;
        private readonly int _rowPerPage;
        private readonly long _maxFileSize;

        public ProcedureController(IConfiguration configuration)
        {
            _configuration = configuration;
            _conString = _configuration.GetValue<string>("ConnectionStrings:Bpi");
            _uploadPath = _configuration.GetValue<string>("File:Procedure:UploadPath");
            _archivePath = _configuration.GetValue<string>("File:Procedure:ArchivePath");
            _rowPerPage = _configuration.GetValue<int>("Paging:Procedure:RowPerPage");
            _maxFileSize = _configuration.GetValue<long>("File:Procedure:MaxUpload");
        }

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

        internal bool isProcedureDataPresent(string ProcNo)
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
                    command.CommandText = "[isProcedurePresent]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ProcedureNo", ProcNo);
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

        internal bool isDepartmentProcedureDataPresent(string procNo, string deptNo)
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
                    command.CommandText = "[isDepartmentProcedurePresent]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ProcedureNo", procNo);
                    command.Parameters.AddWithValue("@DepartmentID", deptNo);
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

        // create DA

        internal void createProcedureData(QueryModel<Procedure> data)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[createProcedureData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ProcedureNo", data.Data.ProcedureNo);
                    command.Parameters.AddWithValue("@ProcedureName", data.Data.ProcedureName);
                    command.Parameters.AddWithValue("@ProcedureDate", data.Data.ProcedureDate);
                    command.Parameters.AddWithValue("@ProcedureWiPath", data.Data.ProcedureWi);
                    command.Parameters.AddWithValue("@ProcedureSopPath", data.Data.ProcedureSop);
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

        internal void createDepartmentProcedureData(QueryModel<DepartmentProcedure> data)
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
                    command.CommandText = "[createDepartmentProcedureData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ProcedureNo", data.Data.ProcedureNo);
                    command.Parameters.AddWithValue("@DepartmentID", data.Data.DepartmentID);
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

        internal void createHistoryAccessData(QueryModel<HistoryAccess> data)
        {

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[createHistoryAccessData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ProcedureNo", data.Data.ProcedureNo);
                    command.Parameters.AddWithValue("@ProcedureName", data.Data.ProcedureName);
                    command.Parameters.AddWithValue("@userEmail", data.Data.UserEmail);
                    command.Parameters.AddWithValue("@historyAccessDate", data.Data.HistoryAccessDate);
                    command.Parameters.AddWithValue("@auditUser", data.userEmail);
                    command.Parameters.AddWithValue("@auditAction", data.userAction);
                    command.Parameters.AddWithValue("@auditActionDate", data.userActionDate);

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

        // get DA

        internal DataTable getAllProcedureData()
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
                    command.CommandText = "[getAllProcedureData]";
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

        internal DataTable getDepartmentProcedureData()
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
                    command.CommandText = "[getAllDepartmentProcedureData]";
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

        internal DataTable getDepartmentProcedureDatawithFilter(DashboardFilter data)
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
                    command.CommandText = "[getDepartmentProcedureDatabyFilterwithPaging]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", data.locationId);
                    command.Parameters.AddWithValue("@PageNo", data.pageNo);
                    command.Parameters.AddWithValue("@RowPerPage", data.rowPerPage);
                    command.Parameters.AddWithValue("@FilterNo", data.filterNo);
                    command.Parameters.AddWithValue("@FilterName", data.filterName);
                    command.Parameters.AddWithValue("@FilterDept", data.filterDept);
                    command.Parameters.AddWithValue("@FilterBU", data.filterBU);

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

        internal DataTable getDepartmentProcedureDatawithPaging(string loc, int pageNo, int rowPerPage)
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
                    command.CommandText = "[getAllDepartmentProcedureDatawithPaging]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", loc);
                    command.Parameters.AddWithValue("@PageNo", pageNo);
                    command.Parameters.AddWithValue("@RowPerPage", rowPerPage);

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

        internal DataTable getAllHistoryAccessDatawithPaging(int pageNo, int rowPerPage)
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
                    command.CommandText = "[getHistoryAccessDatawithPaging]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@PageNo", pageNo);
                    command.Parameters.AddWithValue("@RowPerPage", rowPerPage);

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

        internal DataTable getAllHistoryAccessDatabyFilterwithPaging(AccessHistoryFilter data)
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
                    command.CommandText = "[getHistoryAccessDatabyFilterwithPaging]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@PageNo", data.pageNo);
                    command.Parameters.AddWithValue("@RowPerPage", data.rowPerPage);
                    command.Parameters.AddWithValue("@FilterType", data.filterType);
                    command.Parameters.AddWithValue("@FilterDetails", data.filterDetails);

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

        internal DataTable getAllHistoryAccessDataReportbyFilter(AccessHistoryReport data)
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
                    command.CommandText = "[getHistoryAccessDataReportbyFilter]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ProcedureNo", data.procedureNo);
                    command.Parameters.AddWithValue("@StartDate", data.startDate);
                    command.Parameters.AddWithValue("@EndDate", data.endDate);

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

        internal int getDepartmentProcedureNumberofPage(string loc, int RowPerPage)
        {
            int conInt = 0;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[getDepartmentProcedureDataNumberofPages]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", loc);
                    command.Parameters.AddWithValue("@RowPerPage", RowPerPage);
                    var data = command.ExecuteScalar();
                    conInt = Convert.ToInt32(data);

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
            return conInt;
        }

        internal int getDepartmentProcedurewithFilterNumberofPage(DashboardFilter filData)
        {
            int conInt = 0;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[getDepartmentProcedureDatabyFilterwithPagingNumberofPages]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@RowPerPage", filData.locationId);
                    command.Parameters.AddWithValue("@RowPerPage", filData.rowPerPage);
                    command.Parameters.AddWithValue("@FilterNo", filData.filterNo);
                    command.Parameters.AddWithValue("@FilterName", filData.filterName);
                    command.Parameters.AddWithValue("@FilterDept", filData.filterDept);
                    command.Parameters.AddWithValue("@FilterBU", filData.filterBU);
                    var data = command.ExecuteScalar();
                    conInt = Convert.ToInt32(data);

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
            return conInt;
        }

        internal int getHistoryAccessNumberofPage(int RowPerPage)
        {
            int conInt = 0;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[getHistoryAccessDataNumberofPages]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@RowPerPage", RowPerPage);
                    var data = command.ExecuteScalar();
                    conInt = Convert.ToInt32(data);

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
            return conInt;
        }

        internal int getHistoryAccessbyFilterwithPagingNumberofPage(AccessHistoryFilter filData)
        {
            int conInt = 0;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[getHistoryAccessDatabyFilterwithPagingNumberofPages]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@RowPerPage", filData.rowPerPage);
                    command.Parameters.AddWithValue("@FilterType", filData.filterType);
                    command.Parameters.AddWithValue("@FilterDetails", filData.filterDetails);
                    var data = command.ExecuteScalar();
                    conInt = Convert.ToInt32(data);

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
            return conInt;
        }

        internal DateTime getProcedureUploadDate(string procNo)
        {
            DateTime temp;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[getProcedureUploadDate]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ProcedureNo", procNo);
                    var data = command.ExecuteScalar();
                    temp = Convert.ToDateTime(data);

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
            return temp;
        }

        // edit DA

        internal void editProcedureData(QueryModel<Procedure> data)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[editProcedureData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ProcedureNo", data.Data.ProcedureNo);
                    command.Parameters.AddWithValue("@ProcedureName", data.Data.ProcedureName);
                    command.Parameters.AddWithValue("@ProcedureDate", data.Data.ProcedureDate);
                    command.Parameters.AddWithValue("@ProcedureWiPath", data.Data.ProcedureWi);
                    command.Parameters.AddWithValue("@ProcedureSopPath", data.Data.ProcedureSop);
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

        // delete DA

        internal void deleteDepartmentProcedureDatabyProcNoDeptID(QueryModel<DepartmentProcedure> data)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[deleteDepartmentProcedureData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ProcedureNo", data.Data.ProcedureNo);
                    command.Parameters.AddWithValue("@DepartmentID", data.Data.DepartmentID);
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

        // retrieve file for client download

        internal Byte[] getFileStream(string dataPath, string procNo)
        {
            var date = getProcedureUploadDate(procNo);

            string type = string.Empty;

            if (dataPath.Contains("WI"))
            {
                type = "WI";
            }
            else if (dataPath.Contains("SOP"))
            {
                type = "SOP";
            }

            string path = Path.Combine(_uploadPath, type, date.Year.ToString(), date.Month.ToString(), date.Day.ToString(), Path.GetFileName(dataPath));

            return System.IO.File.ReadAllBytes(path);
        }

        // save file
        internal async Task saveFiletoDirectory(string path, Byte[] content)
        {
            string dir = Path.GetDirectoryName(path);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            await using FileStream fs = new(path, FileMode.Create);
            Stream stream = new MemoryStream(content);
            await stream.CopyToAsync(fs);
        }

        // http request

        // get

        [HttpGet("getAllProcedureData")]
        public async Task<IActionResult> getAllProcedureDataTable()
        {
            ResultModel<List<Procedure>> res = new ResultModel<List<Procedure>>();
            List<Procedure> procedure = new List<Procedure>();
            DataTable dtProcedure = new DataTable("ProcedureData");
            IActionResult actionResult = null;

            try
            {
                dtProcedure = getAllProcedureData();

                if (dtProcedure.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtProcedure.Rows)
                    {
                        Procedure temp = new Procedure();

                        temp.ProcedureNo = dt["procedureNo"].ToString();
                        temp.ProcedureName = dt["procedureName"].ToString();
                        temp.ProcedureDate = Convert.ToDateTime(dt["procedureDate"]);
                        temp.ProcedureWi = dt["procedureWiPath"].ToString();
                        temp.ProcedureSop = dt["procedureSopPath"].ToString();

                        procedure.Add(temp);
                    }

                    res.Data = procedure;
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

        [HttpGet("getDepartmentProcedureData")]
        public async Task<IActionResult> getAllDepartmentProcedureDataTable()
        {
            ResultModel<List<DepartmentProcedure>> res = new ResultModel<List<DepartmentProcedure>>();
            List<DepartmentProcedure> departmentProcedure = new List<DepartmentProcedure>();
            DataTable dtDepartmentProcedure = new DataTable("appliedProcedureData");
            IActionResult actionResult = null;

            try
            {
                dtDepartmentProcedure = getDepartmentProcedureData();

                if (dtDepartmentProcedure.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtDepartmentProcedure.Rows)
                    {
                        DepartmentProcedure temp = new DepartmentProcedure();

                        temp.ProcedureNo = dt["procedureNo"].ToString();
                        temp.DepartmentID = dt["departmentID"].ToString();

                        temp.Procedure = new Procedure();
                        temp.Procedure.ProcedureNo = dt["procedureNo"].ToString();
                        temp.Procedure.ProcedureName = dt["procedureName"].ToString();
                        temp.Procedure.ProcedureDate = Convert.ToDateTime(dt["procedureDate"]);
                        temp.Procedure.ProcedureWi = dt["procedureWiPath"].ToString();
                        temp.Procedure.ProcedureSop = dt["procedureSopPath"].ToString();

                        temp.Department = new Department();
                        temp.Department.DepartmentID = dt["departmentID"].ToString();
                        temp.Department.DepartmentName = dt["departmentName"].ToString();
                        temp.Department.DepartmentLabel = dt["departmentLabel"].ToString();

                        temp.Department.BisnisUnit = new BisnisUnit();
                        temp.Department.BisnisUnit.BisnisUnitID = dt["bisnisUnitID"].ToString();
                        temp.Department.BisnisUnit.BisnisUnitName = dt["bisnisUnitName"].ToString();
                        temp.Department.BisnisUnit.BisnisUnitLabel = dt["bisnisUnitLabel"].ToString();

                        departmentProcedure.Add(temp);
                    }

                    res.Data = departmentProcedure;
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

        [HttpGet("getDepartmentProcedureDatawithPaging/{param}")]
        public async Task<IActionResult> getAllDepartmentProcedureDataTablebyPaging(string param)
        {
            ResultModel<List<DepartmentProcedure>> res = new ResultModel<List<DepartmentProcedure>>();
            List<DepartmentProcedure> departmentProcedure = new List<DepartmentProcedure>();
            DataTable dtDepartmentProcedure = new DataTable("appliedProcedureData");
            IActionResult actionResult = null;

            try
            {
                string tempz = Base64Decode(param);

                string loc = tempz.Split("!_!")[0].Equals("") ? "HO" : tempz.Split("!_!")[0];
                int pageNo = Convert.ToInt32(tempz.Split("!_!")[1]);

                dtDepartmentProcedure = getDepartmentProcedureDatawithPaging(loc, pageNo, _rowPerPage);

                if (dtDepartmentProcedure.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtDepartmentProcedure.Rows)
                    {
                        DepartmentProcedure temp = new DepartmentProcedure();

                        temp.ProcedureNo = dt["procedureNo"].ToString();
                        temp.DepartmentID = dt["departmentID"].ToString();

                        temp.Procedure = new Procedure();
                        temp.Procedure.ProcedureNo = dt["procedureNo"].ToString();
                        temp.Procedure.ProcedureName = dt["procedureName"].ToString();
                        temp.Procedure.ProcedureDate = Convert.ToDateTime(dt["procedureDate"]);
                        temp.Procedure.ProcedureWi = dt["procedureWiPath"].ToString();
                        temp.Procedure.ProcedureSop = dt["procedureSopPath"].ToString();

                        temp.Department = new Department();
                        temp.Department.DepartmentID = dt["departmentID"].ToString();
                        temp.Department.DepartmentName = dt["departmentName"].ToString();
                        temp.Department.DepartmentLabel = dt["departmentLabel"].ToString();

                        temp.Department.BisnisUnit = new BisnisUnit();
                        temp.Department.BisnisUnit.BisnisUnitID = dt["bisnisUnitID"].ToString();
                        temp.Department.BisnisUnit.BisnisUnitName = dt["bisnisUnitName"].ToString();
                        temp.Department.BisnisUnit.BisnisUnitLabel = dt["bisnisUnitLabel"].ToString();

                        departmentProcedure.Add(temp);
                    }

                    res.Data = departmentProcedure;
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

        [HttpPost("getDepartmentProcedureDatawithFilterbyPaging")]
        public async Task<IActionResult> getAllDepartmentProcedureDataTablewithFilterbyPaging(DashboardFilter data)
        {
            ResultModel<List<DepartmentProcedure>> res = new ResultModel<List<DepartmentProcedure>>();
            List<DepartmentProcedure> departmentProcedure = new List<DepartmentProcedure>();
            DataTable dtDepartmentProcedure = new DataTable("appliedProcedureData");
            IActionResult actionResult = null;

            try
            {
                data.rowPerPage = _rowPerPage;

                dtDepartmentProcedure = getDepartmentProcedureDatawithFilter(data);

                if (dtDepartmentProcedure.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtDepartmentProcedure.Rows)
                    {
                        DepartmentProcedure temp = new DepartmentProcedure();

                        temp.ProcedureNo = dt["procedureNo"].ToString();
                        temp.DepartmentID = dt["departmentID"].ToString();

                        temp.Procedure = new Procedure();
                        temp.Procedure.ProcedureNo = dt["procedureNo"].ToString();
                        temp.Procedure.ProcedureName = dt["procedureName"].ToString();
                        temp.Procedure.ProcedureDate = Convert.ToDateTime(dt["procedureDate"]);
                        temp.Procedure.ProcedureWi = dt["procedureWiPath"].ToString();
                        temp.Procedure.ProcedureSop = dt["procedureSopPath"].ToString();

                        temp.Department = new Department();
                        temp.Department.DepartmentID = dt["departmentID"].ToString();
                        temp.Department.DepartmentName = dt["departmentName"].ToString();
                        temp.Department.DepartmentLabel = dt["departmentLabel"].ToString();

                        temp.Department.BisnisUnit = new BisnisUnit();
                        temp.Department.BisnisUnit.BisnisUnitID = dt["bisnisUnitID"].ToString();
                        temp.Department.BisnisUnit.BisnisUnitName = dt["bisnisUnitName"].ToString();
                        temp.Department.BisnisUnit.BisnisUnitLabel = dt["bisnisUnitLabel"].ToString();

                        departmentProcedure.Add(temp);
                    }

                    res.Data = departmentProcedure;
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

        [HttpGet("getAllHistoryAccessDatawithPaging/{pageNo}")]
        public async Task<IActionResult> getAllHistoryAccessDataTablewithPaging(int pageNo)
        {
            ResultModel<List<HistoryAccess>> res = new ResultModel<List<HistoryAccess>>();
            List<HistoryAccess> historyAccess = new List<HistoryAccess>();
            DataTable dtHistoryAccess = new DataTable("HistoryAccessData");
            IActionResult actionResult = null;

            try
            {
                dtHistoryAccess = getAllHistoryAccessDatawithPaging(pageNo, _rowPerPage);

                if (dtHistoryAccess.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtHistoryAccess.Rows)
                    {
                        HistoryAccess temp = new HistoryAccess();

                        temp.ProcedureNo = dt["procedureNo"].ToString();
                        temp.ProcedureName = dt["procedureName"].ToString();
                        temp.UserEmail = dt["userEmail"].ToString();
                        temp.HistoryAccessDate = Convert.ToDateTime(dt["historyAccessDate"]);

                        historyAccess.Add(temp);
                    }

                    res.Data = historyAccess;
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

        [HttpPost("getAllHistoryAccessDatabyFilterwithPaging")]
        public async Task<IActionResult> getAllHistoryAccessDataTablebyFilterwithPaging(AccessHistoryFilter data)
        {
            ResultModel<List<HistoryAccess>> res = new ResultModel<List<HistoryAccess>>();
            List<HistoryAccess> historyAccess = new List<HistoryAccess>();
            DataTable dtHistoryAccess = new DataTable("HistoryAccessData");
            IActionResult actionResult = null;

            try
            {
                data.rowPerPage = _rowPerPage;

                dtHistoryAccess = getAllHistoryAccessDatabyFilterwithPaging(data);

                if (dtHistoryAccess.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtHistoryAccess.Rows)
                    {
                        HistoryAccess temp = new HistoryAccess();

                        temp.ProcedureNo = dt["procedureNo"].ToString();
                        temp.ProcedureName = dt["procedureName"].ToString();
                        temp.UserEmail = dt["userEmail"].ToString();
                        temp.HistoryAccessDate = Convert.ToDateTime(dt["historyAccessDate"]);

                        historyAccess.Add(temp);
                    }

                    res.Data = historyAccess;
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

        [HttpPost("getAllHistoryAccessDataReportbyFilter")]
        public async Task<IActionResult> getAllHistoryAccessDataTableReportbyFilter(AccessHistoryReport data)
        {
            ResultModel<List<HistoryAccess>> res = new ResultModel<List<HistoryAccess>>();
            List<HistoryAccess> historyAccess = new List<HistoryAccess>();
            DataTable dtHistoryAccess = new DataTable("HistoryAccessData");
            IActionResult actionResult = null;

            try
            {
                dtHistoryAccess = getAllHistoryAccessDataReportbyFilter(data);

                if (dtHistoryAccess.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtHistoryAccess.Rows)
                    {
                        HistoryAccess temp = new HistoryAccess();

                        temp.ProcedureNo = dt["procedureNo"].ToString();
                        temp.ProcedureName = dt["procedureName"].ToString();
                        temp.UserEmail = dt["userEmail"].ToString();
                        temp.HistoryAccessDate = Convert.ToDateTime(dt["historyAccessDate"]);

                        historyAccess.Add(temp);
                    }

                    res.Data = historyAccess;
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

        [HttpGet("getDepartmentProcedureNumberofPage/{param}")]
        public async Task<IActionResult> getDepartmentProcedureNumberofPageData(string param)
        {
            ResultModel<int> res = new ResultModel<int>();
            IActionResult actionResult = null;

            try
            {
                //string loc = param.Equals("") ? "HO" : param;
                string temp = Base64Decode(param);

                res.Data = getDepartmentProcedureNumberofPage(temp, _rowPerPage);

                res.isSuccess = true;
                res.ErrorCode = "00";
                res.ErrorMessage = "";

                actionResult = Ok(res);
            }
            catch (Exception ex)
            {
                res.Data = 0;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        [HttpPost("getDepartmentProcedurewithFilterNumberofPage")]
        public async Task<IActionResult> getDepartmentProcedurewithFilterNumberofPageData(DashboardFilter data)
        {
            ResultModel<int> res = new ResultModel<int>();
            IActionResult actionResult = null;

            try
            {
                data.rowPerPage = _rowPerPage;

                res.Data = getDepartmentProcedurewithFilterNumberofPage(data);

                res.isSuccess = true;
                res.ErrorCode = "00";
                res.ErrorMessage = "";

                actionResult = Ok(res);
            }
            catch (Exception ex)
            {
                res.Data = 0;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        [HttpGet("getHistoryAccessNumberofPage")]
        public async Task<IActionResult> getHistoryAccessNumberofPageData()
        {
            ResultModel<int> res = new ResultModel<int>();
            IActionResult actionResult = null;

            try
            {
                res.Data = getHistoryAccessNumberofPage(_rowPerPage);

                res.isSuccess = true;
                res.ErrorCode = "00";
                res.ErrorMessage = "";

                actionResult = Ok(res);
            }
            catch (Exception ex)
            {
                res.Data = 0;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        [HttpPost("getHistoryAccessbyFilterwithPagingNumberofPage")]
        public async Task<IActionResult> getHistoryAccessNumberofPageData(AccessHistoryFilter data)
        {
            ResultModel<int> res = new ResultModel<int>();
            IActionResult actionResult = null;

            try
            {
                data.rowPerPage = _rowPerPage;

                res.Data = getHistoryAccessbyFilterwithPagingNumberofPage(data);

                res.isSuccess = true;
                res.ErrorCode = "00";
                res.ErrorMessage = "";

                actionResult = Ok(res);
            }
            catch (Exception ex)
            {
                res.Data = 0;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        [HttpGet("getFile/{path}")]
        public async Task<IActionResult> getFiletoDownload(string path)
        {
            ResultModel<BPIDA.Models.MainModel.Stream.FileStream> res = new ResultModel<BPIDA.Models.MainModel.Stream.FileStream>();
            IActionResult actionResult = null;

            var temp = Base64Decode(path);
            try
            {
                // string[] file = Directory.GetFiles("F:\\BPI\\MainData\\WI\\2022\\08\\01");

                string[] splt = temp.Split("!_!");

                res.Data = new BPIDA.Models.MainModel.Stream.FileStream();
                res.Data.content = getFileStream(splt[0], splt[1]);
                res.Data.fileType = "PDF";
                res.Data.fileSize = res.Data.content.Length;

                // foreach (string f in file)
                res.Data.fileName = Path.GetFileName(splt[0]);

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


        [HttpGet("isProcedureDataPresent/{ProcNo}")]
        public async Task<IActionResult> isProcedureDataPresentInTable(string ProcNo)
        {
            ResultModel<string> res = new ResultModel<string>();
            bool present = false;
            DataTable dtProcedure = new DataTable("BisnisUnitData");
            IActionResult actionResult = null;

            string temp = Base64Decode(ProcNo);

            try
            {
                present = isProcedureDataPresent(temp);

                if (present)
                {
                    res.Data = ProcNo;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = ProcNo;
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

        // post

        [HttpPost("createProcedureData")]
        public async Task<IActionResult> createProcedureDataTableandFileSave(ProcedureStream data)
        {
            ResultModel<ProcedureStream> res = new ResultModel<ProcedureStream>();
            IActionResult actionResult = null;

            // create procedure data to db
            try
            {
                // save file

                string pathwi = "";
                string pathsop = "";

                foreach (var f in data.files)
                {
                    if (f.type == "WI")
                    {
                        // process for wi

                        string trustedFileName = Path.GetRandomFileName() + "_" + f.fileName;

                        pathwi = Path.Combine(_uploadPath, "WI", DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), trustedFileName);

                        await saveFiletoDirectory(pathwi, f.content);

                    }
                    else if (f.type == "SOP")
                    {
                        // process for sop

                        string trustedFileName = Path.GetRandomFileName() + "_" + f.fileName;

                        pathsop = Path.Combine(_uploadPath, "SOP", DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), trustedFileName);

                        await saveFiletoDirectory(pathsop, f.content);

                    }
                    else
                    {
                        res.Data = new ProcedureStream();
                        res.Data.procedureDetails = data.procedureDetails;
                        res.Data.files = data.files;
                        res.isSuccess = false;
                        res.ErrorCode = "01";
                        res.ErrorMessage = "Data Type is not Supported";

                        actionResult = Ok(res);
                    }
                }

                // insert data
                data.procedureDetails.Data.ProcedureWi = pathwi;
                data.procedureDetails.Data.ProcedureSop = pathsop;

                createProcedureData(data.procedureDetails);

                res.Data = new ProcedureStream();
                res.Data.procedureDetails = data.procedureDetails;
                res.Data.files = data.files;
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


        [HttpPost("createDepartmentProcedureData")]
        public async Task<IActionResult> createDepartmentProcedureDataTable(QueryModel<List<DepartmentProcedure>> data)
        {
            ResultModel<QueryModel<List<DepartmentProcedure>>> res = new ResultModel<QueryModel<List<DepartmentProcedure>>>();
            IActionResult actionResult = null;

            try
            {
                foreach (var deptapl in data.Data)
                {
                    if (isDepartmentProcedureDataPresent(deptapl.ProcedureNo , deptapl.DepartmentID))
                        continue;

                    QueryModel<DepartmentProcedure> dt = new QueryModel<DepartmentProcedure>();
                    dt.Data = new DepartmentProcedure();

                    dt.Data.ProcedureNo = deptapl.ProcedureNo;
                    dt.Data.DepartmentID = deptapl.DepartmentID;
                    dt.userEmail = data.userEmail;
                    dt.userAction = data.userAction;
                    dt.userActionDate = data.userActionDate;

                    createDepartmentProcedureData(dt);
                }

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


        [HttpPost("createHistoryAccessData")]
        public async Task<IActionResult> createHistoryAccessDataTable(QueryModel<HistoryAccess> data)
        {
            ResultModel<QueryModel<HistoryAccess>> res = new ResultModel<QueryModel<HistoryAccess>>();
            IActionResult actionResult = null;

            try
            {
                createHistoryAccessData(data);

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

        [HttpPost("editProcedureData")]
        public async Task<IActionResult> editProcedureDataTable(ProcedureStream data)
        {
            ResultModel<ProcedureStream> res = new ResultModel<ProcedureStream>();
            IActionResult actionResult = null;

            try
            {
                string pathwi = "";
                string pathsop = "";

                if (!string.IsNullOrEmpty(data.procedureDetails.Data.ProcedureWi))
                {
                    // migrating wi file to archive

                    if (System.IO.File.Exists(data.procedureDetails.Data.ProcedureWi))
                    {
                        // check if exist
                        string trustedFileNameArc = Path.GetFileName(data.procedureDetails.Data.ProcedureWi);

                        string pathArchive = Path.Combine(_archivePath, "WI", DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), trustedFileNameArc);

                        await saveFiletoDirectory(pathArchive, getFileStream(data.procedureDetails.Data.ProcedureWi, data.procedureDetails.Data.ProcedureNo));
                        System.IO.File.Delete(data.procedureDetails.Data.ProcedureWi);
                    }
                }

                if (!string.IsNullOrEmpty(data.procedureDetails.Data.ProcedureSop))
                {
                    // migrating sop file to archive

                    if (System.IO.File.Exists(data.procedureDetails.Data.ProcedureSop))
                    {
                        // check if exist
                        string trustedFileNameArc = Path.GetFileName(data.procedureDetails.Data.ProcedureSop);

                        string pathArchive = Path.Combine(_archivePath, "SOP", DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), trustedFileNameArc);

                        await saveFiletoDirectory(pathArchive, getFileStream(data.procedureDetails.Data.ProcedureSop, data.procedureDetails.Data.ProcedureNo));
                        System.IO.File.Delete(data.procedureDetails.Data.ProcedureSop);
                    }
                }

                foreach (var f in data.files)
                {
                    if (f.type == "WI")
                    {
                        // process for wi

                        string trustedFileName = Path.GetRandomFileName() + "_" + f.fileName;

                        pathwi = Path.Combine(_uploadPath, "WI", DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), trustedFileName);

                        await saveFiletoDirectory(pathwi, f.content);

                    }
                    else if (f.type == "SOP")
                    {
                        // process for sop

                        string trustedFileName = Path.GetRandomFileName() + "_" + f.fileName;

                        pathsop = Path.Combine(_uploadPath, "SOP", DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), trustedFileName);

                        await saveFiletoDirectory(pathsop, f.content);

                    }
                    else
                    {
                        res.Data = new ProcedureStream();
                        res.Data.procedureDetails = data.procedureDetails;
                        res.Data.files = data.files;
                        res.isSuccess = false;
                        res.ErrorCode = "01";
                        res.ErrorMessage = "Data Type is not Supported";

                        actionResult = Ok(res);
                    }
                }

                // insert data
                data.procedureDetails.Data.ProcedureWi = pathwi;
                data.procedureDetails.Data.ProcedureSop = pathsop;

                editProcedureData(data.procedureDetails);

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


        [HttpPost("deleteDepartmentProcedureData")]
        public async Task<IActionResult> deleteDepartmentProcedureDataTablebyProcNoDeptID(QueryModel<List<DepartmentProcedure>> data)
        {
            ResultModel<QueryModel<List<DepartmentProcedure>>> res = new ResultModel<QueryModel<List<DepartmentProcedure>>>();
            IActionResult actionResult = null;

            try
            {
                foreach (var deptapl in data.Data)
                {
                    QueryModel<DepartmentProcedure> dt = new QueryModel<DepartmentProcedure>();
                    dt.Data = new DepartmentProcedure();

                    dt.Data.ProcedureNo = deptapl.ProcedureNo;
                    dt.Data.DepartmentID = deptapl.DepartmentID;
                    dt.userEmail = data.userEmail;
                    dt.userAction = data.userAction;
                    dt.userActionDate = data.userActionDate;

                    deleteDepartmentProcedureDatabyProcNoDeptID(dt);
                }

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

        // OTHER

        [HttpGet("getProcedureMaxSizeUpload")]
        public async Task<IActionResult> getProcedureMaxSizeUpload()
        {
            ResultModel<long> res = new ResultModel<long>();
            IActionResult actionResult = null;

            try
            {
                // megabyte
                res.Data = 1024 * 1024 * _maxFileSize;

                res.isSuccess = true;
                res.ErrorCode = "00";
                res.ErrorMessage = "";

                actionResult = Ok(res);
            }
            catch (Exception ex)
            {
                res.Data = 0;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }
    }


    [Route("api/DA/PettyCash")]
    [ApiController]
    public class PettyCashController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _conString;
        private readonly int _rowPerPage;

        public PettyCashController(IConfiguration config)
        {
            _configuration = config;
            _conString = _configuration.GetValue<string>("ConnectionStrings:Bpi");
            _rowPerPage = _configuration.GetValue<int>("Paging:PettyCash:RowPerPage");
        }

        private static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static DataTable ListToDataTable<T>(List<T> list, string auditUser, string auditAction, DateTime auditDate, string _tableName)
        {
            DataTable dt = new DataTable(_tableName);

            foreach (PropertyInfo info in typeof(T).GetProperties())
            {
                dt.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            dt.Columns.Add(new DataColumn("AuditUser", Nullable.GetUnderlyingType(auditUser.GetType()) ?? auditUser.GetType()));
            dt.Columns.Add(new DataColumn("AuditAction", Nullable.GetUnderlyingType(auditAction.GetType()) ?? auditAction.GetType()));
            dt.Columns.Add(new DataColumn("AuditActionDate", Nullable.GetUnderlyingType(auditDate.GetType()) ?? auditDate.GetType()));

            foreach (T t in list)
            {
                DataRow row = dt.NewRow();

                foreach (PropertyInfo info in typeof(T).GetProperties())
                {
                    row[info.Name] = info.GetValue(t, null) ?? DBNull.Value;
                }
                row["AuditUser"] = auditUser;
                row["AuditAction"] = auditAction;
                row["AuditActionDate"] = auditDate;

                dt.Rows.Add(row);
            }
            return dt;
        }

        // is

        internal bool isAdvanceDataPresent(string AdvanceId)
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
                    command.CommandText = "[isAdvancePresent]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@AdvanceID", AdvanceId);
                    var data = command.ExecuteScalar();
                    conBool = Convert.ToBoolean(data);

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

        // create

        internal bool createPettyCashTableSchema(string objName)
        {
            bool conBool = false;
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[createPettyCashSchemaTables]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@objectName", objName);

                    int ret = command.ExecuteNonQuery();

                    if (ret > 0)
                        conBool = true;
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

        internal DataTable createIDData(string docType)
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
                    command.CommandText = "[createID]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@DocumentName", docType);

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

        internal void createAdvanceData(QueryModel<Advance> data)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[createAdvanceData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@AdvanceID", data.Data.AdvanceID);
                    command.Parameters.AddWithValue("@LocationID", data.Data.LocationID);
                    command.Parameters.AddWithValue("@Approver", data.Data.Approver);
                    command.Parameters.AddWithValue("@AdvanceDate", data.Data.AdvanceDate);
                    command.Parameters.AddWithValue("@DepartmentID", data.Data.DepartmentID);
                    command.Parameters.AddWithValue("@AdvanceNIK", data.Data.AdvanceNIK);
                    command.Parameters.AddWithValue("@AdvanceNote", data.Data.AdvanceNote);
                    command.Parameters.AddWithValue("@AdvanceType", data.Data.AdvanceType);
                    command.Parameters.AddWithValue("@TypeAccount", data.Data.TypeAccount);
                    command.Parameters.AddWithValue("@AdvanceStatus", data.Data.AdvanceStatus);
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

        //internal void createAdvanceLine(QueryModel<AdvanceLine> data)
        //{
        //    using (SqlConnection con = new SqlConnection(_conString))
        //    {
        //        con.Open();
        //        SqlCommand command = new SqlCommand();

        //        try
        //        {
        //            command.Connection = con;
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.CommandText = "[createAdvanceLines]";
        //            command.CommandTimeout = 1000;

        //            command.Parameters.Clear();
        //            command.Parameters.AddWithValue("@AdvanceID", data.Data.AdvanceID);
        //            command.Parameters.AddWithValue("@LineNum", data.Data.LineNo);
        //            command.Parameters.AddWithValue("@Details", data.Data.Details);
        //            command.Parameters.AddWithValue("@Amount", data.Data.Amount);
        //            command.Parameters.AddWithValue("@AStatus", data.Data.Status);
        //            command.Parameters.AddWithValue("@AuditUser", data.userEmail);
        //            command.Parameters.AddWithValue("@AuditAction", data.userAction);
        //            command.Parameters.AddWithValue("@AuditActionDate", data.userActionDate);

        //            command.ExecuteNonQuery();
        //        }
        //        catch (SqlException ex)
        //        {
        //            throw ex;
        //        }
        //        finally
        //        {
        //            con.Close();
        //        }
        //    }
        //}

        internal void createAdvanceLine(DataTable data)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[createAdvanceLines]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@AdvanceLinesData", data);

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

        internal void createExpenseData(QueryModel<Expense> data)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[createExpenseData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ExpenseID", data.Data.ExpenseID);
                    command.Parameters.AddWithValue("@AdvanceID", data.Data.AdvanceID);
                    command.Parameters.AddWithValue("@LocationID", data.Data.LocationID);
                    command.Parameters.AddWithValue("@Approver", data.Data.Approver);
                    command.Parameters.AddWithValue("@ExpenseDate", data.Data.ExpenseDate);
                    command.Parameters.AddWithValue("@DepartmentID", data.Data.DepartmentID);
                    command.Parameters.AddWithValue("@ExpenseNIK", data.Data.ExpenseNIK);
                    command.Parameters.AddWithValue("@ExpenseNote", data.Data.ExpenseNote);
                    command.Parameters.AddWithValue("@ExpenseType", data.Data.ExpenseType);
                    command.Parameters.AddWithValue("@TypeAccount", data.Data.TypeAccount);
                    command.Parameters.AddWithValue("@ExpenseStatus", data.Data.ExpenseStatus);
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

        //internal void createExpenseLine(QueryModel<ExpenseLine> data)
        //{
        //    using (SqlConnection con = new SqlConnection(_conString))
        //    {
        //        con.Open();
        //        SqlCommand command = new SqlCommand();

        //        try
        //        {
        //            command.Connection = con;
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.CommandText = "[createExpenseLines]";
        //            command.CommandTimeout = 1000;

        //            command.Parameters.Clear();
        //            command.Parameters.AddWithValue("@ExpenseID", data.Data.ExpenseID);
        //            command.Parameters.AddWithValue("@LineNum", data.Data.LineNo);
        //            command.Parameters.AddWithValue("@Details", data.Data.Details);
        //            command.Parameters.AddWithValue("@Amount", data.Data.Amount);
        //            command.Parameters.AddWithValue("@ActualAmount", data.Data.ActualAmount);
        //            command.Parameters.AddWithValue("@EStatus", data.Data.Status);
        //            command.Parameters.AddWithValue("@AuditUser", data.userEmail);
        //            command.Parameters.AddWithValue("@AuditAction", data.userAction);
        //            command.Parameters.AddWithValue("@AuditActionDate", data.userActionDate);

        //            command.ExecuteNonQuery();
        //        }
        //        catch (SqlException ex)
        //        {
        //            throw ex;
        //        }
        //        finally
        //        {
        //            con.Close();
        //        }
        //    }
        //}

        internal void createExpenseLine(DataTable data)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[createExpenseLines]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ExpenseLinesData", data);

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

        //internal void createExpenseAttachLine(QueryModel<BPIDA.Models.MainModel.Stream.FileStream> data)
        //{
        //    using (SqlConnection con = new SqlConnection(_conString))
        //    {
        //        con.Open();
        //        SqlCommand command = new SqlCommand();

        //        try
        //        {
        //            command.Connection = con;
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.CommandText = "[createExpenseAttachData]";
        //            command.CommandTimeout = 1000;

        //            command.Parameters.Clear();
        //            command.Parameters.AddWithValue("@ExpenseID", data.Data.type);
        //            command.Parameters.AddWithValue("@PathFile", data.Data.fileName);
        //            command.Parameters.AddWithValue("@AuditUser", data.userEmail);
        //            command.Parameters.AddWithValue("@AuditAction", data.userAction);
        //            command.Parameters.AddWithValue("@AuditActionDate", data.userActionDate);

        //            command.ExecuteNonQuery();
        //        }
        //        catch (SqlException ex)
        //        {
        //            throw ex;
        //        }
        //        finally
        //        {
        //            con.Close();
        //        }
        //    }
        //}

        internal void createExpenseAttachLine(DataTable data)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[createExpenseAttachData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ExpenseAttachData", data);

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

        internal void createReimburseData(QueryModel<Reimburse> data)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[createReimburseData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ReimburseID", data.Data.ReimburseID);
                    command.Parameters.AddWithValue("@LocationID", data.Data.LocationID);
                    command.Parameters.AddWithValue("@ReimburseNote", data.Data.ReimburseNote);
                    command.Parameters.AddWithValue("@ReimburseDate", data.Data.ReimburseDate);
                    command.Parameters.AddWithValue("@ReimburseStatus", data.Data.ReimburseStatus);
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

        //internal void createReimburseLine(QueryModel<ReimburseLine> data)
        //{
        //    using (SqlConnection con = new SqlConnection(_conString))
        //    {
        //        con.Open();
        //        SqlCommand command = new SqlCommand();

        //        try
        //        {
        //            command.Connection = con;
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.CommandText = "[createReimburseLines]";
        //            command.CommandTimeout = 1000;

        //            command.Parameters.Clear();
        //            command.Parameters.AddWithValue("@ReimburseID", data.Data.ReimburseID);
        //            command.Parameters.AddWithValue("@ExpenseID", data.Data.ExpenseID);
        //            command.Parameters.AddWithValue("@LineNum", data.Data.LineNo);
        //            command.Parameters.AddWithValue("@AccountNo", data.Data.AccountNo);
        //            command.Parameters.AddWithValue("@Details", data.Data.Details);
        //            command.Parameters.AddWithValue("@Amount", data.Data.Amount);
        //            command.Parameters.AddWithValue("@ApprovedAmount", data.Data.ApprovedAmount);
        //            command.Parameters.AddWithValue("@RStatus", data.Data.Status);
        //            command.Parameters.AddWithValue("@AuditUser", data.userEmail);
        //            command.Parameters.AddWithValue("@AuditAction", data.userAction);
        //            command.Parameters.AddWithValue("@AuditActionDate", data.userActionDate);

        //            command.ExecuteNonQuery();
        //        }
        //        catch (SqlException ex)
        //        {
        //            throw ex;
        //        }
        //        finally
        //        {
        //            con.Close();
        //        }
        //    }
        //}

        internal void createReimburseLine(DataTable data)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[createReimburseLines]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ReimburseLinesData", data);

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

        //internal void createReimburseAttachLine(QueryModel<BPIDA.Models.MainModel.Stream.FileStream> data)
        //{
        //    using (SqlConnection con = new SqlConnection(_conString))
        //    {
        //        con.Open();
        //        SqlCommand command = new SqlCommand();

        //        try
        //        {
        //            string exp = data.Data.type.Split("!_!")[0];
        //            string rmb = data.Data.type.Split("!_!")[1];

        //            command.Connection = con;
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.CommandText = "[createReimburseAttachData]";
        //            command.CommandTimeout = 1000;

        //            command.Parameters.Clear();
        //            command.Parameters.AddWithValue("@ReimburseID", rmb);
        //            command.Parameters.AddWithValue("@ExpenseID", exp);
        //            command.Parameters.AddWithValue("@PathFile", data.Data.fileName);
        //            command.Parameters.AddWithValue("@AuditUser", data.userEmail);
        //            command.Parameters.AddWithValue("@AuditAction", data.userAction);
        //            command.Parameters.AddWithValue("@AuditActionDate", data.userActionDate);

        //            command.ExecuteNonQuery();
        //        }
        //        catch (SqlException ex)
        //        {
        //            throw ex;
        //        }
        //        finally
        //        {
        //            con.Close();
        //        }
        //    }
        //}

        internal void createReimburseAttachLine(DataTable data)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[createReimburseAttachData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ReimburseAttachData", data);

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

        // update

        internal void updateSettleAdvance(QueryModel<string> data)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[editAdvanceSettlement]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@AdvanceID", data.Data);
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

        internal void updateSettleExpense(string Id, string user, string act, DateTime actdate)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[editExpenseSettlement]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ExpenseID", Id);
                    command.Parameters.AddWithValue("@AuditUser", user);
                    command.Parameters.AddWithValue("@AuditAction", act);
                    command.Parameters.AddWithValue("@AuditActionDate", actdate);

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

        internal void updateSettleReimburse(QueryModel<string> data)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[editReimburseSettlement]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ReimburseID", data.Data);
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

        internal bool updateDocumentStatus(string TbName, string Id, string statusValue, string reimburseNote, string user, string act, DateTime actdate)
        {
            bool conBool = false;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[editPettyCashDocumentStatus]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@TbName", TbName);
                    command.Parameters.AddWithValue("@DocID", Id);
                    command.Parameters.AddWithValue("@StatusValue", statusValue);
                    command.Parameters.AddWithValue("@ReimburseNote", reimburseNote);
                    command.Parameters.AddWithValue("@AuditUser", user);
                    command.Parameters.AddWithValue("@AuditAction", act);
                    command.Parameters.AddWithValue("@AuditActionDate", actdate);

                    int ret = command.ExecuteNonQuery();
                        
                    if (ret >= 0)
                        conBool = true;
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

        internal void updateReimburseLine(QueryModel<ReimburseLine> line)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[editReimburseLine]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ReimburseID", line.Data.ReimburseID);
                    command.Parameters.AddWithValue("@ExpenseID", line.Data.ExpenseID);
                    command.Parameters.AddWithValue("@LineNum", line.Data.LineNo);
                    command.Parameters.AddWithValue("@AccountNo", line.Data.AccountNo);
                    command.Parameters.AddWithValue("@Details", line.Data.Details);
                    command.Parameters.AddWithValue("@Amount", line.Data.Amount);
                    command.Parameters.AddWithValue("@ApprovedAmount", line.Data.ApprovedAmount);
                    command.Parameters.AddWithValue("@RStatus", line.Data.Status);
                    command.Parameters.AddWithValue("@AuditUser", line.userEmail);
                    command.Parameters.AddWithValue("@AuditAction", line.userAction);
                    command.Parameters.AddWithValue("@AuditActionDate", line.userActionDate);

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

        internal void updateLocationBudget(QueryModel<BalanceDetails> data)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[editLocationBudget]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", data.Data.LocationID);
                    command.Parameters.AddWithValue("@Budget", data.Data.BudgetAmount);
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

        internal void updateLocationCutoffDate(QueryModel<CutoffDetails> data)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[editTableCutoffDate]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", data.Data.LocationID);
                    command.Parameters.AddWithValue("@ModuleLedgerName", data.Data.ModuleLedgerName);
                    command.Parameters.AddWithValue("@CutoffDate", data.Data.CutoffDate);
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

        // get

        internal DataTable getPettyCashLedgerDatabyDate(ledgerParam data)
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
                    command.CommandText = "[getPettyCashLedgerDataEntry]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@StartDate", data.startDate);
                    command.Parameters.AddWithValue("@EndDate", data.endDate);
                    command.Parameters.AddWithValue("@LocationID", data.locationID);

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

        internal DataTable getAttachmentLines(string Id, string denom)
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
                    command.CommandText = "[getAttachmentLines]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ExpenseID", Id);
                    command.Parameters.AddWithValue("@TbDenom", denom);

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

        internal DataTable getLocationBudgetDetails(string loc)
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
                    command.CommandText = "[getLocationBudgetDetails]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", loc);

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

        internal DataTable getAdvanceDatabyLocation(string denom, string location, string value, string type, string filValue, int PageNo, int rowPerPage)
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
                    command.CommandText = "[getAdvanceDatabyLocation]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@TbDenom", denom);
                    command.Parameters.AddWithValue("@LocationID", location);
                    command.Parameters.AddWithValue("@StatusValue", value);
                    command.Parameters.AddWithValue("@FilterType", type);
                    command.Parameters.AddWithValue("@FilterValue", filValue);
                    command.Parameters.AddWithValue("@PageNo", PageNo);
                    command.Parameters.AddWithValue("@RowPerPage", rowPerPage);

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

        internal DataTable getAdvanceDatabyUser(string userName)
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
                    command.CommandText = "[getAdvanceDatabyUser]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@AuditUser", userName);

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

        internal DataTable getExpenseDatabyLocation(string denom, string location, string value, string type, string filValue, int PageNo, int rowPerPage)
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
                    command.CommandText = "[getExpenseDatabyLocation]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@TbDenom", denom);
                    command.Parameters.AddWithValue("@LocationID", location);
                    command.Parameters.AddWithValue("@StatusValue", value);
                    command.Parameters.AddWithValue("@FilterType", type);
                    command.Parameters.AddWithValue("@FilterValue", filValue);
                    command.Parameters.AddWithValue("@PageNo", PageNo);
                    command.Parameters.AddWithValue("@RowPerPage", rowPerPage);

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

        internal DataTable getReimburseDatabyLocation(string denom, string location, string value, string type, string filValue, int PageNo, int rowPerPage)
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
                    command.CommandText = "[getReimburseDatabyLocation]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@TbDenom", denom);
                    command.Parameters.AddWithValue("@LocationID", location);
                    command.Parameters.AddWithValue("@StatusValue", value);
                    command.Parameters.AddWithValue("@FilterType", type);
                    command.Parameters.AddWithValue("@FilterValue", filValue);
                    command.Parameters.AddWithValue("@PageNo", PageNo);
                    command.Parameters.AddWithValue("@RowPerPage", rowPerPage);

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

        internal DataTable getAdvanceLinesbyID(string AdvanceId, string denom)
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
                    command.CommandText = "[getAdvanceLinesbyID]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@AdvanceID", AdvanceId);
                    command.Parameters.AddWithValue("@TbDenom", denom);

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

        internal DataTable getExpenseLinesbyID(string ExpenseId, string denom)
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
                    command.CommandText = "[getExpenseLinesbyID]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ExpenseID", ExpenseId);
                    command.Parameters.AddWithValue("@TbDenom", denom);

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

        internal DataTable getReimburseLinesbyID(string ReimburseId, string denom)
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
                    command.CommandText = "[getReimburseLinesbyID]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ReimburseID", ReimburseId);
                    command.Parameters.AddWithValue("@TbDenom", denom);

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

        internal int getModuleNumberOfPage(int RowPerPage, string TbName, string TbCol, string value, string type, string filValue, string loc)
        {
            int conInt = 0;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[getPettyCashModulePageSize]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@TbName", TbName);
                    command.Parameters.AddWithValue("@TbCol", TbCol);
                    command.Parameters.AddWithValue("@StatusValue", value);
                    command.Parameters.AddWithValue("@FilterType", type);
                    command.Parameters.AddWithValue("@FilterValue", filValue);
                    command.Parameters.AddWithValue("@LocationID", loc);
                    command.Parameters.AddWithValue("@RowPerPage", RowPerPage);
                    
                    var data = command.ExecuteScalar();
                    conInt = Convert.ToInt32(data);

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
            return conInt;
        }

        internal decimal getAdvanceOutstandingAmount(string loc)
        {
            decimal conInt = decimal.Zero;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[getAdvanceOutstandingAmount]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", loc);

                    var data = command.ExecuteScalar();
                    conInt = Convert.ToDecimal(data);

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
            return conInt;
        }

        internal decimal getAdvanceApprovedAmount(string loc)
        {
            decimal conInt = decimal.Zero;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[getAdvanceApprovedAmount]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", loc);

                    var data = command.ExecuteScalar();
                    conInt = Convert.ToDecimal(data);

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
            return conInt;
        }

        internal decimal getExpenseOutstandingAmount(string loc)
        {
            decimal conInt = decimal.Zero;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[getExpenseOutstandingAmount]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", loc);

                    var data = command.ExecuteScalar();
                    conInt = Convert.ToDecimal(data);

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
            return conInt;
        }

        internal decimal getExpenseApprovedAmount(string loc)
        {
            decimal conInt = decimal.Zero;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[getExpenseApprovedAmount]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", loc);

                    var data = command.ExecuteScalar();
                    conInt = Convert.ToDecimal(data);

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
            return conInt;
        }

        internal DataTable getReimburseOutstandingAmount(string loc)
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
                    command.CommandText = "[getReimburseOutstandingAmount]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", loc);

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

        internal decimal getLocationOnhandAmount(string loc)
        {
            decimal conInt = decimal.Zero;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[getLocationOnhandAmount]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", loc);

                    var data = command.ExecuteScalar();
                    conInt = Convert.ToDecimal(data);
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

            return conInt;
        }

        internal DateTime getCutoffDate(string loc)
        {
            DateTime temp;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[getTableCutoffDate]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", loc);
                    var data = command.ExecuteScalar();
                    temp = Convert.ToDateTime(data);

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
            return temp;
        }

        internal DataTable getCoabyModule(string moduleName)
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
                    command.CommandText = "[getCOAbyModule]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ModuleName", moduleName);

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

        internal DataTable getMailingDetails(string moduleName, string ActionName, string loc)
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
                    command.CommandText = "[getMailingDetails]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ModuleName", moduleName);
                    command.Parameters.AddWithValue("@ActionName", ActionName);
                    command.Parameters.AddWithValue("@LocationID", loc);

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

        // http

        // is

        [HttpGet("isAdvanceDataPresent/{AdvanceId}")]
        public async Task<IActionResult> isAdvanceDataPresentInTable(string AdvanceId)
        {
            ResultModel<string> res = new ResultModel<string>();
            bool present = false;
            IActionResult actionResult = null;

            string temp = Base64Decode(AdvanceId);

            try
            {
                present = isAdvanceDataPresent(temp);

                if (present)
                {
                    res.Data = AdvanceId;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = AdvanceId;
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

        [HttpGet("createID/{docType}")]
        public async Task<IActionResult> createID(string docType)
        {
            ResultModel<string> res = new ResultModel<string>();
            string code = string.Empty;
            DataTable dtIdentity = new DataTable("Identity");
            IActionResult actionResult = null;

            try
            {

                dtIdentity = createIDData(docType);

                if (dtIdentity.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtIdentity.Rows)
                    {
                        string zero = string.Empty;

                        for (int i = 0; i < (7 - Convert.ToInt32(dt["IDLength"])); i++)
                        {
                            zero = zero + "0";
                        }

                        code = dt["Code"].ToString() + DateTime.Now.Year.ToString() + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + zero + dt["DocNumber"].ToString() + dt["Parity"].ToString();

                    }

                    res.Data = code;
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

        // create

        [HttpPost("createAdvanceData")]
        public async Task<IActionResult> createAdvanceDataTable(QueryModel<Advance> data)
        {
            ResultModel<QueryModel<Advance>> res = new ResultModel<QueryModel<Advance>>();
            IActionResult actionResult = null;

            try
            {
                createAdvanceData(data);

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

        [HttpPost("createAdvanceLineData")]
        public async Task<IActionResult> createAdvanceLineDataTable(QueryModel<List<AdvanceLine>> data)
        {
            ResultModel<QueryModel<List<AdvanceLine>>> res = new ResultModel<QueryModel<List<AdvanceLine>>>();
            DataTable dtTable = new DataTable("Data");
            IActionResult actionResult = null;

            try
            {
                //foreach (var dt in data.Data)
                //{
                //    QueryModel<AdvanceLine> line = new QueryModel<AdvanceLine>();
                //    line.Data = new AdvanceLine();

                //    line.Data = dt;
                //    line.userEmail = data.userEmail;
                //    line.userAction = data.userAction;
                //    line.userActionDate = data.userActionDate;

                //    createAdvanceLine(line);
                //}

                createAdvanceLine(ListToDataTable<AdvanceLine>(data.Data, data.userEmail, data.userAction, data.userActionDate, "AdvanceLine"));

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

        [HttpPost("createExpenseData")]
        public async Task<IActionResult> createExpenseDataTable(QueryModel<Expense> data)
        {
            ResultModel<QueryModel<Expense>> res = new ResultModel<QueryModel<Expense>>();
            IActionResult actionResult = null;

            try
            {
                createExpenseData(data);

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

        [HttpPost("createExpenseLineData")]
        public async Task<IActionResult> createExpenseLineDataTable(QueryModel<List<ExpenseLine>> data)
        {
            ResultModel<QueryModel<List<ExpenseLine>>> res = new ResultModel<QueryModel<List<ExpenseLine>>>();
            IActionResult actionResult = null;

            try
            {
                //foreach (var dt in data.Data)
                //{
                //    QueryModel<ExpenseLine> line = new QueryModel<ExpenseLine>();
                //    line.Data = new ExpenseLine();

                //    line.Data = dt;
                //    line.userEmail = data.userEmail;
                //    line.userAction = data.userAction;
                //    line.userActionDate = data.userActionDate;

                //    createExpenseLine(line);
                //    //createExpenseAttachLine(line);
                //}

                createExpenseLine(ListToDataTable<ExpenseLine>(data.Data, data.userEmail, data.userAction, data.userActionDate, "ExpenseLine"));

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

        [HttpPost("createExpenseAttachLineData")]
        public async Task<IActionResult> createExpenseLineDataTable(QueryModel<List<BPIDA.Models.MainModel.Stream.FileStream>> data)
        {
            ResultModel<QueryModel<List<BPIDA.Models.MainModel.Stream.FileStream>>> res = new ResultModel<QueryModel<List<BPIDA.Models.MainModel.Stream.FileStream>>>();
            List<ExpenseAttachmentLine> dtLine = new();
            IActionResult actionResult = null;

            try
            {
                //foreach (var dt in data.Data)
                //{
                //    QueryModel<BPIDA.Models.MainModel.Stream.FileStream> line = new QueryModel<BPIDA.Models.MainModel.Stream.FileStream>();
                //    line.Data = new BPIDA.Models.MainModel.Stream.FileStream();

                //    line.Data = dt;
                //    line.userEmail = data.userEmail;
                //    line.userAction = data.userAction;
                //    line.userActionDate = data.userActionDate;

                //    createExpenseAttachLine(line);
                //}

                foreach (var dt in data.Data)
                {
                    ExpenseAttachmentLine temp = new();

                    temp.ExpenseID = dt.type;
                    temp.PathFile = dt.fileName;

                    dtLine.Add(temp);
                }

                createExpenseAttachLine(ListToDataTable<ExpenseAttachmentLine>(dtLine, data.userEmail, data.userAction, data.userActionDate, "ExpenseAttach"));

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

        [HttpPost("createReimburseData")]
        public async Task<IActionResult> createReimburseDataTable(QueryModel<Reimburse> data)
        {
            ResultModel<QueryModel<Reimburse>> res = new ResultModel<QueryModel<Reimburse>>();
            IActionResult actionResult = null;

            try
            {
                createReimburseData(data);

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

        [HttpPost("createReimburseLineData")]
        public async Task<IActionResult> createReimburseLineDataTable(QueryModel<List<ReimburseLine>> data)
        {
            ResultModel<QueryModel<List<ReimburseLine>>> res = new ResultModel<QueryModel<List<ReimburseLine>>>();
            IActionResult actionResult = null;

            try
            {
                //foreach (var dt in data.Data)
                //{
                //    QueryModel<ReimburseLine> line = new QueryModel<ReimburseLine>();
                //    line.Data = new ReimburseLine();

                //    line.Data = dt;
                //    line.userEmail = data.userEmail;
                //    line.userAction = data.userAction;
                //    line.userActionDate = data.userActionDate;

                //    createReimburseLine(line);
                //    //createExpenseAttachLine(line);
                //}

                createReimburseLine(ListToDataTable<ReimburseLine>(data.Data, data.userEmail, data.userAction, data.userActionDate, "ReimburseLine"));

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

        [HttpPost("createReimburseAttachLineData")]
        public async Task<IActionResult> createReimburseLineDataTable(QueryModel<List<BPIDA.Models.MainModel.Stream.FileStream>> data)
        {
            ResultModel<QueryModel<List<BPIDA.Models.MainModel.Stream.FileStream>>> res = new ResultModel<QueryModel<List<BPIDA.Models.MainModel.Stream.FileStream>>>();
            List<ReimburseAttachmentLine> dtLine = new();
            IActionResult actionResult = null;

            try
            {
                //foreach (var dt in data.Data)
                //{
                //    QueryModel<BPIDA.Models.MainModel.Stream.FileStream> line = new QueryModel<BPIDA.Models.MainModel.Stream.FileStream>();
                //    line.Data = new BPIDA.Models.MainModel.Stream.FileStream();

                //    line.Data = dt;
                //    line.userEmail = data.userEmail;
                //    line.userAction = data.userAction;
                //    line.userActionDate = data.userActionDate;

                //    createReimburseAttachLine(line);
                //}

                foreach (var dt in data.Data)
                {
                    ReimburseAttachmentLine temp = new();

                    string exp = dt.type.Split("!_!")[0];
                    string rmb = dt.type.Split("!_!")[1];

                    temp.ReimburseID = rmb;
                    temp.ExpenseID = exp;
                    temp.PathFile = dt.fileName;

                    dtLine.Add(temp);
                }

                createReimburseAttachLine(ListToDataTable<ReimburseAttachmentLine>(dtLine, data.userEmail, data.userAction, data.userActionDate, "ReimburseAttach"));

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

        [HttpPost("AdvanceSettlement")]
        public async Task<IActionResult> updateAdvanceDataSettlement(QueryModel<string> data)
        {
            ResultModel<QueryModel<string>> res = new ResultModel<QueryModel<string>>();
            IActionResult actionResult = null;

            try
            {
                updateSettleAdvance(data);

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

        [HttpPost("ExpenseSettlement")]
        public async Task<IActionResult> updateExpenseDataSettlement(QueryModel<List<string>> data)
        {
            ResultModel<QueryModel<List<string>>> res = new ResultModel<QueryModel<List<string>>>();
            
            IActionResult actionResult = null;

            try
            {
                foreach (var id in data.Data)
                {
                    updateSettleExpense(id, data.userEmail, data.userAction, data.userActionDate);
                }
                
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

        [HttpPost("ReimburseSettlement")]
        public async Task<IActionResult> updateReimburseDataSettlement(QueryModel<string> data)
        {
            ResultModel<QueryModel<string>> res = new ResultModel<QueryModel<string>>();
            IActionResult actionResult = null;

            try
            {
                updateSettleReimburse(data);

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

        [HttpPost("editDocumentStatus")]
        public async Task<IActionResult> editDocumentStatusData(QueryModel<string> data)
        {
            ResultModel<QueryModel<string>> res = new ResultModel<QueryModel<string>>();
            IActionResult actionResult = null;

            try
            {
                string TbName = data.Data.Split("!_!")[0];
                string id = data.Data.Split("!_!")[1];
                string value = data.Data.Split("!_!")[2];
                string note = data.Data.Split("!_!")[3];

                bool flag = updateDocumentStatus(TbName, id, value, note, data.userEmail, data.userAction, data.userActionDate);

                if (flag)
                {
                    res.Data = data;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = data;
                    res.isSuccess = true;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Data Might have been Approved";

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

        [HttpPost("editReimburseLine")]
        public async Task<IActionResult> editReimburseLineData(QueryModel<Reimburse> data)
        {
            ResultModel<QueryModel<Reimburse>> res = new ResultModel<QueryModel<Reimburse>>();
            IActionResult actionResult = null;

            try
            {
                foreach (var id in data.Data.lines)
                {
                    QueryModel<ReimburseLine> temp = new();

                    temp.Data = id;
                    temp.userEmail = data.userEmail;
                    temp.userAction = data.userAction;
                    temp.userActionDate = data.userActionDate;

                    updateReimburseLine(temp);
                }

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

        [HttpPost("getLedgerDataEntriesbyDate")]
        public async Task<IActionResult> getLedgerDataEntriesbyDate(List<ledgerParam> data)
        {
            ResultModel<List<PettyCashLedger>> res = new ResultModel<List<PettyCashLedger>>();
            List<PettyCashLedger> pettyCashLedgers = new List<PettyCashLedger>();
            DataTable dtPettyCashLedger = new DataTable("PettyCashLedger");
            bool flag = false;
            IActionResult actionResult = null;

            try
            {
                foreach (var dat in data)
                {
                    dtPettyCashLedger = getPettyCashLedgerDatabyDate(dat);

                    if (dtPettyCashLedger.Rows.Count > 0)
                    {
                        flag = true;

                        foreach (DataRow dt in dtPettyCashLedger.Rows)
                        {
                            PettyCashLedger temp1 = new PettyCashLedger();

                            temp1.TransactionDate = Convert.ToDateTime(dt["TransactionDate"]);
                            temp1.TransactionType = dt["TransactionType"].ToString();
                            temp1.DocumentID = dt["DocumentID"].ToString();
                            temp1.LocationID = dt["LocationID"].ToString();
                            temp1.Amount = Convert.ToDecimal(dt["Amount"]);
                            temp1.Actor = dt["Actor"].ToString();
                            temp1.ExternalDocument = dt.IsNull("ExternalDocument") ? "" : dt["ExternalDocument"].ToString();
                            temp1.Applicant = dt["Applicant"].ToString();
                            temp1.DocumentDate = Convert.ToDateTime(dt["DocumentDate"]);
                            temp1.DepartmentID = dt.IsNull("DepartmentID") ? "" : dt["DepartmentID"].ToString();
                            temp1.Note = dt.IsNull("Note") ? "" : dt["Note"].ToString();
                            temp1.NIK = dt.IsNull("NIK") ? "" : dt["NIK"].ToString();
                            temp1.Type = dt.IsNull("Type") ? "" : dt["Type"].ToString();
                            temp1.BankAccount = dt.IsNull("BankAccount") ? "" : dt["BankAccount"].ToString();
                            temp1.Status = dt["Status"].ToString();

                            pettyCashLedgers.Add(temp1);
                        }
                    }
                    else
                    {
                        flag = false;
                    }
                }

                if (flag)
                {
                    res.Data = pettyCashLedgers;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = pettyCashLedgers;
                    res.isSuccess = true;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "At Least 1 Location Have 0 Data";

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

        [HttpGet("getAttachmentLines/{Id}")]
        public async Task<IActionResult> getAttachmentLinesbyID(string Id)
        {
            ResultModel<List<AttachmentLine>> res = new ResultModel<List<AttachmentLine>>();
            List<AttachmentLine> attachmentLines = new List<AttachmentLine>();
            DataTable dtAttachmentLine = new DataTable("AttachmentLine");
            IActionResult actionResult = null;

            try
            {
                string temp = Base64Decode(Id);

                dtAttachmentLine = getAttachmentLines(temp.Split("!_!")[0], temp.Split("!_!")[1]);

                if (dtAttachmentLine.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtAttachmentLine.Rows)
                    {
                        AttachmentLine temp1 = new AttachmentLine();

                        temp1.ExpenseID = dt["ExpenseID"].ToString();
                        temp1.PathFile = dt["PathFile"].ToString();
                        //temp.DateModified = Convert.ToDateTime(dt["AuditActionDate"]);

                        attachmentLines.Add(temp1);
                    }

                    res.Data = attachmentLines;
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

        [HttpGet("getAdvanceDatabyLocation/{locPage}")]
        public async Task<IActionResult> getAdvanceDataTablebyLocation(string locPage)
        {
            ResultModel<List<Advance>> res = new ResultModel<List<Advance>>();
            List<Advance> advance = new List<Advance>();
            DataTable dtAdvance = new DataTable("AdvanceData");
            IActionResult actionResult = null;

            try
            {
                var temp = Base64Decode(locPage);

                string denom = temp.Split("!_!")[0];
                string loc = temp.Split("!_!")[1].Equals("") ? "HO" : temp.Split("!_!")[1];
                string val = temp.Split("!_!")[2];
                string tp = temp.Split("!_!")[3];
                string filVal = temp.Split("!_!")[4];
                int page = Convert.ToInt32(temp.Split("!_!")[5]);

                dtAdvance = getAdvanceDatabyLocation(denom, loc, val, tp, filVal, page, _rowPerPage);

                if (dtAdvance.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtAdvance.Rows)
                    {
                        Advance temp1 = new Advance();
                        temp1.statusDetails = new();

                        temp1.AdvanceID = dt["AdvanceID"].ToString();
                        temp1.LocationID = dt["LocationID"].ToString();
                        temp1.Approver = dt["Approver"].ToString();
                        temp1.AdvanceDate = Convert.ToDateTime(dt["AdvanceDate"]);
                        temp1.DepartmentID = dt["DepartmentID"].ToString();
                        temp1.AdvanceNIK = dt["AdvanceNIK"].ToString();
                        temp1.AdvanceNote = dt["AdvanceNote"].ToString();
                        temp1.AdvanceType = dt["AdvanceType"].ToString();
                        temp1.TypeAccount = dt["TypeAccount"].ToString();
                        temp1.AdvanceStatus = dt["AdvanceStatus"].ToString();
                        temp1.Applicant = dt["Applicant"].ToString();

                        temp1.statusDetails.submitDate = dt.IsNull("SubmitDate") ? DateTime.MinValue : Convert.ToDateTime(dt["SubmitDate"]);
                        temp1.statusDetails.submitUser = dt.IsNull("SubmitUser") ? "" : dt["SubmitUser"].ToString();
                        temp1.statusDetails.confirmDate = dt.IsNull("ConfirmDate") ? DateTime.MinValue : Convert.ToDateTime(dt["ConfirmDate"]);
                        temp1.statusDetails.confirmUser = dt.IsNull("ConfirmUser") ? "" : dt["ConfirmUser"].ToString();
                        temp1.statusDetails.rejectDate = dt.IsNull("RejectDate") ? DateTime.MinValue : Convert.ToDateTime(dt["RejectDate"]);
                        temp1.statusDetails.rejectUser = dt.IsNull("RejectUser") ? "" : dt["RejectUser"].ToString();

                        List<AdvanceLine> advanceLines = new List<AdvanceLine>();
                        DataTable dtAdvanceLine = new DataTable("AdvanceLine");

                        dtAdvanceLine = getAdvanceLinesbyID(dt["AdvanceID"].ToString(), denom);

                        if (dtAdvanceLine.Rows.Count > 0)
                        {
                            foreach (DataRow linedt in dtAdvanceLine.Rows)
                            {
                                AdvanceLine temp2 = new AdvanceLine();

                                temp2.AdvanceID = linedt["AdvanceID"].ToString();
                                temp2.LineNo = Convert.ToInt32(linedt["LineNum"]);
                                temp2.Details = linedt["Details"].ToString();
                                temp2.Amount = Convert.ToDecimal(linedt["Amount"]);
                                temp2.Status = linedt["AStatus"].ToString();

                                advanceLines.Add(temp2);
                            }
                        }

                        temp1.lines = new List<AdvanceLine>();
                        temp1.lines = advanceLines;

                        advance.Add(temp1);
                    }

                    res.Data = advance;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = true;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Fetch Empty";

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

        [HttpGet("getAdvanceDatabyUser/{user}")]
        public async Task<IActionResult> getAdvanceDataTablebyUser(string user)
        {
            ResultModel<List<Advance>> res = new ResultModel<List<Advance>>();
            List<Advance> advance = new List<Advance>();
            DataTable dtAdvance = new DataTable("AdvanceData");
            IActionResult actionResult = null;

            try
            {
                var temp = Base64Decode(user);

                dtAdvance = getAdvanceDatabyUser(temp.Split("!_!")[0]);

                if (dtAdvance.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtAdvance.Rows)
                    {
                        Advance temp1 = new Advance();
                        temp1.statusDetails = new();

                        temp1.AdvanceID = dt["AdvanceID"].ToString();
                        temp1.LocationID = dt["LocationID"].ToString();
                        temp1.AdvanceDate = Convert.ToDateTime(dt["AdvanceDate"]);
                        temp1.DepartmentID = dt["DepartmentID"].ToString();
                        temp1.AdvanceNIK = dt["AdvanceNIK"].ToString();
                        temp1.AdvanceNote = dt["AdvanceNote"].ToString();
                        temp1.AdvanceType = dt["AdvanceType"].ToString();
                        temp1.TypeAccount = dt["TypeAccount"].ToString();
                        temp1.AdvanceStatus = dt["AdvanceStatus"].ToString();
                        temp1.Applicant = dt["Applicant"].ToString();

                        temp1.statusDetails.submitDate = dt.IsNull("SubmitDate") ? DateTime.MinValue : Convert.ToDateTime(dt["SubmitDate"]);
                        temp1.statusDetails.submitUser = dt.IsNull("SubmitUser") ? "" : dt["SubmitUser"].ToString();
                        temp1.statusDetails.confirmDate = dt.IsNull("ConfirmDate") ? DateTime.MinValue : Convert.ToDateTime(dt["ConfirmDate"]);
                        temp1.statusDetails.confirmUser = dt.IsNull("ConfirmUser") ? "" : dt["ConfirmUser"].ToString();
                        temp1.statusDetails.rejectDate = dt.IsNull("RejectDate") ? DateTime.MinValue : Convert.ToDateTime(dt["RejectDate"]);
                        temp1.statusDetails.rejectUser = dt.IsNull("RejectUser") ? "" : dt["RejectUser"].ToString();

                        List<AdvanceLine> advanceLines = new List<AdvanceLine>();
                        DataTable dtAdvanceLine = new DataTable("AdvanceLine");

                        dtAdvanceLine = getAdvanceLinesbyID(dt["AdvanceID"].ToString(), temp.Split("!_!")[1]);

                        if (dtAdvanceLine.Rows.Count > 0)
                        {
                            foreach (DataRow linedt in dtAdvanceLine.Rows)
                            {
                                AdvanceLine temp2 = new AdvanceLine();

                                temp2.AdvanceID = linedt["AdvanceID"].ToString();
                                temp2.LineNo = Convert.ToInt32(linedt["LineNum"]);
                                temp2.Details = linedt["Details"].ToString();
                                temp2.Amount = Convert.ToDecimal(linedt["Amount"]);
                                temp2.Status = linedt["AStatus"].ToString();

                                advanceLines.Add(temp2);
                            }
                        }

                        temp1.lines = new List<AdvanceLine>();
                        temp1.lines = advanceLines;

                        advance.Add(temp1);
                    }

                    res.Data = advance;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = true;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Fetch Empty";

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

        [HttpGet("getExpenseDatabyLocation/{locPage}")]
        public async Task<IActionResult> getExpenseDataTablebyLocation(string locPage)
        {
            ResultModel<List<Expense>> res = new ResultModel<List<Expense>>();
            List<Expense> expense = new List<Expense>();
            DataTable dtExpense = new DataTable("ExpenseData");
            IActionResult actionResult = null;

            try
            {
                var temp = Base64Decode(locPage);

                string denom = temp.Split("!_!")[0];
                string loc = temp.Split("!_!")[1].Equals("") ? "HO" : temp.Split("!_!")[1];
                string val = temp.Split("!_!")[2];
                string tp = temp.Split("!_!")[3];
                string filVal = temp.Split("!_!")[4];
                int page = Convert.ToInt32(temp.Split("!_!")[5]);

                dtExpense = getExpenseDatabyLocation(denom, loc, val, tp, filVal, page, _rowPerPage);

                if (dtExpense.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtExpense.Rows)
                    {
                        Expense temp1 = new Expense();
                        temp1.statusDetails = new();

                        temp1.ExpenseID = dt["ExpenseID"].ToString();
                        temp1.AdvanceID = dt["AdvanceID"].ToString();
                        temp1.LocationID = dt["LocationID"].ToString();
                        temp1.Approver = dt["Approver"].ToString();
                        temp1.ExpenseDate = Convert.ToDateTime(dt["ExpenseDate"]);
                        temp1.DepartmentID = dt["DepartmentID"].ToString();
                        temp1.ExpenseNIK = dt["ExpenseNIK"].ToString();
                        temp1.ExpenseNote = dt["ExpenseNote"].ToString();
                        temp1.ExpenseType = dt["ExpenseType"].ToString();
                        temp1.TypeAccount = dt["TypeAccount"].ToString();
                        temp1.ExpenseStatus = dt["ExpenseStatus"].ToString();
                        temp1.Applicant = dt["Applicant"].ToString();

                        temp1.statusDetails.submitDate = dt.IsNull("SubmitDate") ? DateTime.MinValue : Convert.ToDateTime(dt["SubmitDate"]);
                        temp1.statusDetails.submitUser = dt.IsNull("SubmitUser") ? "" : dt["SubmitUser"].ToString();
                        temp1.statusDetails.confirmDate = dt.IsNull("ConfirmDate") ? DateTime.MinValue : Convert.ToDateTime(dt["ConfirmDate"]);
                        temp1.statusDetails.confirmUser = dt.IsNull("ConfirmUser") ? "" : dt["ConfirmUser"].ToString();
                        temp1.statusDetails.rejectDate = dt.IsNull("RejectDate") ? DateTime.MinValue : Convert.ToDateTime(dt["RejectDate"]);
                        temp1.statusDetails.rejectUser = dt.IsNull("RejectUser") ? "" : dt["RejectUser"].ToString();

                        List<ExpenseLine> expenseLines = new List<ExpenseLine>();
                        DataTable dtExpenseLine = new DataTable("ExpenseLine");

                        dtExpenseLine = getExpenseLinesbyID(dt["ExpenseID"].ToString(), denom);

                        if (dtExpenseLine.Rows.Count > 0)
                        {
                            foreach (DataRow linedt in dtExpenseLine.Rows)
                            {
                                ExpenseLine temp2 = new ExpenseLine();

                                temp2.ExpenseID = linedt["ExpenseID"].ToString();
                                temp2.LineNo = Convert.ToInt32(linedt["LineNum"]);
                                temp2.Details = linedt["Details"].ToString();
                                temp2.Amount = Convert.ToDecimal(linedt["Amount"]);
                                temp2.ActualAmount = Convert.ToDecimal(linedt["ActualAmount"]);
                                //temp2.Attach = linedt["EAttach"].ToString();
                                //temp2.Attach = string.Empty;
                                temp2.Status = linedt["EStatus"].ToString();

                                expenseLines.Add(temp2);
                            }
                        }

                        temp1.lines = new List<ExpenseLine>();
                        temp1.lines = expenseLines;

                        expense.Add(temp1);
                    }

                    res.Data = expense;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = true;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Fetch Empty";

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

        [HttpGet("getReimburseDatabyLocation/{locPage}")]
        public async Task<IActionResult> getReimburseDataTablebyLocation(string locPage)
        {
            ResultModel<List<Reimburse>> res = new ResultModel<List<Reimburse>>();
            List<Reimburse> reimburse = new List<Reimburse>();
            DataTable dtReimburse = new DataTable("ReimburseData");
            IActionResult actionResult = null;

            try
            {
                var temp = Base64Decode(locPage);

                string denom = temp.Split("!_!")[0];
                string loc = temp.Split("!_!")[1].Equals("") ? "HO" : temp.Split("!_!")[1];
                string val = temp.Split("!_!")[2];
                string tp = temp.Split("!_!")[3];
                string filVal = temp.Split("!_!")[4];
                int page = Convert.ToInt32(temp.Split("!_!")[5]);

                dtReimburse = getReimburseDatabyLocation(denom, loc, val, tp, filVal, page, _rowPerPage);

                if (dtReimburse.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtReimburse.Rows)
                    {
                        Reimburse temp1 = new Reimburse();
                        temp1.statusDetails = new();

                        temp1.ReimburseID = dt["ReimburseID"].ToString();
                        temp1.LocationID = dt["LocationID"].ToString();
                        temp1.ReimburseNote = dt["ReimburseNote"].ToString();
                        temp1.ReimburseDate = Convert.ToDateTime(dt["ReimburseDate"]);
                        temp1.ReimburseStatus = dt["ReimburseStatus"].ToString();
                        temp1.Applicant = dt["Applicant"].ToString();

                        //temp1.statusDetails.confirmDate = Convert.ToDateTime(dt["ConfirmDate"]);
                        //temp1.statusDetails.confirmUser = dt["ConfirmUser"].ToString();
                        //temp1.statusDetails.verifyDate = Convert.ToDateTime(dt["VerifyDate"]);
                        //temp1.statusDetails.verifyUser = dt["VerifyUser"].ToString();
                        //temp1.statusDetails.releaseDate = Convert.ToDateTime(dt["ReleaseDate"]);
                        //temp1.statusDetails.releaseUser = dt["ReleaseUser"].ToString();
                        //temp1.statusDetails.approveDate = Convert.ToDateTime(dt["ApproveDate"]);
                        //temp1.statusDetails.approveUser = dt["ApproveUser"].ToString();
                        //temp1.statusDetails.claimDate = Convert.ToDateTime(dt["ClaimDate"]);
                        //temp1.statusDetails.claimUser = dt["ClaimUser"].ToString();
                        //temp1.statusDetails.rejectDate = Convert.ToDateTime(dt["RejectDate"]);
                        //temp1.statusDetails.rejectUser = dt["RejectUser"].ToString();

                        temp1.statusDetails.confirmDate = dt.IsNull("ConfirmDate") ? DateTime.MinValue : Convert.ToDateTime(dt["ConfirmDate"]);
                        temp1.statusDetails.confirmUser = dt.IsNull("ConfirmUser") ? "" : dt["ConfirmUser"].ToString();
                        temp1.statusDetails.verifyDate = dt.IsNull("VerifyDate") ? DateTime.MinValue : Convert.ToDateTime(dt["VerifyDate"]);
                        temp1.statusDetails.verifyUser = dt.IsNull("VerifyUser") ? "" : dt["VerifyUser"].ToString();
                        temp1.statusDetails.releaseDate = dt.IsNull("ReleaseDate") ? DateTime.MinValue : Convert.ToDateTime(dt["ReleaseDate"]);
                        temp1.statusDetails.releaseUser = dt.IsNull("ReleaseUser") ? "" : dt["ReleaseUser"].ToString();
                        temp1.statusDetails.approveDate = dt.IsNull("ApproveDate") ? DateTime.MinValue : Convert.ToDateTime(dt["ApproveDate"]);
                        temp1.statusDetails.approveUser = dt.IsNull("ApproveUser") ? "" : dt["ApproveUser"].ToString();
                        temp1.statusDetails.claimDate = dt.IsNull("ClaimDate") ? DateTime.MinValue : Convert.ToDateTime(dt["ClaimDate"]);
                        temp1.statusDetails.claimUser = dt.IsNull("ClaimUser") ? "" : dt["ClaimUser"].ToString();
                        temp1.statusDetails.resolveDate = dt.IsNull("ResolveDate") ? DateTime.MinValue : Convert.ToDateTime(dt["ResolveDate"]);
                        temp1.statusDetails.resolveUser = dt.IsNull("ResolveUser") ? "" : dt["ResolveUser"].ToString();
                        temp1.statusDetails.rejectDate = dt.IsNull("RejectDate") ? DateTime.MinValue : Convert.ToDateTime(dt["RejectDate"]);
                        temp1.statusDetails.rejectUser = dt.IsNull("RejectUser") ? "" : dt["RejectUser"].ToString();


                        List<ReimburseLine> reimburseLines = new List<ReimburseLine>();
                        DataTable dtReimburseLine = new DataTable("ReimburseLine");

                        dtReimburseLine = getReimburseLinesbyID(dt["ReimburseID"].ToString(), denom);

                        if (dtReimburseLine.Rows.Count > 0)
                        {
                            foreach (DataRow linedt in dtReimburseLine.Rows)
                            {
                                ReimburseLine temp2 = new ReimburseLine();

                                temp2.ReimburseID = linedt["ReimburseID"].ToString();
                                temp2.ExpenseID = linedt["ExpenseID"].ToString();
                                temp2.LineNo = Convert.ToInt32(linedt["LineNum"]);
                                temp2.AccountNo = linedt["AccountNo"].ToString();
                                temp2.Details = linedt["Details"].ToString();
                                temp2.Amount = Convert.ToDecimal(linedt["Amount"]);
                                temp2.ApprovedAmount = Convert.ToDecimal(linedt["ApprovedAmount"]);
                                //temp2.Attach = linedt["EAttach"].ToString();
                                //temp2.Attach = string.Empty;
                                temp2.Status = linedt["RStatus"].ToString();

                                reimburseLines.Add(temp2);
                            }
                        }

                        temp1.lines = new List<ReimburseLine>();
                        temp1.lines = reimburseLines;

                        reimburse.Add(temp1);
                    }

                    res.Data = reimburse;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = true;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Fetch Empty";

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

        //[HttpGet("getAdvanceLinesbyID/{AdvanceId}")]
        //public async Task<IActionResult> getAdvanceLinesTablebyID(string AdvanceId)
        //{
        //    ResultModel<List<AdvanceLine>> res = new ResultModel<List<AdvanceLine>>();
        //    List<AdvanceLine> advanceLines = new List<AdvanceLine>();
        //    DataTable dtAdvanceLine = new DataTable("AdvanceLine");
        //    IActionResult actionResult = null;

        //    try
        //    {
        //        dtAdvanceLine = getAdvanceLinesbyID(AdvanceId);

        //        if (dtAdvanceLine.Rows.Count > 0)
        //        {
        //            foreach (DataRow dt in dtAdvanceLine.Rows)
        //            {
        //                AdvanceLine temp = new AdvanceLine();

        //                temp.AdvanceID = dt["AdvanceID"].ToString();
        //                temp.LineNo = Convert.ToInt32(dt["LineNum"]);
        //                temp.Details = dt["Details"].ToString();
        //                temp.Amount = Convert.ToDecimal(dt["Amount"]);
        //                temp.Status = dt["AStatus"].ToString();

        //                advanceLines.Add(temp);
        //            }

        //            res.Data = advanceLines;
        //            res.isSuccess = true;
        //            res.ErrorCode = "00";
        //            res.ErrorMessage = "";

        //            actionResult = Ok(res);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Data = null;
        //        res.isSuccess = false;
        //        res.ErrorCode = "99";
        //        res.ErrorMessage = ex.Message;

        //        actionResult = BadRequest(res);
        //    }

        //    return actionResult;
        //}

        [HttpGet("getPettyCashOutstandingAmountandLocBalanceDetails/{loc}")]
        public async Task<IActionResult> getPettyCashOutstandingAmount(string loc)
        {
            ResultModel<LocationBalanceDetails> res = new ResultModel<LocationBalanceDetails>();
            DataTable dtReimburseOutstanding = new DataTable("ReimburseOutstanding");
            DataTable dtBalanceDetails = new DataTable("BalanceDetails");
            IActionResult actionResult = null;

            try
            {
                res.Data = new();
                res.Data.outstandingBalance = new();
                res.Data.balanceDetails = new();

                // outstanding
                res.Data.outstandingBalance.locationID = loc;
                res.Data.outstandingBalance.locationOnhandAmount = getLocationOnhandAmount(loc);
                res.Data.outstandingBalance.advanceOutstandingAmount = getAdvanceOutstandingAmount(loc);
                res.Data.outstandingBalance.expenseOutstandingAmount = getExpenseOutstandingAmount(loc);
                res.Data.outstandingBalance.advanceApprovedAmount = getAdvanceApprovedAmount(loc);
                res.Data.outstandingBalance.expenseApprovedAmount = getExpenseApprovedAmount(loc);
                res.Data.CutOffDate = getCutoffDate(loc);

                dtReimburseOutstanding = getReimburseOutstandingAmount(loc);

                if (dtReimburseOutstanding.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtReimburseOutstanding.Rows)
                    {
                        res.Data.outstandingBalance.reimbursementReqOutstandingAmount = Convert.ToDecimal(dt["amtRequest"]);
                        res.Data.outstandingBalance.reimbursementApvOutstandingAmount = Convert.ToDecimal(dt["amtApv"]);
                        res.Data.outstandingBalance.reimbursementApvRejectedAmount = Convert.ToDecimal(dt["amtRejected"]);
                    }
                }

                res.Data.outstandingBalance.lastFetch = DateTime.Now;

                // location balance details
                res.Data.balanceDetails.LocationID = loc;

                dtBalanceDetails = getLocationBudgetDetails(loc);

                if (dtBalanceDetails.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtBalanceDetails.Rows)
                    {
                        res.Data.balanceDetails.BudgetAmount = Convert.ToDecimal(dt["Budget"]);
                        res.Data.balanceDetails.LatestAuditUser = dt["AuditUser"].ToString();
                        res.Data.balanceDetails.AuditDate = Convert.ToDateTime(dt["AuditActionDate"]);
                    }
                }

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

        //[HttpGet("getLocationBudgetDetails/{loc}")]
        //public async Task<IActionResult> getLocationBudgetDetailsData(string loc)
        //{
        //    ResultModel<BalanceDetails> res = new ResultModel<BalanceDetails>();
        //    DataTable dtBalanceDetails = new DataTable("BalanceDetails");
        //    IActionResult actionResult = null;

        //    try
        //    {
        //        res.Data = new();

        //        res.Data.LocationID = loc;

        //        dtBalanceDetails = getLocationBudgetDetails(loc);

        //        if (dtBalanceDetails.Rows.Count > 0)
        //        {
        //            foreach (DataRow dt in dtBalanceDetails.Rows)
        //            {
        //                res.Data.BudgetAmount = Convert.ToDecimal(dt["Budget"]);
        //                res.Data.LatestAuditUser = dt["AuditUser"].ToString();
        //                res.Data.AuditDate = Convert.ToDateTime(dt["AuditActionDate"]);
        //            }
        //        }

        //        res.isSuccess = true;
        //        res.ErrorCode = "00";
        //        res.ErrorMessage = "";

        //        actionResult = Ok(res);
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Data = null;
        //        res.isSuccess = false;
        //        res.ErrorCode = "99";
        //        res.ErrorMessage = ex.Message;

        //        actionResult = BadRequest(res);
        //    }
            
        //    return actionResult;
        //}

        [HttpPost("updateLocationBudget")]
        public async Task<IActionResult> updateLocationBudgetData(QueryModel<BalanceDetails> data)
        {
            ResultModel<QueryModel<BalanceDetails>> res = new ResultModel<QueryModel<BalanceDetails>>();
            IActionResult actionResult = null;

            try
            {
                createPettyCashTableSchema(data.Data.LocationID);
                updateLocationBudget(data);

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

        [HttpPost("updateLocationCutoffDate")]
        public async Task<IActionResult> updateLocationCutoffDateData(QueryModel<CutoffDetails> data)
        {
            ResultModel<QueryModel<CutoffDetails>> res = new ResultModel<QueryModel<CutoffDetails>>();
            IActionResult actionResult = null;

            try
            {
                updateLocationCutoffDate(data);

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

        [HttpGet("getModulePageSize/{Table}")]
        public async Task<IActionResult> getModulePageSize(string Table)
        {
            ResultModel<int> res = new ResultModel<int>();
            IActionResult actionResult = null;

            try
            {
                string tbname = Base64Decode(Table).Split("!_!")[0];
                string tbcol = Base64Decode(Table).Split("!_!")[1];
                string value = Base64Decode(Table).Split("!_!")[2];
                string tp = Base64Decode(Table).Split("!_!")[3];
                string filVal = Base64Decode(Table).Split("!_!")[4];
                string loc = Base64Decode(Table).Split("!_!")[5];

                res.Data = getModuleNumberOfPage(_rowPerPage, tbname, tbcol, value, tp, filVal, loc);

                res.isSuccess = true;
                res.ErrorCode = "00";
                res.ErrorMessage = "";

                actionResult = Ok(res);
            }
            catch (Exception ex)
            {
                res.Data = 0;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        [HttpGet("getCoabyModule/{moduleName}")]
        public async Task<IActionResult> getCoabyModuleData(string moduleName)
        {
            ResultModel<List<Account>> res = new ResultModel<List<Account>>();
            List<Account> accountLines = new List<Account>();
            DataTable dtAccount = new DataTable("Account");
            IActionResult actionResult = null;

            try
            {
                dtAccount = getCoabyModule(moduleName);

                if (dtAccount.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtAccount.Rows)
                    {
                        Account temp = new Account();

                        temp.AccountCode = dt["AccountNo"].ToString();
                        temp.AccountDescription = dt["AccDescription"].ToString();

                        accountLines.Add(temp);
                    }

                    res.Data = accountLines;
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

        [HttpGet("getMailingDetails/{param}")]
        public async Task<IActionResult> getMailingDetailsData(string param)
        {
            ResultModel<Mailing> res = new ResultModel<Mailing>();
            DataTable dtMailingList = new DataTable("Mailing");
            IActionResult actionResult = null;

            try
            {
                string temp = Base64Decode(param);

                string mod = temp.Split("!_!")[0];
                string actName = temp.Split("!_!")[1];
                string loc = temp.Split("!_!")[2].Equals("") ? "HO" : temp.Split("!_!")[2];

                dtMailingList = getMailingDetails(mod, actName, loc);

                if (dtMailingList.Rows.Count > 0)
                {
                    res.Data = new();

                    foreach (DataRow dt in dtMailingList.Rows)
                    {
                        res.Data.ModuleName = dt["ModuleName"].ToString();
                        res.Data.ActionName = dt["ActionName"].ToString();
                        res.Data.Receiver = dt.IsNull("Receiver") ? "" : dt["Receiver"].ToString();
                        //res.Data.MailTemplate = dt.IsNull("MailTemplate") ? "" : dt["MailTemplate"].ToString();
                        res.Data.LocationID = dt.IsNull("LocationID") ? "" : dt["LocationID"].ToString();
                        res.Data.MailSubject = dt["MailSubject"].ToString();
                        res.Data.MailBeginningBody = dt.IsNull("BeginningBody") ? "" : dt["BeginningBody"].ToString();
                        res.Data.MailMainBody = dt.IsNull("MainBody") ? "" : dt["MainBody"].ToString();
                        res.Data.MailFooter = dt.IsNull("Footer") ? "" : dt["Footer"].ToString();
                        res.Data.MailNote = dt.IsNull("MailNote") ? "" : dt["MailNote"].ToString();
                    }

                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.isSuccess = true;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Empty Data";

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

        //
    }

    [Route("api/DA/CashierLogbook")]
    [ApiController]
    public class CashierLogbookController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _conString;
        private readonly int _rowPerPage;

        public CashierLogbookController(IConfiguration config)
        {
            _configuration = config;
            _conString = _configuration.GetValue<string>("ConnectionStrings:Bpi");
            _rowPerPage = _configuration.GetValue<int>("Paging:CashierLogbook:RowPerPage");
        }
        
        public static DataTable ListToDataTable<T>(List<T> list, string auditUser, string auditAction, DateTime auditDate, string _tableName)
        {
            DataTable dt = new DataTable(_tableName);

            foreach (PropertyInfo info in typeof(T).GetProperties())
            {
                dt.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            dt.Columns.Add(new DataColumn("AuditUser", Nullable.GetUnderlyingType(auditUser.GetType()) ?? auditUser.GetType()));
            dt.Columns.Add(new DataColumn("AuditAction", Nullable.GetUnderlyingType(auditAction.GetType()) ?? auditAction.GetType()));
            dt.Columns.Add(new DataColumn("AuditActionDate", Nullable.GetUnderlyingType(auditDate.GetType()) ?? auditDate.GetType()));

            foreach (T t in list)
            {
                DataRow row = dt.NewRow();

                foreach (PropertyInfo info in typeof(T).GetProperties())
                {
                    row[info.Name] = info.GetValue(t, null) ?? DBNull.Value;
                }
                row["AuditUser"] = auditUser;
                row["AuditAction"] = auditAction;
                row["AuditActionDate"] = auditDate;

                dt.Rows.Add(row);
            }
            return dt;
        }

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

        internal DataTable createBrankasActionLogData(QueryModel<CashierLogAction> data)
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
                    command.CommandText = "[createBrankasActionLogData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LogID", data.Data.LogID);
                    command.Parameters.AddWithValue("@LocationID", data.Data.LocationID);
                    command.Parameters.AddWithValue("@UserEmail", data.Data.UserEmail);
                    command.Parameters.AddWithValue("@LogAction", data.Data.LogAction);
                    command.Parameters.AddWithValue("@ActionDate", data.Data.ActionDate);
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

            return dt;
        }

        internal bool createCashierLogbookTableSchema(string objName)
        {
            bool conBool = false;
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[createBrankasSchemaTables]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@objectName", objName);

                    int ret = command.ExecuteNonQuery();

                    if (ret > 0)
                        conBool = true;
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

        internal DataTable createIDData(string docType)
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
                    command.CommandText = "[createID]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@DocumentName", docType);

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

        internal DataTable getShiftbyModule(string moduleName)
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
                    command.CommandText = "[getShiftbyModule]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ModuleName", moduleName);

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

        internal DataTable getAmountCategories()
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
                    command.CommandText = "[getBrankasAmountCategoriesData]";
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

        internal DataTable getAmountSubCategories()
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
                    command.CommandText = "[getBrankasAmountSubCategoriesData]";
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

        internal DataTable getAmountType()
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
                    command.CommandText = "[getBrankasAmountTypeData]";
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

        internal void createLogData(QueryModel<CashierLogDataConv> data, string type)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[createBrankasLogbookData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LogType", type);
                    command.Parameters.AddWithValue("@LogID", data.Data.LogID);
                    command.Parameters.AddWithValue("@LocationID", data.Data.LocationID);
                    command.Parameters.AddWithValue("@Applicant", data.Data.Applicant);
                    command.Parameters.AddWithValue("@LogDate", data.Data.LogDate);
                    command.Parameters.AddWithValue("@LogStatus", data.Data.LogStatus);
                    command.Parameters.AddWithValue("@LogStatusDate", data.Data.LogStatusDate);
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

        //internal void createLogHeaderData(QueryModel<CashierLogCategoryDetail> data, string loc)
        //{
        //    using (SqlConnection con = new SqlConnection(_conString))
        //    {
        //        con.Open();
        //        SqlCommand command = new SqlCommand();

        //        try
        //        {
        //            command.Connection = con;
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.CommandText = "[createBrankasCategoryDetails]";
        //            command.CommandTimeout = 1000;

        //            command.Parameters.Clear();
        //            command.Parameters.AddWithValue("@LogID", data.Data.LogID);
        //            command.Parameters.AddWithValue("@LocationID", loc);
        //            command.Parameters.AddWithValue("@BrankasCategoryID", data.Data.BrankasCategoryID);
        //            command.Parameters.AddWithValue("@AmountCategoryID", data.Data.AmountCategoryID);
        //            command.Parameters.AddWithValue("@HeaderAmount", data.Data.HeaderAmount);
        //            command.Parameters.AddWithValue("@ActualAmount", data.Data.ActualAmount);
        //            command.Parameters.AddWithValue("@AuditUser", data.userEmail);
        //            command.Parameters.AddWithValue("@AuditAction", data.userAction);
        //            command.Parameters.AddWithValue("@AuditActionDate", data.userActionDate);

        //            command.ExecuteNonQuery();
        //        }
        //        catch (SqlException ex)
        //        {
        //            throw ex;
        //        }
        //        finally
        //        {
        //            con.Close();
        //        }
        //    }
        //}

        internal void createLogHeaderData(DataTable data, string loc)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[createBrankasCategoryDetails]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", loc);
                    command.Parameters.AddWithValue("@BrankasCategoryDetailData", data);

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

        //internal void createLogLineData(QueryModel<CashierLogLineDetail> data, string loc)
        //{
        //    using (SqlConnection con = new SqlConnection(_conString))
        //    {
        //        con.Open();
        //        SqlCommand command = new SqlCommand();

        //        try
        //        {
        //            command.Connection = con;
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.CommandText = "[createBrankasCategoryLine]";
        //            command.CommandTimeout = 1000;

        //            command.Parameters.Clear();
        //            command.Parameters.AddWithValue("@LineAction", "ACT");
        //            command.Parameters.AddWithValue("@LocationID", loc);
        //            command.Parameters.AddWithValue("@BrankasCategoryID", data.Data.BrankasCategoryID);
        //            command.Parameters.AddWithValue("@LineNum", data.Data.LineNo);
        //            command.Parameters.AddWithValue("@AmountSubCategoryID", data.Data.AmountSubCategoryID);
        //            command.Parameters.AddWithValue("@AmountType", data.Data.AmountType);
        //            command.Parameters.AddWithValue("@ShiftID", data.Data.ShiftID);
        //            command.Parameters.AddWithValue("@LineAmount", data.Data.LineAmount);
        //            command.Parameters.AddWithValue("@AuditUser", data.userEmail);
        //            command.Parameters.AddWithValue("@AuditAction", data.userAction);
        //            command.Parameters.AddWithValue("@AuditActionDate", data.userActionDate);

        //            command.ExecuteNonQuery();
        //        }
        //        catch (SqlException ex)
        //        {
        //            throw ex;
        //        }
        //        finally
        //        {
        //            con.Close();
        //        }
        //    }
        //}

        internal void createLogLineData(DataTable data, string loc, string logId)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[createBrankasCategoryLine]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", loc);
                    command.Parameters.AddWithValue("@LogID", logId);
                    command.Parameters.AddWithValue("@BrankasCategoryLineData", data);

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

        internal void createBrankasApproveLogData(DataTable data, string loc)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[createBrankasApproveLogData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", loc);
                    command.Parameters.AddWithValue("@BrankasApproveLogData", data);

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

        internal void editBrankasApproveLogOnConfirmData(QueryModel<CashierLogApproval> data)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[editBrankasApproveLogOnConfirmData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LogID", data.Data.LogID);
                    command.Parameters.AddWithValue("@LocationID", data.Data.LocationID);
                    command.Parameters.AddWithValue("@ShiftID", data.Data.ShiftID);
                    command.Parameters.AddWithValue("@CreateUser", data.Data.CreateUser);
                    command.Parameters.AddWithValue("@CreateDate", data.Data.CreateDate);
                    command.Parameters.AddWithValue("@ConfirmUser", data.Data.ConfirmUser);
                    command.Parameters.AddWithValue("@ConfirmDate", data.Data.ConfirmDate);
                    command.Parameters.AddWithValue("@ApproveNote", data.Data.ApproveNote);
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

        internal void deleteBrankasDetailsandLinesByLogIDData(string logId, string loc)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[deleteBrankasDetailsandLinesByLogID]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LogID", logId);
                    command.Parameters.AddWithValue("@LocationID", loc);

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

        internal void updateApproveLogData(DataTable data, string loc, string logId)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[createBrankasCategoryLine]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", loc);
                    command.Parameters.AddWithValue("@LogID", logId);
                    command.Parameters.AddWithValue("@BrankasCategoryLineData", data);

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

        internal void updateBrankasDocumentStatusData(string logId, string loc, string statVal, string user, string act, DateTime actDate)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[editBrankasDocumentStatus]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LogID", logId);
                    command.Parameters.AddWithValue("@LocationID", loc);
                    command.Parameters.AddWithValue("@StatusValue", statVal);
                    command.Parameters.AddWithValue("@AuditUser", user);
                    command.Parameters.AddWithValue("@AuditAction", act);
                    command.Parameters.AddWithValue("@AuditActionDate", actDate);

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

        internal DataTable getLogData(string logType, string loc, string statValue, string conditions, int pageNo, int rowPerPage)
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
                    command.CommandText = "[getBrankasLogData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LogType", logType);
                    command.Parameters.AddWithValue("@LocationID", loc);
                    command.Parameters.AddWithValue("@StatusValue", statValue);
                    command.Parameters.AddWithValue("@Conditions", conditions);
                    command.Parameters.AddWithValue("@PageNo", pageNo);
                    command.Parameters.AddWithValue("@RowPerPage", rowPerPage);

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

        internal DataTable getLogHeaderData(string logID, string loc)
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
                    command.CommandText = "[getBrankasLogHeaderDetailbyLogID]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LogID", logID);
                    command.Parameters.AddWithValue("@LocationID", loc);

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

        internal DataTable getLogLineData(string logCatID, string loc)
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
                    command.CommandText = "[getBrankasHeaderLineDetailbyCategoryID]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@BrankasCategoryID", logCatID);
                    command.Parameters.AddWithValue("@LocationID", loc);

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

        internal DataTable getLogApprovalData(string logID, string loc)
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
                    command.CommandText = "[getBrankasApproveDatabyLogID]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LogID", logID);
                    command.Parameters.AddWithValue("@LocationID", loc);

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

        internal DataTable getBrankasActionLogData(string loc, string orderBy, string filType, string filVal, int pageNo, int rowPerPage)
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
                    command.CommandText = "[getBrankasActionLogData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", loc);
                    command.Parameters.AddWithValue("@OrderBy", orderBy);
                    command.Parameters.AddWithValue("@FilterType", filType);
                    command.Parameters.AddWithValue("@FilterValue", filVal);
                    command.Parameters.AddWithValue("@PageNo", pageNo);
                    command.Parameters.AddWithValue("@RowPerPage", rowPerPage);

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

        internal int getModuleNumberOfPage(string TbName, string loc, string conditions, int rowPerPage)
        {
            int conInt = 0;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[getBrankasModulePageSize]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@TbName", TbName);
                    command.Parameters.AddWithValue("@LocationID", loc);
                    command.Parameters.AddWithValue("@Conditions", conditions);
                    command.Parameters.AddWithValue("@RowPerPage", rowPerPage);

                    var data = command.ExecuteScalar();
                    conInt = Convert.ToInt32(data);

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
            return conInt;
        }

        internal int getNumberofLogExisting(string loc, string conditions)
        {
            int conInt = 0;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[getNumberofLogExist]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", loc);
                    command.Parameters.AddWithValue("@Conditions", conditions);

                    var data = command.ExecuteScalar();
                    conInt = Convert.ToInt32(data);

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
            return conInt;
        }

        // http

        [HttpPost("createLogData")]
        public async Task<IActionResult> createLogDataTable(QueryModel<CashierLogData> data)
        {
            ResultModel<QueryModel<CashierLogData>> res = new ResultModel<QueryModel<CashierLogData>>();
            DataTable dtMainIdentity = new DataTable("Identity");
            string logID = string.Empty;
            string headerID = string.Empty;
            IActionResult actionResult = null;

            try
            {
                createCashierLogbookTableSchema(data.Data.LocationID);
                
                dtMainIdentity = createIDData("CashierLogbook");

                if (dtMainIdentity.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtMainIdentity.Rows)
                    {
                        string zero = string.Empty;

                        for (int i = 0; i < (5 - Convert.ToInt32(dt["IDLength"])); i++)
                        {
                            zero = zero + "0";
                        }

                        logID = dt["Code"].ToString() + DateTime.Now.Year.ToString() + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + zero + dt["DocNumber"].ToString() + dt["Parity"].ToString();

                    }
                }

                data.Data.LogID = logID;

                QueryModel<CashierLogDataConv> temp1 = new();
                temp1.Data = new();

                temp1.Data.LogID = data.Data.LogID;
                temp1.Data.LocationID = data.Data.LocationID;
                temp1.Data.Applicant = data.Data.Applicant;
                temp1.Data.LogDate = data.Data.LogDate;
                temp1.Data.LogStatus = data.Data.LogStatus;
                temp1.Data.LogStatusDate = data.Data.LogStatusDate;
                temp1.userEmail = data.userEmail;
                temp1.userAction = data.userAction;
                temp1.userActionDate = data.userActionDate;

                createLogData(temp1, data.Data.LogType);

                List<CashierLogLineDetailConv> lines = new();
                List<CashierLogCategoryDetailConv> headers = new();

                foreach (var hd in data.Data.header)
                {
                    DataTable dtHeaderIdentity = new DataTable("HIdentity");
                    dtHeaderIdentity = createIDData("CashierLogHeader");

                    if (dtHeaderIdentity.Rows.Count > 0)
                    {
                        foreach (DataRow dt in dtHeaderIdentity.Rows)
                        {
                            string zero = string.Empty;

                            for (int i = 0; i < (5 - Convert.ToInt32(dt["IDLength"])); i++)
                            {
                                zero = zero + "0";
                            }

                            headerID = dt["Code"].ToString() + DateTime.Now.Year.ToString() + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + zero + dt["DocNumber"].ToString() + dt["Parity"].ToString();

                        }
                    }

                    CashierLogCategoryDetailConv temp2 = new();

                    temp2.LogID = data.Data.LogID;
                    temp2.BrankasCategoryID = headerID;
                    temp2.AmountCategoryID = hd.AmountCategoryID;
                    temp2.HeaderAmount = hd.lines.Sum(x => x.LineAmount);
                    temp2.ActualAmount = hd.ActualAmount;
                    temp2.CategoryNote = hd.CategoryNote;

                    headers.Add(temp2);

                    int c = 1;

                    foreach (var dt in hd.lines)
                    {
                        CashierLogLineDetailConv temp3 = new();

                        temp3.BrankasCategoryID = headerID;
                        temp3.LineNum = c;
                        temp3.AmountSubCategoryID = dt.AmountSubCategoryID;
                        temp3.AmountType = dt.AmountType;
                        temp3.ShiftID = dt.ShiftID;
                        temp3.LineAmount = dt.LineAmount;

                        c++;
                        lines.Add(temp3);
                    }
                }

                //QueryModel<CashierLogDataConv> temp1 = new();
                //temp1.Data = new();

                //temp1.Data.LogID = data.Data.LogID;
                //temp1.Data.LocationID = data.Data.LocationID;
                //temp1.Data.Applicant = data.Data.Applicant;
                //temp1.Data.LogDate = data.Data.LogDate;
                //temp1.Data.LogStatus = data.Data.LogStatus;
                //temp1.Data.LogStatusDate = data.Data.LogStatusDate;
                //temp1.userEmail = data.userEmail;
                //temp1.userAction = data.userAction;
                //temp1.userActionDate = data.userActionDate;

                //createLogData(temp1, data.Data.LogType);

                //List<CashierLogCategoryDetailConv> headers = new();

                //foreach (var dt in data.Data.header)
                //{
                //    CashierLogCategoryDetailConv temp = new();

                //    temp.LogID = data.Data.LogID;
                //    temp.BrankasCategoryID = dt.BrankasCategoryID;
                //    temp.AmountCategoryID = dt.AmountCategoryID;
                //    temp.HeaderAmount = dt.HeaderAmount;
                //    temp.ActualAmount = dt.ActualAmount;
                //    temp.CategoryNote = dt.CategoryNote;

                //    headers.Add(temp);
                //}

                createLogHeaderData(ListToDataTable<CashierLogCategoryDetailConv>(headers, data.userEmail, data.userAction, data.userActionDate, "Headers"), data.Data.LocationID);

                //List<CashierLogLineDetailConv> lines = new();

                //foreach (var dt in data.Data.header.SelectMany(x => x.lines))
                //{
                //    CashierLogLineDetailConv temp = new();

                //    temp.BrankasCategoryID = dt.BrankasCategoryID;
                //    temp.LineNum = dt.LineNo;
                //    temp.AmountSubCategoryID = dt.AmountSubCategoryID;
                //    temp.AmountType = dt.AmountType;
                //    temp.ShiftID = dt.ShiftID;
                //    temp.LineAmount = dt.LineAmount;

                //    lines.Add(temp);
                //}

                createLogLineData(ListToDataTable<CashierLogLineDetailConv>(lines, data.userEmail, data.userAction, data.userActionDate, "Lines"), data.Data.LocationID, logID);

                List<CashierLogApproval> apvLines = new();

                foreach (var apv in data.Data.approvals)
                {
                    apvLines.Add(new CashierLogApproval
                    {
                        LogID = logID,
                        LocationID = apv.LocationID,
                        ShiftID = apv.ShiftID,
                        CreateUser = apv.CreateUser,
                        CreateDate = apv.CreateDate,
                        ConfirmUser = apv.ConfirmUser,
                        ConfirmDate = apv.ConfirmDate,
                        ApproveNote = apv.ApproveNote
                    });
                }

                createBrankasApproveLogData(ListToDataTable<CashierLogApproval>(apvLines, data.userEmail, data.userAction, data.userActionDate, "Apv"), data.Data.LocationID);

                //foreach (var dt in data.Data.header)
                //{
                //    QueryModel<CashierLogCategoryDetail> temp2 = new();
                //    temp2.Data = new();

                //    temp2.Data = dt;
                //    temp2.userEmail = data.userEmail;
                //    temp2.userAction = data.userAction;
                //    temp2.userActionDate = data.userActionDate;

                //    createLogHeaderData(temp2, data.Data.LocationID);

                //    foreach (var dtLine in dt.lines)
                //    {
                //        QueryModel<CashierLogLineDetail> temp3 = new();
                //        temp3.Data = new();

                //        temp3.Data = dtLine;
                //        temp3.userEmail = data.userEmail;
                //        temp3.userAction = data.userAction;
                //        temp3.userActionDate = data.userActionDate;

                //        createLogLineData(temp3, data.Data.LocationID);
                //    }
                //}

                QueryModel<CashierLogAction> logAction = new();
                logAction.Data = new();

                logAction.Data.LogID = logID;
                logAction.Data.LocationID = data.Data.LocationID;
                logAction.Data.UserEmail = data.userEmail;
                logAction.Data.LogAction = "Created";
                logAction.Data.ActionDate = DateTime.Now;
                logAction.userEmail = data.userEmail;
                logAction.userAction = data.userAction;
                logAction.userActionDate = data.userActionDate;

                createBrankasActionLogData(logAction);

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

        [HttpPost("editLogData")]
        public async Task<IActionResult> editLogData(QueryModel<CashierLogData> data)
        {
            ResultModel<QueryModel<CashierLogData>> res = new ResultModel<QueryModel<CashierLogData>>();
            //string logID = string.Empty;
            string headerID = string.Empty;
            IActionResult actionResult = null;

            try
            {
                //dtMainIdentity = createIDData("CashierLogbook");

                //if (dtMainIdentity.Rows.Count > 0)
                //{
                //    foreach (DataRow dt in dtMainIdentity.Rows)
                //    {
                //        string zero = string.Empty;

                //        for (int i = 0; i < (5 - Convert.ToInt32(dt["IDLength"])); i++)
                //        {
                //            zero = zero + "0";
                //        }

                //        logID = dt["Code"].ToString() + DateTime.Now.Year.ToString() + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + zero + dt["DocNumber"].ToString() + dt["Parity"].ToString();

                //    }
                //}

                //data.Data.LogID = logID;

                QueryModel<CashierLogDataConv> temp1 = new();
                temp1.Data = new();

                temp1.Data.LogID = data.Data.LogID;
                temp1.Data.LocationID = data.Data.LocationID;
                temp1.Data.Applicant = data.Data.Applicant;
                temp1.Data.LogDate = data.Data.LogDate;
                temp1.Data.LogStatus = data.Data.LogStatus;
                temp1.Data.LogStatusDate = data.Data.LogStatusDate;
                temp1.userEmail = data.userEmail;
                temp1.userAction = data.userAction;
                temp1.userActionDate = data.userActionDate;

                createLogData(temp1, data.Data.LogType);

                deleteBrankasDetailsandLinesByLogIDData(data.Data.LogID, data.Data.LocationID);

                //List<CashierLogLineDetailConv> linesUpdated = new();
                //List<CashierLogCategoryDetailConv> headersUpdated = new();
                //List<CashierLogLineDetailConv> linesNew = new();
                //List<CashierLogCategoryDetailConv> headersNew = new();

                List<CashierLogLineDetailConv> lines = new();
                List<CashierLogCategoryDetailConv> header = new();

                foreach (var hd in data.Data.header)
                {
                    //if (hd.BrankasCategoryID.IsNullOrEmpty())
                    //{
                        DataTable dtHeaderIdentity = new DataTable("HIdentity");
                        dtHeaderIdentity = createIDData("CashierLogHeader");

                        if (dtHeaderIdentity.Rows.Count > 0)
                        {
                            foreach (DataRow dt in dtHeaderIdentity.Rows)
                            {
                                string zero = string.Empty;

                                for (int i = 0; i < (5 - Convert.ToInt32(dt["IDLength"])); i++)
                                {
                                    zero = zero + "0";
                                }

                                headerID = dt["Code"].ToString() + DateTime.Now.Year.ToString() + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + zero + dt["DocNumber"].ToString() + dt["Parity"].ToString();

                            }
                        }

                        CashierLogCategoryDetailConv temp2 = new();

                        temp2.LogID = data.Data.LogID;
                        temp2.BrankasCategoryID = headerID;
                        temp2.AmountCategoryID = hd.AmountCategoryID;
                        temp2.HeaderAmount = hd.lines.Sum(x => x.LineAmount);
                        temp2.ActualAmount = hd.ActualAmount;
                        temp2.CategoryNote = hd.CategoryNote;

                        header.Add(temp2);

                        int c = 1;

                        foreach (var dt in hd.lines)
                        {
                            CashierLogLineDetailConv temp3 = new();

                            temp3.BrankasCategoryID = headerID;
                            temp3.LineNum = c;
                            temp3.AmountSubCategoryID = dt.AmountSubCategoryID;
                            temp3.AmountType = dt.AmountType;
                            temp3.ShiftID = dt.ShiftID;
                            temp3.LineAmount = dt.LineAmount;

                            c++;
                            lines.Add(temp3);
                        }

                        //createLogHeaderData(ListToDataTable<CashierLogCategoryDetailConv>(headersNew, data.userEmail, data.userAction, data.userActionDate, "Headers"), data.Data.LocationID);

                        //createLogLineData(ListToDataTable<CashierLogLineDetailConv>(linesNew, data.userEmail, data.userAction, data.userActionDate, "Lines"), data.Data.LocationID, data.Data.LogID);
                    //}
                    //else
                    //{
                    //    CashierLogCategoryDetailConv temp2 = new();

                    //    temp2.LogID = data.Data.LogID;
                    //    temp2.BrankasCategoryID = hd.BrankasCategoryID;
                    //    temp2.AmountCategoryID = hd.AmountCategoryID;
                    //    temp2.HeaderAmount = hd.lines.Sum(x => x.LineAmount);
                    //    temp2.ActualAmount = hd.ActualAmount;
                    //    temp2.CategoryNote = hd.CategoryNote;

                    //    header.Add(temp2);

                    //    int c = 1;

                    //    foreach (var dt in hd.lines)
                    //    {
                    //        CashierLogLineDetailConv temp3 = new();

                    //        temp3.BrankasCategoryID = hd.BrankasCategoryID;
                    //        temp3.LineNum = c;
                    //        temp3.AmountSubCategoryID = dt.AmountSubCategoryID;
                    //        temp3.AmountType = dt.AmountType;
                    //        temp3.ShiftID = dt.ShiftID;
                    //        temp3.LineAmount = dt.LineAmount;

                    //        c++;
                    //        lines.Add(temp3);
                    //    }

                    //    //createLogHeaderData(ListToDataTable<CashierLogCategoryDetailConv>(headersUpdated, data.userEmail, data.userAction, data.userActionDate, "Headers"), data.Data.LocationID);

                    //    //createLogLineData(ListToDataTable<CashierLogLineDetailConv>(linesUpdated, data.userEmail, data.userAction, data.userActionDate, "Lines"), data.Data.LocationID, data.Data.LogID);
                    //}
                }

                createLogHeaderData(ListToDataTable<CashierLogCategoryDetailConv>(header, data.userEmail, data.userAction, data.userActionDate, "Headers"), data.Data.LocationID);
                createLogLineData(ListToDataTable<CashierLogLineDetailConv>(lines, data.userEmail, data.userAction, data.userActionDate, "Lines"), data.Data.LocationID, data.Data.LogID);

                List<CashierLogApproval> apvLines = new();

                foreach (var apv in data.Data.approvals)
                {
                    apvLines.Add(new CashierLogApproval
                    {
                        LogID = data.Data.LogID,
                        LocationID = apv.LocationID,
                        ShiftID = apv.ShiftID,
                        CreateUser = apv.CreateUser,
                        CreateDate = apv.CreateDate,
                        ConfirmUser = apv.ConfirmUser,
                        ConfirmDate = apv.ConfirmDate,
                        ApproveNote = apv.ApproveNote
                    });
                }

                createBrankasApproveLogData(ListToDataTable<CashierLogApproval>(apvLines, data.userEmail, data.userAction, data.userActionDate, "Apv"), data.Data.LocationID);

                //createLogHeaderData(ListToDataTable<CashierLogCategoryDetailConv>(headersUpdated, data.userEmail, data.userAction, data.userActionDate, "Headers"), data.Data.LocationID);
                //createLogLineData(ListToDataTable<CashierLogLineDetailConv>(linesUpdated, data.userEmail, data.userAction, data.userActionDate, "Lines"), data.Data.LocationID, data.Data.LogID);

                //DataTable dtHeaderIdentity = new DataTable("HIdentity");
                //dtHeaderIdentity = createIDData("CashierLogHeader");

                //if (dtHeaderIdentity.Rows.Count > 0)
                //{
                //    foreach (DataRow dt in dtHeaderIdentity.Rows)
                //    {
                //        string zero = string.Empty;

                //        for (int i = 0; i < (5 - Convert.ToInt32(dt["IDLength"])); i++)
                //        {
                //            zero = zero + "0";
                //        }

                //        headerID = dt["Code"].ToString() + DateTime.Now.Year.ToString() + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + zero + dt["DocNumber"].ToString() + dt["Parity"].ToString();

                //    }
                //}

                //createLogHeaderData(ListToDataTable<CashierLogCategoryDetailConv>(headers, data.userEmail, data.userAction, data.userActionDate, "Headers"), data.Data.LocationID);

                //createLogLineData(ListToDataTable<CashierLogLineDetailConv>(lines, data.userEmail, data.userAction, data.userActionDate, "Lines"), data.Data.LocationID, data.Data.LogID);

                QueryModel<CashierLogAction> logAction = new();
                logAction.Data = new();

                logAction.Data.LogID = data.Data.LogID;
                logAction.Data.LocationID = data.Data.LocationID;
                logAction.Data.UserEmail = data.userEmail;
                logAction.Data.LogAction = "Edited";
                logAction.Data.ActionDate = DateTime.Now;
                logAction.userEmail = data.userEmail;
                logAction.userAction = data.userAction;
                logAction.userActionDate = data.userActionDate;

                createBrankasActionLogData(logAction);

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

        [HttpPost("editBrankasDocumentStatus")]
        public async Task<IActionResult> updateBrankasDocumentStatusDataTable(QueryModel<string> data)
        {
            ResultModel<QueryModel<string>> res = new ResultModel<QueryModel<string>>();
            IActionResult actionResult = null;

            try
            {
                string temp = Base64Decode(data.Data);

                string logid = temp.Split("!_!")[0];
                string loc = temp.Split("!_!")[1].Equals("") ? "HO" : temp.Split("!_!")[1];
                string statVal = temp.Split("!_!")[2];

                updateBrankasDocumentStatusData(logid, loc, statVal, data.userEmail, data.userAction, data.userActionDate);

                QueryModel<CashierLogAction> logAction = new();
                logAction.Data = new();

                logAction.Data.LogID = logid;
                logAction.Data.LocationID = loc;
                logAction.Data.UserEmail = data.userEmail;
                logAction.Data.LogAction = "Archived";
                logAction.Data.ActionDate = DateTime.Now;
                logAction.userEmail = data.userEmail;
                logAction.userAction = data.userAction;
                logAction.userActionDate = data.userActionDate;

                createBrankasActionLogData(logAction);

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

        [HttpGet("getLogData/{locPage}")]
        public async Task<IActionResult> getLogDataTable(string locPage)
        {
            ResultModel<List<CashierLogData>> res = new ResultModel<List<CashierLogData>>();
            List<CashierLogData> cashierLogbooks = new List<CashierLogData>();
            DataTable dtCashierLogbook = new DataTable("CashierLogbook");
            bool flag = true;
            IActionResult actionResult = null;
            
            try
            {
                string temp = Base64Decode(locPage);

                string type = temp.Split("!_!")[0];
                string loc = temp.Split("!_!")[1].Equals("") ? "HO" : temp.Split("!_!")[1];
                string status = temp.Split("!_!")[2];
                string cond = temp.Split("!_!")[3];
                int page = Convert.ToInt32(temp.Split("!_!")[4]);

                dtCashierLogbook = getLogData(type, loc, status, cond, page, _rowPerPage);

                if (dtCashierLogbook.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtCashierLogbook.Rows)
                    {
                        CashierLogData temp1 = new();

                        temp1.LogType = dt["LogType"].ToString();
                        temp1.LogID = dt["LogID"].ToString();
                        temp1.LocationID = dt["LocationID"].ToString();
                        temp1.Applicant = dt["Applicant"].ToString();
                        temp1.LogDate = Convert.ToDateTime(dt["LogDate"]);
                        temp1.LogStatus = dt["LogStatus"].ToString();
                        temp1.LogStatusDate = dt.IsNull("LogStatusDate") ? DateTime.MinValue : Convert.ToDateTime(dt["LogStatusDate"]);
                        
                        DataTable dtHeader = new DataTable("Header");
                        List<CashierLogCategoryDetail> cashierLogCategoryDetail = new();

                        dtHeader = getLogHeaderData(dt["LogID"].ToString(), loc);
                        temp1.header = new();

                        if (dtCashierLogbook.Rows.Count > 0)
                        {
                            foreach (DataRow rHeader in dtHeader.Rows)
                            {
                                CashierLogCategoryDetail temp2 = new();

                                temp2.LogID = rHeader["LogID"].ToString();
                                temp2.BrankasCategoryID = rHeader["BrankasCategoryID"].ToString();
                                temp2.AmountCategoryID = rHeader["AmountCategoryID"].ToString();
                                temp2.AmountCategoryName = rHeader["CategoryName"].ToString();
                                temp2.HeaderAmount = Convert.ToDecimal(rHeader["HeaderAmount"]);
                                temp2.ActualAmount = Convert.ToDecimal(rHeader["ActualAmount"]);
                                temp2.CategoryNote = rHeader.IsNull("CategoryNote") ? "" : rHeader["CategoryNote"].ToString();

                                DataTable dtLines = new DataTable("Lines");
                                List<CashierLogLineDetail> cashierLogLineDetail = new();
                                dtLines = getLogLineData(rHeader["BrankasCategoryID"].ToString(), loc);
                                temp2.lines = new();

                                if (dtLines.Rows.Count > 0)
                                {
                                    foreach (DataRow rLine in dtLines.Rows)
                                    {
                                        CashierLogLineDetail temp3 = new();

                                        temp3.BrankasCategoryID = rLine["BrankasCategoryID"].ToString();
                                        temp3.LineNo = Convert.ToInt32(rLine["LineNum"]);
                                        temp3.AmountSubCategoryID = rLine["AmountSubCategoryID"].ToString();
                                        temp3.AmountSubCategoryName = rLine["CategoryName"].ToString();
                                        temp3.AmountType = rLine["AmountType"].ToString();
                                        temp3.AmountDesc = rLine["AmountDesc"].ToString();
                                        temp3.ShiftID = Convert.ToInt32(rLine["ShiftID"]);
                                        temp3.ShiftDesc = rLine["ShiftDesc"].ToString();
                                        temp3.LineAmount = Convert.ToDecimal(rLine["LineAmount"]);

                                        cashierLogLineDetail.Add(temp3);
                                    }

                                    temp2.lines = cashierLogLineDetail;
                                }
                                else
                                {
                                    flag = false;
                                }

                                cashierLogCategoryDetail.Add(temp2);
                            }

                            temp1.header = cashierLogCategoryDetail;
                        }
                        else
                        {
                            flag = false;
                        }

                        DataTable dtApv = new DataTable("Approval");
                        List<CashierLogApproval> cashierLogApproval = new();

                        dtApv = getLogApprovalData(dt["LogID"].ToString(), loc);

                        if (dtApv.Rows.Count > 0)
                        {
                            foreach (DataRow rApproval in dtApv.Rows)
                            {
                                CashierLogApproval temp4 = new();

                                temp4.LogID = rApproval["LogID"].ToString();
                                temp4.ShiftID = Convert.ToInt32(rApproval["ShiftID"]);
                                temp4.LocationID = rApproval["LocationID"].ToString();
                                temp4.CreateUser = rApproval["CreateUser"].ToString();
                                temp4.CreateDate = Convert.ToDateTime(rApproval["CreateDate"]);
                                temp4.ConfirmUser = rApproval.IsNull("ConfirmUser") ? "" : rApproval["ConfirmUser"].ToString();
                                temp4.ConfirmDate = rApproval.IsNull("ConfirmDate") ? DateTime.MinValue : Convert.ToDateTime(rApproval["ConfirmDate"]);
                                temp4.ApproveNote = rApproval.IsNull("ApproveNote") ? "" : rApproval["ApproveNote"].ToString();

                                cashierLogApproval.Add(temp4);
                            }

                            temp1.approvals = cashierLogApproval;
                        }
                        else
                        {
                            temp1.approvals = null;
                            flag = false;
                        }
                        
                        cashierLogbooks.Add(temp1);
                    }
                }
                else
                {
                    flag = false;
                }

                if (flag)
                {
                    res.Data = cashierLogbooks;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = cashierLogbooks;
                    res.isSuccess = true;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Fail Fetch Data";

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

        [HttpGet("getShiftbyModule/{moduleName}")]
        public async Task<IActionResult> getShiftbyModuleData(string moduleName)
        {
            ResultModel<List<Shift>> res = new ResultModel<List<Shift>>();
            List<Shift> shiftLines = new List<Shift>();
            DataTable dtShift = new DataTable("Shift");
            IActionResult actionResult = null;

            try
            {
                dtShift = getShiftbyModule(moduleName);

                if (dtShift.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtShift.Rows)
                    {
                        Shift temp = new Shift();

                        temp.ShiftID = Convert.ToInt32(dt["ShiftID"]);
                        temp.ShiftDesc = dt["ShiftDesc"].ToString();

                        shiftLines.Add(temp);
                    }

                    res.Data = shiftLines;
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

        [HttpGet("getCashierLogbookCategories")]
        public async Task<IActionResult> getCashierLogbookCategories()
        {
            ResultModel<CashierLogbookCategories> res = new ResultModel<CashierLogbookCategories>();
            List<AmountCategories> categoryLines = new List<AmountCategories>();
            List<AmountSubCategories> subCategoryLines = new List<AmountSubCategories>();
            List<AmountTypes> typeLines = new List<AmountTypes>();
            DataTable dtCategory = new DataTable("Category");
            DataTable dtSubCategory = new DataTable("SubCategory");
            DataTable dtType = new DataTable("Type");
            IActionResult actionResult = null;

            try
            {
                dtCategory = getAmountCategories();
                dtSubCategory = getAmountSubCategories();
                dtType = getAmountType();

                if (dtCategory.Rows.Count > 0 && dtSubCategory.Rows.Count > 0 && dtType.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtCategory.Rows)
                    {
                        AmountCategories temp1 = new AmountCategories();

                        temp1.AmountCategoryID = dt["AmountCategoryID"].ToString();
                        temp1.AmountCategoryName = dt["CategoryName"].ToString();

                        categoryLines.Add(temp1);
                    }

                    foreach (DataRow dt in dtSubCategory.Rows)
                    {
                        AmountSubCategories temp2 = new AmountSubCategories();

                        temp2.AmountSubCategoryID = dt["AmountSubCategoryID"].ToString();
                        temp2.AmountSubCategoryName = dt["CategoryName"].ToString();
                        temp2.AmountType = "";

                        subCategoryLines.Add(temp2);
                    }

                    foreach (DataRow dt in dtType.Rows)
                    {
                        AmountTypes temp3 = new AmountTypes();

                        temp3.AmountType = dt["AmountType"].ToString();
                        temp3.AmountDesc = dt["AmountDesc"].ToString();

                        typeLines.Add(temp3);
                    }

                    res.Data = new();
                    res.Data.categories = new();
                    res.Data.subCategories = new();
                    res.Data.types = new();

                    res.Data.categories = categoryLines;
                    res.Data.subCategories = subCategoryLines;
                    res.Data.types = typeLines;

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

        [HttpGet("getBrankasActionLogData/{locPage}")]
        public async Task<IActionResult> getBrankasActionLogDataTable(string locPage)
        {
            ResultModel<List<CashierLogAction>> res = new ResultModel<List<CashierLogAction>>();
            List<CashierLogAction> actionLines = new List<CashierLogAction>();
            DataTable dtAction = new DataTable("Action");
            IActionResult actionResult = null;

            try
            {
                string temp = Base64Decode(locPage);

                string loc = temp.Split("!_!")[0].Equals("") ? "HO" : temp.Split("!_!")[0];
                string orderby = temp.Split("!_!")[1];
                string filType = temp.Split("!_!")[2];
                string filVal = temp.Split("!_!")[3];
                int page = Convert.ToInt32(temp.Split("!_!")[4]);

                dtAction = getBrankasActionLogData(loc, orderby, filType, filVal, page, _rowPerPage);

                if (dtAction.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtAction.Rows)
                    {
                        CashierLogAction temp1 = new CashierLogAction();

                        temp1.LogType = dt["LogType"].ToString();
                        temp1.LogID = dt["LogID"].ToString();
                        temp1.LocationID = dt["LocationID"].ToString();
                        temp1.UserEmail = dt["UserEmail"].ToString();
                        temp1.LogAction = dt["LogAction"].ToString();
                        temp1.ActionDate = Convert.ToDateTime(dt["ActionDate"]);

                        actionLines.Add(temp1);
                    }

                    res.Data = actionLines;
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

        [HttpGet("getModulePageSize/{Table}")]
        public async Task<IActionResult> getModulePageSize(string Table)
        {
            ResultModel<int> res = new ResultModel<int>();
            IActionResult actionResult = null;

            try
            {
                string tbname = Base64Decode(Table).Split("!_!")[0];
                string loc = Base64Decode(Table).Split("!_!")[1];
                string cond = Base64Decode(Table).Split("!_!")[2];

                res.Data = getModuleNumberOfPage(tbname, loc, cond, _rowPerPage);

                res.isSuccess = true;
                res.ErrorCode = "00";
                res.ErrorMessage = "";

                actionResult = Ok(res);
            }
            catch (Exception ex)
            {
                res.Data = 0;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        [HttpGet("getNumberofLogExisting/{param}")]
        public async Task<IActionResult> getNumberofLogExistingData(string param)
        {
            ResultModel<int> res = new ResultModel<int>();
            IActionResult actionResult = null;

            try
            {
                string loc = Base64Decode(param).Split("!_!")[0];
                string cond = Base64Decode(param).Split("!_!")[1];

                res.Data = getNumberofLogExisting(loc, cond);

                res.isSuccess = true;
                res.ErrorCode = "00";
                res.ErrorMessage = "";

                actionResult = Ok(res);
            }
            catch (Exception ex)
            {
                res.Data = 0;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        [HttpPost("editBrankasApproveLogOnConfirm")]
        public async Task<IActionResult> editBrankasApproveLogOnConfirm(QueryModel<CashierLogApproval> data)
        {
            ResultModel<QueryModel<CashierLogApproval>> res = new ResultModel<QueryModel<CashierLogApproval>>();
            IActionResult actionResult = null;

            try
            {
                editBrankasApproveLogOnConfirmData(data);

                QueryModel<CashierLogAction> logAction = new();
                logAction.Data = new();

                logAction.Data.LogID = data.Data.LogID;
                logAction.Data.LocationID = data.Data.LocationID;
                logAction.Data.UserEmail = data.userEmail;
                logAction.Data.LogAction = "Confirmed";
                logAction.Data.ActionDate = DateTime.Now;
                logAction.userEmail = data.userEmail;
                logAction.userAction = data.userAction;
                logAction.userActionDate = data.userActionDate;

                createBrankasActionLogData(logAction);

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

        //
    }

    [Route("api/DA/Standarization")]
    [ApiController]
    public class StandarizationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _conString;
        private readonly int _rowPerPage;

        public StandarizationController(IConfiguration config)
        {
            _configuration = config;
            _conString = _configuration.GetValue<string>("ConnectionStrings:Bpi");
            _rowPerPage = _configuration.GetValue<int>("Paging:Standarization:RowPerPage");
        }

        public static DataTable ListToDataTable<T>(List<T> list, string auditUser, string auditAction, DateTime auditDate, string _tableName)
        {
            DataTable dt = new DataTable(_tableName);

            foreach (PropertyInfo info in typeof(T).GetProperties())
            {
                dt.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            dt.Columns.Add(new DataColumn("AuditUser", Nullable.GetUnderlyingType(auditUser.GetType()) ?? auditUser.GetType()));
            dt.Columns.Add(new DataColumn("AuditAction", Nullable.GetUnderlyingType(auditAction.GetType()) ?? auditAction.GetType()));
            dt.Columns.Add(new DataColumn("AuditActionDate", Nullable.GetUnderlyingType(auditDate.GetType()) ?? auditDate.GetType()));

            foreach (T t in list)
            {
                DataRow row = dt.NewRow();

                foreach (PropertyInfo info in typeof(T).GetProperties())
                {
                    row[info.Name] = info.GetValue(t, null) ?? DBNull.Value;
                }
                row["AuditUser"] = auditUser;
                row["AuditAction"] = auditAction;
                row["AuditActionDate"] = auditDate;

                dt.Rows.Add(row);
            }
            return dt;
        }

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

        internal DataTable createIDData(string docType)
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
                    command.CommandText = "[createID]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@DocumentName", docType);

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

        internal bool createStandarizationData(QueryModel<Standarizations> data)
        {
            bool flag = false;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[createStandarizationData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@TypeID", data.Data.TypeID);
                    command.Parameters.AddWithValue("@StandarizationID", data.Data.StandarizationID);
                    command.Parameters.AddWithValue("@StandarizationDetails", data.Data.StandarizationDetails);
                    command.Parameters.AddWithValue("@StandarizationDate", data.Data.StandarizationDate);
                    command.Parameters.AddWithValue("@AuditUser", data.userEmail);
                    command.Parameters.AddWithValue("@AuditAction", data.userAction);
                    command.Parameters.AddWithValue("@AuditActionDate", data.userActionDate);

                    int ret = command.ExecuteNonQuery();

                    if (ret >= 0)
                        flag = true;

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

            return flag;
        }

        internal bool createStandarizationTagData(DataTable data)
        {
            bool flag = false;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[createStandarizationTags]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@StandarizationTags", data);

                    int ret = command.ExecuteNonQuery();

                    if (ret >= 0)
                        flag = true;

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

            return flag;
        }

        internal bool createStandarizationAttachmentData(DataTable data)
        {
            bool flag = false;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[createStandarizationAttachment]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@StandarizationAttachments", data);

                    int ret = command.ExecuteNonQuery();

                    if (ret >= 0)
                        flag = true;

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

            return flag;
        }

        internal bool editStandarizationData(QueryModel<Standarizations> data)
        {
            bool flag = false;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[editStandarizationData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@TypeID", data.Data.TypeID);
                    command.Parameters.AddWithValue("@StandarizationID", data.Data.StandarizationID);
                    command.Parameters.AddWithValue("@StandarizationDetails", data.Data.StandarizationDetails);
                    command.Parameters.AddWithValue("@StandarizationDate", data.Data.StandarizationDate);
                    command.Parameters.AddWithValue("@AuditUser", data.userEmail);
                    command.Parameters.AddWithValue("@AuditAction", data.userAction);
                    command.Parameters.AddWithValue("@AuditActionDate", data.userActionDate);

                    int ret = command.ExecuteNonQuery();

                    if (ret >= 0)
                        flag = true;

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

            return flag;
        }

        internal bool editStandarizationTagData(DataTable data)
        {
            bool flag = false;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[editStandarizationTags]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@StandarizationTags", data);

                    int ret = command.ExecuteNonQuery();

                    if (ret >= 0)
                        flag = true;

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

            return flag;
        }

        internal bool editStandarizationAttachmentData(DataTable data)
        {
            bool flag = false;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[editStandarizationAttachment]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@StandarizationAttachments", data);

                    int ret = command.ExecuteNonQuery();

                    if (ret >= 0)
                        flag = true;

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

            return flag;
        }

        internal DataTable getStandarizationTypeData()
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
                    command.CommandText = "[getStandarizationsData]";
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

        internal DataTable getStandarizationData(string conditions, int pageNo, int rowPerPage)
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
                    command.CommandText = "[getStandarizationDetailsData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    //command.Parameters.AddWithValue("@TypeID", type);
                    command.Parameters.AddWithValue("@Condition", conditions);
                    command.Parameters.AddWithValue("@PageNo", pageNo);
                    command.Parameters.AddWithValue("@RowPerPage", rowPerPage);

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

        internal DataTable getStandarizationTagData(string id)
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
                    command.CommandText = "[getStandarizationTagsData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@StandarizationID", id);

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

        internal DataTable getStandarizationAttachmentData(string id)
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
                    command.CommandText = "[getStandarizationAttachmentData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@StandarizationID", id);

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

        internal int getModuleNumberOfPage(string TbName, string loc, string conditions, int rowPerPage)
        {
            int conInt = 0;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[getStandarizationModulePageSize]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@TbName", TbName);
                    command.Parameters.AddWithValue("@LocationID", loc);
                    command.Parameters.AddWithValue("@Conditions", conditions);
                    command.Parameters.AddWithValue("@RowPerPage", rowPerPage);

                    var data = command.ExecuteScalar();
                    conInt = Convert.ToInt32(data);

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
            return conInt;
        }

        internal bool deleteStandarizationData(string id, string user, string act, DateTime actDate)
        {
            bool flag = false;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[deleteStandarizationData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@StandarizationID", id);
                    command.Parameters.AddWithValue("@AuditUser", user);
                    command.Parameters.AddWithValue("@AuditAction", act);
                    command.Parameters.AddWithValue("@AuditActionDate", actDate);

                    int ret = command.ExecuteNonQuery();

                    if (ret >= 0)
                        flag = true;

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

            return flag;
        }

        [HttpPost("createStandarizationData")]
        public async Task<IActionResult> createStandarizationDataTable(QueryModel<Standarizations> data)
        {
            ResultModel<QueryModel<Standarizations>> res = new ResultModel<QueryModel<Standarizations>>();
            //DataTable dtMainIdentity = new("Identity");
            //string standarizationId = string.Empty;
            IActionResult actionResult = null;

            try
            {
                //dtMainIdentity = createIDData("CashierLogbook");

                //if (dtMainIdentity.Rows.Count > 0)
                //{
                //    foreach (DataRow dt in dtMainIdentity.Rows)
                //    {
                //        string zero = string.Empty;

                //        for (int i = 0; i < 16 - (Convert.ToInt32(dt["IDLength"]) + dt["Code"].ToString().Length); i++)
                //        {
                //            zero = zero + "0";
                //        }

                //        standarizationId = dt["Code"].ToString() + DateTime.Now.Year.ToString() + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + zero + dt["DocNumber"].ToString() + dt["Parity"].ToString();

                //    }
                //}

                //data.Data.StandarizationID = standarizationId;
                
                List<StandarizationTag> tags = new();

                foreach (var tag in data.Data.Tags)
                {
                    tags.Add(new StandarizationTag
                    {
                        rowGuid = Guid.NewGuid(),
                        StandarizationID = tag.StandarizationID,
                        TagDescriptions = tag.TagDescriptions
                    });
                }

                bool success = createStandarizationData(data);

                if (!success)
                    throw new Exception("Fail Create Standarization Header");

                success = createStandarizationTagData(ListToDataTable<StandarizationTag>(tags, data.userEmail, data.userAction, data.userActionDate, "Tags"));

                if (!success)
                    throw new Exception("Fail Create Standarization Tags");

                success = createStandarizationAttachmentData(ListToDataTable<StandarizationAttachment>(data.Data.Attachments, data.userEmail, data.userAction, data.userActionDate, "Attachments"));

                if (!success)
                    throw new Exception("Fail Create Standarization Attachment");


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

        [HttpPost("editStandarizationData")]
        public async Task<IActionResult> editStandarizationDataTable(QueryModel<Standarizations> data)
        {
            ResultModel<QueryModel<Standarizations>> res = new ResultModel<QueryModel<Standarizations>>();
            IActionResult actionResult = null;

            try
            {
                deleteStandarizationData(data.Data.StandarizationID, data.userEmail, data.userAction, data.userActionDate);

                //bool success = editStandarizationData(data);

                //if (!success)
                //    throw new Exception("Fail Create Standarization Header");

                //success = editStandarizationTagData(ListToDataTable<StandarizationTag>(data.Data.Tags, data.userEmail, data.userAction, data.userActionDate, "Tags"));

                //if (!success)
                //    throw new Exception("Fail Create Standarization Tags");

                //success = editStandarizationAttachmentData(ListToDataTable<StandarizationAttachment>(data.Data.Attachments, data.userEmail, data.userAction, data.userActionDate, "Attachments"));

                //if (!success)
                //    throw new Exception("Fail Create Standarization Attachment");

                List<StandarizationTag> tags = new();

                foreach (var tag in data.Data.Tags)
                {
                    tags.Add(new StandarizationTag
                    {
                        rowGuid = Guid.NewGuid(),
                        StandarizationID = tag.StandarizationID,
                        TagDescriptions = tag.TagDescriptions
                    });
                }

                bool success = createStandarizationData(data);

                if (!success)
                    throw new Exception("Fail Create Standarization Header");

                success = createStandarizationTagData(ListToDataTable<StandarizationTag>(tags, data.userEmail, data.userAction, data.userActionDate, "Tags"));

                if (!success)
                    throw new Exception("Fail Create Standarization Tags");

                success = createStandarizationAttachmentData(ListToDataTable<StandarizationAttachment>(data.Data.Attachments, data.userEmail, data.userAction, data.userActionDate, "Attachments"));

                if (!success)
                    throw new Exception("Fail Create Standarization Attachment");


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

        [HttpPost("deleteStandarizationData")]
        public async Task<IActionResult> deleteStandarizationDataTable(QueryModel<string> data)
        {
            ResultModel<QueryModel<string>> res = new ResultModel<QueryModel<string>>();
            IActionResult actionResult = null;

            try
            {
                string temp = Base64Decode(data.Data);

                bool flag = deleteStandarizationData(temp.Split("!_!")[0], data.userEmail, data.userAction, data.userActionDate);

                if (flag)
                {
                    res.Data = data;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = data;
                    res.isSuccess = false;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Data Might Already Deleted";

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

        [HttpGet("getStandarizationTypes")]
        public async Task<IActionResult> getStandarizationTypesDataTable()
        {
            ResultModel<List<StandarizationType>> res = new ResultModel<List<StandarizationType>>();
            List<StandarizationType> typeLines = new List<StandarizationType>();
            DataTable dtType = new DataTable("Shift");
            IActionResult actionResult = null;

            try
            {
                dtType = getStandarizationTypeData();

                if (dtType.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtType.Rows)
                    {
                        StandarizationType temp = new StandarizationType();

                        temp.TypeID = dt["TypeID"].ToString();
                        temp.Descriptions = dt["Descriptions"].ToString();

                        typeLines.Add(temp);
                    }

                    res.Data = typeLines;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = true;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Fetch Empty";

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

        [HttpGet("getStandarizationData/{param}")]
        public async Task<IActionResult> getStandarizationDataTable(string param)
        {
            ResultModel<List<Standarizations>> res = new ResultModel<List<Standarizations>>();
            List<Standarizations> standarizationLines = new List<Standarizations>();
            List<StandarizationTag> tagLines = new List<StandarizationTag>();
            List<StandarizationAttachment> attachLines = new List<StandarizationAttachment>();
            DataTable dtStandarization = new DataTable("Standarizations");
            DataTable dtTag = new DataTable("Tags");
            DataTable dtAttach = new DataTable("Attachments");
            IActionResult actionResult = null;

            try
            {
                string temp = Base64Decode(param);

                //string id = temp.Split("!_!")[0];
                string cond = temp.Split("!_!")[0];
                int pageNo = Convert.ToInt32(temp.Split("!_!")[1]);

                dtStandarization = getStandarizationData(cond, pageNo, _rowPerPage);

                if (dtStandarization.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtStandarization.Rows)
                    {
                        Standarizations temp1 = new Standarizations();

                        temp1.TypeID = dt["TypeID"].ToString();
                        temp1.StandarizationID = dt["StandarizationID"].ToString();
                        temp1.StandarizationDetails = dt["StandarizationDetails"].ToString();
                        temp1.StandarizationDate = Convert.ToDateTime(dt["StandarizationDate"]);
                        //temp1.StandarizationCategory = dt["StandarizationCategory"].ToString();
                        //temp1.PathFile = dt["PathFile"].ToString();

                        temp1.Tags = new();
                        temp1.Attachments = new();

                        dtTag = getStandarizationTagData(dt["StandarizationID"].ToString());

                        if (dtTag.Rows.Count > 0)
                        {
                            foreach (DataRow rTag in dtTag.Rows)
                            {
                                StandarizationTag temp2 = new();

                                temp2.StandarizationID = rTag["StandarizationID"].ToString();
                                temp2.TagDescriptions = rTag["TagDescriptions"].ToString();

                                temp1.Tags.Add(temp2);
                            }
                        }

                        dtAttach = getStandarizationAttachmentData(dt["StandarizationID"].ToString());

                        if (dtAttach.Rows.Count > 0)
                        {
                            foreach (DataRow rAttach in dtAttach.Rows)
                            {
                                StandarizationAttachment temp3 = new();

                                temp3.StandarizationID = rAttach["StandarizationID"].ToString();
                                temp3.Descriptions = rAttach["Descriptions"].ToString();
                                temp3.UploadDate = Convert.ToDateTime(rAttach["UploadDate"]);
                                temp3.FileExtention = rAttach["FileExtention"].ToString();
                                temp3.FilePath = rAttach["FilePath"].ToString();

                                temp1.Attachments.Add(temp3);
                            }
                        }

                        standarizationLines.Add(temp1);
                    }

                    res.Data = standarizationLines;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = true;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Fetch Empty";

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

        [HttpGet("getStandarizationAttachment/{param}")]
        public async Task<IActionResult> getStandarizationAttachmentDataTable(string param)
        {
            ResultModel<List<StandarizationAttachment>> res = new ResultModel<List<StandarizationAttachment>>();
            List<StandarizationAttachment> standarizationLines = new List<StandarizationAttachment>();
            DataTable dtAttach = new DataTable("Attachment");
            IActionResult actionResult = null;

            try
            {
                string temp = Base64Decode(param);

                dtAttach = getStandarizationAttachmentData(temp.Split("!_!")[0]);

                if (dtAttach.Rows.Count > 0)
                {
                    foreach (DataRow rAttach in dtAttach.Rows)
                    {
                        StandarizationAttachment temp1 = new();

                        temp1.StandarizationID = rAttach["StandarizationID"].ToString();
                        temp1.Descriptions = rAttach["Descriptions"].ToString();
                        temp1.UploadDate = Convert.ToDateTime(rAttach["UploadDate"]);
                        temp1.FileExtention = rAttach["FileExtention"].ToString();
                        temp1.FilePath = rAttach["FilePath"].ToString();

                        standarizationLines.Add(temp1);
                    }

                    res.Data = standarizationLines;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = true;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Fetch Empty";

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

        [HttpGet("getModulePageSize/{Table}")]
        public async Task<IActionResult> getModulePageSize(string Table)
        {
            ResultModel<int> res = new ResultModel<int>();
            IActionResult actionResult = null;

            try
            {
                string tbname = Base64Decode(Table).Split("!_!")[0];
                string loc = Base64Decode(Table).Split("!_!")[1];
                string cond = Base64Decode(Table).Split("!_!")[2];

                res.Data = getModuleNumberOfPage(tbname, loc, cond, _rowPerPage);

                res.isSuccess = true;
                res.ErrorCode = "00";
                res.ErrorMessage = "";

                actionResult = Ok(res);
            }
            catch (Exception ex)
            {
                res.Data = 0;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        //
    }
}
