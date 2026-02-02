using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterviewTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddMorePropertiesToEmployer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "dateOfJoining",
                table: "Employers",
                type: "DATE",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "interviewLevel",
                table: "Employers",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "notes",
                table: "Employers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "offeredRole",
                table: "Employers",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "dateOfJoining",
                table: "Employers");

            migrationBuilder.DropColumn(
                name: "interviewLevel",
                table: "Employers");

            migrationBuilder.DropColumn(
                name: "notes",
                table: "Employers");

            migrationBuilder.DropColumn(
                name: "offeredRole",
                table: "Employers");
        }
    }
}
