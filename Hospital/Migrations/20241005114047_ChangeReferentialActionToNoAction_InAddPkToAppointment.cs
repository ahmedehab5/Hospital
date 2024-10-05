using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Migrations
{
    /// <inheritdoc />
    public partial class ChangeReferentialActionToNoAction_InAddPkToAppointment : Migration
    {
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			// Drop the existing foreign key constraints first
			migrationBuilder.DropForeignKey(
				name: "FK_Appointments_Doctors_DoctorId",
				table: "Appointments");

			migrationBuilder.DropForeignKey(
				name: "FK_Appointments_Patients_PatientId",
				table: "Appointments");

			// Add the new foreign key constraints with ON DELETE NO ACTION
			migrationBuilder.AddForeignKey(
				name: "FK_Appointments_Doctors_DoctorId",
				table: "Appointments",
				column: "DoctorId",
				principalTable: "Doctors",
				principalColumn: "Id",
				onDelete: ReferentialAction.NoAction); // Changed to NoAction

			migrationBuilder.AddForeignKey(
				name: "FK_Appointments_Patients_PatientId",
				table: "Appointments",
				column: "PatientId",
				principalTable: "Patients",
				principalColumn: "Id",
				onDelete: ReferentialAction.NoAction); // Changed to NoAction
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			// Drop the new foreign key constraints in Down method as well
			migrationBuilder.DropForeignKey(
				name: "FK_Appointments_Doctors_DoctorId",
				table: "Appointments");

			migrationBuilder.DropForeignKey(
				name: "FK_Appointments_Patients_PatientId",
				table: "Appointments");

			// Optionally, you can add back the previous constraints
			migrationBuilder.AddForeignKey(
				name: "FK_Appointments_Doctors_DoctorId",
				table: "Appointments",
				column: "DoctorId",
				principalTable: "Doctors",
				principalColumn: "Id",
				onDelete: ReferentialAction.SetNull); // Original setting

			migrationBuilder.AddForeignKey(
				name: "FK_Appointments_Patients_PatientId",
				table: "Appointments",
				column: "PatientId",
				principalTable: "Patients",
				principalColumn: "Id",
				onDelete: ReferentialAction.SetNull); // Original setting
		}

	}
}
