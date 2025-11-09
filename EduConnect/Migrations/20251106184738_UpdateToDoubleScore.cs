using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class UpdateToDoubleScore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Diem",
                table: "CauHoiHocLieus",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldPrecision: 5,
                oldScale: 2);

            migrationBuilder.CreateTable(
                name: "BaiLamHocLieus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HocLieuId = table.Column<int>(type: "int", nullable: false),
                    HocSinhId = table.Column<int>(type: "int", nullable: true),
                    TenHocSinh = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ThoiGianBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThoiGianNop = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TongDiem = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaiLamHocLieus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietBaiLamHocLieus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BaiLamHocLieuId = table.Column<int>(type: "int", nullable: false),
                    CauHoiId = table.Column<int>(type: "int", nullable: false),
                    DapAnChon = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    DapAnDung = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Diem = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietBaiLamHocLieus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChiTietBaiLamHocLieus_BaiLamHocLieus_BaiLamHocLieuId",
                        column: x => x.BaiLamHocLieuId,
                        principalTable: "BaiLamHocLieus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietBaiLamHocLieus_BaiLamHocLieuId",
                table: "ChiTietBaiLamHocLieus",
                column: "BaiLamHocLieuId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietBaiLamHocLieus");

            migrationBuilder.DropTable(
                name: "BaiLamHocLieus");

            migrationBuilder.AlterColumn<decimal>(
                name: "Diem",
                table: "CauHoiHocLieus",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
