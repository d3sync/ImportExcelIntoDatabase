namespace ImportExcelIntoDatabase.Models
{
    public class ColumnMapping
    {
        public string ExcelColumn { get; set; } = string.Empty;
        public string SqlColumn { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
        public string DataType { get; set; } = string.Empty;
    }
}
