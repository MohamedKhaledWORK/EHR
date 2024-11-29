using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class NOVISITINMEDICATION : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicationPrescriptions_Visits_VisitId",
                table: "MedicationPrescriptions");

            migrationBuilder.DropIndex(
                name: "IX_MedicationPrescriptions_VisitId",
                table: "MedicationPrescriptions");

            migrationBuilder.AddColumn<int>(
                name: "DoctorId",
                table: "MedicationPrescriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "MedicationPrescriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MedicationPrescriptions_DoctorId",
                table: "MedicationPrescriptions",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicationPrescriptions_PatientId",
                table: "MedicationPrescriptions",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicationPrescriptions_Doctors_DoctorId",
                table: "MedicationPrescriptions",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicationPrescriptions_Patients_PatientId",
                table: "MedicationPrescriptions",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicationPrescriptions_Doctors_DoctorId",
                table: "MedicationPrescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicationPrescriptions_Patients_PatientId",
                table: "MedicationPrescriptions");

            migrationBuilder.DropIndex(
                name: "IX_MedicationPrescriptions_DoctorId",
                table: "MedicationPrescriptions");

            migrationBuilder.DropIndex(
                name: "IX_MedicationPrescriptions_PatientId",
                table: "MedicationPrescriptions");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "MedicationPrescriptions");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "MedicationPrescriptions");

            migrationBuilder.CreateIndex(
                name: "IX_MedicationPrescriptions_VisitId",
                table: "MedicationPrescriptions",
                column: "VisitId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicationPrescriptions_Visits_VisitId",
                table: "MedicationPrescriptions",
                column: "VisitId",
                principalTable: "Visits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
