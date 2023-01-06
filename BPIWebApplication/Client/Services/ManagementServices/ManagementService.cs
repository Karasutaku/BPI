using BPIWebApplication.Shared;
using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.FileUploadModel;
using BPIWebApplication.Shared.PagesModel.AddEditProject;
using BPIWebApplication.Shared.PagesModel.AddEditUser;
using System.Net.Http.Json;

namespace BPIWebApplication.Client.Services.ManagementServices
{
    public class ManagementService : IManagementService
    {
        private readonly HttpClient _http;
        public ManagementService(HttpClient http)
        {
            _http = http;
        }

        public List<BisnisUnit> bisnisUnits { get; set; } = new List<BisnisUnit>();
        public List<Department> departments { get; set; } = new List<Department>();
        public List<UserAdmin> users { get; set; } = new List<UserAdmin>();
        public List<Project> projects { get; set; } = new List<Project>();

        // encode decode base 64

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

        // get

        public async Task<ResultModel<List<BisnisUnit>>> GetAllBisnisUnit()
        {
            ResultModel<List<BisnisUnit>> resData = new ResultModel<List<BisnisUnit>>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<BisnisUnit>>>("api/Management/getAllBisnisUnitData");
                //var result = await _http.GetFromJsonAsync<ResultModel<List<BisnisUnit>>>("api/endUser/BPIBase/getAllBisnisUnitData");

                if (result.isSuccess)
                {
                    bisnisUnits = result.Data;

                    resData.Data = result.Data;
                    resData.isSuccess = result.isSuccess;
                    resData.ErrorCode = result.ErrorCode;
                    resData.ErrorMessage = result.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                resData.Data = null;
                resData.isSuccess = false;
                resData.ErrorCode = "99";
                resData.ErrorMessage = ex.Message;
            }
            return resData;
        }

        public async Task<ResultModel<List<Department>>> GetAllDepartment()
        {
            ResultModel<List<Department>> resData = new ResultModel<List<Department>>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<Department>>>("api/Management/getAllDepartmentData");

                if (result.isSuccess)
                {
                    departments = result.Data;

                    resData.Data = result.Data;
                    resData.isSuccess = result.isSuccess;
                    resData.ErrorCode = result.ErrorCode;
                    resData.ErrorMessage = result.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                resData.Data = null;
                resData.isSuccess = false;
                resData.ErrorCode = "99";
                resData.ErrorMessage = ex.Message;
            }
            return resData;
        }

        public async Task<ResultModel<List<UserAdmin>>> GetAllUserAdmin()
        {
            ResultModel<List<UserAdmin>> resData = new ResultModel<List<UserAdmin>>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<UserAdmin>>>("api/Management/getAllUserAdminData");

                if (result.isSuccess)
                {
                    users = result.Data;

                    resData.Data = result.Data;
                    resData.isSuccess = result.isSuccess;
                    resData.ErrorCode = result.ErrorCode;
                    resData.ErrorMessage = result.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                resData.Data = null;
                resData.isSuccess = false;
                resData.ErrorCode = "99";
                resData.ErrorMessage = ex.Message;
            }
            return resData;
        }

        public async Task<ResultModel<List<Project>>> GetAllProject()
        {
            ResultModel<List<Project>> resData = new ResultModel<List<Project>>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<Project>>>("api/Management/getAllProjectData");

                if (result.isSuccess)
                {
                    projects = result.Data;

                    resData.Data = result.Data;
                    resData.isSuccess = result.isSuccess;
                    resData.ErrorCode = result.ErrorCode;
                    resData.ErrorMessage = result.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                resData.Data = null;
                resData.isSuccess = false;
                resData.ErrorCode = "99";
                resData.ErrorMessage = ex.Message;
            }
            return resData;
        }

        // create

        public async Task<ResultModel<QueryModel<Department>>> createNewDepartment(QueryModel<Department> data)
        {
            ResultModel<QueryModel<Department>> resData = new ResultModel<QueryModel<Department>>();

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<Department>>("api/Management/createNewDepartmentData", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<Department>>>();

                    if (respBody.isSuccess)
                    {
                        resData.Data = respBody.Data;
                        resData.isSuccess = respBody.isSuccess;
                        resData.ErrorCode = respBody.ErrorCode;
                        resData.ErrorMessage = respBody.ErrorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                resData.Data = null;
                resData.isSuccess = false;
                resData.ErrorCode = "99";
                resData.ErrorMessage = ex.Message;
            }

            return resData;
        }

        public async Task<ResultModel<QueryModel<UserAdmin>>> createNewUserAdmin(QueryModel<UserAdmin> data)
        {
            ResultModel<QueryModel<UserAdmin>> resData = new ResultModel<QueryModel<UserAdmin>>();

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<UserAdmin>>("api/Management/createNewUserAdminData", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<UserAdmin>>>();

                    if (respBody.isSuccess)
                    {
                        resData.Data = respBody.Data;
                        resData.isSuccess = respBody.isSuccess;
                        resData.ErrorCode = respBody.ErrorCode;
                        resData.ErrorMessage = respBody.ErrorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                resData.Data = null;
                resData.isSuccess = false;
                resData.ErrorCode = "99";
                resData.ErrorMessage = ex.Message;
            }

            return resData;
        }

        public async Task<ResultModel<QueryModel<Project>>> createNewProject(QueryModel<Project> data)
        {
            ResultModel<QueryModel<Project>> resData = new ResultModel<QueryModel<Project>>();

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<Project>>("api/Management/createNewProjectData", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<Project>>>();

                    if (respBody.isSuccess)
                    {
                        resData.Data = respBody.Data;
                        resData.isSuccess = respBody.isSuccess;
                        resData.ErrorCode = respBody.ErrorCode;
                        resData.ErrorMessage = respBody.ErrorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                resData.Data = null;
                resData.isSuccess = false;
                resData.ErrorCode = "99";
                resData.ErrorMessage = ex.Message;
            }

            return resData;
        }

        // edit

        public async Task<ResultModel<QueryModel<Department>>> editDepartment(QueryModel<Department> data)
        {
            ResultModel<QueryModel<Department>> resData = new ResultModel<QueryModel<Department>>();

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<Department>>("api/Management/editDepartmentData", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<Department>>>();

                    if (respBody.isSuccess)
                    {
                        resData.Data = respBody.Data;
                        resData.isSuccess = respBody.isSuccess;
                        resData.ErrorCode = respBody.ErrorCode;
                        resData.ErrorMessage = respBody.ErrorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                resData.Data = null;
                resData.isSuccess = false;
                resData.ErrorCode = "99";
                resData.ErrorMessage = ex.Message;
            }

            return resData;
        }

        public async Task<ResultModel<QueryModel<UserAdmin>>> editUser(QueryModel<UserAdmin> data)
        {
            ResultModel<QueryModel<UserAdmin>> resData = new ResultModel<QueryModel<UserAdmin>>();

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<UserAdmin>>("api/Management/editUserAdminData", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<UserAdmin>>>();

                    if (respBody.isSuccess)
                    {
                        resData.Data = respBody.Data;
                        resData.isSuccess = respBody.isSuccess;
                        resData.ErrorCode = respBody.ErrorCode;
                        resData.ErrorMessage = respBody.ErrorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                resData.Data = null;
                resData.isSuccess = false;
                resData.ErrorCode = "99";
                resData.ErrorMessage = ex.Message;
            }

            return resData;
        }

        public async Task<ResultModel<QueryModel<Project>>> editProject(QueryModel<Project> data)
        {
            ResultModel<QueryModel<Project>> resData = new ResultModel<QueryModel<Project>>();

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<Project>>("api/Management/editProjectData", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<Project>>>();

                    if (respBody.isSuccess)
                    {
                        resData.Data = respBody.Data;
                        resData.isSuccess = respBody.isSuccess;
                        resData.ErrorCode = respBody.ErrorCode;
                        resData.ErrorMessage = respBody.ErrorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                resData.Data = null;
                resData.isSuccess = false;
                resData.ErrorCode = "99";
                resData.ErrorMessage = ex.Message;
            }

            return resData;
        }

        // check existing

        public async Task<bool> checkDepartmentExisting(string DeptID)
        {
            ResultModel<string> resData = new ResultModel<string>();

            var temp = Base64Encode(DeptID);

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<string>>($"api/Procedure/isProcedureDataPresent/{temp}");

                if (result.isSuccess)
                {
                    resData.Data = result.Data;
                    resData.isSuccess = result.isSuccess;
                    resData.ErrorCode = result.ErrorCode;
                    resData.ErrorMessage = result.ErrorMessage;
                }
                else
                {
                    resData.Data = result.Data;
                    resData.isSuccess = result.isSuccess;
                    resData.ErrorCode = result.ErrorCode;
                    resData.ErrorMessage = result.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                resData.Data = null;
                resData.isSuccess = false;
                resData.ErrorCode = "99";
                resData.ErrorMessage = ex.Message;
            }
            return resData.isSuccess;
        }

        public async Task<bool> checkUserAdminExisting(string userEmail)
        {
            ResultModel<string> resData = new ResultModel<string>();

            var temp = Base64Encode(userEmail);

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<string>>($"api/Procedure/isUserAdminPresent/{temp}");

                if (result.isSuccess)
                {
                    resData.Data = result.Data;
                    resData.isSuccess = result.isSuccess;
                    resData.ErrorCode = result.ErrorCode;
                    resData.ErrorMessage = result.ErrorMessage;
                }
                else
                {
                    resData.Data = result.Data;
                    resData.isSuccess = result.isSuccess;
                    resData.ErrorCode = result.ErrorCode;
                    resData.ErrorMessage = result.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                resData.Data = null;
                resData.isSuccess = false;
                resData.ErrorCode = "99";
                resData.ErrorMessage = ex.Message;
            }
            return resData.isSuccess;
        }

        public async Task<bool> checkProjectExisting(string projectName)
        {
            ResultModel<string> resData = new ResultModel<string>();

            var temp = Base64Encode(projectName);

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<string>>($"api/Procedure/isProjectPresent/{temp}");

                if (result.isSuccess)
                {
                    resData.Data = result.Data;
                    resData.isSuccess = result.isSuccess;
                    resData.ErrorCode = result.ErrorCode;
                    resData.ErrorMessage = result.ErrorMessage;
                }
                else
                {
                    resData.Data = result.Data;
                    resData.isSuccess = result.isSuccess;
                    resData.ErrorCode = result.ErrorCode;
                    resData.ErrorMessage = result.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                resData.Data = null;
                resData.isSuccess = false;
                resData.ErrorCode = "99";
                resData.ErrorMessage = ex.Message;
            }
            return resData.isSuccess;
        }


    }
}
