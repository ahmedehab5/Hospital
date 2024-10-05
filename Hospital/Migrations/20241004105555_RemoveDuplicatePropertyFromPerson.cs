using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDuplicatePropertyFromPerson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Doctors");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Patients",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Patients",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Doctors",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Doctors",
                newName: "FirstName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Patients",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Patients",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Doctors",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Doctors",
                newName: "Password");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
