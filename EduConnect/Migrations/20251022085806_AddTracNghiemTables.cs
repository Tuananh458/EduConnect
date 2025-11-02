using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class AddTracNghiemTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TRACNGHIEM",
                columns: table => new
                {
                    maTracNghiem = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenDeThi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    chuDe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    doKho = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    soLuongCauHoi = table.Column<int>(type: "int", nullable: false),
                    ngayTao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRACNGHIEM", x => x.maTracNghiem);
                });

            migrationBuilder.CreateTable(
                name: "TRACNGHIEM_CAUHOI",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    maTracNghiem = table.Column<int>(type: "int", nullable: false),
                    maCauHoi = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRACNGHIEM_CAUHOI", x => x.id);
                    table.ForeignKey(
                        name: "FK_TRACNGHIEM_CAUHOI_CAUHOI_maCauHoi",
                        column: x => x.maCauHoi,
                        principalTable: "CAUHOI",
                        principalColumn: "maCauHoi",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TRACNGHIEM_CAUHOI_TRACNGHIEM_maTracNghiem",
                        column: x => x.maTracNghiem,
                        principalTable: "TRACNGHIEM",
                        principalColumn: "maTracNghiem",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TRACNGHIEM_CAUHOI_maCauHoi",
                table: "TRACNGHIEM_CAUHOI",
                column: "maCauHoi");

            migrationBuilder.CreateIndex(
                name: "IX_TRACNGHIEM_CAUHOI_maTracNghiem",
                table: "TRACNGHIEM_CAUHOI",
                column: "maTracNghiem");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TRACNGHIEM_CAUHOI");

            migrationBuilder.DropTable(
                name: "TRACNGHIEM");
        }
    }
}
