using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObject.Migrations
{
    public partial class InitialDatabaseV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FieldId",
                table: "Field",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Slots",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    field_id = table.Column<int>(type: "int", nullable: false),
                    fieldId = table.Column<int>(type: "int", nullable: true),
                    start_time = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    end_time = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slots", x => x.id);
                    table.ForeignKey(
                        name: "FK_Slots_Field_fieldId",
                        column: x => x.fieldId,
                        principalTable: "Field",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "varchar(255)", nullable: false),
                    password = table.Column<string>(type: "varchar(255)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    is_admin = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    total_price = table.Column<decimal>(type: "money", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.id);
                    table.ForeignKey(
                        name: "FK_Bookings_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingDetails",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    booking_id = table.Column<int>(type: "int", nullable: false),
                    start_time = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    end_time = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    field_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    slot_number = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<decimal>(type: "money", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingDetails", x => x.id);
                    table.ForeignKey(
                        name: "FK_BookingDetails_Bookings_booking_id",
                        column: x => x.booking_id,
                        principalTable: "Bookings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingDetails_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_BookingDetails_Field_field_id",
                        column: x => x.field_id,
                        principalTable: "Field",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingDetails_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Field_category_id",
                table: "Field",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_Field_FieldId",
                table: "Field",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingDetails_booking_id",
                table: "BookingDetails",
                column: "booking_id");

            migrationBuilder.CreateIndex(
                name: "IX_BookingDetails_CategoryId",
                table: "BookingDetails",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingDetails_field_id",
                table: "BookingDetails",
                column: "field_id");

            migrationBuilder.CreateIndex(
                name: "IX_BookingDetails_user_id",
                table: "BookingDetails",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_user_id",
                table: "Bookings",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Slots_fieldId",
                table: "Slots",
                column: "fieldId");

            migrationBuilder.AddForeignKey(
                name: "FK_Field_Categories_category_id",
                table: "Field",
                column: "category_id",
                principalTable: "Categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Field_Field_FieldId",
                table: "Field",
                column: "FieldId",
                principalTable: "Field",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Field_Categories_category_id",
                table: "Field");

            migrationBuilder.DropForeignKey(
                name: "FK_Field_Field_FieldId",
                table: "Field");

            migrationBuilder.DropTable(
                name: "BookingDetails");

            migrationBuilder.DropTable(
                name: "Slots");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Field_category_id",
                table: "Field");

            migrationBuilder.DropIndex(
                name: "IX_Field_FieldId",
                table: "Field");

            migrationBuilder.DropColumn(
                name: "FieldId",
                table: "Field");
        }
    }
}
