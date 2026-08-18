using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EmployeeManagement.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "employee",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    employee_name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    mobile_no = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    email_id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    pan_card_no = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    joining_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    previous_company_last_working_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    education = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employee", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "employee");
        }
    }
}
