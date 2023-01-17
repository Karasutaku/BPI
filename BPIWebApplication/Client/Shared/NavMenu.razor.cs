using BPIWebApplication.Shared.MainModel.Login;
using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using System.Runtime.Serialization;

namespace BPIWebApplication.Client.Shared
{
    public partial class NavMenu : ComponentBase
    {
        private bool collapseNavMenu = true;
        private bool showSopMenu = true;

        private FacadeUserModule moduleData = new();

        //private bool showModalTrigger = false;

        private int clickedMainMenuId = 0;
        private int prevClickedMainMenuId = 0;
        private bool hasPageName = false;
        private bool expandMainMenu = false;

        //private ActiveUser<LoginUser> activeUser = new ActiveUser<LoginUser>();
        //private ActiveUser activeUser = new();
        private List<FacadeUserModuleResp> module = new();
        private List<ModuleCategory> mainModule = new();

        private UserPrivileges privData = new();
        private List<string> userPriv = new();

        private string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        //private void showModal() => showModalTrigger = true;
        //private void hideModal() => showModalTrigger = false;

        protected override async Task OnInitializedAsync()
        {
            //activeUser.Name = Base64Decode(await sessionStorage.GetItemAsync<string>("userName"));
            //activeUser.role = Base64Decode(await sessionStorage.GetItemAsync<string>("role"));

            //activeUser.token = await sessionStorage.GetItemAsync<string>("token");
            //activeUser.userName = Base64Decode(await sessionStorage.GetItemAsync<string>("userName"));
            //activeUser.company = Base64Decode(await sessionStorage.GetItemAsync<string>("CompLoc")).Split("_")[0];
            //activeUser.location = Base64Decode(await sessionStorage.GetItemAsync<string>("CompLoc")).Split("_")[1];
            //activeUser.sessionId = await sessionStorage.GetItemAsync<string>("SessionId");
            //activeUser.appV = Convert.ToInt32(Base64Decode(await sessionStorage.GetItemAsync<string>("AppV")));

            await getUserModule();
        }

        private static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        private async Task getUserModule()
        {
            // module

            moduleData.SoapHeader.sessionid = LoginService.activeUser.sessionId;
            moduleData.SoapHeader.macaddress = ""; //
            moduleData.SoapHeader.ipclient = ""; //
            moduleData.SoapHeader.applicationid = LoginService.activeUser.appV; //
            moduleData.SoapHeader.locationid = LoginService.activeUser.location;
            moduleData.SoapHeader.companyid = Convert.ToInt32(LoginService.activeUser.company);
            moduleData.CompanyId = Convert.ToInt32(LoginService.activeUser.company);
            moduleData.LocationId = LoginService.activeUser.location;

            if (LoginService.activeUser.location.IsNullOrEmpty())
            {
                moduleData.ModuleTypeId = "CMP";
            }
            else
            {
                moduleData.ModuleTypeId = "LOC";
            }

            moduleData.ApplicationId = LoginService.activeUser.appV; //
            moduleData.UserName = LoginService.activeUser.userName;

            var moduleResp = await LoginService.frameworkApiFacadeModule(moduleData, LoginService.activeUser.token);

            if (moduleResp.Data.Any())
            {
                module = moduleResp.Data;

                foreach (var modId in module)
                {
                    ModuleCategory temp = new();

                    temp.moduleCategoryId = modId.moduleCategoryId;
                    temp.moduleCategoryName = modId.moduleCategoryName;

                    if (mainModule.FirstOrDefault(x => x.moduleCategoryId.Equals(temp.moduleCategoryId)) == null)
                    {
                        mainModule.Add(temp);
                    }
                    
                }
            }

            
        }

        private async void ToggleNavMenu(FacadeUserModuleResp menu)
        {
            collapseNavMenu = !collapseNavMenu;

            string tkn = await sessionStorage.GetItemAsync<string>("token");

            //privData.moduleId = menu.moduleId;
            //privData.UserName = Base64Decode(await sessionStorage.GetItemAsync<string>("userName"));
            //privData.userLocationParam = new();
            //privData.userLocationParam.SessionId = await sessionStorage.GetItemAsync<string>("SessionId");
            //privData.userLocationParam.MacAddress = "";
            //privData.userLocationParam.IpClient = "";
            //privData.userLocationParam.ApplicationId = Convert.ToInt32(Base64Decode(await sessionStorage.GetItemAsync<string>("AppV")));
            //privData.userLocationParam.LocationId = Base64Decode(await sessionStorage.GetItemAsync<string>("CompLoc")).Split("_")[1];
            //privData.userLocationParam.Name = Base64Decode(await sessionStorage.GetItemAsync<string>("userName"));
            //privData.userLocationParam.CompanyId = Convert.ToInt32(Base64Decode(await sessionStorage.GetItemAsync<string>("CompLoc")).Split("_")[0]);
            //privData.userLocationParam.PageIndex = 1;
            //privData.userLocationParam.PageSize = 100;
            //privData.privileges = new();

            privData.moduleId = menu.moduleId;
            privData.UserName = LoginService.activeUser.userName;
            privData.userLocationParam = new();
            privData.userLocationParam.SessionId = LoginService.activeUser.sessionId;
            privData.userLocationParam.MacAddress = "";
            privData.userLocationParam.IpClient = "";
            privData.userLocationParam.ApplicationId = LoginService.activeUser.appV;
            privData.userLocationParam.LocationId = LoginService.activeUser.location;
            privData.userLocationParam.Name = LoginService.activeUser.userName;
            privData.userLocationParam.CompanyId = Convert.ToInt32(LoginService.activeUser.company);
            privData.userLocationParam.PageIndex = 1;
            privData.userLocationParam.PageSize = 100;
            privData.privileges = new();

            var res = await LoginService.frameworkApiFacadePrivilege(privData, tkn);

            userPriv.Clear();

            if (res.isSuccess)
            {
                if (res.Data.privileges.Any())
                {
                    foreach (var priv in res.Data.privileges)
                    {
                        userPriv.Add(priv.privilegeId);
                    }
                }

                //syncSessionStorage.RemoveItem("PagePrivileges");
                //await sessionStorage.SetItemAsync("PagePrivileges", userPriv);

                LoginService.activeUser.userPrivileges = userPriv;
            }

        }

        private void toggleMainMenu(ModuleCategory menu)
        {
            //showSopMenu = !showSopMenu;

            clickedMainMenuId = menu.moduleCategoryId;

            if (prevClickedMainMenuId != clickedMainMenuId)
            {
                if (!menu.moduleCategoryName.IsNullOrEmpty())
                {
                    hasPageName = true;
                    expandMainMenu = !expandMainMenu;
                }
                else
                {
                    expandMainMenu = false;
                    hasPageName = false;
                }
            }
            else
            {
                expandMainMenu = !expandMainMenu;
            }

            prevClickedMainMenuId = clickedMainMenuId;

        }


        private async void confirmLogout()
        {
            await sessionStorage.ClearAsync();

            navigate.NavigateTo("/");
        }
    }
}
