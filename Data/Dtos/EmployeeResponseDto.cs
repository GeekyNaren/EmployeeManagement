namespace EmployeeManagement.Data.Dtos
{
    public class EmployeeResponseDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string MobileNo { get; set; } = string.Empty;
        public string EmailId { get; set; } = string.Empty;
        public string? PANCardNo { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime? PreviousCompanyLastWorkingDate { get; set; }
        public string Education { get; set; } = string.Empty;
    }
}
