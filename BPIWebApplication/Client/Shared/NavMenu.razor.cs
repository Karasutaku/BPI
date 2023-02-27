using BPIWebApplication.Shared.MainModel.Login;
using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using System.Reflection;
using System.Runtime.Serialization;

namespace BPIWebApplication.Client.Shared
{
    public partial class NavMenu : ComponentBase
    {
        private bool collapseNavMenu = true;
        private bool showSopMenu = true;

        private FacadeUserModule moduleData = new();

        List<ChildApplication> childApplications = new();
        List<ModuleCategory> userModuleCategories = new();

        //private bool showModalTrigger = false;

        private int? clickedMainMenuId = 0;
        private int? prevClickedMainMenuId = 0;
        private bool hasPageName = false;
        private bool expandMainMenu = false;

        private int? clickedCaId = 0;
        private int? prevClickedCaId = 0;
        private bool caHasPageName = false;
        private bool expandCaMenu = false;

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

            if (await sessionStorage.ContainKeyAsync("userName"))
            {
                moduleData.SoapHeader.sessionid = await sessionStorage.GetItemAsync<string>("SessionId");
                moduleData.SoapHeader.macaddress = ""; //
                moduleData.SoapHeader.ipclient = ""; //
                moduleData.SoapHeader.applicationid = Convert.ToInt32(Base64Decode(await sessionStorage.GetItemAsync<string>("AppV"))); //
                moduleData.SoapHeader.locationid = Base64Decode(await sessionStorage.GetItemAsync<string>("CompLoc")).Split("_")[1];
                moduleData.SoapHeader.companyid = Convert.ToInt32(Base64Decode(await sessionStorage.GetItemAsync<string>("CompLoc")).Split("_")[0]);
                moduleData.CompanyId = Convert.ToInt32(Base64Decode(await sessionStorage.GetItemAsync<string>("CompLoc")).Split("_")[0]);
                moduleData.LocationId = Base64Decode(await sessionStorage.GetItemAsync<string>("CompLoc")).Split("_")[1];

                moduleData.ApplicationId = Convert.ToInt32(Base64Decode(await sessionStorage.GetItemAsync<string>("AppV"))); //
                moduleData.UserName = Base64Decode(await sessionStorage.GetItemAsync<string>("userName"));

                string locStr = Base64Decode(await sessionStorage.GetItemAsync<string>("CompLoc")).Split("_")[1];

                if (locStr.IsNullOrEmpty())
                {
                    moduleData.ModuleTypeId = "CMP";
                }
                else
                {
                    moduleData.ModuleTypeId = "LOC";
                }

                //moduleData.ModuleTypeId = "CMP";

                string tkn = await sessionStorage.GetItemAsync<string>("token");

                var moduleResp = await LoginService.frameworkApiFacadeModule(moduleData, tkn);

                module = moduleResp.Data;
                //if (moduleResp.Data.Any())
                //{
                //    module = moduleResp.Data;

                //    foreach (var modId in module)
                //    {
                //        ModuleCategory temp = new();

                //        temp.moduleCategoryId = modId.moduleCategoryId;
                //        temp.moduleCategoryName = modId.moduleCategoryName;

                //        if (mainModule.FirstOrDefault(x => x.moduleCategoryId.Equals(temp.moduleCategoryId)) == null)
                //        {
                //            mainModule.Add(temp);
                //        }

                //    }
                //}

                //var mChild = moduleResp.Data.GroupBy(x => x.childApplicationId).Select(x => x.FirstOrDefault()).OrderBy(x => x.childApplicationId).ToList();

                //foreach (var b in mChild)
                //{
                //    childApplications.Add(new ChildApplication
                //    {
                //        ChildApplicationId = b.childApplicationId,
                //        ChildApplicationName = b.childApplicationName,
                //        moduleId = b.moduleId,
                //        moduleName = b.moduleName,
                //        url = b.url,
                //        icon = b.icon
                //    });
                //}

                //var mCat = moduleResp.Data.GroupBy(x => x.moduleId).Select(x => x.FirstOrDefault()).OrderBy(x => x.childApplicationId).ThenBy(x => x.moduleCategoryId).ToList();

                //foreach (var a in mCat)
                //{
                //    userModuleCategories.Add(new UserModuleCategory
                //    {
                //        UserModuleCategoryId = a.moduleCategoryId,
                //        UserModuleCategoryName = a.moduleCategoryName,
                //        ApplicationId = a.applicationId,
                //        ChildApplicationId = a.childApplicationId
                //    });
                //}

                var mCat = moduleResp.Data.GroupBy(x => x.moduleCategoryId).Select(x => x.FirstOrDefault()).ToList();

                foreach (var a in mCat)
                {
                    userModuleCategories.Add(new ModuleCategory
                    {
                        moduleCategoryId = a.moduleCategoryId,
                        moduleCategoryName = a.moduleCategoryName,
                        ApplicationId = a.applicationId,
                        ChildApplicationId = a.childApplicationId
                    });
                }

                var mChild = moduleResp.Data.GroupBy(x => x.childApplicationId).Select(x => x.FirstOrDefault()).ToList();

                foreach (var b in mChild)
                {
                    childApplications.Add(new ChildApplication
                    {
                        ChildApplicationId = b.childApplicationId,
                        ChildApplicationName = b.childApplicationName,
                        moduleId = b.moduleId,
                        moduleName = b.moduleName,
                        url = b.url,
                        icon = b.icon
                    });
                }

            }
            else
            {
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

                //moduleData.ModuleTypeId = "CMP";

                moduleData.ApplicationId = LoginService.activeUser.appV; //
                moduleData.UserName = LoginService.activeUser.userName;

                var moduleResp = await LoginService.frameworkApiFacadeModule(moduleData, LoginService.activeUser.token);

                module = moduleResp.Data;
                //if (moduleResp.Data.Any())
                //{
                //    module = moduleResp.Data;

                //    foreach (var modId in module)
                //    {
                //        ModuleCategory temp = new();

                //        temp.moduleCategoryId = modId.moduleCategoryId;
                //        temp.moduleCategoryName = modId.moduleCategoryName;

                //        if (mainModule.FirstOrDefault(x => x.moduleCategoryId.Equals(temp.moduleCategoryId)) == null)
                //        {
                //            mainModule.Add(temp);
                //        }

                //    }
                //}


                //var mChild = moduleResp.Data.GroupBy(x => x.childApplicationId).Select(x => x.FirstOrDefault()).OrderBy(x => x.childApplicationId).ToList();

                //foreach (var b in mChild)
                //{
                //    childApplications.Add(new ChildApplication
                //    {
                //        ChildApplicationId = b.childApplicationId,
                //        ChildApplicationName = b.childApplicationName
                //    });
                //}

                //var mCat = moduleResp.Data.GroupBy(x => x.moduleId).Select(x => x.FirstOrDefault()).OrderBy(x => x.childApplicationId).ThenBy(x => x.moduleCategoryId).ToList();

                //foreach (var a in mCat)
                //{
                //    userModuleCategories.Add(new UserModuleCategory
                //    {
                //        UserModuleCategoryId = a.moduleCategoryId,
                //        UserModuleCategoryName = a.moduleCategoryName,
                //        ApplicationId = a.applicationId,
                //        ChildApplicationId = a.childApplicationId
                //    });
                //}

                var mCat = moduleResp.Data.GroupBy(x => x.moduleCategoryId).Select(x => x.FirstOrDefault()).ToList();

                foreach (var a in mCat)
                {
                    userModuleCategories.Add(new ModuleCategory
                    {
                        moduleCategoryId = a.moduleCategoryId,
                        moduleCategoryName = a.moduleCategoryName,
                        ApplicationId = a.applicationId,
                        ChildApplicationId = a.childApplicationId
                    });
                }

                var mChild = moduleResp.Data.GroupBy(x => x.childApplicationId).Select(x => x.FirstOrDefault()).ToList();

                foreach (var b in mChild)
                {
                    childApplications.Add(new ChildApplication
                    {
                        ChildApplicationId = b.childApplicationId,
                        ChildApplicationName = b.childApplicationName,
                        moduleId = b.moduleId,
                        moduleName = b.moduleName,
                        url = b.url,
                        icon = b.icon
                    });
                }

            }
            
        }

        private async void ToggleNavMenu(FacadeUserModuleResp menu)
        {
            collapseNavMenu = !collapseNavMenu;

            //if (await sessionStorage.ContainKeyAsync("PagePrivileges"))
            //{
            //    await sessionStorage.RemoveItemAsync("PagePrivileges");
            //}

            if (!LoginService.activeUser.userPrivileges.IsNullOrEmpty())
            {
                LoginService.activeUser.userPrivileges.Clear();
            }

            string tkn = await sessionStorage.GetItemAsync<string>("token");

            if (await sessionStorage.ContainKeyAsync("userName"))
            {
                privData.moduleId = menu.moduleId;
                privData.UserName = Base64Decode(await sessionStorage.GetItemAsync<string>("userName"));
                privData.userLocationParam = new();
                privData.userLocationParam.SessionId = await sessionStorage.GetItemAsync<string>("SessionId");
                privData.userLocationParam.MacAddress = "";
                privData.userLocationParam.IpClient = "";
                privData.userLocationParam.ApplicationId = Convert.ToInt32(Base64Decode(await sessionStorage.GetItemAsync<string>("AppV")));
                privData.userLocationParam.LocationId = Base64Decode(await sessionStorage.GetItemAsync<string>("CompLoc")).Split("_")[1];
                privData.userLocationParam.Name = Base64Decode(await sessionStorage.GetItemAsync<string>("userName"));
                privData.userLocationParam.CompanyId = Convert.ToInt32(Base64Decode(await sessionStorage.GetItemAsync<string>("CompLoc")).Split("_")[0]);
                privData.userLocationParam.PageIndex = 1;
                privData.userLocationParam.PageSize = 100;
                privData.privileges = new();
            }
            else
            {
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
            }

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

                await sessionStorage.SetItemAsync("PagePrivileges", userPriv);
                
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
                    expandMainMenu = true;
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

            if (expandMainMenu.Equals(false))
            {
                clickedMainMenuId = 0;
            }
        }

        private void toggleCaMenu(ChildApplication ca)
        {
            //showSopMenu = !showSopMenu;

            clickedCaId = ca.ChildApplicationId;

            if (prevClickedCaId != clickedCaId)
            {
                if (!ca.ChildApplicationName.IsNullOrEmpty())
                {
                    caHasPageName = true;
                    expandCaMenu = true;
                }
                else
                {
                    caHasPageName = false;
                    expandCaMenu = false;
                }
            }
            else
            {
                expandCaMenu = !expandCaMenu;
            }

            prevClickedCaId = clickedCaId;
        }

        private async void confirmLogout()
        {
            await sessionStorage.ClearAsync();

            LoginService.activeUser.token = "";
            LoginService.activeUser.userName = "";
            LoginService.activeUser.company = "";
            LoginService.activeUser.location = "";
            LoginService.activeUser.appV = 0;
            LoginService.activeUser.userPrivileges.Clear();

            navigate.NavigateTo("/");
        }
    }
}
