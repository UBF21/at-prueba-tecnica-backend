# Setup con Docker

## Arquitectura

```
┌─────────────────────────────────────┐
│   Frontend (React)                  │
│   docker run ... at-frontend        │
│   Port: 5173                        │
└────────────────┬────────────────────┘
                 │ (HTTP)
┌────────────────▼────────────────────┐
│   Backend (.NET 9)                  │
│   docker-compose (Backend + DB)     │
│   Port: 5000                        │
└────────────────┬────────────────────┘
                 │ (SQL)
┌────────────────▼────────────────────┐
│   SQL Server 2022 Express           │
│   docker-compose                    │
│   Port: 1433                        │
└─────────────────────────────────────┘
```

## Paso 1: Levantar Backend + BD

```bash
# Desde la raíz del backend
cd ~/RiderProjects/at-prueba-tecnica-backend

# Verificar que .env existe con:
# DB_PASSWORD=RetoPedidos_Pass123!
# JWT_SECRET=super-secret-key-for-jwt-at-least-32-chars-long-please

# Levantar
docker-compose up --build
```

**Espera a ver:**
```
[✓] SQL Server started
[✓] Backend running on http://localhost:5000
[✓] API docs on http://localhost:5000/scalar
```

## Paso 2: Levantar Frontend (en otra terminal)

```bash
cd ~/Documents/Proyectos/at-prueba-tecnica-frontend

# Build
docker build -t at-prueba-tecnica-frontend .

# Run
docker run -p 5173:80 \
  -e VITE_API_URL=http://localhost:5000 \
  at-prueba-tecnica-frontend
```

## Acceder

- **Frontend**: http://localhost:5173
- **API Documentation**: http://localhost:5000/scalar
- **Login**: admin@retopedidos.com / Admin123!

## Detener

```bash
# Terminal 1 (Backend)
Ctrl+C

# Terminal 2 (Frontend)
Ctrl+C
```

## Limpiar contenedores

```bash
docker-compose down -v  # Elimina volúmenes también
```

## Troubleshooting

### "Port 5000 already in use"
```bash
docker ps  # Ver contenedores activos
docker stop container_name
```

### "Cannot connect to database"
```bash
# Esperar 30 segundos más (SQL Server tarda en iniciar)
# Ver logs:
docker-compose logs sqlserver
```

### Frontend no puede conectar a Backend
Asegúrate que VITE_API_URL apunta a http://localhost:5000
