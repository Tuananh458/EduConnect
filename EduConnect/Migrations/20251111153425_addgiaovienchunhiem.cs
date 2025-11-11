using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduConnect.Migrations
{
    /// <inheritdoc />
    public partial class addgiaovienchunhiem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "maGiaoVienChuNhiem",
                table: "LOPHOC",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ngayTao",
                table: "HOCSINH",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LOPHOC_maGiaoVienChuNhiem",
                table: "LOPHOC",
                column: "maGiaoVienChuNhiem");

            migrationBuilder.AddForeignKey(
                name: "FK_LOPHOC_GIAOVIEN_maGiaoVienChuNhiem",
                table: "LOPHOC",
                column: "maGiaoVienChuNhiem",
                principalTable: "GIAOVIEN",
                principalColumn: "maGiaoVien",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LOPHOC_GIAOVIEN_maGiaoVienChuNhiem",
                table: "LOPHOC");

            migrationBuilder.DropIndex(
                name: "IX_LOPHOC_maGiaoVienChuNhiem",
                table: "LOPHOC");

            migrationBuilder.DropColumn(
                name: "maGiaoVienChuNhiem",
                table: "LOPHOC");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ngayTao",
                table: "HOCSINH",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
