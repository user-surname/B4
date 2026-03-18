# ==========================================================
# ETAPA 1: BUILD (Compilación del proyecto Angular)
# ==========================================================

# Usa una imagen ligera de Node.js 20 basada en Alpine Linux
# Esta imagen contiene Node y npm necesarios para compilar Angular
FROM node:20-alpine AS builder

# Define el directorio de trabajo dentro del contenedor
# Todos los comandos posteriores se ejecutarán dentro de /app
WORKDIR /app


# ---------- VARIABLES DE ENTORNO OPCIONALES ----------

# Define una variable que se puede pasar al construir la imagen
# Ejemplo:
# docker build --build-arg API_URL=http://api:5000 .
ARG API_URL

# Convierte el argumento anterior en una variable de entorno
# que puede ser utilizada durante el proceso de build
ENV API_URL=${API_URL}


# ---------- INSTALACIÓN DE DEPENDENCIAS ----------

# Copia únicamente los archivos de dependencias primero
# Esto permite que Docker use cache si el package.json no cambia
# y evita reinstalar dependencias en cada build
COPY package*.json ./

# Instala todas las dependencias del proyecto Angular
# según lo definido en package.json
RUN npm install


# ---------- COPIA DEL CÓDIGO DEL PROYECTO ----------

# Copia el resto del código fuente del proyecto Angular
# (componentes, servicios, assets, etc.)
COPY . .


# ---------- BUILD DEL PROYECTO ----------

# Compila el proyecto Angular en modo producción
#
# Esto genera archivos optimizados en la carpeta:
#
# /app/dist/B4.Web.Slava/
#
# Los archivos generados son:
# - HTML
# - CSS
# - JavaScript
# - assets
#
# Estos archivos son estáticos y pueden ser servidos por cualquier servidor web
RUN npm run build -- --configuration production



# ==========================================================
# ETAPA 2: PRODUCCIÓN (Servidor Nginx)
# ==========================================================

# Usa una imagen ligera de Nginx para servir los archivos estáticos
# Nginx es un servidor web rápido y muy usado para aplicaciones SPA
FROM nginx:stable-alpine AS production


# ---------- LIMPIEZA DEL CONTENIDO POR DEFECTO ----------

# Elimina la página HTML por defecto que trae Nginx
RUN rm -rf /usr/share/nginx/html/*


# ---------- CONFIGURACIÓN DE NGINX ----------

# Copia el archivo de configuración personalizado de Nginx
# Esto es especialmente importante para Angular SPA,
# porque permite redirigir todas las rutas al index.html
#
# Ejemplo típico:
# try_files $uri $uri/ /index.html;
COPY ./nginx.conf /etc/nginx/conf.d/default.conf


# ---------- COPIA DEL BUILD DE ANGULAR ----------

# Copia los archivos generados en la etapa builder
#
# /app/dist/B4.Web.Slava/browser  -> carpeta generada por Angular
#
# /usr/share/nginx/html           -> carpeta donde Nginx sirve archivos
COPY --from=builder /app/dist/B4.Web.Slava/browser /usr/share/nginx/html


# ---------- PUERTO EXPUESTO ----------

# Indica que el contenedor servirá HTTP en el puerto 80
EXPOSE 80


# ---------- COMANDO DE INICIO ----------

# Inicia Nginx en primer plano
# (Docker necesita que el proceso principal no se cierre)
CMD ["nginx", "-g", "daemon off;"]
