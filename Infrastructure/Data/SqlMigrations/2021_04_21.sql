START TRANSACTION;

ALTER TABLE "Courses" DROP CONSTRAINT "FK_Courses_Departments_DepartmentId";

ALTER TABLE "Courses" DROP COLUMN "Semester";

ALTER TABLE "StudentCourses" ADD "AcademicYear" integer NOT NULL DEFAULT 0;

ALTER TABLE "StudentCourses" ADD "AverageAssessment" text NULL;

ALTER TABLE "StudentCourses" ADD "Semester" integer NOT NULL DEFAULT 0;

ALTER TABLE "Courses" ALTER COLUMN "DepartmentId" DROP NOT NULL;

ALTER TABLE "Courses" ADD CONSTRAINT "FK_Courses_Departments_DepartmentId" FOREIGN KEY ("DepartmentId") REFERENCES "Departments" ("Id") ON DELETE RESTRICT;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20210419235447_SynchronizingAdded', '5.0.4');

COMMIT;

START TRANSACTION;

ALTER TABLE "Assessments" ALTER COLUMN "Point" DROP NOT NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20210421162801_AssessmentChanged', '5.0.4');

COMMIT;

