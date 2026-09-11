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

- Visual Studio 2022
- .NET Framework 4.8
- Docker Desktop

### Arranque de la base de datos

Desde la raíz del repositorio ejecutar:

```powershell
docker compose up -d

La primera inicialización de Oracle puede tardar unos minutos. El estado del contenedor puede comprobarse mediante:

docker ps

Cuando el contenedor aparezca como healthy, la base de datos estará disponible.

Inicialización automática

El contenedor ejecuta automáticamente los scripts incluidos en:

scripts/startup/

El script 001_initialize_renting.sql es idempotente y se encarga de garantizar la existencia de:

Usuario/esquema RENTING.
Tabla CUSTOMERS.
Tabla VEHICLES.
Tabla RENTALS.
Relaciones y restricciones de integridad.
Columna CREATED_AT de RENTALS.

Al ser idempotente, el script puede ejecutarse tanto durante la creación inicial como en posteriores reinicios del contenedor sin recrear los objetos existentes.

La carpeta:

scripts/setup/

conserva los scripts que representan la creación inicial y la evolución del esquema de base de datos.

En particular, 003_add_rental_created_at.sql representa una modificación posterior del esquema, añadiendo la columna:

CREATED_AT TIMESTAMP DEFAULT SYSTIMESTAMP NOT NULL

Esta modificación se incorporó posteriormente al modelo Entity Framework Database First mediante la actualización del fichero EDMX.

Configuración

El archivo .env.example contiene los valores de configuración de ejemplo utilizados por Docker.

Si se desea personalizar la configuración, puede copiarse como .env:

Copy-Item .env.example .env

El fichero .env está excluido del repositorio mediante .gitignore.