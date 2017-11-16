using Microsoft.EntityFrameworkCore.Migrations;

namespace P01_HospitalDatabase.Migrations
{
    public partial class AddDoctors2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_Visitations_Doctors_DoctorId",
                "Visitations");

            migrationBuilder.AlterColumn<int>(
                "DoctorId",
                "Visitations",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                "FK_Visitations_Doctors_DoctorId",
                "Visitations",
                "DoctorId",
                "Doctors",
                principalColumn: "DoctorId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_Visitations_Doctors_DoctorId",
                "Visitations");

            migrationBuilder.AlterColumn<int>(
                "DoctorId",
                "Visitations",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                "FK_Visitations_Doctors_DoctorId",
                "Visitations",
                "DoctorId",
                "Doctors",
                principalColumn: "DoctorId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}