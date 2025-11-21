namespace ImportExcelIntoDatabase.Models
{
    public class ExcelData
    {
        public List<string> Headers { get; set; } = new();
        public List<List<object>> Rows { get; set; } = new();
        public int StartRow { get; set; }
        public bool HasHeaders { get; set; }
    }
}
