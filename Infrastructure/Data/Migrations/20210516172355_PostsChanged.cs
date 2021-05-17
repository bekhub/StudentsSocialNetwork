using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class PostsChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_PostId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Posts_PostId",
                table: "Tags");

            migrationBuilder.DropTable(
                name: "CommentReplies");

            migrationBuilder.DropTable(
                name: "PostComments");

            migrationBuilder.DropIndex(
                name: "IX_Tags_PostId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Tags");

            migrationBuilder.AddColumn<int>(
                name: "TargetId",
                table: "Comments",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_TargetId",
                table: "Comments",
                column: "TargetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_TargetId",
                table: "Comments",
                column: "TargetId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Posts_PostId",
                table: "Comments",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_TargetId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_PostId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_TargetId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "TargetId",
                table: "Comments");

            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "Tags",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CommentReplies",
                columns: table => new
                {
                    TargetId = table.Column<int>(type: "integer", nullable: false),
                    ReplyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentReplies", x => new { x.TargetId, x.ReplyId });
                    table.ForeignKey(
                        name: "FK_CommentReplies_Comments_ReplyId",
                        column: x => x.ReplyId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentReplies_Comments_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostComments",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "integer", nullable: false),
                    CommentId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostComments", x => new { x.PostId, x.CommentId });
                    table.ForeignKey(
                        name: "FK_PostComments_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostComments_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tags_PostId",
                table: "Tags",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentReplies_ReplyId",
                table: "CommentReplies",
                column: "ReplyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostComments_CommentId",
                table: "PostComments",
                column: "CommentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Posts_PostId",
                table: "Comments",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Posts_PostId",
                table: "Tags",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
