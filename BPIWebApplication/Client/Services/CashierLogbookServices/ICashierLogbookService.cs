using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.MainModel;
using BPIWebApplication.Shared.MainModel.CashierLogbook;
using BPIWebApplication.Shared.MainModel.Company;
using BPIWebApplication.Shared.PagesModel.CashierLogbook;

namespace BPIWebApplication.Client.Services.CashierLogbookServices
{
    public interface ICashierLogbookService
    {
        public List<Shift> Shifts { get; set; }
        public List<AmountTypes> types { get; set; }
        public List<AmountCategories> categories { get; set; }
        public List<AmountSubCategories> subCategories { get; set; }
        public List<CashierLogData> mainLogs { get; set; }
        public List<CashierLogData> transitLogs { get; set; }

        // get
        public Task<ResultModel<List<Shift>>> getShiftData(string moduleName);
        public Task<ResultModel<CashierLogbookCategories>> getLogbookCategories();
        public Task<ResultModel<List<CashierLogData>>> getLogData(string locPage);

        // create
        public Task<ResultModel<QueryModel<CashierLogData>>> createLogData(QueryModel<CashierLogData> data);

        // edit
        public Task<ResultModel<QueryModel<CashierLogData>>> editLogData(QueryModel<CashierLogData> data);
        public Task<ResultModel<QueryModel<CashierLogApproval>>> editBrankasApproveLogOnConfirm(QueryModel<CashierLogApproval> data);
    }
}
