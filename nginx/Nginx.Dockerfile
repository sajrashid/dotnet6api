FROM nginx:latest
RUN apt-get update && apt-get install -y nginx-extras

COPY nginx.conf /etc/nginx/nginx.conf
COPY fullchain.pem /etc/ssl/certs/cert.csr
COPY privkey.pem /etc/ssl/private/privkey.key
