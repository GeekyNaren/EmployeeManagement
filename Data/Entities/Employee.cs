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
        public required string EmployeeName { get; set; } = null!;

        [Required]
        [MaxLength(10)]
        [Column("mobile_no")]
        public required string MobileNo { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        [Column("email_id")]
        public required string EmailId { get; set; } = null!;

        [MaxLength(10)]
        [Column("pan_card_no")]
        public string? PANCardNo { get; set; }

        [Required]
        [Column("joining_date")]
        public required DateTime JoiningDate { get; set; }

        [Column("previous_company_last_working_date")]
        public DateTime? PreviousCompanyLastWorkingDate { get; set; }

        [Required]
        [MaxLength(10)]
        [Column("education")]
        public required string Education { get; set; }
    }
}
