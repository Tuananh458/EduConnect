using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class renameuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HOCLIEU",
                columns: table => new
                {
                    hocLieuId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenHocLieu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    maLoaiHocLieu = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    tenLoaiHocLieu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ngayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    nguonTao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    laHocLieuTuDo = table.Column<bool>(type: "bit", nullable: false),
                    laHocLieuAn = table.Column<bool>(type: "bit", nullable: false),
                    khoaHocId = table.Column<int>(type: "int", nullable: true),
                    tenKhoaHoc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HOCLIEU", x => x.hocLieuId);
                });

            migrationBuilder.CreateTable(
                name: "KHOIHOC",
                columns: table => new
                {
                    maKhoiHoc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenKhoiHoc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KHOIHOC", x => x.maKhoiHoc);
                });

            migrationBuilder.CreateTable(
                name: "CauHoiHocLieus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HocLieuId = table.Column<int>(type: "int", nullable: false),
                    TieuDe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoaiCauHoi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DoKho = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Diem = table.Column<double>(type: "float", nullable: false),
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
                        name: "FK_CauHoiHocLieus_HOCLIEU_HocLieuId",
                        column: x => x.HocLieuId,
                        principalTable: "HOCLIEU",
                        principalColumn: "hocLieuId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LOPHOC",
                columns: table => new
                {
                    maLopHoc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenLopHoc = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    maKhoiHoc = table.Column<int>(type: "int", nullable: false),
                    siSo = table.Column<int>(type: "int", nullable: true),
                    trangThai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ngayTao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    KhoiHocMaKhoiHoc = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOPHOC", x => x.maLopHoc);
                    table.ForeignKey(
                        name: "FK_LOPHOC_KHOIHOC_KhoiHocMaKhoiHoc",
                        column: x => x.KhoiHocMaKhoiHoc,
                        principalTable: "KHOIHOC",
                        principalColumn: "maKhoiHoc");
                    table.ForeignKey(
                        name: "FK_LOPHOC_KHOIHOC_maKhoiHoc",
                        column: x => x.maKhoiHoc,
                        principalTable: "KHOIHOC",
                        principalColumn: "maKhoiHoc",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HocLieuCauHois",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HocLieuId = table.Column<int>(type: "int", nullable: false),
                    CauHoiId = table.Column<int>(type: "int", nullable: false),
                    ThuTu = table.Column<int>(type: "int", nullable: false),
                    NgayThem = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HocLieuCauHois", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HocLieuCauHois_CauHoiHocLieus_CauHoiId",
                        column: x => x.CauHoiId,
                        principalTable: "CauHoiHocLieus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HocLieuCauHois_HOCLIEU_HocLieuId",
                        column: x => x.HocLieuId,
                        principalTable: "HOCLIEU",
                        principalColumn: "hocLieuId");
                });

            migrationBuilder.CreateTable(
                name: "BAILAMCHITIET",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    maBaiLam = table.Column<int>(type: "int", nullable: false),
                    maCauHoi = table.Column<int>(type: "int", nullable: false),
                    traLoi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dungSai = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BAILAMCHITIET", x => x.id);
                    table.ForeignKey(
                        name: "FK_BAILAMCHITIET_CauHoiHocLieus_maCauHoi",
                        column: x => x.maCauHoi,
                        principalTable: "CauHoiHocLieus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BAILAMHOCLIEU",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    hocLieuId = table.Column<int>(type: "int", nullable: false),
                    hocSinhId = table.Column<int>(type: "int", nullable: false),
                    ngayBatDau = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ngayNop = table.Column<DateTime>(type: "datetime2", nullable: true),
                    diem = table.Column<double>(type: "float", nullable: true),
                    trangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BAILAMHOCLIEU", x => x.id);
                    table.ForeignKey(
                        name: "FK_BAILAMHOCLIEU_HOCLIEU_hocLieuId",
                        column: x => x.hocLieuId,
                        principalTable: "HOCLIEU",
                        principalColumn: "hocLieuId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DATLAIMATKHAU",
                columns: table => new
                {
                    maDatLaiMatKhau = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    maNguoiDung = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    token = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ngayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    hetHanLuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    daSuDung = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DATLAIMATKHAU", x => x.maDatLaiMatKhau);
                });

            migrationBuilder.CreateTable(
                name: "GIAOVIEN",
                columns: table => new
                {
                    maGiaoVien = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    maNguoiDung = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    chuyenMon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    trinhDo = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    ngayBatDau = table.Column<DateTime>(type: "datetime2", nullable: true),
                    diaChi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    trangThaiCongTac = table.Column<bool>(type: "bit", nullable: false),
                    ngayTao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GIAOVIEN", x => x.maGiaoVien);
                });

            migrationBuilder.CreateTable(
                name: "HOCSINH",
                columns: table => new
                {
                    maHocSinh = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    maNguoiDung = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    maLopHoc = table.Column<int>(type: "int", nullable: true),
                    maDinhDanh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ngaySinh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    laLopTruong = table.Column<bool>(type: "bit", nullable: false),
                    ngayTao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HOCSINH", x => x.maHocSinh);
                    table.ForeignKey(
                        name: "FK_HOCSINH_LOPHOC_maLopHoc",
                        column: x => x.maLopHoc,
                        principalTable: "LOPHOC",
                        principalColumn: "maLopHoc",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LIENKET_PHUHUYNH_HOCSINH",
                columns: table => new
                {
                    maLienKet = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    maPhuHuynh = table.Column<int>(type: "int", nullable: false),
                    maHocSinh = table.Column<int>(type: "int", nullable: false),
                    moiQuanHe = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    trangThai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LIENKET_PHUHUYNH_HOCSINH", x => x.maLienKet);
                    table.ForeignKey(
                        name: "FK_LIENKET_PHUHUYNH_HOCSINH_HOCSINH_maHocSinh",
                        column: x => x.maHocSinh,
                        principalTable: "HOCSINH",
                        principalColumn: "maHocSinh",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NGUOIDUNG",
                columns: table => new
                {
                    maNguoiDung = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    tenDangNhap = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    hoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    soDienThoai = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    anhDaiDien = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true, defaultValue: "/template/img/avt.svg"),
                    matKhauHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vaiTro = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    nguonXacThuc = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    phaiDoiMatKhau = table.Column<bool>(type: "bit", nullable: false),
                    trangThai = table.Column<byte>(type: "tinyint", nullable: false),
                    ngayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    lanDangNhapCuoi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HocSinhMaHocSinh = table.Column<int>(type: "int", nullable: true),
                    GiaoVienMaGiaoVien = table.Column<int>(type: "int", nullable: true),
                    PhuHuynhMaPhuHuynh = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NGUOIDUNG", x => x.maNguoiDung);
                    table.ForeignKey(
                        name: "FK_NGUOIDUNG_GIAOVIEN_GiaoVienMaGiaoVien",
                        column: x => x.GiaoVienMaGiaoVien,
                        principalTable: "GIAOVIEN",
                        principalColumn: "maGiaoVien");
                    table.ForeignKey(
                        name: "FK_NGUOIDUNG_HOCSINH_HocSinhMaHocSinh",
                        column: x => x.HocSinhMaHocSinh,
                        principalTable: "HOCSINH",
                        principalColumn: "maHocSinh");
                });

            migrationBuilder.CreateTable(
                name: "PHUHUYNH",
                columns: table => new
                {
                    maPhuHuynh = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    maNguoiDung = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ngheNghiep = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    diaChi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    mucDoLienKet = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ngayTao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PHUHUYNH", x => x.maPhuHuynh);
                    table.ForeignKey(
                        name: "FK_PHUHUYNH_NGUOIDUNG_maNguoiDung",
                        column: x => x.maNguoiDung,
                        principalTable: "NGUOIDUNG",
                        principalColumn: "maNguoiDung");
                });

            migrationBuilder.CreateTable(
                name: "TOKENLAMMOI",
                columns: table => new
                {
                    maTokenLamMoi = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    maNguoiDung = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    token = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ngayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    hetHanLuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    biHuyLuc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TOKENLAMMOI", x => x.maTokenLamMoi);
                    table.ForeignKey(
                        name: "FK_TOKENLAMMOI_NGUOIDUNG_maNguoiDung",
                        column: x => x.maNguoiDung,
                        principalTable: "NGUOIDUNG",
                        principalColumn: "maNguoiDung",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "XACTHUCEMAIL",
                columns: table => new
                {
                    maXacThuc = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    maNguoiDung = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    maCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ngayHetHan = table.Column<DateTime>(type: "datetime2", nullable: false),
                    daSuDung = table.Column<bool>(type: "bit", nullable: false),
                    ngayTao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XACTHUCEMAIL", x => x.maXacThuc);
                    table.ForeignKey(
                        name: "FK_XACTHUCEMAIL_NGUOIDUNG_maNguoiDung",
                        column: x => x.maNguoiDung,
                        principalTable: "NGUOIDUNG",
                        principalColumn: "maNguoiDung",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "KHOIHOC",
                columns: new[] { "maKhoiHoc", "tenKhoiHoc" },
                values: new object[,]
                {
                    { 10, "Khối 10" },
                    { 11, "Khối 11" },
                    { 12, "Khối 12" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BAILAMCHITIET_maBaiLam",
                table: "BAILAMCHITIET",
                column: "maBaiLam");

            migrationBuilder.CreateIndex(
                name: "IX_BAILAMCHITIET_maCauHoi",
                table: "BAILAMCHITIET",
                column: "maCauHoi");

            migrationBuilder.CreateIndex(
                name: "IX_BAILAMHOCLIEU_hocLieuId",
                table: "BAILAMHOCLIEU",
                column: "hocLieuId");

            migrationBuilder.CreateIndex(
                name: "IX_BAILAMHOCLIEU_hocSinhId",
                table: "BAILAMHOCLIEU",
                column: "hocSinhId");

            migrationBuilder.CreateIndex(
                name: "IX_CauHoiHocLieus_HocLieuId",
                table: "CauHoiHocLieus",
                column: "HocLieuId");

            migrationBuilder.CreateIndex(
                name: "IX_DATLAIMATKHAU_maNguoiDung",
                table: "DATLAIMATKHAU",
                column: "maNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_GIAOVIEN_maNguoiDung",
                table: "GIAOVIEN",
                column: "maNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_HocLieuCauHois_CauHoiId",
                table: "HocLieuCauHois",
                column: "CauHoiId");

            migrationBuilder.CreateIndex(
                name: "IX_HocLieuCauHois_HocLieuId",
                table: "HocLieuCauHois",
                column: "HocLieuId");

            migrationBuilder.CreateIndex(
                name: "IX_HOCSINH_maLopHoc",
                table: "HOCSINH",
                column: "maLopHoc");

            migrationBuilder.CreateIndex(
                name: "IX_HOCSINH_maNguoiDung",
                table: "HOCSINH",
                column: "maNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_LIENKET_PHUHUYNH_HOCSINH_maHocSinh",
                table: "LIENKET_PHUHUYNH_HOCSINH",
                column: "maHocSinh");

            migrationBuilder.CreateIndex(
                name: "IX_LIENKET_PHUHUYNH_HOCSINH_maPhuHuynh",
                table: "LIENKET_PHUHUYNH_HOCSINH",
                column: "maPhuHuynh");

            migrationBuilder.CreateIndex(
                name: "IX_LOPHOC_KhoiHocMaKhoiHoc",
                table: "LOPHOC",
                column: "KhoiHocMaKhoiHoc");

            migrationBuilder.CreateIndex(
                name: "IX_LOPHOC_maKhoiHoc_tenLopHoc",
                table: "LOPHOC",
                columns: new[] { "maKhoiHoc", "tenLopHoc" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NGUOIDUNG_email",
                table: "NGUOIDUNG",
                column: "email",
                unique: true,
                filter: "[email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_NGUOIDUNG_GiaoVienMaGiaoVien",
                table: "NGUOIDUNG",
                column: "GiaoVienMaGiaoVien");

            migrationBuilder.CreateIndex(
                name: "IX_NGUOIDUNG_HocSinhMaHocSinh",
                table: "NGUOIDUNG",
                column: "HocSinhMaHocSinh");

            migrationBuilder.CreateIndex(
                name: "IX_NGUOIDUNG_PhuHuynhMaPhuHuynh",
                table: "NGUOIDUNG",
                column: "PhuHuynhMaPhuHuynh");

            migrationBuilder.CreateIndex(
                name: "IX_NGUOIDUNG_tenDangNhap",
                table: "NGUOIDUNG",
                column: "tenDangNhap",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PHUHUYNH_maNguoiDung",
                table: "PHUHUYNH",
                column: "maNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_TOKENLAMMOI_maNguoiDung",
                table: "TOKENLAMMOI",
                column: "maNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_XACTHUCEMAIL_maNguoiDung",
                table: "XACTHUCEMAIL",
                column: "maNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK_BAILAMCHITIET_BAILAMHOCLIEU_maBaiLam",
                table: "BAILAMCHITIET",
                column: "maBaiLam",
                principalTable: "BAILAMHOCLIEU",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BAILAMHOCLIEU_HOCSINH_hocSinhId",
                table: "BAILAMHOCLIEU",
                column: "hocSinhId",
                principalTable: "HOCSINH",
                principalColumn: "maHocSinh",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DATLAIMATKHAU_NGUOIDUNG_maNguoiDung",
                table: "DATLAIMATKHAU",
                column: "maNguoiDung",
                principalTable: "NGUOIDUNG",
                principalColumn: "maNguoiDung",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GIAOVIEN_NGUOIDUNG_maNguoiDung",
                table: "GIAOVIEN",
                column: "maNguoiDung",
                principalTable: "NGUOIDUNG",
                principalColumn: "maNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK_HOCSINH_NGUOIDUNG_maNguoiDung",
                table: "HOCSINH",
                column: "maNguoiDung",
                principalTable: "NGUOIDUNG",
                principalColumn: "maNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK_LIENKET_PHUHUYNH_HOCSINH_PHUHUYNH_maPhuHuynh",
                table: "LIENKET_PHUHUYNH_HOCSINH",
                column: "maPhuHuynh",
                principalTable: "PHUHUYNH",
                principalColumn: "maPhuHuynh",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NGUOIDUNG_PHUHUYNH_PhuHuynhMaPhuHuynh",
                table: "NGUOIDUNG",
                column: "PhuHuynhMaPhuHuynh",
                principalTable: "PHUHUYNH",
                principalColumn: "maPhuHuynh");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NGUOIDUNG_HOCSINH_HocSinhMaHocSinh",
                table: "NGUOIDUNG");

            migrationBuilder.DropForeignKey(
                name: "FK_GIAOVIEN_NGUOIDUNG_maNguoiDung",
                table: "GIAOVIEN");

            migrationBuilder.DropForeignKey(
                name: "FK_PHUHUYNH_NGUOIDUNG_maNguoiDung",
                table: "PHUHUYNH");

            migrationBuilder.DropTable(
                name: "BAILAMCHITIET");

            migrationBuilder.DropTable(
                name: "DATLAIMATKHAU");

            migrationBuilder.DropTable(
                name: "HocLieuCauHois");

            migrationBuilder.DropTable(
                name: "LIENKET_PHUHUYNH_HOCSINH");

            migrationBuilder.DropTable(
                name: "TOKENLAMMOI");

            migrationBuilder.DropTable(
                name: "XACTHUCEMAIL");

            migrationBuilder.DropTable(
                name: "BAILAMHOCLIEU");

            migrationBuilder.DropTable(
                name: "CauHoiHocLieus");

            migrationBuilder.DropTable(
                name: "HOCLIEU");

            migrationBuilder.DropTable(
                name: "HOCSINH");

            migrationBuilder.DropTable(
                name: "LOPHOC");

            migrationBuilder.DropTable(
                name: "KHOIHOC");

            migrationBuilder.DropTable(
                name: "NGUOIDUNG");

            migrationBuilder.DropTable(
                name: "GIAOVIEN");

            migrationBuilder.DropTable(
                name: "PHUHUYNH");
        }
    }
}
