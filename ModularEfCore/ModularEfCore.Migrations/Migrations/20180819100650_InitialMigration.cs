using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ModularEfCore.Migrations.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Customer");

            migrationBuilder.CreateTable(
                name: "Sample",
                schema: "Customer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "Customer",
                table: "Sample",
                columns: new[] { "Id", "FirstName", "LastName" },
                values: new object[] { 1, "Philipp", "Wagner" });

            migrationBuilder.InsertData(
                schema: "Customer",
                table: "Sample",
                columns: new[] { "Id", "FirstName", "LastName" },
                values: new object[] { 2, "Max", "Mustermann" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sample",
                schema: "Customer");
        }
    }
}
