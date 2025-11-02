using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BAILAM_TRACNGHIEM",
                columns: table => new
                {
                    maBaiLam = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    maHocSinh = table.Column<int>(type: "int", nullable: false),
                    maTracNghiem = table.Column<int>(type: "int", nullable: false),
                    thoiGianBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    thoiGianKetThuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    soCauDung = table.Column<int>(type: "int", nullable: false),
                    diemSo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    trangThai = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BAILAM_TRACNGHIEM", x => x.maBaiLam);
                });

            migrationBuilder.CreateTable(
                name: "CAUHOI",
                columns: table => new
                {
                    maCauHoi = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    noiDung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    maMonHoc = table.Column<int>(type: "int", nullable: false),
                    maKhoiHoc = table.Column<int>(type: "int", nullable: false),
                    thuTu = table.Column<int>(type: "int", nullable: false),
                    diemDoiDa = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CAUHOI", x => x.maCauHoi);
                });

            migrationBuilder.CreateTable(
                name: "GOIY_HOCTAP",
                columns: table => new
                {
                    maGoiY = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    maGoiYHocTap = table.Column<int>(type: "int", nullable: false),
                    maMonHoc = table.Column<int>(type: "int", nullable: false),
                    noiDungGoiY = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    duongDanTaiLieu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ngayCapNhat = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GOIY_HOCTAP", x => x.maGoiY);
                });

            migrationBuilder.CreateTable(
                name: "CHITIET_TRALOI",
                columns: table => new
                {
                    maChiTiet = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    maBaiLam = table.Column<int>(type: "int", nullable: false),
                    maCauHoi = table.Column<int>(type: "int", nullable: false),
                    maDapAnChon = table.Column<int>(type: "int", nullable: true),
                    isDung = table.Column<bool>(type: "bit", nullable: true),
                    diemDatDuoc = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHITIET_TRALOI", x => x.maChiTiet);
                    table.ForeignKey(
                        name: "FK_CHITIET_TRALOI_BAILAM_TRACNGHIEM_maBaiLam",
                        column: x => x.maBaiLam,
                        principalTable: "BAILAM_TRACNGHIEM",
                        principalColumn: "maBaiLam",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CHITIET_TRALOI_CAUHOI_maCauHoi",
                        column: x => x.maCauHoi,
                        principalTable: "CAUHOI",
                        principalColumn: "maCauHoi",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DAPAN",
                columns: table => new
                {
                    maDapAn = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    maCauHoi = table.Column<int>(type: "int", nullable: false),
                    noiDung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    laDapAnDung = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DAPAN", x => x.maDapAn);
                    table.ForeignKey(
                        name: "FK_DAPAN_CAUHOI_maCauHoi",
                        column: x => x.maCauHoi,
                        principalTable: "CAUHOI",
                        principalColumn: "maCauHoi",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CHITIET_TRALOI_maBaiLam",
                table: "CHITIET_TRALOI",
                column: "maBaiLam");

            migrationBuilder.CreateIndex(
                name: "IX_CHITIET_TRALOI_maCauHoi",
                table: "CHITIET_TRALOI",
                column: "maCauHoi");

            migrationBuilder.CreateIndex(
                name: "IX_DAPAN_maCauHoi",
                table: "DAPAN",
                column: "maCauHoi");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CHITIET_TRALOI");

            migrationBuilder.DropTable(
                name: "DAPAN");

            migrationBuilder.DropTable(
                name: "GOIY_HOCTAP");

            migrationBuilder.DropTable(
                name: "BAILAM_TRACNGHIEM");

            migrationBuilder.DropTable(
                name: "CAUHOI");
        }
    }
}
