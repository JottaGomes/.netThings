using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _netThings.Migrations
{
    /// <inheritdoc />
    public partial class frist1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Characters",
                table: "Characters");

            migrationBuilder.RenameTable(
                name: "Characters",
                newName: "Characters1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Characters1",
                table: "Characters1",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Characters1",
                table: "Characters1");

            migrationBuilder.RenameTable(
                name: "Characters1",
                newName: "Characters");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Characters",
                table: "Characters",
                column: "id");
        }
    }
}
