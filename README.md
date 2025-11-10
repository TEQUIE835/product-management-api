# 🧱 Product Management API

API REST desarrollada en **.NET 9** como parte de un simulacro técnico para aprender arquitectura limpia, autenticación JWT y control de acceso por roles.  
El sistema permite la **gestión de productos y usuarios**, incluyendo autenticación, registro y autorización basada en roles.

---

## 🚀 Características principales

- CRUD completo para **Productos** y **Usuarios**
- **Autenticación JWT** (inicio de sesión y registro)
- **Autorización por Roles** (Admin / User)
- Arquitectura **Clean Architecture**:
  - Domain
  - Application
  - Infrastructure
  - Api
- Uso de **Entity Framework Core**
- Configuración de **Swagger / OpenAPI**
- Preparado para **Docker** y despliegue en **Render**

---

## 🧩 Estructura del Proyecto

´´´
product-management-api/
├── ProductManagement.Domain/         # Entidades base y modelos
├── ProductManagement.Application/    # Casos de uso, servicios y lógica de negocio
├── ProductManagement.Infrastructure/ # Contexto EF Core y persistencia
├── ProductManagement.Api/            # Controladores, configuración y capa de presentación
└── Dockerfile                        # Configuración para contenedor
´´´

---

## ⚙️ Requisitos Previos

- [.NET SDK 9.0+](https://dotnet.microsoft.com/)
- [PostgreSQL](https://www.postgresql.org/) (u otro motor compatible)
- (Opcional) [Docker](https://www.docker.com/) para ejecución en contenedor

---

## 🧰 Instalación y Ejecución

### 1️⃣ Clonar el repositorio

```
git clone https://github.com/TEQUIE835/product-management-api.git
cd product-management-api
# Nota: ejecutar los siguientes comandos desde la carpeta ProductManagement.Api
```

### 2️⃣ Configurar las variables de entorno

Ejemplo de `appsettings.json` (o usa variables de entorno).  
He agregado comentarios dentro para indicar qué debes modificar.

```
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=product_management;Username=postgres;Password=yourpassword"
  },
  "JwtSettings": {
    "Key": "tu_clave_secreta_segura",          // Cambia esta clave
    "Issuer": "ProductManagementAPI",
    "Audience": "ProductManagementClient"
  }
}
```

### 3️⃣ Ejecutar migraciones y correr la API

```
cd ProductManagement.Api
dotnet ef database update
dotnet run
```

La API se iniciará en:  
- https://localhost:5001/swagger → Swagger UI  
- http://localhost:5000 → HTTP normal  

---

## 🧪 Endpoints Principales

### 🔐 Autenticación
| Método | Endpoint              | Descripción               |
|--------|-----------------------|----------------------------|
| POST   | `/api/auth/register`  | Registra un nuevo usuario  |
| POST   | `/api/auth/login`     | Inicia sesión y obtiene token JWT |

### 👤 Usuarios
| Método | Endpoint              | Descripción               |
|--------|-----------------------|----------------------------|
| GET    | `/api/users`          | Lista usuarios (solo Admin) |
| PUT    | `/api/users/{id}`     | Actualiza un usuario       |
| DELETE | `/api/users/{id}`     | Elimina un usuario         |

### 📦 Productos
| Método | Endpoint              | Descripción               |
|--------|-----------------------|----------------------------|
| GET    | `/api/products`       | Lista todos los productos  |
| GET    | `/api/products/{id}`  | Obtiene un producto por id |
| POST   | `/api/products`       | Crea un nuevo producto (solo Admin) |
| PUT    | `/api/products/{id}`  | Actualiza un producto      |
| DELETE | `/api/products/{id}`  | Elimina un producto        |

---

## 🔑 Autenticación JWT

1. Usa `/api/auth/login` para obtener un token.  
2. Copia el token JWT que devuelve la respuesta.  
3. En **Swagger**, haz clic en el candado 🔒 → pega el token con el prefijo `Bearer`.

---

## 🐳 Ejecución con Docker

```
docker build -t product-management-api .
docker run -p 8080:8080 product-management-api
```
---

## ☁️ Despliegue en Render (opcional)

1. Sube tu repositorio a GitHub.  
2. Crea un nuevo servicio en [Render.com](https://render.com).  
3. Selecciona tu repo y elige **Docker** como método de despliegue.  
4. Configura las variables de entorno:
   - `ConnectionStrings__DefaultConnection`
   - `JwtSettings__Key`
   - `JwtSettings__Issuer`
   - `JwtSettings__Audience`
5. Render construirá la imagen usando tu `Dockerfile` automáticamente.

---

## 🧠 Aprendizajes del Proyecto

- Configuración y uso de JWT y Claims en .NET
- Manejo de roles y políticas de autorización
- Separación de capas siguiendo arquitectura limpia
- Implementación de repositorios y servicios
- Despliegue con Docker y Render

---

## 📄 Licencia

Este proyecto se distribuye con fines educativos.  
© 2025 — Desarrollado por **David Orjuela**
