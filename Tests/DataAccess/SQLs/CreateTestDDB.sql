BACKUP DATABASE EMSdb
TO DISK = N'D:\Backups\EMSdb_Test.bak'
WITH FORMAT;

USE master;
RESTORE DATABASE EMSdb_Test
FROM DISK = N'D:\Backups\EMSdb_Test.bak'
WITH REPLACE,
MOVE 'EMSdb' TO 'D:\Backups\EMSdb.mdf',
MOVE 'EMSdb_log' TO 'D:\Backups\EMSdb_log.ldf';