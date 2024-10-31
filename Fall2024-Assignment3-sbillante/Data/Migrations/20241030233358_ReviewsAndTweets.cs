using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fall2024_Assignment3_sbillante.Data.Migrations
{
    /// <inheritdoc />
    public partial class ReviewsAndTweets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "Reviews",
                table: "Movie",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "Photo",
                table: "Actor",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.AddColumn<string>(
                name: "Tweets",
                table: "Actor",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reviews",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "Tweets",
                table: "Actor");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Photo",
                table: "Actor",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reviews",
                table: "Actor",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
