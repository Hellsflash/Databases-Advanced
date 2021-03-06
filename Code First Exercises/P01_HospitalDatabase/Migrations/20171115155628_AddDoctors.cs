﻿using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace P01_HospitalDatabase.Migrations
{
    public partial class AddDoctors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                "DoctorId",
                "Visitations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                "Doctors",
                table => new
                {
                    DoctorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Specialty = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Doctors", x => x.DoctorId); });

            migrationBuilder.CreateIndex(
                "IX_Visitations_DoctorId",
                "Visitations",
                "DoctorId");

            migrationBuilder.AddForeignKey(
                "FK_Visitations_Doctors_DoctorId",
                "Visitations",
                "DoctorId",
                "Doctors",
                principalColumn: "DoctorId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_Visitations_Doctors_DoctorId",
                "Visitations");

            migrationBuilder.DropTable(
                "Doctors");

            migrationBuilder.DropIndex(
                "IX_Visitations_DoctorId",
                "Visitations");

            migrationBuilder.DropColumn(
                "DoctorId",
                "Visitations");
        }
    }
}