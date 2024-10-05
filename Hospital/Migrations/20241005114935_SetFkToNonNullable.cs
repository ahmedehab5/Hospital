using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Migrations
{
    /// <inheritdoc />
    public partial class SetFkToNonNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.DropForeignKey(
			name: "FK_Appointments_Doctors_DoctorId",
			table: "Appointments");

			migrationBuilder.DropForeignKey(
				name: "FK_Appointments_Patients_PatientId",
				table: "Appointments");


			migrationBuilder.DropColumn(
				name: "DoctorId",
				table: "Appointments");


			migrationBuilder.DropColumn(
				name: "PatientId",
				table: "Appointments");


			migrationBuilder.AddColumn<string>(
			name: "PatientId",
			table: "Appointments",
			type: "nvarchar(450)",
			nullable: false);

			migrationBuilder.AddColumn<string>(
				name: "DoctorId",
				table: "Appointments",
				type: "nvarchar(450)",
				nullable: false);


			migrationBuilder.AddForeignKey(
				name: "FK_Appointments_Doctors_DoctorId",
				table: "Appointments",
				column: "DoctorId",
				principalTable: "Doctors",
				principalColumn: "Id",
				onDelete: ReferentialAction.NoAction);

			migrationBuilder.AddForeignKey(
				name: "FK_Appointments_Patients_PatientId",
				table: "Appointments",
				column: "PatientId",
				principalTable: "Patients",
				principalColumn: "Id",
				onDelete: ReferentialAction.NoAction);

		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.AddForeignKey(
			name: "FK_Appointments_Doctors_DoctorId",
			table: "Appointments",
			column: "DoctorId",
			principalTable: "Doctors",
			principalColumn: "Id",
			onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_Appointments_Patients_PatientId",
				table: "Appointments",
				column: "PatientId",
				principalTable: "Patients",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddColumn<int>(
			name: "PatientId",
			table: "Appointments",
			nullable: false,
			defaultValue: 0); // Adjust the default value as necessary


			migrationBuilder.AddColumn<int>(
				name: "DoctorId",
				table: "Appointments",
				nullable: false,
				defaultValue: 0); // Adjust the default value as necessary


			migrationBuilder.DropForeignKey(
			   name: "FK_Appointments_Doctors_DoctorId",
			   table: "Appointments");

			migrationBuilder.DropForeignKey(
				name: "FK_Appointments_Patients_PatientId",
				table: "Appointments");

			migrationBuilder.DropColumn(
			name: "DoctorId",
			table: "Appointments");

			// Remove the PatientId column
			migrationBuilder.DropColumn(
				name: "PatientId",
				table: "Appointments");


		}
    }
}
