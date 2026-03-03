using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CapStone.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUnderwritingAndCommissionFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PolicyCoverages",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444441"));

            migrationBuilder.DeleteData(
                table: "PolicyCoverages",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444442"));

            migrationBuilder.DeleteData(
                table: "PolicyCoverages",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444443"));

            migrationBuilder.DeleteData(
                table: "PolicyCoverages",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "PolicyTypes",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333331"));

            migrationBuilder.DeleteData(
                table: "PolicyTypes",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333332"));

            migrationBuilder.DeleteData(
                table: "PolicyTypes",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "PolicyTypes",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333334"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "PolicyRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "NomineeName",
                table: "PolicyRequests",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NomineeRelation",
                table: "PolicyRequests",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "PolicyRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "AgentCommissionAmount",
                table: "Policies",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "PolicyRequests");

            migrationBuilder.DropColumn(
                name: "NomineeName",
                table: "PolicyRequests");

            migrationBuilder.DropColumn(
                name: "NomineeRelation",
                table: "PolicyRequests");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "PolicyRequests");

            migrationBuilder.AlterColumn<decimal>(
                name: "AgentCommissionAmount",
                table: "Policies",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "CreatedAt", "DateOfBirth", "Email", "IsActive", "Name", "Occupation", "PasswordHash", "Phone", "Role" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), null, new DateTime(2026, 2, 28, 21, 16, 17, 653, DateTimeKind.Utc).AddTicks(1571), null, "abhishekallaboina@gmail.com", true, "Abhishek", null, "$2a$11$VrpPaBviDrIvMoyJfTdcIuWLzKoEMlpCEjnB77Yng828yf2kdHeg6", null, 0 },
                    { new Guid("22222222-2222-2222-2222-222222222222"), null, new DateTime(2026, 2, 28, 21, 16, 18, 59, DateTimeKind.Utc).AddTicks(464), null, "customer@example.com", true, "John Customer", null, "$2a$11$TEjDAedfVF7y9364DZqhKeaFGibJxPH0k3tPBMGicGe2aT/XL0iJm", null, 2 }
                });

            migrationBuilder.InsertData(
                table: "PolicyTypes",
                columns: new[] { "Id", "BaseCoverageAmount", "BasePremium", "CreatedBy", "DurationMonths", "Name", "Status" },
                values: new object[,]
                {
                    { new Guid("33333333-3333-3333-3333-333333333331"), 50000m, 500m, new Guid("11111111-1111-1111-1111-111111111111"), 12, "Basic Accident", "Active" },
                    { new Guid("33333333-3333-3333-3333-333333333332"), 100000m, 1000m, new Guid("11111111-1111-1111-1111-111111111111"), 12, "Standard Accident", "Active" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), 200000m, 2000m, new Guid("11111111-1111-1111-1111-111111111111"), 24, "Premium Accident", "Active" },
                    { new Guid("33333333-3333-3333-3333-333333333334"), 300000m, 3000m, new Guid("11111111-1111-1111-1111-111111111111"), 24, "Family Accident", "Active" }
                });

            migrationBuilder.InsertData(
                table: "PolicyCoverages",
                columns: new[] { "Id", "CoverageCategory", "Description", "MaxWeeks", "PercentageOfCoverage", "PolicyTypeId", "WeeklyCompensationPercentage" },
                values: new object[,]
                {
                    { new Guid("44444444-4444-4444-4444-444444444441"), 0, "Accidental death benefit", null, 100m, new Guid("33333333-3333-3333-3333-333333333331"), null },
                    { new Guid("44444444-4444-4444-4444-444444444442"), 0, "Accidental death benefit", null, 100m, new Guid("33333333-3333-3333-3333-333333333332"), null },
                    { new Guid("44444444-4444-4444-4444-444444444443"), 1, "Permanent total disability", null, 80m, new Guid("33333333-3333-3333-3333-333333333333"), null },
                    { new Guid("44444444-4444-4444-4444-444444444444"), 3, "Temporary disability", 52, 60m, new Guid("33333333-3333-3333-3333-333333333334"), 70m }
                });
        }
    }
}
