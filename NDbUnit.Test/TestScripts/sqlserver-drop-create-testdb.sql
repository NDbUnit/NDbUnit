if exists(select * from sys.databases where name = 'testdb')
    drop database [testdb]
;
if not exists(select * from sys.databases where name = 'testdb')
    create database [testdb]