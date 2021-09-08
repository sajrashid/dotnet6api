FROM nginx:latest

COPY nginx.conf /etc/nginx/nginx.conf
COPY nginx.conf /etc/nginx/nginx.conf
COPY cert.csr /etc/ssl/certs/cert.crt
COPY privkey.key /etc/ssl/private/privkey.key
