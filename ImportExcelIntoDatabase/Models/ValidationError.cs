namespace ImportExcelIntoDatabase.Models
{
    public class ValidationError
    {
        public int RowNumber { get; set; }
        public string ColumnName { get; set; } = string.Empty;
        public string ExcelValue { get; set; } = string.Empty;
        public string ExpectedType { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public ValidationSeverity Severity { get; set; }
    }
    
    public enum ValidationSeverity
    {
        Warning,  // Can be imported but might cause issues
        Error     // Will definitely fail import
    }
    
    public class ValidationResult
    {
        public bool IsValid => Errors.Count == 0;
        public List<ValidationError> Errors { get; set; } = new();
        public List<ValidationError> Warnings { get; set; } = new();
        public int TotalRowsValidated { get; set; }
        public int ErrorCount => Errors.Count;
        public int WarningCount => Warnings.Count;
        
        public string Summary => 
            $"Validated {TotalRowsValidated} rows: {ErrorCount} errors, {WarningCount} warnings";
    }
}
