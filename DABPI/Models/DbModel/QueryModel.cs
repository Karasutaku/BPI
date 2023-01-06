namespace BPIDA.Models.DbModel
{
    public class QueryModel<T>
    {
        public T? Data { get; set; }
        public string userEmail { get; set; } = string.Empty;
        public string userAction { get; set; } = string.Empty;
        public DateTime userActionDate { get; set; } = DateTime.Now;
    }
}
