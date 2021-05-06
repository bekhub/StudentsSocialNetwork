START TRANSACTION;

ALTER TABLE "Students" ADD "IsActive" boolean NULL DEFAULT TRUE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20210506000331_StudentChanged', '5.0.5');

COMMIT;

