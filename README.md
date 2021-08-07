# DocsAPI

## Instructions

###install Git Client

sudo apt install git docker-ce docker-compose

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

  mysql --user="root" --database="Usersdb" --password="Root0++" -e "USE Usersdb; CREATE TABLE IF NOT EXISTS  Usersdb.Users (id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,UserName VARCHAR(30) NOT NULL,Hobbies VARCHAR(30) NOT NULL,Location VARCHAR(50));"

  
Api End Point
http://109.74.203.6:5000/swagger/index.html