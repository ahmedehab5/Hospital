using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Migrations
{
    /// <inheritdoc />
    public partial class AddPkToAppointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

			migrationBuilder.AddColumn<string>(
	        name: "PatientId",
	        table: "Appointments",
	        type: "nvarchar(450)",
	        nullable: true); 

			migrationBuilder.AddColumn<string>(
				name: "DoctorId",
				table: "Appointments",
				type: "nvarchar(450)",
				nullable: true); 


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
