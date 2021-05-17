START TRANSACTION;

ALTER TABLE "Comments" DROP CONSTRAINT "FK_Comments_Posts_PostId";

ALTER TABLE "Tags" DROP CONSTRAINT "FK_Tags_Posts_PostId";

DROP TABLE "CommentReplies";

DROP TABLE "PostComments";

DROP INDEX "IX_Tags_PostId";

ALTER TABLE "Tags" DROP COLUMN "PostId";

ALTER TABLE "Comments" ADD "TargetId" integer NULL;

CREATE INDEX "IX_Comments_TargetId" ON "Comments" ("TargetId");

ALTER TABLE "Comments" ADD CONSTRAINT "FK_Comments_Comments_TargetId" FOREIGN KEY ("TargetId") REFERENCES "Comments" ("Id") ON DELETE CASCADE;

ALTER TABLE "Comments" ADD CONSTRAINT "FK_Comments_Posts_PostId" FOREIGN KEY ("PostId") REFERENCES "Posts" ("Id") ON DELETE CASCADE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20210516172355_PostsChanged', '5.0.5');

COMMIT;

