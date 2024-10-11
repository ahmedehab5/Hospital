using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Migrations
{
    /// <inheritdoc />
    public partial class DropFkInAppointment : Migration
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
	


		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
        {


			migrationBuilder.AddForeignKey(
			name: "FK_Appointments_Patients_PatientId",
			table: "Appointments",
			column: "PatientId",
			principalTable: "Patients",
			principalColumn: "Id",
			onDelete: ReferentialAction.Cascade);

			

			migrationBuilder.AddForeignKey(
				name: "FK_Appointments_Doctors_DoctorId",
				table: "Appointments",
				column: "DoctorId",
				principalTable: "Doctors",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

		
		}
    }
}

