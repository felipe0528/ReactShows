using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class Shows : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(nullable: true),
                    code = table.Column<string>(nullable: true),
                    timezone = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    medium = table.Column<string>(nullable: true),
                    original = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Schedule",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    time = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedule", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Network",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(nullable: true),
                    countryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Network", x => x.id);
                    table.ForeignKey(
                        name: "FK_Network_Country_countryId",
                        column: x => x.countryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DayObject",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    day = table.Column<int>(nullable: false),
                    ScheduleId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayObject", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DayObject_Schedule_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Shows",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    url = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    type = table.Column<string>(nullable: true),
                    language = table.Column<string>(nullable: true),
                    status = table.Column<string>(nullable: true),
                    runtime = table.Column<int>(nullable: false),
                    premiered = table.Column<string>(nullable: true),
                    officialSite = table.Column<string>(nullable: true),
                    scheduleId = table.Column<int>(nullable: true),
                    ratingValue = table.Column<double>(nullable: false),
                    weight = table.Column<int>(nullable: false),
                    networkid = table.Column<int>(nullable: true),
                    webChannel = table.Column<string>(nullable: true),
                    imageId = table.Column<int>(nullable: true),
                    summary = table.Column<string>(nullable: true),
                    updated = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shows", x => x.id);
                    table.ForeignKey(
                        name: "FK_Shows_Image_imageId",
                        column: x => x.imageId,
                        principalTable: "Image",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Shows_Network_networkid",
                        column: x => x.networkid,
                        principalTable: "Network",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Shows_Schedule_scheduleId",
                        column: x => x.scheduleId,
                        principalTable: "Schedule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Genere",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    genereName = table.Column<string>(nullable: true),
                    Showid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genere", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Genere_Shows_Showid",
                        column: x => x.Showid,
                        principalTable: "Shows",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "da2dd130-c411-4295-adc7-8d3e1788f52d", "b208ef46-cbf0-46a4-8024-de33bfd163af", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "78ba56fe-cdea-41d0-84b7-6ce447cb365d", "78094f43-1a5c-4541-bdb9-939510a54945", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.CreateIndex(
                name: "IX_DayObject_ScheduleId",
                table: "DayObject",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Genere_Showid",
                table: "Genere",
                column: "Showid");

            migrationBuilder.CreateIndex(
                name: "IX_Network_countryId",
                table: "Network",
                column: "countryId");

            migrationBuilder.CreateIndex(
                name: "IX_Shows_imageId",
                table: "Shows",
                column: "imageId");

            migrationBuilder.CreateIndex(
                name: "IX_Shows_networkid",
                table: "Shows",
                column: "networkid");

            migrationBuilder.CreateIndex(
                name: "IX_Shows_scheduleId",
                table: "Shows",
                column: "scheduleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DayObject");

            migrationBuilder.DropTable(
                name: "Genere");

            migrationBuilder.DropTable(
                name: "Shows");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropTable(
                name: "Network");

            migrationBuilder.DropTable(
                name: "Schedule");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "78ba56fe-cdea-41d0-84b7-6ce447cb365d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "da2dd130-c411-4295-adc7-8d3e1788f52d");
        }
    }
}
