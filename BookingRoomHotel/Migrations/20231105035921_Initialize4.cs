using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingRoomHotel.Migrations
{
    public partial class Initialize4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StaffNotification_Staffs_Id",
                table: "StaffNotification");

            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "StaffNotification");

            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "CustomerNotification");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "StaffNotification",
                newName: "StaffId");

            migrationBuilder.RenameIndex(
                name: "IX_StaffNotification_Id",
                table: "StaffNotification",
                newName: "IX_StaffNotification_StaffId");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "StaffNotification",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "CustomerNotification",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_StaffNotification_Staffs_StaffId",
                table: "StaffNotification",
                column: "StaffId",
                principalTable: "Staffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StaffNotification_Staffs_StaffId",
                table: "StaffNotification");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "StaffNotification");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "CustomerNotification");

            migrationBuilder.RenameColumn(
                name: "StaffId",
                table: "StaffNotification",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_StaffNotification_StaffId",
                table: "StaffNotification",
                newName: "IX_StaffNotification_Id");

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "StaffNotification",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "CustomerNotification",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_StaffNotification_Staffs_Id",
                table: "StaffNotification",
                column: "Id",
                principalTable: "Staffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
