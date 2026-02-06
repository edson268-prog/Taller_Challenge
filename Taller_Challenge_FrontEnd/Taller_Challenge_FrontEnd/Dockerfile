FROM node:18-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build:docker

FROM nginx:alpine
# Copy the compiled files from the previous stage
COPY --from=build /app/dist/taller-challenge-front-end/browser /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]