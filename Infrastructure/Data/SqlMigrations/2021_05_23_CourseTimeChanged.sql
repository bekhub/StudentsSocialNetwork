START TRANSACTION;

ALTER TABLE "CourseTimes" DROP CONSTRAINT "FK_CourseTimes_Courses_CourseId";

ALTER TABLE "CourseTimes" DROP COLUMN "AcademicYearFrom";

ALTER TABLE "CourseTimes" RENAME COLUMN "Time" TO "TimeTo";

ALTER TABLE "CourseTimes" RENAME COLUMN "AcademicYearTo" TO "StudentCourseId";

ALTER TABLE "StudentCourses" ADD "Teacher" text NULL;

ALTER TABLE "CourseTimes" ALTER COLUMN "CourseId" DROP NOT NULL;

ALTER TABLE "CourseTimes" ADD "Classroom" text NULL;

ALTER TABLE "CourseTimes" ADD "TimeFrom" interval NOT NULL DEFAULT INTERVAL '00:00:00';

CREATE INDEX "IX_CourseTimes_StudentCourseId" ON "CourseTimes" ("StudentCourseId");

ALTER TABLE "CourseTimes" ADD CONSTRAINT "FK_CourseTimes_Courses_CourseId" FOREIGN KEY ("CourseId") REFERENCES "Courses" ("Id") ON DELETE RESTRICT;

ALTER TABLE "CourseTimes" ADD CONSTRAINT "FK_CourseTimes_StudentCourses_StudentCourseId" FOREIGN KEY ("StudentCourseId") REFERENCES "StudentCourses" ("Id") ON DELETE CASCADE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20210522154837_CourseTimeChanged', '5.0.5');

COMMIT;

