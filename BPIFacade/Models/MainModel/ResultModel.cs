namespace BPIFacade.Models.MainModel
{
    public class ResultModel<T>
    {
        public T? Data { get; set; }
        public bool isSuccess { get; set; }
        public string ErrorCode { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
