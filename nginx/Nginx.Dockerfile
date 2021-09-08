FROM nginx:latest

COPY nginx.conf /etc/nginx/nginx.conf
COPY ./cert/cert.csr /etc/ssl/certs/cert.crt
COPY ./cert/privkey.key /etc/ssl/private/privkey.key
