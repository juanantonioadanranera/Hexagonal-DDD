# Vehicle Rental - Hexagonal DDD

Aplicación de escritorio para la gestión del alquiler de vehículos, desarrollada como prueba técnica utilizando **.NET Framework 4.8**, **WPF**, **Domain-Driven Design (DDD)** y **Arquitectura Hexagonal**.

La aplicación permite:

- Dar de alta vehículos de la flota.
- Dar de alta clientes.
- Consultar los vehículos disponibles.
- Alquilar un vehículo.
- Devolver un vehículo.
- Consultar los vehículos actualmente alquilados.

## Reglas de negocio

La aplicación implementa, entre otras, las siguientes reglas:

- Un cliente no puede tener más de un vehículo alquilado simultáneamente.
- Un vehículo ya alquilado no puede volver a alquilarse hasta que sea devuelto.
- No se pueden incorporar a la flota vehículos con más de cinco años de antigüedad.
## Arquitectura


La solución está organizada siguiendo los principios de **Arquitectura Hexagonal (Ports and Adapters)** y **Domain-Driven Design (DDD)**.

Las responsabilidades se distribuyen entre los siguientes proyectos:

- **HexagonalDDD.Domain**  
  Contiene el núcleo de dominio de la aplicación: agregados, reglas de negocio e interfaces de los repositorios. No depende de la infraestructura ni de la interfaz gráfica.

- **HexagonalDDD.Application**  
  Contiene los casos de uso de la aplicación y sus comandos. Orquesta las operaciones sobre el dominio utilizando las interfaces de repositorio definidas por éste.

- **HexagonalDDD.Infraestructure**  
  Implementa la persistencia mediante **Entity Framework 6** y **Oracle Managed Data Access**. El modelo de persistencia se ha generado utilizando **Database First** a partir del esquema Oracle.

- **HexagonalDDD.Presentation**  
  Aplicación de escritorio desarrollada con **WPF**. Configura la inyección de dependencias y proporciona la interfaz de usuario para las operaciones de alquiler.

- **HexagonalDDD.Unit.Test**  
  Pruebas unitarias del dominio y de los casos de uso aislados de la infraestructura.

- **HexagonalDDD.Functional.Test**  
  Pruebas funcionales de integración que verifican el flujo entre Application, Domain e Infrastructure sin utilizar el host WPF.

- **HexagonalDDD.Infraestructure.Test**  
  Pruebas de infraestructura contra la base de datos Oracle.
  
## Base de datos Oracle y Docker

La persistencia de la aplicación utiliza **Oracle Database Free** y **Entity Framework 6 Database First**.

Para facilitar la ejecución de la prueba sin necesidad de disponer de una instalación local de Oracle, la base de datos se proporciona mediante Docker.

### Requisitos

- Visual Studio 2022 (solución desarrollada y probada con Visual Studio 2022 17.14)
- Workload **Desarrollo de escritorio de .NET**
- .NET Framework 4.8 / .NET Framework 4.8 Targeting Pack
- Docker Desktop

### Arranque de la base de datos

Desde la raíz del repositorio ejecutar:

```powershell
docker compose up -d
```

La primera inicialización de Oracle puede tardar unos minutos. El estado del contenedor puede comprobarse mediante:

```powershell
docker ps
```

Cuando el contenedor aparezca como `healthy`, la base de datos estará disponible.

### Inicialización automática

El contenedor ejecuta automáticamente los scripts incluidos en:

```text
scripts/startup/
```

El script `001_initialize_renting.sql` es idempotente y se encarga de garantizar la existencia de:

- Usuario/esquema `RENTING`.
- Tabla `CUSTOMERS`.
- Tabla `VEHICLES`.
- Tabla `RENTALS`.
- Relaciones y restricciones de integridad.
- Columna `CREATED_AT` de `RENTALS`.

Al ser idempotente, el script puede ejecutarse tanto durante la creación inicial como en posteriores reinicios del contenedor sin recrear los objetos existentes.

La carpeta:

```text
scripts/setup/
```

conserva los scripts que representan la creación inicial y la evolución del esquema de base de datos.

En particular, `003_add_rental_created_at.sql` representa una modificación posterior del esquema, añadiendo la columna:

```sql
CREATED_AT TIMESTAMP DEFAULT SYSTIMESTAMP NOT NULL
```

Esta modificación se incorporó posteriormente al modelo **Entity Framework Database First** mediante la actualización del fichero EDMX.

### Configuración

El archivo `.env.example` contiene los valores de configuración de ejemplo utilizados por Docker.

Si se desea personalizar la configuración, puede copiarse como `.env`:

```powershell
Copy-Item .env.example .env
```

El fichero `.env` está excluido del repositorio mediante `.gitignore`.

## Compilación y ejecución

### 1. Iniciar Oracle

Con Docker Desktop en ejecución, desde la raíz del repositorio:

```powershell
docker compose up -d
```

Esperar hasta que el contenedor aparezca como `healthy`:

```powershell
docker ps
```

### 2. Abrir la solución

Abrir la solución en **Visual Studio 2022** y restaurar los paquetes NuGet si fuera necesario.

La solución utiliza **.NET Framework 4.8**.

### 3. Ejecutar la aplicación

Establecer **HexagonalDDD.Presentation** como proyecto de inicio y ejecutar la aplicación desde Visual Studio.

La aplicación WPF permite gestionar el ciclo completo de alquiler:

1. Crear clientes y vehículos.
2. Consultar los vehículos disponibles.
3. Alquilar un vehículo a un cliente.
4. Consultar los vehículos alquilados.
5. Devolver un vehículo.

Los datos se almacenan en Oracle y permanecen disponibles después de cerrar y volver a iniciar la aplicación.

## Pruebas automatizadas

La solución está diseñada para permitir la ejecución de pruebas en distintos niveles, separando las pruebas unitarias, funcionales y de infraestructura.

### Pruebas unitarias

Proyecto:

```text
HexagonalDDD.Unit.Test
```

Las pruebas unitarias verifican reglas de negocio y casos de uso de forma aislada, sin acceder a Oracle ni a la interfaz WPF.

Entre las reglas verificadas se encuentran:

- Validación de la antigüedad máxima permitida para un vehículo.
- Control del estado disponible/alquilado de los vehículos.
- Imposibilidad de alquilar simultáneamente más de un vehículo al mismo cliente.
- Comportamiento de los casos de uso utilizando repositorios aislados de la infraestructura.

### Pruebas funcionales

Proyecto:

```text
HexagonalDDD.Functional.Test
```

Las pruebas funcionales verifican la integración entre las capas **Application**, **Domain** e **Infrastructure**, excluyendo el host WPF.

Se incluye un flujo completo que:

1. Crea un cliente.
2. Crea un vehículo.
3. Realiza el alquiler.
4. Comprueba la persistencia y el estado resultante.

La prueba utiliza la base de datos Oracle y elimina los datos creados durante su ejecución.

### Pruebas de infraestructura

Proyecto:

```text
HexagonalDDD.Infraestructure.Test
```

Las pruebas de infraestructura verifican tanto la interacción real con Oracle como la recepción y validación de datos a nivel de aplicación, sin necesidad de recorrer todas las capas.

Se comprueba la persistencia y recuperación de vehículos mediante `OracleVehicleRepository`. Además, se incluye una prueba sobre `CreateVehicleHandler` que valida el rechazo de un vehículo con más de cinco años de antigüedad y comprueba que, cuando la validación falla, no se invoca al repositorio.

Los datos utilizados por estas pruebas se eliminan después de su ejecución para evitar dejar información residual en la base de datos.

### Ejecución de las pruebas

Antes de ejecutar las pruebas que utilizan infraestructura, el contenedor Oracle debe estar iniciado y en estado `healthy`:

```powershell
docker compose up -d
docker ps
```

Las pruebas pueden ejecutarse desde **Test Explorer** de Visual Studio 2022 mediante la opción **Run All Tests**.

### Credenciales de desarrollo local

Las credenciales incluidas en los archivos de configuración y en los scripts SQL se utilizan exclusivamente para el entorno local de desarrollo de esta prueba técnica.

El usuario de aplicación creado automáticamente es:

```text
Usuario: RENTING
Contraseña: Renting123
```
