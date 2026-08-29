# evaluacion20262

Aplicación ASP.NET Core MVC (.NET 10) para el registro y consulta de solicitudes de servicio.

## Ejecución local

```bash
dotnet build
dotnet run            # http://localhost:5011 / https://localhost:7065 (ver launchSettings.json)
```

La base de datos SQLite (`tecnogas.db`) se crea y migra automáticamente al iniciar la aplicación (`db.Database.Migrate()` en `Program.cs`), no hace falta ejecutar `dotnet ef` manualmente. Rutas principales:

- `/SolicitudServicio` — tabla con todas las solicitudes
- `/SolicitudServicio/Create` — formulario de registro

## Despliegue en Render

La app se despliega como contenedor Docker a partir del `Dockerfile` de la raíz.

### Puerto

Render asigna una variable de entorno `PORT` (por defecto `10000`). `Program.cs` vincula Kestrel a `http://0.0.0.0:$PORT` (fallback `10000`) en contenedores, escuchando en todas las interfaces; Render detecta el puerto automáticamente.

### Configuración en el Dashboard

1. **New Web Service** → selecciona el repositorio y su rama (`develop`).
2. **Language/Runtime**: `Docker` (el `Dockerfile` está en la raíz, no hace falta indicar ruta).
3. Elegir plan de cómputo y crear el servicio.

### Variables de entorno requeridas

| Variable | Valor | Motivo |
| --- | --- | --- |
| `ConnectionStrings__DefaultConnection` | `Data Source=/var/render/tecnogas.db` | (Recomendada) Dirigir la BD al disco persistente. |

El doble guion bajo (`__`) equivale a `:` en la configuración de .NET, por lo que esta variable sobreescribe la cadena de conexión de `appsettings.json`. Sin esta variable, el contenedor escribe la BD en un directorio escribible (`/var/render` si el disco está montado, o `/tmp`), de modo que la app arranca igual, aunque los datos se pierden en cada *redeploy*.

### Disco persistente (obligatorio para conservar datos)

Como la app usa una base de datos SQLite en archivo:

1. Crear un **Persistent Disk** desde *Advanced* del servicio.
2. Montarlo en `/var/render/`.
3. Conectar la cadena de conexión del punto anterior.

Sin disco, los datos se pierden en cada *redeploy*/reinicio (en el plan gratis el service se duerme).

### Notas

- Render termina el TLS en su balanceador y reenvía por HTTP; el warning de `UseHttpsRedirection` ("Failed to determine the https port") es esperado y no afecta el funcionamiento.
- Las migraciones de EF Core (`dotnet ef migrations add InitialCreate`) ya existen en `Migrations/`; el contenedor las aplica automáticamente al arrancar, incluida la migración inicial sobre una BD vacía.