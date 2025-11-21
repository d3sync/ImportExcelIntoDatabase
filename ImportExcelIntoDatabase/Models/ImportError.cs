namespace ImportExcelIntoDatabase.Models
{
    public class ImportError
    {
        public int RowNumber { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public List<object> RowData { get; set; } = new();
        public Exception? Exception { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
