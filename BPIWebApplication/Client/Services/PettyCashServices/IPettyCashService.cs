using BPIWebApplication.Shared.MainModel.PettyCash;
using BPIWebApplication.Shared.MainModel.Company;
using BPIWebApplication.Shared.MainModel;
using BPIWebApplication.Shared.DbModel;
using static System.Net.WebRequestMethods;

namespace BPIWebApplication.Client.Services.PettyCashServices
{
    public interface IPettyCashService
    {
        // data pool
        List<Advance> advances { get; set; }
        //List<AdvanceLine> advanceLines { get; set; }
        List<Expense> expenses { get; set; }
        //List<ExpenseLine> expenseLines { get; set; }
        List<Reimburse> reimburses { get; set; }
        List<Advance> padvances { get; set; }
        List<Expense> pexpenses { get; set; }
        List<Reimburse> preimburses { get; set; }
        List<Account> coas { get; set; }
        List<BPIWebApplication.Shared.MainModel.Stream.FileStream> fileStreams { get; set; }

        // get

        Task<ResultModel<List<Account>>> getCoabyModule(string moduleName);
        Task<ResultModel<LocationBalanceDetails>> getPettyCashOutstandingAmount(string loc);
        //Task<ResultModel<BalanceDetails>> getLocationBudgetDetails(string loc);
        Task<ResultModel<List<Advance>>> getAdvanceDatabyLocation(string locPage);
        Task<ResultModel<List<Advance>>> getAdvanceDatabyUser(string locPage);
        Task<ResultModel<List<Expense>>> getExpenseDatabyLocation(string locPage);
        Task<ResultModel<List<Reimburse>>> getReimburseDatabyLocation(string locPage);
        //Task<ResultModel<List<AdvanceLine>>> getAdvanceLinesbyID(string AdvanceId);
        Task<ResultModel<List<BPIWebApplication.Shared.MainModel.Stream.FileStream>>> getAttachmentFileStream(string Id);
        Task<ResultModel<BPIWebApplication.Shared.MainModel.Stream.FileStream>> getLedgerDataEntriesbyDate(List<ledgerParam> data);

        // create

        Task<ResultModel<QueryModel<Advance>>> createAdvanceData(QueryModel<Advance> data);
        Task<ResultModel<QueryModel<Expense>>> createExpenseData(ExpenseStream data);
        Task<ResultModel<QueryModel<Reimburse>>> createReimburseData(ReimburseStream data);
        Task<ResultModel<string>> createDocumentID(string docType);

        // update
        Task<ResultModel<QueryModel<BalanceDetails>>> updateLocationBudgetDetails(QueryModel<BalanceDetails> data);
        Task<ResultModel<QueryModel<CutoffDetails>>> updateLocationCutoffDate(QueryModel<CutoffDetails> data);
        Task<ResultModel<QueryModel<string>>> updateAdvanceDataSettlement(QueryModel<string> data);
        Task<ResultModel<QueryModel<List<string>>>> updateExpenseDataSettlement(QueryModel<List<string>> data);
        Task<ResultModel<QueryModel<string>>> updateReimburseDataSettlement(QueryModel<string> data);
        Task<ResultModel<QueryModel<string>>> updateDocumentStatus(QueryModel<string> data);
        Task<ResultModel<QueryModel<Reimburse>>> updateReimburseLineData(QueryModel<Reimburse> data);

        // is

        Task<bool> isAdvancePresent(string AdvanceId);

        // other

        Task<int> getModulePageSize(string Table);
        Task<ResultModel<bool>> autoEmail(string param);

    }
}
