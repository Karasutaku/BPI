using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.PagesModel.AddEditProject;
using BPIWebApplication.Shared.PagesModel.AddEditUser;

namespace BPIWebApplication.Client.Services.ManagementServices
{
    public interface IManagementService
    {
        // data pool
        List<BisnisUnit> bisnisUnits { get; set; }
        List<Department> departments { get; set; }
        List<UserAdmin> users { get; set; }
        List<Project> projects { get; set; }

        // get
        Task<ResultModel<List<BisnisUnit>>> GetAllBisnisUnit();
        Task<ResultModel<List<Department>>> GetAllDepartment();
        Task<ResultModel<List<UserAdmin>>> GetAllUserAdmin();
        Task<ResultModel<List<Project>>> GetAllProject();

        // create

        Task<ResultModel<QueryModel<Department>>> createNewDepartment(QueryModel<Department> data);
        Task<ResultModel<QueryModel<UserAdmin>>> createNewUserAdmin(QueryModel<UserAdmin> data);
        Task<ResultModel<QueryModel<Project>>> createNewProject(QueryModel<Project> data);

        // edit

        Task<ResultModel<QueryModel<Department>>> editDepartment(QueryModel<Department> data);
        Task<ResultModel<QueryModel<UserAdmin>>> editUser(QueryModel<UserAdmin> data);
        Task<ResultModel<QueryModel<Project>>> editProject(QueryModel<Project> data);

        // check existing

        Task<bool> checkDepartmentExisting(string DeptID);
        Task<bool> checkUserAdminExisting(string userEmail);
        Task<bool> checkProjectExisting(string projectNo);
    }
}
