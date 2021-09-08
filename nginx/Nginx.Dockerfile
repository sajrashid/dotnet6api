FROM nginx:latest

COPY nginx.conf /etc/nginx/nginx.conf
COPY fullchain.pem /etc/ssl/certs/cert.csr
COPY privkey.pem /etc/ssl/private/privkey.key
