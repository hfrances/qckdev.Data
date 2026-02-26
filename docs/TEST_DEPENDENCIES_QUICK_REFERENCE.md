# Referencia Rápida: Actualización de Dependencias de Tests

## Resumen Ejecutivo

Actualiza los tests unitarios para ejecutarse en múltiples frameworks (.NET Core 3.1, .NET 5.0, .NET 6.0, .NET 8.0, .NET 10.0) incluso cuando la librería solo soporta `netstandard2.0`.

## Checklist de Cambios

### ✅ Paso 1: Identificar Frameworks Actuales

```powershell
# En el archivo .Test.csproj actual, anota todos los frameworks existentes
# NO los elimines a menos que se indique explícitamente
```

**Importante**: Lee bien el archivo actual. Si tiene `net461`, `net7.0`, `net9.0`, etc., debes incluirlos en los cambios.

### ✅ Paso 2: Cambiar a TargetFrameworks (Plural)

```xml
<!-- ANTES -->
<TargetFramework>net6.0</TargetFramework>

<!-- DESPUÉS - MANTÉN FRAMEWORKS EXISTENTES -->
<TargetFrameworks>netcoreapp3.1;net5.0;net6.0;net8.0;net10.0;net461;net7.0;net9.0</TargetFrameworks>
<!-- ↑ ejemplo con frameworks existentes preservados -->
```

### ✅ Paso 3: Actualizar Testing Framework

**Reemplaza TODA la sección de testing con esto** (ajustando condiciones según tus frameworks):

```xml
<!-- Para frameworks modernos (.NET Core / .NET 5+) -->
<ItemGroup Condition="'$(TargetFramework)'=='netcoreapp3.1' or '$(TargetFramework)'=='net5.0' or '$(TargetFramework)'=='net6.0' or '$(TargetFramework)'=='net8.0' or '$(TargetFramework)'=='net10.0' or '$(TargetFramework)'=='net7.0' or '$(TargetFramework)'=='net9.0'">
  <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.11.1" />
  <PackageReference Include="MSTest.TestAdapter" Version="3.2.2" />
  <PackageReference Include="MSTest.TestFramework" Version="3.2.2" />
  <PackageReference Include="coverlet.msbuild" Version="6.0.0">
    <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    <PrivateAssets>all</PrivateAssets>
  </PackageReference>
  <PackageReference Include="coverlet.collector" Version="6.0.0" />
</ItemGroup>

<!-- Para net461 - versiones específicas que funcionan con .NET Framework -->
<ItemGroup Condition="'$(TargetFramework)'=='net461'">
  <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.11.0" />
  <PackageReference Include="MSTest.TestAdapter" Version="2.2.10" />
  <PackageReference Include="MSTest.TestFramework" Version="2.2.10" />
  <PackageReference Include="coverlet.msbuild" Version="3.1.2">
    <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    <PrivateAssets>all</PrivateAssets>
  </PackageReference>
  <PackageReference Include="coverlet.collector" Version="1.2.0" />
  <Reference Include="Microsoft.CSharp" />
</ItemGroup>
```

**⚠️ IMPORTANTE**: No cambies las versiones para net461. Son específicas para .NET Framework y son las únicas que funcionan.

### ✅ Paso 4: Coverlet (Sin Condicionar)

```xml
<ItemGroup>
  <PackageReference Include="coverlet.msbuild" Version="6.0.0">
    <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    <PrivateAssets>all</PrivateAssets>
  </PackageReference>
  <PackageReference Include="coverlet.collector" Version="6.0.0" />
</ItemGroup>
```

### ✅ Paso 5: Condicionar Dependencias por Framework

**Patrón general**:
```xml
<ItemGroup Condition="'$(TargetFramework)'=='netcoreapp3.1' or '$(TargetFramework)'=='net5.0'">
  <!-- Versión antigua -->
</ItemGroup>

<ItemGroup Condition="'$(TargetFramework)'=='net6.0' or '$(TargetFramework)'=='net7.0' or '$(TargetFramework)'=='net8.0'">
  <!-- Versión media -->
</ItemGroup>

<ItemGroup Condition="'$(TargetFramework)'=='net9.0' or '$(TargetFramework)'=='net10.0'">
  <!-- Versión nueva -->
</ItemGroup>

<ItemGroup Condition="'$(TargetFramework)'=='net461'">
  <!-- Versión legacy -->
</ItemGroup>
```

## Tabla de Versiones de Referencia

### Testing (Versiones diferentes por tipo de framework)
```
Para .NET Core / .NET 5+ (netcoreapp3.1, net5.0, net6.0, net7.0, net8.0, net9.0, net10.0):
  Microsoft.NET.Test.Sdk: 17.11.1
  MSTest.TestAdapter: 3.2.2
  MSTest.TestFramework: 3.2.2
  coverlet.msbuild: 6.0.0
  coverlet.collector: 6.0.0

Para .NET Framework (net461):
  Microsoft.NET.Test.Sdk: 17.11.0
  MSTest.TestAdapter: 2.2.10
  MSTest.TestFramework: 2.2.10
  coverlet.msbuild: 3.1.2
  coverlet.collector: 1.2.0
  
⚠️ NO CAMBIES las versiones de net461 - son las únicas que funcionan
```

### Testing Framework por Target Framework

| Framework | Microsoft.NET.Test.Sdk | MSTest.TestAdapter | MSTest.TestFramework | coverlet.msbuild | coverlet.collector | Notas |
|-----------|---|---|---|---|---|---|
| netcoreapp3.1 | 17.11.1 | 3.2.2 | 3.2.2 | 6.0.0 | 6.0.0 | .NET Core 3.1 |
| net5.0 | 17.11.1 | 3.2.2 | 3.2.2 | 6.0.0 | 6.0.0 | .NET 5.0 |
| net6.0 | 17.11.1 | 3.2.2 | 3.2.2 | 6.0.0 | 6.0.0 | .NET 6.0 |
| net7.0 | 17.11.1 | 3.2.2 | 3.2.2 | 6.0.0 | 6.0.0 | .NET 7.0 |
| net8.0 | 17.11.1 | 3.2.2 | 3.2.2 | 6.0.0 | 6.0.0 | .NET 8.0 |
| net9.0 | 17.11.1 | 3.2.2 | 3.2.2 | 6.0.0 | 6.0.0 | .NET 9.0 |
| net10.0 | 17.11.1 | 3.2.2 | 3.2.2 | 6.0.0 | 6.0.0 | .NET 10.0 |
| **net461** | **17.11.0** | **2.2.10** | **2.2.10** | **3.1.2** | **1.2.0** | **.NET Framework 4.6.1 - NO CAMBIAR** |

| Paquete | netcoreapp3.1 | net5.0 | net6.0 | net7.0 | net8.0 | net9.0 | net10.0 | net461 |
|---------|---|---|---|---|---|---|---|---|
| Microsoft.Extensions.Configuration | 3.1.32 | 3.1.32 | 8.0.0 | 8.0.0 | 8.0.0 | 9.0.0 | 10.0.0 | 2.1.1 |
| Microsoft.Extensions.Configuration.Binder | - | - | 8.0.2 | 8.0.2 | 8.0.2 | 9.0.0 | 10.0.0 | - |
| Microsoft.Extensions.Configuration.EnvironmentVariables | 3.1.32 | 3.1.32 | 8.0.0 | 8.0.0 | 8.0.0 | 9.0.0 | 10.0.0 | 2.1.1 |
| Microsoft.Extensions.Configuration.Json | 3.1.32 | 3.1.32 | 8.0.1 | 8.0.1 | 8.0.1 | 9.0.0 | 10.0.0 | 2.1.1 |

### Entity Framework Core

| Paquete | netcoreapp3.1 | net5.0 | net6.0 | net7.0 | net8.0 | net9.0 | net10.0 | net461 |
|---------|---|---|---|---|---|---|---|---|
| Microsoft.EntityFrameworkCore | 3.1.32 | 3.1.32 | 6.0.36 | 6.0.36 | 8.0.11 | 9.0.0 | 10.0.0 | 2.1.14 |
| Microsoft.EntityFrameworkCore.SqlServer | 3.1.32 | 3.1.32 | 6.0.36 | 6.0.36 | 8.0.11 | 9.0.0 | 10.0.0 | 2.1.14 |
| Microsoft.EntityFrameworkCore.Sqlite | 3.1.32 | 3.1.32 | 6.0.36 | 6.0.36 | 8.0.11 | 9.0.0 | 10.0.0 | - |

### Data Access

| Paquete | netcoreapp3.1 | net5.0 | net6.0 | net7.0 | net8.0 | net9.0 | net10.0 | net461 |
|---------|---|---|---|---|---|---|---|---|
| Microsoft.Data.Sqlite | 3.1.32 | 3.1.32 | 6.0.36 | 6.0.36 | 8.0.11 | 9.0.0 | 10.0.0 | - |
| Microsoft.Data.SqlClient | 5.2.2 | 5.2.2 | 5.2.2 | 5.2.2 | 5.2.2 | 5.2.2 | 5.2.2 | 4.0.6 |

## Comandos de Prueba

```powershell
# Compilar para todos los frameworks
dotnet build TestProject.csproj

# Ejecutar tests para todos los frameworks
dotnet test TestProject.csproj

# Ejecutar tests de un framework específico
dotnet test TestProject.csproj -f net10.0
dotnet test TestProject.csproj -f net8.0
dotnet test TestProject.csproj -f netcoreapp3.1

# Ver qué frameworks se compilaron
dotnet test TestProject.csproj --list-tests
```

## Errores Comunes y Soluciones

### Error: "Framework X no compatible con paquete Y"

**Causa**: Versión de paquete incorrecta para el framework.

**Solución**: Verifica la tabla de versiones y ajusta la condición.

```xml
<!-- ❌ Incorrecto: net461 no soporta Entity Framework Core 8.x -->
<ItemGroup Condition="'$(TargetFramework)'=='net8.0' or '$(TargetFramework)'=='net461'">
  <PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.11" />
</ItemGroup>

<!-- ✅ Correcto: Versiones separadas -->
<ItemGroup Condition="'$(TargetFramework)'=='net8.0'">
  <PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.11" />
</ItemGroup>

<ItemGroup Condition="'$(TargetFramework)'=='net461'">
  <PackageReference Include="Microsoft.EntityFrameworkCore" Version="2.1.14" />
</ItemGroup>
```

### Error: "¿Por qué eliminaste net461?"

**Respuesta**: No debería haberse eliminado. Revisa el archivo original y restaura todos los frameworks que tenía.

**Regla de Oro**: 
- ✅ AGREGAR nuevos frameworks
- ✅ ACTUALIZAR versiones de dependencias
- ❌ NUNCA ELIMINAR frameworks existentes sin autorización explícita

## Archivos de Referencia

- `qckdev.Data/docs/TEST_DEPENDENCIES_UPDATE.md` - Guía detallada
- `qckdev.Data/qckdev.DataTest/qckdev.DataTest.csproj` - Ejemplo con múltiples frameworks
- `qckdev.Data.Dapper/qckdev.Data.Dapper.Test/qckdev.Data.Dapper.Test.csproj` - Otro ejemplo
