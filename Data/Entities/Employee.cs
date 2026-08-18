using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Data.Entities
{
    [Table("employee")]
    public class Employee
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeId { get; set; }

        [Required]
        [MaxLength(150)]
        [Column("employee_name")]
        public string EmployeeName { get; set; }

        [Required]
        [MaxLength(10)]
        [Column("mobile_no")]
        public string MobileNo { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("email_id")]
        public string EmailId { get; set; }

        [MaxLength(10)]
        [Column("pan_card_no")]
        public string? PANCardNo { get; set; }

        [Required]
        [Column("joining_date")]
        public DateOnly JoiningDate { get; set; }

        [Column("previous_company_last_working_date")]
        public DateOnly? PreviousCompanyLastWorkingDate { get; set; }

        [Required]
        [MaxLength(10)]
        [Column("education")]
        public string Education { get; set; }
    }
}
