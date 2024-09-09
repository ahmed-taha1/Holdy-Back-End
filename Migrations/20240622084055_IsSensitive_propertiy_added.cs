using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Holdy.Migrations
{
    /// <inheritdoc />
    public partial class IsSensitive_propertiy_added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsProtectedAttribute",
                table: "AccountAttributes",
                newName: "IsSensitive");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsSensitive",
                table: "AccountAttributes",
                newName: "IsProtectedAttribute");
        }
    }
}
