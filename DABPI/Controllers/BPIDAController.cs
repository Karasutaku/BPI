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

        internal DataTable getDepartmentProcedureDatawithPaging(int pageNo, int rowPerPage)
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

        internal int getDepartmentProcedureNumberofPage(int RowPerPage)
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

        [HttpGet("getDepartmentProcedureDatawithPaging/{pageNo}")]
        public async Task<IActionResult> getAllDepartmentProcedureDataTablebyPaging(int pageNo)
        {
            ResultModel<List<DepartmentProcedure>> res = new ResultModel<List<DepartmentProcedure>>();
            List<DepartmentProcedure> departmentProcedure = new List<DepartmentProcedure>();
            DataTable dtDepartmentProcedure = new DataTable("appliedProcedureData");
            IActionResult actionResult = null;

            try
            {
                dtDepartmentProcedure = getDepartmentProcedureDatawithPaging(pageNo, _rowPerPage);

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

        [HttpGet("getDepartmentProcedureNumberofPage")]
        public async Task<IActionResult> getDepartmentProcedureNumberofPageData()
        {
            ResultModel<int> res = new ResultModel<int>();
            IActionResult actionResult = null;

            try
            {
                res.Data = getDepartmentProcedureNumberofPage(_rowPerPage);

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


}
