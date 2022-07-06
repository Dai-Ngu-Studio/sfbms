using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObject.Migrations
{
    public partial class AddRelationshipFeedbackAndDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "image_url",
                table: "Field",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "booking_detail_id",
                table: "Feedbacks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_booking_detail_id",
                table: "Feedbacks",
                column: "booking_detail_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_BookingDetails_booking_detail_id",
                table: "Feedbacks",
                column: "booking_detail_id",
                principalTable: "BookingDetails",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_BookingDetails_booking_detail_id",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_booking_detail_id",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "booking_detail_id",
                table: "Feedbacks");

            migrationBuilder.AlterColumn<string>(
                name: "image_url",
                table: "Field",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
