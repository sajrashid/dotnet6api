FROM nginx:latest

COPY nginx.conf /etc/nginx/nginx.conf
COPY ./cert/cert.csr /etc/ssl/certs/cert.csr
COPY ./cert/privkey.key /etc/ssl/private/privkey.key
