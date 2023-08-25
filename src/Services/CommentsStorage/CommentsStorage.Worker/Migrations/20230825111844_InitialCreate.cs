using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommentsStorage.Worker.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Etag = table.Column<string>(type: "text", nullable: false),
                    AuthorDisplayName = table.Column<string>(type: "text", nullable: false),
                    AuthorProfileImageUrl = table.Column<string>(type: "text", nullable: false),
                    AuthorChannelUrl = table.Column<string>(type: "text", nullable: false),
                    AuthorChannelId = table.Column<string>(type: "text", nullable: false),
                    ChannelId = table.Column<string>(type: "text", nullable: false),
                    VideoId = table.Column<string>(type: "text", nullable: false),
                    TextDisplay = table.Column<string>(type: "text", nullable: false),
                    TextOriginal = table.Column<string>(type: "text", nullable: false),
                    ViewerRating = table.Column<string>(type: "text", nullable: false),
                    LikeCount = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    PublishedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    TotalReplyCount = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");
        }
    }
}
