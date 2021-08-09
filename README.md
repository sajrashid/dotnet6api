# DocsAPI

## Instructions
apt-get update  
followed by 
apt-get upgrade 
###install Git Client

sudo apt install git docker-ce docker-compose


git clone https://sajrashid:r3dstar99@github.com/sajrashid/DocsAPI.git

git clone https://github.com/sajrashid/DocsAPI/

### first build

docker-compose -f docker-compose.yml up

### subsequent
docker-compose up --build


### to login start container

docker exec -it docsapi_database_1 /bin/bash

### then

cd to sql folder usr/bin

mysql -u root -p At the Enter password: prompt, well, enter root's password 

 mysql --user="root" --database="Usersdb" --password="Root0++" < "sql-scripts"

 or 

 mysql --user="root" --database="Usersdb" --password="Root0++" -e "DROP TABLE Usersdb.Users;"
 
mysql --user="root" --database="Usersdb" --password="Root0++" -e "CREATE TABLE Usersdb.Users (Id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY, UserAgent VARCHAR(256) NOT NULL,IP VARCHAR(30) NOT NULL,CanvasId VARCHAR(8));"

mysql --user="root" --database="Usersdb" --password="Root0++" -e "CREATE TABLE Usersdb.Users (Id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY, UserAgent VARCHAR(256) NOT NULL,IP VARCHAR(30) NOT NULL,CanvasId VARCHAR(8),LastVisit DATETIME, Count INT);"
  
Api End Point
http://109.74.203.6:5000/swagger/index.html