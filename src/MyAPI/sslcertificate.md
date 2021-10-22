# How to create a local SSL certificate in Windows

## For use in Docker


### Remove  existing dev certificate(s)

```ps
dotnet dev-certs https --clean
```
###  Generate & trust, new  certificate

Generate a new self-signed certificate, trust it and also export it to a password-protected .pfx file
for use in kestrel without nginx proxy
```sh
dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\aspnetapp.pfx -p SECRETPASSWORD
dotnet dev-certs https --trust
```

Add to docker-dev file  Enviroment section under services api 
```sh
- ASPNETCORE_Kestrel__Certificates__Default__Password=SECRETPASSWORD
- ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx
```

for nginx we need to create a private key
```sh
openssl genrsa -des3 -out myCA.key 2048
```
then the cert
```yml
    docker-compose -f docker-compose.dev.yml -f docker-compose.dev.yml up
```

### Trust the cert on windows
rename or copy the cert myCA.pem to myCA.crt
double click the cert to add to trust store or use cmd line 

```ps
copy myCA.pem myCa.crt
certmgr /add /c myCA.crt /s /r localMachine root
```

