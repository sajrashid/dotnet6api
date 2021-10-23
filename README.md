[![CodeFactor](https://www.codefactor.io/repository/github/sajrashid/dotnet6api/badge)](https://www.codefactor.io/repository/github/sajrashid/dotnet6api)

# DotNet 6 Demo API

Demo APi built with Docker-Compose, user Nginx as a reverse proxy, setup with let's encrypt ssl.
CI-CD, runs tests, replaces secrets, connection strings etc. securely deploys to a linux vm using SSH

TLDR; to run in dev `docker-compose -f docker-compose.dev.yml up`.

## Asp.net 6

Latest release candiate , note: no minimal api's

### Xunit

Lot's of tests, over 95% coverage

### Docker-compose

Prod is hosted with MySQL DB running on a separate box, see the mysql docker-compose file

### Nginx Revese proxy

Copies the SSL certificates into the docker container see nginx docker file

### Dapper Micro ORM

Ultra lightweight, the API consumes about 130mb of memory

### MySql

MySql Running in docker, great and not great.

### JWT Bearer Authentication
Standard roles based


### Server-Setup
`apt-get update`
`apt-get upgrade`

**install Git Client**

**install Docker**
`sudo apt install git docker-ce docker-compose`

### General How to's

**To start container shell**

`docker exec -it containername /bin/bash`

**then**

cd to sql folder usr/bin

`mysql -u root -p At the Enter password: prompt, well, enter root's password`

**Manualy run sql scripts**

 `mysql --user="root" --database="Userssdb" --password="root" < "sql-scripts"`
*Or*
 `mysql --user="root" --database="somedb" --password="root" -e "DROP TABLE somedb.Users;"`

**Zap all conatiners**
 `docker stop $(docker ps -a -q) `
 `docker rm $(docker ps -a -q) `

### LetsencrypT SSL

  `git clone https://github.com/lukas2511/dehydrated.git `
  
`~$ git clone https://github.com/jbjonesjr/letsencrypt-manual-hook.git dehydrated/hooks/manual`
`~$ cd dehydrated`
`~$ ./dehydrated --register --accept-terms`
`~$ ./dehydrated --cron --challenge dns-01 --domain your.domain.com --hook ./hooks/manual/manual_hook.rb`
**outputs**

*!! WARNING !! No main config file found, using default config!

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

`~$ dig TXT _acme-challenge.your.domain.com. +short @8.8.8.8`
`"gkIxxxxxxxIcAESmjF8pjZGQrrZxxxxxxxxxxx"`
`~$`
*Now, press enter at the prompt. you may have to press Ctrl+C and run the command again.*

`~$ ./dehydrated --cron --challenge dns-01 --domain your.domain.com --hook ./hooks/manual/manual_hook.rb`


 *!! WARNING !! No main config file found, using default config!*

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

Now, your public and private certs are present here.

`$ ls certs/your.domain.com/privkey.pem certs/your.domain.com/fullchain-1517576424.pem`
To renew (minimum wait time is 30 days), just the same command again.

`~$ ./dehydrated --cron --challenge dns-01 --domain your.domain.com --hook ./hooks/manua`

### Create pfx file from cert

`openssl pkcs12 -export -out certificate.pfx -inkey privkey.pem -in cert.pem -certfile chain.pem`

create crt file from cert.pem and copy to /usr/local/share/ca-certificates, cp cert.pem to cert.crt

then add to server certificate store

`sudo update-ca-certificates`

you should see entry 1 added