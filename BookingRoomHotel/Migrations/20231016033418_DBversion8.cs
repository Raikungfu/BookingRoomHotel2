using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingRoomHotel.Migrations
{
    public partial class DBversion8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "RoomTypes",
                newName: "View");

            migrationBuilder.AddColumn<int>(
                name: "Bed",
                table: "RoomTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description1",
                table: "RoomTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description2",
                table: "RoomTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description3",
                table: "RoomTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Max",
                table: "RoomTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Size",
                table: "RoomTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Media",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "For",
                table: "Media",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RoomTypeID",
                table: "Media",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Media_RoomTypeID",
                table: "Media",
                column: "RoomTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Media_RoomTypes_RoomTypeID",
                table: "Media",
                column: "RoomTypeID",
                principalTable: "RoomTypes",
                principalColumn: "RoomTypeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Media_RoomTypes_RoomTypeID",
                table: "Media");

            migrationBuilder.DropIndex(
                name: "IX_Media_RoomTypeID",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "Bed",
                table: "RoomTypes");

            migrationBuilder.DropColumn(
                name: "Description1",
                table: "RoomTypes");

            migrationBuilder.DropColumn(
                name: "Description2",
                table: "RoomTypes");

            migrationBuilder.DropColumn(
                name: "Description3",
                table: "RoomTypes");

            migrationBuilder.DropColumn(
                name: "Max",
                table: "RoomTypes");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "RoomTypes");

            migrationBuilder.DropColumn(
                name: "For",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "RoomTypeID",
                table: "Media");

            migrationBuilder.RenameColumn(
                name: "View",
                table: "RoomTypes",
                newName: "Description");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Media",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
