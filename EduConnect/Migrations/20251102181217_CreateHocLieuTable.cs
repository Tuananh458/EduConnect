using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class CreateHocLieuTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GIAOVIEN",
                columns: table => new
                {
                    maGiaoVien = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    maNguoiDung = table.Column<int>(type: "int", nullable: false),
                    chuyenMon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    trinhDo = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    ngayBatDau = table.Column<DateTime>(type: "datetime2", nullable: true),
                    diaChi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    trangThaiCongTac = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GIAOVIEN", x => x.maGiaoVien);
                });

            migrationBuilder.CreateTable(
                name: "HocLieu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenHocLieu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MaLoaiHocLieu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TenLoaiHocLieu = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KhoaHocId = table.Column<int>(type: "int", nullable: true),
                    LaHocLieuTuDo = table.Column<bool>(type: "bit", nullable: false),
                    LaHocLieuAn = table.Column<bool>(type: "bit", nullable: false),
                    NguonTao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HocLieu", x => x.Id);
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
                name: "PHUHUYNH",
                columns: table => new
                {
                    maPhuHuynh = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    maNguoiDung = table.Column<int>(type: "int", nullable: false),
                    ngheNghiep = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    diaChi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    mucDoLienKet = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PHUHUYNH", x => x.maPhuHuynh);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuthProvider = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MustChangePassword = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "LOPHOC",
                columns: table => new
                {
                    maLopHoc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenLopHoc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    maKhoiHoc = table.Column<int>(type: "int", nullable: false),
                    siSo = table.Column<int>(type: "int", nullable: true),
                    trangThai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ngayTao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GiaoVienMaGiaoVien = table.Column<int>(type: "int", nullable: true),
                    KhoiHocMaKhoiHoc = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOPHOC", x => x.maLopHoc);
                    table.ForeignKey(
                        name: "FK_LOPHOC_GIAOVIEN_GiaoVienMaGiaoVien",
                        column: x => x.GiaoVienMaGiaoVien,
                        principalTable: "GIAOVIEN",
                        principalColumn: "maGiaoVien");
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
                name: "EmailVerifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Used = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailVerifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailVerifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PasswordResets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Used = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordResets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PasswordResets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HOCSINH",
                columns: table => new
                {
                    maHocSinh = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    maNguoiDung = table.Column<int>(type: "int", nullable: false),
                    maLopHoc = table.Column<int>(type: "int", nullable: false),
                    ngaySinh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    gioiTinh = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    diaChi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ngayNhapHoc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    trangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
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
                    table.ForeignKey(
                        name: "FK_LIENKET_PHUHUYNH_HOCSINH_PHUHUYNH_maPhuHuynh",
                        column: x => x.maPhuHuynh,
                        principalTable: "PHUHUYNH",
                        principalColumn: "maPhuHuynh",
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
                name: "IX_EmailVerifications_UserId",
                table: "EmailVerifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HOCSINH_maLopHoc",
                table: "HOCSINH",
                column: "maLopHoc");

            migrationBuilder.CreateIndex(
                name: "IX_LIENKET_PHUHUYNH_HOCSINH_maHocSinh",
                table: "LIENKET_PHUHUYNH_HOCSINH",
                column: "maHocSinh");

            migrationBuilder.CreateIndex(
                name: "IX_LIENKET_PHUHUYNH_HOCSINH_maPhuHuynh",
                table: "LIENKET_PHUHUYNH_HOCSINH",
                column: "maPhuHuynh");

            migrationBuilder.CreateIndex(
                name: "IX_LOPHOC_GiaoVienMaGiaoVien",
                table: "LOPHOC",
                column: "GiaoVienMaGiaoVien");

            migrationBuilder.CreateIndex(
                name: "IX_LOPHOC_KhoiHocMaKhoiHoc",
                table: "LOPHOC",
                column: "KhoiHocMaKhoiHoc");

            migrationBuilder.CreateIndex(
                name: "IX_LOPHOC_maKhoiHoc",
                table: "LOPHOC",
                column: "maKhoiHoc");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResets_UserId",
                table: "PasswordResets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailVerifications");

            migrationBuilder.DropTable(
                name: "HocLieu");

            migrationBuilder.DropTable(
                name: "LIENKET_PHUHUYNH_HOCSINH");

            migrationBuilder.DropTable(
                name: "PasswordResets");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "HOCSINH");

            migrationBuilder.DropTable(
                name: "PHUHUYNH");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "LOPHOC");

            migrationBuilder.DropTable(
                name: "GIAOVIEN");

            migrationBuilder.DropTable(
                name: "KHOIHOC");
        }
    }
}
