using BPIWebApplication.Client.Pages.SopPages;
using BPIWebApplication.Shared;
using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.FileUploadModel;
using BPIWebApplication.Shared.PagesModel.AccessHistory;
using BPIWebApplication.Shared.PagesModel.ApplyProcedure;
using BPIWebApplication.Shared.PagesModel.Dashboard;
using BPIWebApplication.Shared.ReportModel;
using Microsoft.AspNetCore.Components.Forms;

namespace BPIWebApplication.Client.Services.ProcedureServices
{
    public interface IProcedureService
    {
        // data pool
        bool isProcedurePresent { get; set; }
        //List<BisnisUnit> bisnisUnits { get; set; }
        //List<Department> departments { get; set; }
        List<Procedure> procedures { get; set; }
        List<HistoryAccess> historyAccess { get; set; }
        List<HistoryAccess> historyAccessReport { get; set; }
        List<DepartmentProcedure> departmentProcedures { get; set;}

        // get
        //Task<ResultModel<List<BisnisUnit>>> GetAllBisnisUnit();
        //Task<ResultModel<List<Department>>> GetAllDepartment();
        Task<ResultModel<List<Procedure>>> GetAllProcedure();
        Task<ResultModel<List<HistoryAccess>>> GetHistoryAccessbyPaging(int pageNo);
        Task<ResultModel<List<HistoryAccess>>> GetHistoryAccessbyFilterwithPaging(AccessHistoryFilter data);
        Task<ResultModel<List<HistoryAccess>>> GetHistoryAccessReportbyFilter(AccessHistoryReport data);
        Task<ResultModel<List<DepartmentProcedure>>> GetAllDepartmentProcedure();
        Task<ResultModel<List<DepartmentProcedure>>> GetDepartmentProcedurewithFilterbyPaging(DashboardFilter data);
        Task<ResultModel<List<DepartmentProcedure>>> GetDepartmentProcedurewithPaging(int pageNo);
        Task<ResultModel<FileReadyDownload>> GetFile(string path);

        // create
        Task<ResultModel<QueryModel<ApplyProcedureMultiDept>>> createDepartmentProcedure(QueryModel<ApplyProcedureMultiDept> data);
        Task<ResultModel<ProcedureUpload>> createProcedure(ProcedureUpload data);
        Task<ResultModel<QueryModel<HistoryAccess>>> createHistoryAccess(QueryModel<HistoryAccess> data);

        // edit
        Task<ResultModel<ProcedureUpload>> editProcedure(ProcedureUpload data);

        // delete
        Task<ResultModel<QueryModel<ApplyProcedureMultiDept>>> deleteDepartmentProcedure(QueryModel<ApplyProcedureMultiDept> data);

        // is exist
        Task<bool> checkProsedureExisting(string ProcNo);
        Task<int> getDepartmentProcedureNumberofPage();
        Task<int> getDepartmentProcedurewithFilterNumberofPage(DashboardFilter data);
        Task<int> getHistoryAccessNumberofPage();
        Task<int> getAccessHistorywithFilterNumberofPage(AccessHistoryFilter data);

        // config

        Task<long> getProcedureMaxFileSize();

    }
}
