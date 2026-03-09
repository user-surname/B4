# --- ETAPA 1: BUILD (Compilación) ---
FROM node:20-alpine AS builder

# Directorio de trabajo dentro del contenedor
WORKDIR /app

# ===== Variables de entorno opcionales (si las necesitas en build) =====
# Puedes pasarlas con --build-arg si quieres
ARG API_URL
ENV API_URL=${API_URL}

# Copiar primero archivos de dependencias para aprovechar cache
COPY package*.json ./

# Instalar dependencias
RUN npm install

# Copiar resto del proyecto
COPY . .

# Construcción para producción (Angular 21)
RUN npm run build -- --configuration production


# --- ETAPA 2: PRODUCTION (Servidor Nginx) ---
FROM nginx:stable-alpine AS production

# Limpiar contenido por defecto de Nginx
RUN rm -rf /usr/share/nginx/html/*

# (Opcional pero recomendado para Angular SPA)
# Si tienes un nginx.conf personalizado descomenta esta línea:
COPY ./nginx.conf /etc/nginx/conf.d/default.conf

# Copiar los archivos generados en la etapa builder
COPY --from=builder /app/dist/B4.Web.Slava/browser /usr/share/nginx/html

# Exponer puerto 80
EXPOSE 80

# Iniciar Nginx en foreground (necesario para Docker)
CMD ["nginx", "-g", "daemon off;"]