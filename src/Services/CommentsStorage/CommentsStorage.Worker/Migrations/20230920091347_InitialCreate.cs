using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

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
                name: "Videos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    FirstComment = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Videos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Etag = table.Column<string>(type: "text", nullable: true),
                    AuthorDisplayName = table.Column<string>(type: "text", nullable: true),
                    AuthorProfileImageUrl = table.Column<string>(type: "text", nullable: true),
                    AuthorChannelUrl = table.Column<string>(type: "text", nullable: true),
                    AuthorChannelId = table.Column<string>(type: "text", nullable: true),
                    ChannelId = table.Column<string>(type: "text", nullable: true),
                    VideoId = table.Column<string>(type: "text", nullable: false),
                    TextDisplay = table.Column<string>(type: "text", nullable: true),
                    TextOriginal = table.Column<string>(type: "text", nullable: true),
                    ViewerRating = table.Column<string>(type: "text", nullable: true),
                    LikeCount = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    PublishedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    TotalReplyCount = table.Column<long>(type: "bigint", nullable: false),
                    TextDisplaySearchVector = table.Column<NpgsqlTsVector>(type: "tsvector", nullable: false)
                        .Annotation("Npgsql:TsVectorConfig", "simple")
                        .Annotation("Npgsql:TsVectorProperties", new[] { "TextDisplay" }),
                    AuthorDisplayNameSearchVector = table.Column<NpgsqlTsVector>(type: "tsvector", nullable: false)
                        .Annotation("Npgsql:TsVectorConfig", "simple")
                        .Annotation("Npgsql:TsVectorProperties", new[] { "AuthorDisplayName" })
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AuthorDisplayNameSearchVector",
                table: "Comments",
                column: "AuthorDisplayNameSearchVector")
                .Annotation("Npgsql:IndexMethod", "GIN");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PublishedAt",
                table: "Comments",
                column: "PublishedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_TextDisplaySearchVector",
                table: "Comments",
                column: "TextDisplaySearchVector")
                .Annotation("Npgsql:IndexMethod", "GIN");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_VideoId",
                table: "Comments",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_VideoId_PublishedAt",
                table: "Comments",
                columns: new[] { "VideoId", "PublishedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Videos_Id",
                table: "Videos",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Videos");
        }
    }
}
