﻿CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "Institutes" (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    "Name" text NOT NULL,
    CONSTRAINT "PK_Institutes" PRIMARY KEY ("Id")
);

CREATE TABLE "Roles" (
    "Id" text NOT NULL,
    "Name" character varying(256) NULL,
    "NormalizedName" character varying(256) NULL,
    "ConcurrencyStamp" text NULL,
    CONSTRAINT "PK_Roles" PRIMARY KEY ("Id")
);

CREATE TABLE "Users" (
    "Id" text NOT NULL,
    "Firstname" text NULL,
    "Lastname" text NULL,
    "ProfilePictureUrl" text NULL,
    "CurrentCity" text NULL,
    "SignUpDate" timestamp without time zone NULL,
    "UserName" character varying(256) NULL,
    "NormalizedUserName" character varying(256) NULL,
    "Email" character varying(256) NULL,
    "NormalizedEmail" character varying(256) NULL,
    "EmailConfirmed" boolean NOT NULL,
    "PasswordHash" text NULL,
    "SecurityStamp" text NULL,
    "ConcurrencyStamp" text NULL,
    "PhoneNumber" text NULL,
    "PhoneNumberConfirmed" boolean NOT NULL,
    "TwoFactorEnabled" boolean NOT NULL,
    "LockoutEnd" timestamp with time zone NULL,
    "LockoutEnabled" boolean NOT NULL,
    "AccessFailedCount" integer NOT NULL,
    CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
);

CREATE TABLE "Departments" (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    "Name" text NOT NULL,
    "ShortName" text NULL,
    "InstituteId" integer NOT NULL,
    CONSTRAINT "PK_Departments" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Departments_Institutes_InstituteId" FOREIGN KEY ("InstituteId") REFERENCES "Institutes" ("Id") ON DELETE CASCADE
);

CREATE TABLE "RoleClaims" (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    "RoleId" text NOT NULL,
    "ClaimType" text NULL,
    "ClaimValue" text NULL,
    CONSTRAINT "PK_RoleClaims" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_RoleClaims_Roles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "Roles" ("Id") ON DELETE CASCADE
);

CREATE TABLE "UserClaims" (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    "UserId" text NOT NULL,
    "ClaimType" text NULL,
    "ClaimValue" text NULL,
    CONSTRAINT "PK_UserClaims" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_UserClaims_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "UserLogins" (
    "LoginProvider" text NOT NULL,
    "ProviderKey" text NOT NULL,
    "ProviderDisplayName" text NULL,
    "UserId" text NOT NULL,
    CONSTRAINT "PK_UserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey"),
    CONSTRAINT "FK_UserLogins_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "UserRoles" (
    "UserId" text NOT NULL,
    "RoleId" text NOT NULL,
    CONSTRAINT "PK_UserRoles" PRIMARY KEY ("UserId", "RoleId"),
    CONSTRAINT "FK_UserRoles_Roles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "Roles" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_UserRoles_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "UserTokens" (
    "UserId" text NOT NULL,
    "LoginProvider" text NOT NULL,
    "Name" text NOT NULL,
    "Value" text NULL,
    CONSTRAINT "PK_UserTokens" PRIMARY KEY ("UserId", "LoginProvider", "Name"),
    CONSTRAINT "FK_UserTokens_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Courses" (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    "Name" text NOT NULL,
    "Code" text NOT NULL,
    "Semester" integer NOT NULL,
    "Theory" integer NOT NULL,
    "Practice" integer NOT NULL,
    "Credits" integer NOT NULL,
    "Grade" integer NULL,
    "DepartmentId" integer NOT NULL,
    CONSTRAINT "PK_Courses" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Courses_Departments_DepartmentId" FOREIGN KEY ("DepartmentId") REFERENCES "Departments" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Students" (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    "StudentNumber" text NOT NULL,
    "StudentEmail" text NOT NULL,
    "StudentPassword" text NOT NULL,
    "AdmissionYear" integer NULL,
    "BirthPlace" text NULL,
    "BirthDate" timestamp without time zone NULL,
    "UserId" text NOT NULL,
    "DepartmentId" integer NOT NULL,
    CONSTRAINT "PK_Students" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Students_Departments_DepartmentId" FOREIGN KEY ("DepartmentId") REFERENCES "Departments" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Students_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "CourseTimes" (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    "Weekday" integer NOT NULL,
    "Time" interval NOT NULL,
    "AcademicYearFrom" integer NOT NULL,
    "AcademicYearTo" integer NOT NULL,
    "CourseId" integer NOT NULL,
    CONSTRAINT "PK_CourseTimes" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_CourseTimes_Courses_CourseId" FOREIGN KEY ("CourseId") REFERENCES "Courses" ("Id") ON DELETE CASCADE
);

CREATE TABLE "StudentCourses" (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    "TheoryAbsent" integer NOT NULL,
    "PracticeAbsent" integer NOT NULL,
    "StudentId" integer NOT NULL,
    "CourseId" integer NOT NULL,
    CONSTRAINT "PK_StudentCourses" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_StudentCourses_Courses_CourseId" FOREIGN KEY ("CourseId") REFERENCES "Courses" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_StudentCourses_Students_StudentId" FOREIGN KEY ("StudentId") REFERENCES "Students" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Assessments" (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    "Point" integer NOT NULL,
    "Type" text NOT NULL,
    "StudentCourseId" integer NOT NULL,
    CONSTRAINT "PK_Assessments" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Assessments_StudentCourses_StudentCourseId" FOREIGN KEY ("StudentCourseId") REFERENCES "StudentCourses" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Assessments_StudentCourseId" ON "Assessments" ("StudentCourseId");

CREATE INDEX "IX_Courses_DepartmentId" ON "Courses" ("DepartmentId");

CREATE INDEX "IX_CourseTimes_CourseId" ON "CourseTimes" ("CourseId");

CREATE INDEX "IX_Departments_InstituteId" ON "Departments" ("InstituteId");

CREATE INDEX "IX_RoleClaims_RoleId" ON "RoleClaims" ("RoleId");

CREATE UNIQUE INDEX "RoleNameIndex" ON "Roles" ("NormalizedName");

CREATE INDEX "IX_StudentCourses_CourseId" ON "StudentCourses" ("CourseId");

CREATE INDEX "IX_StudentCourses_StudentId" ON "StudentCourses" ("StudentId");

CREATE INDEX "IX_Students_DepartmentId" ON "Students" ("DepartmentId");

CREATE INDEX "IX_Students_UserId" ON "Students" ("UserId");

CREATE INDEX "IX_UserClaims_UserId" ON "UserClaims" ("UserId");

CREATE INDEX "IX_UserLogins_UserId" ON "UserLogins" ("UserId");

CREATE INDEX "IX_UserRoles_RoleId" ON "UserRoles" ("RoleId");

CREATE INDEX "EmailIndex" ON "Users" ("NormalizedEmail");

CREATE UNIQUE INDEX "UserNameIndex" ON "Users" ("NormalizedUserName");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20210330172929_Initial', '5.0.4');

COMMIT;

START TRANSACTION;

CREATE UNIQUE INDEX "IX_Users_Email" ON "Users" ("Email");

CREATE UNIQUE INDEX "IX_Users_UserName" ON "Users" ("UserName");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20210330180823_UniqueUsername', '5.0.4');

COMMIT;

