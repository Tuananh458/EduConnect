using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class AddQuizAndHocLieuModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CHITIET_TRALOI");

            migrationBuilder.DropTable(
                name: "GOIY_HOCTAP");

            migrationBuilder.DropTable(
                name: "TRACNGHIEM_CAUHOI");

            migrationBuilder.DropTable(
                name: "BAILAM_TRACNGHIEM");

            migrationBuilder.DropTable(
                name: "TRACNGHIEM");

            migrationBuilder.DropColumn(
                name: "chuDe",
                table: "CAUHOI");

            migrationBuilder.DropColumn(
                name: "diemDoiDa",
                table: "CAUHOI");

            migrationBuilder.DropColumn(
                name: "doKho",
                table: "CAUHOI");

            migrationBuilder.DropColumn(
                name: "maMonHoc",
                table: "CAUHOI");

            migrationBuilder.RenameColumn(
                name: "laDapAnDung",
                table: "DAPAN",
                newName: "isDung");

            migrationBuilder.RenameColumn(
                name: "thuTu",
                table: "CAUHOI",
                newName: "soDapAnDung");

            migrationBuilder.AlterColumn<string>(
                name: "noiDung",
                table: "DAPAN",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "thuTu",
                table: "DAPAN",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "noiDung",
                table: "CAUHOI",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "maKhoiHoc",
                table: "CAUHOI",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "createdBy",
                table: "CAUHOI",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "createdDate",
                table: "CAUHOI",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "huongDanGiai",
                table: "CAUHOI",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "loaiCauHoi",
                table: "CAUHOI",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "maChuDe",
                table: "CAUHOI",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "maChuong",
                table: "CAUHOI",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "maLopHoc",
                table: "CAUHOI",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "monHoc",
                table: "CAUHOI",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "mucDo",
                table: "CAUHOI",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "trangThai",
                table: "CAUHOI",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "updatedDate",
                table: "CAUHOI",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CHUONG",
                columns: table => new
                {
                    maChuong = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenChuong = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    moTa = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    maKhoiHoc = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHUONG", x => x.maChuong);
                    table.ForeignKey(
                        name: "FK_CHUONG_KHOIHOC_maKhoiHoc",
                        column: x => x.maKhoiHoc,
                        principalTable: "KHOIHOC",
                        principalColumn: "maKhoiHoc");
                });

            migrationBuilder.CreateTable(
                name: "CHUDE",
                columns: table => new
                {
                    maChuDe = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenChuDe = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    moTa = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    maChuong = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHUDE", x => x.maChuDe);
                    table.ForeignKey(
                        name: "FK_CHUDE_CHUONG_maChuong",
                        column: x => x.maChuong,
                        principalTable: "CHUONG",
                        principalColumn: "maChuong",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CAUHOI_maChuDe",
                table: "CAUHOI",
                column: "maChuDe");

            migrationBuilder.CreateIndex(
                name: "IX_CAUHOI_maChuong",
                table: "CAUHOI",
                column: "maChuong");

            migrationBuilder.CreateIndex(
                name: "IX_CAUHOI_maKhoiHoc",
                table: "CAUHOI",
                column: "maKhoiHoc");

            migrationBuilder.CreateIndex(
                name: "IX_CAUHOI_maLopHoc",
                table: "CAUHOI",
                column: "maLopHoc");

            migrationBuilder.CreateIndex(
                name: "IX_CHUDE_maChuong",
                table: "CHUDE",
                column: "maChuong");

            migrationBuilder.CreateIndex(
                name: "IX_CHUONG_maKhoiHoc",
                table: "CHUONG",
                column: "maKhoiHoc");

            migrationBuilder.AddForeignKey(
                name: "FK_CAUHOI_CHUDE_maChuDe",
                table: "CAUHOI",
                column: "maChuDe",
                principalTable: "CHUDE",
                principalColumn: "maChuDe",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_CAUHOI_CHUONG_maChuong",
                table: "CAUHOI",
                column: "maChuong",
                principalTable: "CHUONG",
                principalColumn: "maChuong");

            migrationBuilder.AddForeignKey(
                name: "FK_CAUHOI_KHOIHOC_maKhoiHoc",
                table: "CAUHOI",
                column: "maKhoiHoc",
                principalTable: "KHOIHOC",
                principalColumn: "maKhoiHoc");

            migrationBuilder.AddForeignKey(
                name: "FK_CAUHOI_LOPHOC_maLopHoc",
                table: "CAUHOI",
                column: "maLopHoc",
                principalTable: "LOPHOC",
                principalColumn: "maLopHoc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CAUHOI_CHUDE_maChuDe",
                table: "CAUHOI");

            migrationBuilder.DropForeignKey(
                name: "FK_CAUHOI_CHUONG_maChuong",
                table: "CAUHOI");

            migrationBuilder.DropForeignKey(
                name: "FK_CAUHOI_KHOIHOC_maKhoiHoc",
                table: "CAUHOI");

            migrationBuilder.DropForeignKey(
                name: "FK_CAUHOI_LOPHOC_maLopHoc",
                table: "CAUHOI");

            migrationBuilder.DropTable(
                name: "CHUDE");

            migrationBuilder.DropTable(
                name: "CHUONG");

            migrationBuilder.DropIndex(
                name: "IX_CAUHOI_maChuDe",
                table: "CAUHOI");

            migrationBuilder.DropIndex(
                name: "IX_CAUHOI_maChuong",
                table: "CAUHOI");

            migrationBuilder.DropIndex(
                name: "IX_CAUHOI_maKhoiHoc",
                table: "CAUHOI");

            migrationBuilder.DropIndex(
                name: "IX_CAUHOI_maLopHoc",
                table: "CAUHOI");

            migrationBuilder.DropColumn(
                name: "thuTu",
                table: "DAPAN");

            migrationBuilder.DropColumn(
                name: "createdBy",
                table: "CAUHOI");

            migrationBuilder.DropColumn(
                name: "createdDate",
                table: "CAUHOI");

            migrationBuilder.DropColumn(
                name: "huongDanGiai",
                table: "CAUHOI");

            migrationBuilder.DropColumn(
                name: "loaiCauHoi",
                table: "CAUHOI");

            migrationBuilder.DropColumn(
                name: "maChuDe",
                table: "CAUHOI");

            migrationBuilder.DropColumn(
                name: "maChuong",
                table: "CAUHOI");

            migrationBuilder.DropColumn(
                name: "maLopHoc",
                table: "CAUHOI");

            migrationBuilder.DropColumn(
                name: "monHoc",
                table: "CAUHOI");

            migrationBuilder.DropColumn(
                name: "mucDo",
                table: "CAUHOI");

            migrationBuilder.DropColumn(
                name: "trangThai",
                table: "CAUHOI");

            migrationBuilder.DropColumn(
                name: "updatedDate",
                table: "CAUHOI");

            migrationBuilder.RenameColumn(
                name: "isDung",
                table: "DAPAN",
                newName: "laDapAnDung");

            migrationBuilder.RenameColumn(
                name: "soDapAnDung",
                table: "CAUHOI",
                newName: "thuTu");

            migrationBuilder.AlterColumn<string>(
                name: "noiDung",
                table: "DAPAN",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "noiDung",
                table: "CAUHOI",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000);

            migrationBuilder.AlterColumn<int>(
                name: "maKhoiHoc",
                table: "CAUHOI",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "chuDe",
                table: "CAUHOI",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "diemDoiDa",
                table: "CAUHOI",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "doKho",
                table: "CAUHOI",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "maMonHoc",
                table: "CAUHOI",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BAILAM_TRACNGHIEM",
                columns: table => new
                {
                    maBaiLam = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    diemSo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    maHocSinh = table.Column<int>(type: "int", nullable: false),
                    maTracNghiem = table.Column<int>(type: "int", nullable: false),
                    soCauDung = table.Column<int>(type: "int", nullable: false),
                    thoiGianBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    thoiGianKetThuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    trangThai = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BAILAM_TRACNGHIEM", x => x.maBaiLam);
                });

            migrationBuilder.CreateTable(
                name: "GOIY_HOCTAP",
                columns: table => new
                {
                    maGoiY = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    duongDanTaiLieu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    maGoiYHocTap = table.Column<int>(type: "int", nullable: false),
                    maMonHoc = table.Column<int>(type: "int", nullable: false),
                    ngayCapNhat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    noiDungGoiY = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GOIY_HOCTAP", x => x.maGoiY);
                });

            migrationBuilder.CreateTable(
                name: "TRACNGHIEM",
                columns: table => new
                {
                    maTracNghiem = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    chuDe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    doKho = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ngayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    soLuongCauHoi = table.Column<int>(type: "int", nullable: false),
                    tenDeThi = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRACNGHIEM", x => x.maTracNghiem);
                });

            migrationBuilder.CreateTable(
                name: "CHITIET_TRALOI",
                columns: table => new
                {
                    maChiTiet = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    maBaiLam = table.Column<int>(type: "int", nullable: false),
                    maCauHoi = table.Column<int>(type: "int", nullable: false),
                    diemDatDuoc = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    isDung = table.Column<bool>(type: "bit", nullable: true),
                    maDapAnChon = table.Column<int>(type: "int", nullable: true)
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
                name: "TRACNGHIEM_CAUHOI",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    maCauHoi = table.Column<int>(type: "int", nullable: false),
                    maTracNghiem = table.Column<int>(type: "int", nullable: false)
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
                name: "IX_CHITIET_TRALOI_maBaiLam",
                table: "CHITIET_TRALOI",
                column: "maBaiLam");

            migrationBuilder.CreateIndex(
                name: "IX_CHITIET_TRALOI_maCauHoi",
                table: "CHITIET_TRALOI",
                column: "maCauHoi");

            migrationBuilder.CreateIndex(
                name: "IX_TRACNGHIEM_CAUHOI_maCauHoi",
                table: "TRACNGHIEM_CAUHOI",
                column: "maCauHoi");

            migrationBuilder.CreateIndex(
                name: "IX_TRACNGHIEM_CAUHOI_maTracNghiem",
                table: "TRACNGHIEM_CAUHOI",
                column: "maTracNghiem");
        }
    }
}
