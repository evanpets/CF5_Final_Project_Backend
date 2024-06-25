using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProjectAPIBackend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PERFORMERS",
                columns: table => new
                {
                    PERFORMER_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PERFORMERS", x => x.PERFORMER_ID);
                });

            migrationBuilder.CreateTable(
                name: "USERS",
                columns: table => new
                {
                    USER_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    USERNAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PASSWORD = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    EMAIL = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FIRST_NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LAST_NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PHONE_NUMBER = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: true),
                    USER_ROLE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USERS", x => x.USER_ID);
                });

            migrationBuilder.CreateTable(
                name: "VENUE_ADDRESSES",
                columns: table => new
                {
                    V_ADDRESS_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    STREET = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    STREET_NUMBER = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CITY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZIP_CODE = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VENUE_ADDRESSES", x => x.V_ADDRESS_ID);
                });

            migrationBuilder.CreateTable(
                name: "VENUES",
                columns: table => new
                {
                    VENUE_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    VenueAddressId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VENUES", x => x.VENUE_ID);
                    table.ForeignKey(
                        name: "FK_VENUE_ADDRESS",
                        column: x => x.VenueAddressId,
                        principalTable: "VENUE_ADDRESSES",
                        principalColumn: "V_ADDRESS_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EVENTS",
                columns: table => new
                {
                    EVENT_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TITLE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    VenueId = table.Column<int>(type: "int", nullable: true),
                    PRICE = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    EVENT_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CATEGORY = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    IMAGE_URL = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EVENTS", x => x.EVENT_ID);
                    table.ForeignKey(
                        name: "FK_EVENTS_USERS_UserId",
                        column: x => x.UserId,
                        principalTable: "USERS",
                        principalColumn: "USER_ID");
                    table.ForeignKey(
                        name: "FK_VENUE_EVENTS",
                        column: x => x.VenueId,
                        principalTable: "VENUES",
                        principalColumn: "VENUE_ID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "EVENT_SAVES",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    SaveId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("SAVE_ID", x => new { x.UserId, x.EventId });
                    table.ForeignKey(
                        name: "FK_EVENT_SAVES_EVENTS_EventId",
                        column: x => x.EventId,
                        principalTable: "EVENTS",
                        principalColumn: "EVENT_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EVENT_SAVES_USERS_UserId",
                        column: x => x.UserId,
                        principalTable: "USERS",
                        principalColumn: "USER_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EVENTS_PERFORMERS",
                columns: table => new
                {
                    EventsEventId = table.Column<int>(type: "int", nullable: false),
                    PerformersPerformerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EVENTS_PERFORMERS", x => new { x.EventsEventId, x.PerformersPerformerId });
                    table.ForeignKey(
                        name: "FK_EVENTS_PERFORMERS_EVENTS_EventsEventId",
                        column: x => x.EventsEventId,
                        principalTable: "EVENTS",
                        principalColumn: "EVENT_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EVENTS_PERFORMERS_PERFORMERS_PerformersPerformerId",
                        column: x => x.PerformersPerformerId,
                        principalTable: "PERFORMERS",
                        principalColumn: "PERFORMER_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EVENT_SAVES_EventId",
                table: "EVENT_SAVES",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EVENTS_UserId",
                table: "EVENTS",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EVENTS_VenueId",
                table: "EVENTS",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_EVENTS_PERFORMERS_PerformersPerformerId",
                table: "EVENTS_PERFORMERS",
                column: "PerformersPerformerId");

            migrationBuilder.CreateIndex(
                name: "IX_PERFORMER",
                table: "PERFORMERS",
                column: "NAME");

            migrationBuilder.CreateIndex(
                name: "UQ_EMAIL",
                table: "USERS",
                column: "EMAIL",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_USERNAME",
                table: "USERS",
                column: "USERNAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VENUES_VenueAddressId",
                table: "VENUES",
                column: "VenueAddressId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EVENT_SAVES");

            migrationBuilder.DropTable(
                name: "EVENTS_PERFORMERS");

            migrationBuilder.DropTable(
                name: "EVENTS");

            migrationBuilder.DropTable(
                name: "PERFORMERS");

            migrationBuilder.DropTable(
                name: "USERS");

            migrationBuilder.DropTable(
                name: "VENUES");

            migrationBuilder.DropTable(
                name: "VENUE_ADDRESSES");
        }
    }
}
