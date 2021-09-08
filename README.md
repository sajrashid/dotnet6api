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

mysql --user="root" --database="Usersdb" --password="Root0++" -e "CREATE TABLE Usersdb.Users (Id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY, UserAgent VARCHAR(256) NOT NULL,IP VARCHAR(30) NOT NULL,CanvasId VARCHAR(8),LastVisit DATETIME, Count INT);"

Api End Point
http://109.74.203.6:5000/swagger/index.html



# letencryp SSL

 git clone https://github.com/lukas2511/dehydrated.git
~$ git clone https://github.com/jbjonesjr/letsencrypt-manual-hook.git dehydrated/hooks/manual
~$ cd dehydrated
~$ ./dehydrated --register --accept-terms
~$ ./dehydrated --cron --challenge dns-01 --domain your.domain.com --hook ./hooks/manual/manual_hook.rb
#
# !! WARNING !! No main config file found, using default config!
#
Processing your.domain.com
 + Signing domains...
 + Creating new directory /Users/vikas/dehydrated/certs/your.domain.com ...
 + Creating chain cache directory /Users/vikas/dehydrated/chains
 + Generating private key...
 + Generating signing request...
 + Requesting authorization for your.domain.com...
 + 1 pending challenge(s)
 + Deploying challenge tokens...
Checking for pre-existing TXT record for the domain: '_acme-challenge.your.domain.com'.
Create TXT record for the domain: '_acme-challenge.your.domain.com'. TXT record:
'gkIxxxxxxxIcAESmjF8pjZGQrrZxxxxxxxxxxx'
Press enter when DNS has been updated...
You will get a hash (after running the above command), create a TXT record in your DNS. Make sure it works by either running the below command or GSuite Toolbox

~$ dig TXT _acme-challenge.your.domain.com. +short @8.8.8.8
"gkIxxxxxxxIcAESmjF8pjZGQrrZxxxxxxxxxxx"
~$
Now, press enter at the prompt. This did not work for me although the TXT record was updated. I had to press Ctrl+C and run the command again.

~$ ./dehydrated --cron --challenge dns-01 --domain your.domain.com --hook ./hooks/manual/manual_hook.rb
#
# !! WARNING !! No main config file found, using default config!
#
Processing your.domain.com
 + Signing domains...
 + Generating private key...
 + Generating signing request...
 + Requesting authorization for your.domain.com...
 + 1 pending challenge(s)
 + Deploying challenge tokens...
Checking for pre-existing TXT record for the domain: '_acme-challenge.your.domain.com'.
Found gkIxxxxxxxIcAESmjF8pjZGQrrZxxxxxxxxxxx. match.
 + Responding to challenge for your.domain.com authorization...
Challenge complete. Leave TXT record in place to allow easier future refreshes.
 + Challenge is valid!
 + Requesting certificate...
 + Checking certificate...
 + Done!
 + Creating fullchain.pem...
 + Walking chain...
 + Done!
~$
Now, your public and private certs are present here.

$ ls certs/your.domain.com/privkey.pem certs/your.domain.com/fullchain-1517576424.pem
To renew (minimum wait time is 30 days), just the same command again.

~$ ./dehydrated --cron --challenge dns-01 --domain your.domain.com --hook ./hooks/manua

create pfx file from cert

# openssl pkcs12 -export -out certificate.pfx -inkey privkey.pem -in cert.pem -certfile chain.pem

create crt file from cert.pem and copy to /usr/local/share/ca-certificates, cp cert.pem to cert.crt

then add to server certificate store

sudo update-ca-certificates

you should entry 1 added

