/*
ALTER TABLE ONLY public."UserRole" DROP CONSTRAINT "UserRole_UserID_fkey";
ALTER TABLE ONLY public."UserRole" DROP CONSTRAINT "UserRole_RoleID_fkey";
ALTER TABLE ONLY public."User" DROP CONSTRAINT "FK_User_supervisor";
ALTER TABLE ONLY public."User" DROP CONSTRAINT "User_pkey";
ALTER TABLE ONLY public."UserRole" DROP CONSTRAINT "UserRole_pkey";
ALTER TABLE ONLY public."Role" DROP CONSTRAINT "Role_pkey";
DROP TABLE public."UserRole";
DROP TABLE public."User";
DROP TABLE public."Role";
DROP SCHEMA public;
*/

--CREATE SCHEMA public;

--COMMENT ON SCHEMA public IS 'standard public schema';


SET search_path = public, pg_catalog;

SET default_tablespace = '';

SET default_with_oids = false;

CREATE TABLE "Role" (
    "ID" SERIAL NOT NULL,
    "Name" character(45),
    "Description" character(45)
);


CREATE TABLE "User" (
    "ID" SERIAL NOT NULL,
    "FirstName" character(45),
    "LastName" character(45),
    "Age" bigint,
    "SupervisorID" bigint
);


CREATE TABLE "UserRole" (
    "UserID" bigint NOT NULL,
    "RoleID" bigint NOT NULL
);


ALTER TABLE ONLY "Role"
    ADD CONSTRAINT "Role_pkey" PRIMARY KEY ("ID");

ALTER TABLE ONLY "UserRole"
    ADD CONSTRAINT "UserRole_pkey" PRIMARY KEY ("UserID", "RoleID");

ALTER TABLE ONLY "User"
    ADD CONSTRAINT "User_pkey" PRIMARY KEY ("ID");

ALTER TABLE ONLY "User"
    ADD CONSTRAINT "FK_User_supervisor" FOREIGN KEY ("SupervisorID") REFERENCES "User"("ID") ON UPDATE CASCADE ON DELETE SET NULL;

ALTER TABLE ONLY "UserRole"
    ADD CONSTRAINT "UserRole_RoleID_fkey" FOREIGN KEY ("RoleID") REFERENCES "Role"("ID");

ALTER TABLE ONLY "UserRole"
    ADD CONSTRAINT "UserRole_UserID_fkey" FOREIGN KEY ("UserID") REFERENCES "User"("ID");

