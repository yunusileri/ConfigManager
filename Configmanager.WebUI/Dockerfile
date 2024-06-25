FROM node:18.18-alpine as builder

WORKDIR /app

COPY Configmanager.WebUI /app/
COPY Configmanager.WebUI/.env.docker /app/.env

RUN npm install
RUN npm run build
#
FROM node:18.18-alpine as runner

COPY --from=builder /app/.next/standalone /app
COPY --from=builder /app/.next/static /app/.next/static
COPY --from=builder /app/public /app/public

WORKDIR /app

EXPOSE 3000

CMD ["node", "server.js"]
