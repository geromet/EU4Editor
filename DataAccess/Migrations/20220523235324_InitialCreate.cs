using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLibrary.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IdeaGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdeaGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReligionGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DefenderOfFaith = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanFormPersonalUnions = table.Column<bool>(type: "INTEGER", nullable: false),
                    CenterOfReligion = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReligionGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ideas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    IdeaGroupId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ideas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ideas_IdeaGroups_IdeaGroupId",
                        column: x => x.IdeaGroupId,
                        principalTable: "IdeaGroups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Religions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ReligionGroupId = table.Column<int>(type: "INTEGER", nullable: false),
                    HreReligion = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Religions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Religions_ReligionGroups_ReligionGroupId",
                        column: x => x.ReligionGroupId,
                        principalTable: "ReligionGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Modifiers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Power = table.Column<int>(type: "INTEGER", nullable: false),
                    IdeaGroupId = table.Column<int>(type: "INTEGER", nullable: true),
                    IdeaId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modifiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Modifiers_IdeaGroups_IdeaGroupId",
                        column: x => x.IdeaGroupId,
                        principalTable: "IdeaGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Modifiers_Ideas_IdeaId",
                        column: x => x.IdeaId,
                        principalTable: "Ideas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ideas_IdeaGroupId",
                table: "Ideas",
                column: "IdeaGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Modifiers_IdeaGroupId",
                table: "Modifiers",
                column: "IdeaGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Modifiers_IdeaId",
                table: "Modifiers",
                column: "IdeaId");

            migrationBuilder.CreateIndex(
                name: "IX_Religions_ReligionGroupId",
                table: "Religions",
                column: "ReligionGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Modifiers");

            migrationBuilder.DropTable(
                name: "Religions");

            migrationBuilder.DropTable(
                name: "Ideas");

            migrationBuilder.DropTable(
                name: "ReligionGroups");

            migrationBuilder.DropTable(
                name: "IdeaGroups");
        }
    }
}
