using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital.Migrations
{
    /// <inheritdoc />
    public partial class DropFkColumnsInApplointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.DropIndex(
			name: "IX_Appointments_DoctorId",
			table: "Appointments");

			// Step 3: Drop the DoctorId column
			migrationBuilder.DropColumn(
				name: "DoctorId",
				table: "Appointments");


			migrationBuilder.DropIndex(
			name: "IX_Appointments_PatientId",
			table: "Appointments");

			migrationBuilder.DropColumn(
				name: "PatientId",
				table: "Appointments");
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.AddColumn<int>(
			name: "PatientId",
			table: "Appointments",
			nullable: false,
			defaultValue: 0); // Adjust the default value as necessary

			// Step 2: Recreate the index for PatientId
			migrationBuilder.CreateIndex(
				name: "IX_Appointments_PatientId",
				table: "Appointments",
				column: "PatientId");

			// Step 3: Re-add the DoctorId column with the original type (int)
			migrationBuilder.AddColumn<int>(
				name: "DoctorId",
				table: "Appointments",
				nullable: false,
				defaultValue: 0); // Adjust the default value as necessary

			// Step 4: Recreate the index for DoctorId
			migrationBuilder.CreateIndex(
				name: "IX_Appointments_DoctorId",
				table: "Appointments",
				column: "DoctorId");
		}
    }
}

