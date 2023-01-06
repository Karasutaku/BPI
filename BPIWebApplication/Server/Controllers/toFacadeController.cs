using Microsoft.AspNetCore.Mvc;
using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.PagesModel.AddEditProject;
using BPIWebApplication.Shared.FileUploadModel;
using BPIWebApplication.Shared.PagesModel.ApplyProcedure;
using BPIWebApplication.Shared.PagesModel.Dashboard;
using BPIWebApplication.Shared.PagesModel.AccessHistory;
using BPIWebApplication.Shared.ReportModel;
using BPIWebApplication.Shared.MainModel.Login;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net;

namespace BPIWebApplication.Server.Controllers
{

    [ApiController]
    [Route("api/endUser/Login")]
    public class toFacadeLoginController : Controller
    {
        private readonly HttpClient _http;

        public toFacadeLoginController(HttpClient http, IConfiguration config)
        {
            _http = http;
            _http.BaseAddress = new Uri(config.GetValue<string>("ConnectionStrings:SmsApi"));
        }

        [HttpPost("Authenticate")]
        public async Task<IActionResult> authenticateToITFacade(FacadeLogin data)
        {
            FacadeLoginResponse res = new FacadeLoginResponse();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<FacadeLogin>($"api/Facade/BPIBase/createNewDepartmentData", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<FacadeLoginResponse>();
                }
            }
            catch (Exception ex)
            {
                actionResult = BadRequest(res);
            }

            return actionResult;
        }
    }


    [ApiController]
    [Route("api/endUser/BPIBase")]
    public class BpiBaseController : Controller
    {
        private readonly HttpClient _http;

        public BpiBaseController(HttpClient http, IConfiguration config)
        {
            _http = http;
            _http.BaseAddress = new Uri(config.GetValue<string>("ConnectionStrings:BpiFacade"));
        }

        [HttpGet("getAllBisnisUnitData")]
        public async Task<IActionResult> getAllBisnisUnitDataTable()
        {
            ResultModel<List<BisnisUnit>> res = new ResultModel<List<BisnisUnit>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<BisnisUnit>>>("api/Facade/BPIBase/getAllBisnisUnitData");

                if (result.isSuccess)
                {
                    res.Data = result.Data;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

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
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<Department>>>("api/Facade/BPIBase/getAllDepartmentData");

                if (result.isSuccess)
                {
                    res.Data = result.Data;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

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
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<Project>>>("api/Facade/BPIBase/getAllProjectData");

                if (result.isSuccess)
                {
                    res.Data = result.Data;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

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
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<string>>($"api/Facade/BPIBase/isDepartmentDataPresent/{DeptID}");

                if (result.isSuccess)
                {
                    res.Data = result.Data;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
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
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<string>>($"api/Facade/BPIBase/isProjectPresent/{projectNo}");

                if (result.isSuccess)
                {
                    res.Data = result.Data;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
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

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<Department>>($"api/Facade/BPIBase/createNewDepartmentData", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<Department>>>();

                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = result.IsSuccessStatusCode;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Fail Fetch data from createNewDepartmentData Facade";

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

        [HttpPost("createNewProjectData")]
        public async Task<IActionResult> createProjectDataTable(QueryModel<Project> data)
        {
            ResultModel<QueryModel<Project>> res = new ResultModel<QueryModel<Project>>();
            IActionResult actionResult = null;

            // create procedure data to db
            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<Project>>($"api/Facade/BPIBase/createNewProjectData", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<Project>>>();

                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = result.IsSuccessStatusCode;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Fail Fetch data from createNewProjectData Facade";

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

        // http post (edit)

        [HttpPost("editDepartmentData")]
        public async Task<IActionResult> editDepartmentDataTable(QueryModel<Department> data)
        {
            ResultModel<QueryModel<Department>> res = new ResultModel<QueryModel<Department>>();
            IActionResult actionResult = null;

            // create procedure data to db
            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<Department>>($"api/Facade/BPIBase/editDepartmentData", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<Department>>>();

                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = result.IsSuccessStatusCode;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Fail Fetch data from editDepartmentData Facade";

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


        [HttpPost("editProjectData")]
        public async Task<IActionResult> editProjectDataTable(QueryModel<Project> data)
        {
            ResultModel<QueryModel<Project>> res = new ResultModel<QueryModel<Project>>();
            IActionResult actionResult = null;

            // create procedure data to db
            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<Project>>($"api/Facade/BPIBase/editProjectData", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<Project>>>();

                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = result.IsSuccessStatusCode;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Fail Fetch data from editDepartmentData Facade";

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

    }

    [Route("api/endUser/Procedure")]
    [ApiController]
    public class endProcedureController : ControllerBase
    {
        private readonly HttpClient _http;

        public endProcedureController(HttpClient http, IConfiguration config)
        {
            _http = http;
            _http.BaseAddress = new Uri(config.GetValue<string>("ConnectionStrings:BpiFacade"));
        }

        [HttpGet("getAllProcedureData")]
        public async Task<IActionResult> getAllProcedureDataTable()
        {
            ResultModel<List<Procedure>> res = new ResultModel<List<Procedure>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<Procedure>>>("api/Facade/Procedure/getAllProcedureData");

                if (result.isSuccess)
                {
                    res.Data = result.Data;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

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
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<DepartmentProcedure>>>("api/Facade/Procedure/getDepartmentProcedureData");

                if (result.isSuccess)
                {
                    res.Data = result.Data;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

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
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<DepartmentProcedure>>>($"api/Facade/Procedure/getDepartmentProcedureDatawithPaging/{pageNo}");

                if (result.isSuccess)
                {
                    res.Data = result.Data;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

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
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<DashboardFilter>($"api/Facade/Procedure/getDepartmentProcedureDatawithFilterbyPaging", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<List<DepartmentProcedure>>>();

                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = result.IsSuccessStatusCode;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Fail Fetch data from getDepartmentProcedureDatawithFilterbyPaging Facade";

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
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<HistoryAccess>>>($"api/Facade/Procedure/getAllHistoryAccessDatawithPaging/{pageNo}");

                if (result.isSuccess)
                {
                    res.Data = result.Data;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

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
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<AccessHistoryFilter>($"api/Facade/Procedure/getAllHistoryAccessDatabyFilterwithPaging", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<List<HistoryAccess>>>();

                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = result.IsSuccessStatusCode;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Fail Fetch data from getAllHistoryAccessDatabyFilterwithPaging Facade";

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
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<AccessHistoryReport>($"api/Facade/Procedure/getAllHistoryAccessDataReportbyFilter", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<List<HistoryAccess>>>();

                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = result.IsSuccessStatusCode;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Fail Fetch data from getAllHistoryAccessDataReportbyFilter Facade";

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
                var result = await _http.GetFromJsonAsync<ResultModel<int>>("api/Facade/Procedure/getDepartmentProcedureNumberofPage");

                if (result.isSuccess)
                {
                    res.Data = result.Data;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = 0;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
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
                var result = await _http.PostAsJsonAsync<DashboardFilter>($"api/Facade/Procedure/getDepartmentProcedurewithFilterNumberofPage", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<int>>();

                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = 0;

                    res.isSuccess = result.IsSuccessStatusCode;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Fail Fetch data from getDepartmentProcedurewithFilterNumberofPage Facade";

                    actionResult = Ok(res);
                }
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
                var result = await _http.GetFromJsonAsync<ResultModel<int>>("api/Facade/Procedure/getHistoryAccessNumberofPage");

                if (result.isSuccess)
                {
                    res.Data = result.Data;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = 0;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
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
                var result = await _http.PostAsJsonAsync<AccessHistoryFilter>($"api/Facade/Procedure/getHistoryAccessbyFilterwithPagingNumberofPage", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<int>>();

                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = 0;

                    res.isSuccess = result.IsSuccessStatusCode;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Fail Fetch data from getHistoryAccessbyFilterwithPagingNumberofPage Facade";

                    actionResult = Ok(res);
                }
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

        // temp comment

        //[HttpGet("getFile/{path}")]
        //public async Task<IActionResult> getFiletoDownload(string path)
        //{
        //    ResultModel<BPIFacade.Models.MainModel.Stream.FileStream> res = new ResultModel<BPIFacade.Models.MainModel.Stream.FileStream>();
        //    IActionResult actionResult = null;

        //    try
        //    {
        //        var result = await _http.GetFromJsonAsync<ResultModel<BPIFacade.Models.MainModel.Stream.FileStream>>($"api/Facade/Procedure/getFile/{path}");

        //        if (result.isSuccess)
        //        {
        //            res.Data = result.Data;

        //            res.isSuccess = result.isSuccess;
        //            res.ErrorCode = result.ErrorCode;
        //            res.ErrorMessage = result.ErrorMessage;

        //            actionResult = Ok(res);
        //        }
        //        else
        //        {
        //            res.Data = null;

        //            res.isSuccess = result.isSuccess;
        //            res.ErrorCode = result.ErrorCode;
        //            res.ErrorMessage = result.ErrorMessage;

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


        [HttpGet("isProcedureDataPresent/{ProcNo}")]
        public async Task<IActionResult> isProcedureDataPresentInTable(string ProcNo)
        {
            ResultModel<string> res = new ResultModel<string>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<string>>($"api/Facade/Procedure/isProcedureDataPresent/{ProcNo}");

                if (result.isSuccess)
                {
                    res.Data = result.Data;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
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

        //[HttpPost("createProcedureData")]
        //public async Task<IActionResult> createProcedureDataTableandFileSave(ProcedureStream data)
        //{
        //    ResultModel<ProcedureStream> res = new ResultModel<ProcedureStream>();
        //    IActionResult actionResult = null;

        //    try
        //    {
        //        var result = await _http.PostAsJsonAsync<ProcedureStream>($"api/Facade/Procedure/createProcedureData", data);

        //        if (result.IsSuccessStatusCode)
        //        {
        //            var respBody = await result.Content.ReadFromJsonAsync<ResultModel<ProcedureStream>>();

        //            res.Data = respBody.Data;

        //            res.isSuccess = respBody.isSuccess;
        //            res.ErrorCode = respBody.ErrorCode;
        //            res.ErrorMessage = respBody.ErrorMessage;

        //            actionResult = Ok(res);
        //        }
        //        else
        //        {
        //            res.Data = null;

        //            res.isSuccess = result.IsSuccessStatusCode;
        //            res.ErrorCode = "01";
        //            res.ErrorMessage = "Fail Fetch data from createProcedureData Facade";

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


        [HttpPost("createDepartmentProcedureData")]
        public async Task<IActionResult> createDepartmentProcedureDataTable(QueryModel<List<DepartmentProcedure>> data)
        {
            ResultModel<QueryModel<List<DepartmentProcedure>>> res = new ResultModel<QueryModel<List<DepartmentProcedure>>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<List<DepartmentProcedure>>>($"api/Facade/Procedure/createDepartmentProcedureData", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<List<DepartmentProcedure>>>>();

                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = result.IsSuccessStatusCode;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Fail Fetch data from createDepartmentProcedureData Facade";

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


        [HttpPost("createHistoryAccessData")]
        public async Task<IActionResult> createHistoryAccessDataTable(QueryModel<HistoryAccess> data)
        {
            ResultModel<QueryModel<HistoryAccess>> res = new ResultModel<QueryModel<HistoryAccess>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<HistoryAccess>>($"api/Facade/Procedure/createHistoryAccessData", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<HistoryAccess>>>();

                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = result.IsSuccessStatusCode;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Fail Fetch data from createHistoryAccessData Facade";

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

        //[HttpPost("editProcedureData")]
        //public async Task<IActionResult> editProcedureDataTable(ProcedureStream data)
        //{
        //    ResultModel<ProcedureStream> res = new ResultModel<ProcedureStream>();
        //    IActionResult actionResult = null;

        //    try
        //    {
        //        var result = await _http.PostAsJsonAsync<ProcedureStream>($"api/Facade/Procedure/editProcedureData", data);

        //        if (result.IsSuccessStatusCode)
        //        {
        //            var respBody = await result.Content.ReadFromJsonAsync<ResultModel<ProcedureStream>>();

        //            res.Data = respBody.Data;

        //            res.isSuccess = respBody.isSuccess;
        //            res.ErrorCode = respBody.ErrorCode;
        //            res.ErrorMessage = respBody.ErrorMessage;

        //            actionResult = Ok(res);
        //        }
        //        else
        //        {
        //            res.Data = null;

        //            res.isSuccess = result.IsSuccessStatusCode;
        //            res.ErrorCode = "01";
        //            res.ErrorMessage = "Fail Fetch data from editProcedureData Facade";

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


        [HttpPost("deleteDepartmentProcedureData")]
        public async Task<IActionResult> deleteDepartmentProcedureDataTablebyProcNoDeptID(QueryModel<List<DepartmentProcedure>> data)
        {
            ResultModel<QueryModel<List<DepartmentProcedure>>> res = new ResultModel<QueryModel<List<DepartmentProcedure>>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<List<DepartmentProcedure>>>($"api/Facade/Procedure/deleteDepartmentProcedureData", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<List<DepartmentProcedure>>>>();

                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = result.IsSuccessStatusCode;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Fail Fetch data from deleteDepartmentProcedureData Facade";

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

        // OTHER

        [HttpGet("getProcedureMaxSizeUpload")]
        public async Task<IActionResult> getProcedureMaxSizeUpload()
        {
            ResultModel<long> res = new ResultModel<long>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<long>>("api/Facade/Procedure/getProcedureMaxSizeUpload");

                if (result.isSuccess)
                {
                    res.Data = result.Data;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = 0;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
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
