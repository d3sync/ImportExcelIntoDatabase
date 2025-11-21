namespace ImportExcelIntoDatabase.Models
{
    public enum ErrorHandlingStrategy
    {
        StopOnFirstError,
        SkipErrorsAndContinue,
        UseTransaction
    }
    
    public class ImportResult
    {
        public int TotalRows { get; set; }
        public int SuccessfulRows { get; set; }
        public int FailedRows { get; set; }
        public List<ImportError> Errors { get; set; } = new();
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration => EndTime - StartTime;
        public bool IsSuccessful => FailedRows == 0;
        public string Summary => $"Imported {SuccessfulRows} of {TotalRows} rows. {FailedRows} failed.";
    }
}
