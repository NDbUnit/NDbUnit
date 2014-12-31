rem SQL Server setup *****
sqlcmd -S ".\SQL2008R2SP2" -U sa -P Password12! -i "%APPVEYOR_BUILD_FOLDER%\NDbUnit.Test\Scripts\sqlserver-testdb-create.sql"

rem MySQL setup *****
"%APPVEYOR_BUILD_FOLDER%\NDbUnit\tools\MySql\bin\mysql" --user=root --password=Password12! < "%APPVEYOR_BUILD_FOLDER%\NDbUnit.Test\Scripts\mysql-testdb-create.sql"

rem PostgreSQL setup *****
rem NOTE: postgres won't support a CREATE DATABASE call inside a larger script, so CREATE has to be its own invocation...
"%APPVEYOR_BUILD_FOLDER%\NDbUnit\tools\Postgres\bin\psql" --username=postgres --no-password --command="CREATE DATABASE testdb;"
"%APPVEYOR_BUILD_FOLDER%\NDbUnit\tools\Postgres\bin\psql" --username=postgres --no-password --dbname=testdb --file="%APPVEYOR_BUILD_FOLDER%\NDbUnit.Test\Scripts\postgres-testdb-create.sql"

rem ORACLE XE setup *****
rem *** DISABLED UNTIL APPVEYOR SUPPORTS ORA XE ON CI SERVERS ***
rem %ORACLE_HOME%\bin\sqlplus xdba/xdba @"%APPVEYOR_BUILD_FOLDER%\NDbUnit.Test\Scripts\oracle-testdb-create.sql"

