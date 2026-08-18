namespace EmployeeManagement.Data.Dtos
{
    public class UpdateEmployeeDto
    {
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? MobileNo { get; set; }
        public string? EmailId { get; set; }
        public string? PANCardNo { get; set; }
        public DateTime? JoiningDate { get; set; }
        public DateTime? PreviousCompanyLastWorkingDate { get; set; }
        public string? Education { get; set; }
    }
}
