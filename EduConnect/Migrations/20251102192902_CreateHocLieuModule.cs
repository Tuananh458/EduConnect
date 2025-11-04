using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class CreateHocLieuModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_HocLieu",
                table: "HocLieu");

            migrationBuilder.DropColumn(
                name: "TrangThai",
                table: "HocLieu");

            migrationBuilder.RenameTable(
                name: "HocLieu",
                newName: "HocLieus");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "HocLieus",
                newName: "TenKhoaHoc");

            migrationBuilder.AlterColumn<string>(
                name: "TenLoaiHocLieu",
                table: "HocLieus",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "MaLoaiHocLieu",
                table: "HocLieus",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddPrimaryKey(
                name: "PK_HocLieus",
                table: "HocLieus",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CauHoiHocLieus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HocLieuId = table.Column<int>(type: "int", nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoaiCauHoi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DoKho = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Diem = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DapAnA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DapAnB = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DapAnC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DapAnD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DapAnDung = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CauHoiHocLieus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CauHoiHocLieus_HocLieus_HocLieuId",
                        column: x => x.HocLieuId,
                        principalTable: "HocLieus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CauHoiHocLieus_HocLieuId",
                table: "CauHoiHocLieus",
                column: "HocLieuId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CauHoiHocLieus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HocLieus",
                table: "HocLieus");

            migrationBuilder.RenameTable(
                name: "HocLieus",
                newName: "HocLieu");

            migrationBuilder.RenameColumn(
                name: "TenKhoaHoc",
                table: "HocLieu",
                newName: "CreatedBy");

            migrationBuilder.AlterColumn<string>(
                name: "TenLoaiHocLieu",
                table: "HocLieu",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaLoaiHocLieu",
                table: "HocLieu",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<string>(
                name: "TrangThai",
                table: "HocLieu",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HocLieu",
                table: "HocLieu",
                column: "Id");
        }
    }
}
