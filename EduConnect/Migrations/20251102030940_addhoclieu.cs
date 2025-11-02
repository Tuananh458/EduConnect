using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class addhoclieu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CauHoiHocLieus",
                columns: table => new
                {
                    MaCauHoi = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Loai = table.Column<int>(type: "int", nullable: false),
                    DoKho = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GiaiThich = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChuDe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HinhAnhUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NguoiTao = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CauHoiHocLieus", x => x.MaCauHoi);
                });

            migrationBuilder.CreateTable(
                name: "HocLieus",
                columns: table => new
                {
                    MaHocLieu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenHocLieu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TheLoai = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MaKhoiHoc = table.Column<int>(type: "int", nullable: true),
                    MaLopHoc = table.Column<int>(type: "int", nullable: true),
                    MaMonHoc = table.Column<int>(type: "int", nullable: true),
                    HienThi = table.Column<bool>(type: "bit", nullable: false),
                    DaDuyet = table.Column<bool>(type: "bit", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NguoiTao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HocLieus", x => x.MaHocLieu);
                });

            migrationBuilder.CreateTable(
                name: "DapAnHocLieus",
                columns: table => new
                {
                    MaDapAn = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaCauHoi = table.Column<int>(type: "int", nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LaDapAnDung = table.Column<bool>(type: "bit", nullable: false),
                    ThuTu = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DapAnHocLieus", x => x.MaDapAn);
                    table.ForeignKey(
                        name: "FK_DapAnHocLieus_CauHoiHocLieus_MaCauHoi",
                        column: x => x.MaCauHoi,
                        principalTable: "CauHoiHocLieus",
                        principalColumn: "MaCauHoi",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HocLieuCauHois",
                columns: table => new
                {
                    MaHocLieu = table.Column<int>(type: "int", nullable: false),
                    MaCauHoi = table.Column<int>(type: "int", nullable: false),
                    Diem = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HocLieuCauHois", x => new { x.MaHocLieu, x.MaCauHoi });
                    table.ForeignKey(
                        name: "FK_HocLieuCauHois_CauHoiHocLieus_MaCauHoi",
                        column: x => x.MaCauHoi,
                        principalTable: "CauHoiHocLieus",
                        principalColumn: "MaCauHoi",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HocLieuCauHois_HocLieus_MaHocLieu",
                        column: x => x.MaHocLieu,
                        principalTable: "HocLieus",
                        principalColumn: "MaHocLieu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DapAnHocLieus_MaCauHoi",
                table: "DapAnHocLieus",
                column: "MaCauHoi");

            migrationBuilder.CreateIndex(
                name: "IX_HocLieuCauHois_MaCauHoi",
                table: "HocLieuCauHois",
                column: "MaCauHoi");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DapAnHocLieus");

            migrationBuilder.DropTable(
                name: "HocLieuCauHois");

            migrationBuilder.DropTable(
                name: "CauHoiHocLieus");

            migrationBuilder.DropTable(
                name: "HocLieus");
        }
    }
}
