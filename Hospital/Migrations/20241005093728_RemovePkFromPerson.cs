using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Migrations
{
    /// <inheritdoc />
    public partial class RemovePkFromPerson : Migration
    {
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			// Step 1: Drop foreign key constraints (if applicable)
			// Ensure you drop any foreign keys that reference these Ids first, if necessary
			// migrationBuilder.DropForeignKey(name: "FK_Appointments_Patients_PatientId", table: "Appointments");

			// Step 2: Drop primary key constraints (if applicable)
			migrationBuilder.DropPrimaryKey(
				name: "PK_Patients",
				table: "Patients");

			migrationBuilder.DropPrimaryKey(
				name: "PK_Doctors",
				table: "Doctors");

			// Step 3: Drop the existing Id columns
			migrationBuilder.DropColumn(
				name: "Id",
				table: "Patients");

			migrationBuilder.DropColumn(
				name: "Id",
				table: "Doctors");

			// Step 4: Add new Id columns as strings
			migrationBuilder.AddColumn<string>(
				name: "Id",
				table: "Patients",
				type: "nvarchar(450)",
				nullable: false);

			migrationBuilder.AddColumn<string>(
				name: "Id",
				table: "Doctors",
				type: "nvarchar(450)",
				nullable: false);


			// Step 6: Add the primary keys back
			migrationBuilder.AddPrimaryKey(
				name: "PK_Patients",
				table: "Patients",
				column: "Id");

			migrationBuilder.AddPrimaryKey(
				name: "PK_Doctors",
				table: "Doctors",
				column: "Id");

			// Step 7: Restore foreign key constraints if you dropped them
			// migrationBuilder.AddForeignKey(name: "FK_Appointments_Patients_PatientId", table: "Appointments", column: "PatientId", principalTable: "Patients", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			// Reverse the operations in Down method
			migrationBuilder.DropPrimaryKey(
				name: "PK_Patients",
				table: "Patients");

			migrationBuilder.DropPrimaryKey(
				name: "PK_Doctors",
				table: "Doctors");

			migrationBuilder.DropColumn(
				name: "Id",
				table: "Patients");

			migrationBuilder.DropColumn(
				name: "Id",
				table: "Doctors");

			migrationBuilder.AddColumn<int>(
				name: "Id",
				table: "Patients",
				type: "int",
				nullable: false)
				.Annotation("SqlServer:Identity", "1, 1");

			migrationBuilder.AddColumn<int>(
				name: "Id",
				table: "Doctors",
				type: "int",
				nullable: false)
				.Annotation("SqlServer:Identity", "1, 1");

			migrationBuilder.AddPrimaryKey(
				name: "PK_Patients",
				table: "Patients",
				column: "Id");

			migrationBuilder.AddPrimaryKey(
				name: "PK_Doctors",
				table: "Doctors",
				column: "Id");
		}

	}
}
