namespace EmployeeManagement.Data.Dtos
{
    public class EmployeeResponseDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string MobileNo { get; set; } = string.Empty;
        public string EmailId { get; set; } = string.Empty;
        public string? PANCardNo { get; set; }
        public DateOnly JoiningDate { get; set; }
        public DateOnly? PreviousCompanyLastWorkingDate { get; set; }
        public string Education { get; set; } = string.Empty;
    }
}
