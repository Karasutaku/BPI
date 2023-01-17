using Microsoft.AspNetCore.Components;
using BPIWebApplication.Shared.PagesModel.ApplyProcedure;
using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.MainModel.Login;

namespace BPIWebApplication.Client.Pages.SopPages
{
    public partial class ApplyProcedure : ComponentBase
    {
        Procedure previewProcedure = new Procedure();

        List<DeptSelected> deptSelected = new List<DeptSelected>();
        List<DeptSelected> deptDeleted = new List<DeptSelected>();

        //private ActiveUser<LoginUser> activeUser = new ActiveUser<LoginUser>();
        //private ActiveUser activeUser = new();

        // filter
        private string procNoFilter = string.Empty;

        private static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        protected override async Task OnInitializedAsync()
        {
            await ProcedureService.GetAllProcedure();
            await ManagementService.GetAllBisnisUnit();
            await ManagementService.GetAllDepartment();
            await ProcedureService.GetAllDepartmentProcedure();

            //activeUser.UserLogin = new LoginUser();
            //activeUser.Name = Base64Decode(await sessionStorage.GetItemAsync<string>("userName"));
            //activeUser.UserLogin.userName = Base64Decode(await sessionStorage.GetItemAsync<string>("userEmail"));
            //activeUser.role = Base64Decode(await sessionStorage.GetItemAsync<string>("role"));

            //activeUser.token = await sessionStorage.GetItemAsync<string>("token");
            //activeUser.userName = Base64Decode(await sessionStorage.GetItemAsync<string>("userName"));
            //activeUser.company = Base64Decode(await sessionStorage.GetItemAsync<string>("CompLoc")).Split("_")[0];
            //activeUser.location = Base64Decode(await sessionStorage.GetItemAsync<string>("CompLoc")).Split("_")[1];
            //activeUser.sessionId = await sessionStorage.GetItemAsync<string>("SessionId");
            //activeUser.appV = Convert.ToInt32(Base64Decode(await sessionStorage.GetItemAsync<string>("AppV")));
            //activeUser.userPrivileges = await sessionStorage.GetItemAsync<List<string>>("PagePrivileges");
        }

        // modal trigger
        private bool showModal = false;
        private bool defaultModalTrigger = false;
        private bool showModalBodyStoreTrigger = false;
        private bool showModalBodyHoTrigger = false;
        private bool showModalBodyDcTrigger = false;

        // procedure
        private bool procedureSelected = false;

        // alert trigger
        private bool alertTrigger = false;
        private bool successAlert = false;
        private string alertMessage = string.Empty;
        private string alertBody = string.Empty;

        private void modalShow()
        {
            showModal = true;
            showModalBodyStoreTrigger = false;
            showModalBodyHoTrigger = false;
            showModalBodyDcTrigger = false;
            defaultModalTrigger = true;
        }
        private void modalHide()
        {
            showModal = false;
            showModalBodyStoreTrigger = false;
            showModalBodyHoTrigger = false;
            showModalBodyDcTrigger = false;
        } 

        //private void modalSubmit()
        //{

        //}

        private void showModalBodyStore()
        {
            showModalBodyStoreTrigger = true;
            showModalBodyHoTrigger = false;
            showModalBodyDcTrigger = false;
            defaultModalTrigger = false;
        }
        private void showModalBodyHo()
        {
            showModalBodyStoreTrigger = false;
            showModalBodyHoTrigger = true;
            showModalBodyDcTrigger = false;
            defaultModalTrigger = false;
        }
        private void showModalBodyDc()
        {
            showModalBodyStoreTrigger = false;
            showModalBodyHoTrigger = false;
            showModalBodyDcTrigger = true;
            defaultModalTrigger = false;
        }

        private void resetTrigger () => alertTrigger = false;
        private void resetSuccessAlert() => successAlert = false;

        private void appendDeptSelected(string deptID)
        {
            if (deptSelected.FirstOrDefault(a => a.DepartmentID == deptID) == null)
            {
                deptSelected.Add(new DeptSelected { 
                    DepartmentID = deptID,
                    isSelected = true
                });

                var itemRemove = deptDeleted.SingleOrDefault(a => a.DepartmentID == deptID);

                if (itemRemove != null)
                {
                    deptDeleted.Remove(itemRemove);
                }
            }
            else
            {
                var itemRemove = deptSelected.SingleOrDefault(a => a.DepartmentID == deptID);
                
                if (itemRemove != null)
                {
                    deptSelected.Remove(itemRemove);
                    deptDeleted.Add(new DeptSelected
                    {
                        DepartmentID = deptID,
                        isSelected = true
                    });
                }
                    
            }
        }

        private void selectProcedure()
        {
            deptSelected.Clear();
            previewProcedure = new Procedure();

            if (ProcedureService.procedures.FirstOrDefault(a => a.ProcedureNo == procNoFilter) == null)
            {
                alertTrigger = true;
                alertMessage = "Procedure Number not exist !";
                alertBody = "Please check your Procedure Number";
                procedureSelected = false;
            }
            else
            {
                procedureSelected = true;
                previewProcedure = ProcedureService.procedures.FirstOrDefault(a => a.ProcedureNo == procNoFilter);

                List <DepartmentProcedure> temp = ProcedureService.departmentProcedures.Where(a => a.ProcedureNo == previewProcedure.ProcedureNo).ToList();

                foreach (var deptApplied in temp)
                {
                    deptSelected.Add(new DeptSelected
                    {
                        DepartmentID = deptApplied.DepartmentID,
                        isSelected = true
                    });
                }
            }

        }

        public async void handleSubmit()
        {
            try
            {
                // LIFT VALIDATION - untuk aktivasi nonaktivasi
                //if (!deptSelected.Any())
                //{
                //    alertMessage = "No Department(s) selected !";
                //    alertBody = "Please select applied department";
                //    alertTrigger = true;
                //    procedureSelected = false;
                //} 
                //else
                //{
                    QueryModel<ApplyProcedureMultiDept> applyData = new QueryModel<ApplyProcedureMultiDept>();
                    applyData.Data = new ApplyProcedureMultiDept();

                    applyData.Data.ProcedureNo = procNoFilter;
                    applyData.Data.listDepartment = deptSelected;
                    applyData.userEmail = LoginService.activeUser.userName;
                    applyData.userAction = "I";
                    applyData.userActionDate = DateTime.Now;

                    ResultModel<QueryModel<ApplyProcedureMultiDept>> res1 = await ProcedureService.createDepartmentProcedure(applyData);

                    if (res1.isSuccess)
                    {
                        if (deptDeleted.Count > 0)
                        {
                            QueryModel<ApplyProcedureMultiDept> applyDelData = new QueryModel<ApplyProcedureMultiDept>();
                            applyDelData.Data = new ApplyProcedureMultiDept();

                            applyDelData.Data.ProcedureNo = procNoFilter;
                            applyDelData.Data.listDepartment = deptDeleted;
                            applyDelData.userEmail = LoginService.activeUser.userName;
                            applyDelData.userAction = "D";
                            applyDelData.userActionDate = DateTime.Now;

                            await ProcedureService.deleteDepartmentProcedure(applyDelData);
                        }

                        alertMessage = "Success Applying Department !";
                        alertBody = "";
                        successAlert = true;
                        procedureSelected = false;

                        deptSelected.Clear();
                        deptDeleted.Clear();
                        procNoFilter = "";
                        previewProcedure = new Procedure();

                        await ProcedureService.GetAllDepartmentProcedure();
                        StateHasChanged();
                    }
                    else
                    {
                        alertMessage = "Failed Applying Department !";
                        alertBody = "Please retry your action";
                        alertTrigger = true;
                        procedureSelected = false;

                        deptSelected.Clear();
                        deptDeleted.Clear();
                        procNoFilter = "";
                        previewProcedure = new Procedure();

                        await ProcedureService.GetAllDepartmentProcedure();
                        StateHasChanged();
                    }

                //}
            }
            catch (Exception ex)
            {
                throw new Exception(alertMessage, ex);
                // await JS.InvokeVoidAsync("alert", ex.Message.ToString());
            }
        }

    }
}
