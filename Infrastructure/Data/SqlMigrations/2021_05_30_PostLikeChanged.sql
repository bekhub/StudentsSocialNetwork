START TRANSACTION;

ALTER TABLE "PostLikes" ADD "IsLiked" boolean NOT NULL DEFAULT FALSE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20210529181858_PostLikeChanged', '5.0.5');

COMMIT;

