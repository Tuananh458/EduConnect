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
            migrationBuilder.DropColumn(
                name: "MaKhoiHoc",
                table: "HocLieus");

            migrationBuilder.DropColumn(
                name: "MaLopHoc",
                table: "HocLieus");

            migrationBuilder.DropColumn(
                name: "MaMonHoc",
                table: "HocLieus");

            migrationBuilder.DropColumn(
                name: "ChuDe",
                table: "CauHoiHocLieus");

            migrationBuilder.DropColumn(
                name: "HinhAnhUrl",
                table: "CauHoiHocLieus");

            migrationBuilder.DropColumn(
                name: "NguoiTao",
                table: "CauHoiHocLieus");

            migrationBuilder.AlterColumn<string>(
                name: "DoKho",
                table: "CauHoiHocLieus",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaKhoiHoc",
                table: "HocLieus",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaLopHoc",
                table: "HocLieus",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaMonHoc",
                table: "HocLieus",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DoKho",
                table: "CauHoiHocLieus",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "ChuDe",
                table: "CauHoiHocLieus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HinhAnhUrl",
                table: "CauHoiHocLieus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NguoiTao",
                table: "CauHoiHocLieus",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
