using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductGuard.Migrations
{
    /// <inheritdoc />
    public partial class nr1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StorageDevices",
                table: "StorageDevices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RAMs",
                table: "RAMs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Motherboards",
                table: "Motherboards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GPUs",
                table: "GPUs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CPUs",
                table: "CPUs");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "StorageDevices");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Motherboards");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "GPUs");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CPUs");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "RAMs",
                newName: "id");

            migrationBuilder.AddColumn<Guid>(
                name: "Uuid",
                table: "StorageDevices",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "RAMs",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<Guid>(
                name: "Uuid",
                table: "RAMs",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "Uuid",
                table: "Motherboards",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "Uuid",
                table: "GPUs",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "Uuid",
                table: "CPUs",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StorageDevices",
                table: "StorageDevices",
                column: "Uuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RAMs",
                table: "RAMs",
                column: "Uuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Motherboards",
                table: "Motherboards",
                column: "Uuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GPUs",
                table: "GPUs",
                column: "Uuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CPUs",
                table: "CPUs",
                column: "Uuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StorageDevices",
                table: "StorageDevices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RAMs",
                table: "RAMs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Motherboards",
                table: "Motherboards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GPUs",
                table: "GPUs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CPUs",
                table: "CPUs");

            migrationBuilder.DropColumn(
                name: "Uuid",
                table: "StorageDevices");

            migrationBuilder.DropColumn(
                name: "Uuid",
                table: "RAMs");

            migrationBuilder.DropColumn(
                name: "Uuid",
                table: "Motherboards");

            migrationBuilder.DropColumn(
                name: "Uuid",
                table: "GPUs");

            migrationBuilder.DropColumn(
                name: "Uuid",
                table: "CPUs");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "RAMs",
                newName: "Id");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "StorageDevices",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "RAMs",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Motherboards",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "GPUs",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CPUs",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StorageDevices",
                table: "StorageDevices",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RAMs",
                table: "RAMs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Motherboards",
                table: "Motherboards",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GPUs",
                table: "GPUs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CPUs",
                table: "CPUs",
                column: "Id");
        }
    }
}
