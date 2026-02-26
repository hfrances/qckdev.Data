# Actualización de Dependencias de Tests Unitarios para Multi-Framework

## Objetivo

Ejecutar las pruebas unitarias en múltiples frameworks de .NET, incluso cuando la librería principal solo soporta `netstandard2.0`. Esto permite verificar que no hay incompatibilidades entre frameworks.

## Motivación

- **Verificación de compatibilidad**: Aunque la librería se compile para `netstandard2.0`, ejecutar tests en diferentes runtimes (.NET Core 3.1, .NET 5.0, .NET 6.0, .NET 8.0, .NET 10.0, etc.) garantiza que funciona correctamente en cada uno.
- **Detección de problemas temprana**: Algunos problemas de compatibilidad solo se detectan en tiempo de ejecución, no en compilación.
- **Cobertura de versiones**: Permite verificar comportamiento en versiones antiguas de .NET sin comprometer el soporte de versiones nuevas.

## Estructura de Cambios

### 1. TargetFrameworks (Plural)

```xml
<!-- ANTES: Un único framework -->
<TargetFramework>net6.0</TargetFramework>

<!-- DESPUÉS: Múltiples frameworks -->
<TargetFrameworks>netcoreapp3.1;net5.0;net6.0;net8.0;net10.0</TargetFrameworks>
```

**Importante**: Al agregar frameworks, **RESPETA los que ya existen**. No elimines ningún framework a menos que se indique explícitamente. Si el proyecto tenía `net7.0` o `net9.0`, deben mantenerse.

### 2. Dependencias Condicionadas por Framework

Las dependencias de las librerías varían según el framework de ejecución. Por esto, usamos condiciones `Condition`:

```xml
<ItemGroup Condition="'$(TargetFramework)'=='netcoreapp3.1' or '$(TargetFramework)'=='net5.0'">
  <PackageReference Include="SomePackage" Version="3.1.32" />
</ItemGroup>

<ItemGroup Condition="'$(TargetFramework)'=='net6.0' or '$(TargetFramework)'=='net8.0'">
  <PackageReference Include="SomePackage" Version="8.0.0" />
</ItemGroup>

<ItemGroup Condition="'$(TargetFramework)'=='net10.0'">
  <PackageReference Include="SomePackage" Version="10.0.0" />
</ItemGroup>
```

### 3. Versiones de Testing Recomendadas

Para mantener consistencia en todos los proyectos, usa estas versiones:

```xml
<!-- Para frameworks modernos -->
<ItemGroup Condition="'$(TargetFramework)'=='netcoreapp3.1' or '$(TargetFramework)'=='net5.0' or '$(TargetFramework)'=='net6.0' or '$(TargetFramework)'=='net8.0' or '$(TargetFramework)'=='net10.0'">
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

**Importante**: net461 requiere versiones específicas anteriores. NO actualices estas versiones para net461 ya que no funcionarán.

### 4. Herramientas de Cobertura

Las herramientas de cobertura se aplican a todos los frameworks sin condicionar:

```xml
<ItemGroup>
  <PackageReference Include="coverlet.msbuild" Version="6.0.0">
    <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    <PrivateAssets>all</PrivateAssets>
  </PackageReference>
  <PackageReference Include="coverlet.collector" Version="6.0.0" />
</ItemGroup>
```

## Ejemplo Completo: qckdev.Data.Dapper.Test.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFrameworks>netcoreapp3.1;net5.0;net6.0;net8.0;net10.0</TargetFrameworks>
    <IsPackable>false</IsPackable>
  </PropertyGroup>

  <!-- Testing framework - igual para todos los frameworks -->
  <ItemGroup Condition="'$(TargetFramework)'=='netcoreapp3.1' or '$(TargetFramework)'=='net5.0' or '$(TargetFramework)'=='net6.0' or '$(TargetFramework)'=='net8.0' or '$(TargetFramework)'=='net10.0'">
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.11.1" />
    <PackageReference Include="MSTest.TestAdapter" Version="3.2.2" />
    <PackageReference Include="MSTest.TestFramework" Version="3.2.2" />
  </ItemGroup>

  <!-- Code coverage tools - sin condicionar -->
  <ItemGroup>
    <PackageReference Include="coverlet.msbuild" Version="6.0.0">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
    <PackageReference Include="coverlet.collector" Version="6.0.0" />
  </ItemGroup>

  <!-- Entity Framework Core - varía según framework -->
  <ItemGroup Condition="'$(TargetFramework)'=='netcoreapp3.1' or '$(TargetFramework)'=='net5.0'">
    <PackageReference Include="Microsoft.Data.Sqlite" Version="3.1.32" />
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="3.1.32" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="3.1.32" />
  </ItemGroup>

  <ItemGroup Condition="'$(TargetFramework)'=='net6.0'">
    <PackageReference Include="Microsoft.Data.Sqlite" Version="6.0.36" />
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="6.0.36" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="6.0.36" />
  </ItemGroup>

  <ItemGroup Condition="'$(TargetFramework)'=='net8.0'">
    <PackageReference Include="Microsoft.Data.Sqlite" Version="8.0.11" />
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.11" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="8.0.11" />
  </ItemGroup>

  <ItemGroup Condition="'$(TargetFramework)'=='net10.0'">
    <PackageReference Include="Microsoft.Data.Sqlite" Version="10.0.0" />
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="10.0.0" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="10.0.0" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\qckdev.Data.Dapper\qckdev.Data.Dapper.csproj" />
  </ItemGroup>

</Project>
```

## Versiones por Framework

### Testing Framework

```
Para netcoreapp3.1, net5.0, net6.0, net8.0, net10.0 (y net7.0, net9.0 si existen):
  Microsoft.NET.Test.Sdk: 17.11.1
  MSTest.TestAdapter: 3.2.2
  MSTest.TestFramework: 3.2.2
  coverlet.msbuild: 6.0.0
  coverlet.collector: 6.0.0

Para net461 (.NET Framework 4.6.1):
  Microsoft.NET.Test.Sdk: 17.11.0
  MSTest.TestAdapter: 2.2.10
  MSTest.TestFramework: 2.2.10
  coverlet.msbuild: 3.1.2
  coverlet.collector: 1.2.0
```

**IMPORTANTE**: No cambies las versiones de net461. Son las únicas que funcionan correctamente con .NET Framework 4.6.1.

### Microsoft.Extensions.Configuration

| Framework | Versión |
|-----------|---------|
| netcoreapp3.1, net5.0 | 3.1.32 |
| net6.0, net8.0 | 8.0.0 |
| net9.0 | 9.0.0 |
| net10.0 | 10.0.0 |
| net461 | 2.1.1 |

### Microsoft.EntityFrameworkCore.SqlServer

| Framework | Versión |
|-----------|---------|
| netcoreapp3.1, net5.0 | 3.1.32 |
| net6.0, net7.0 | 6.0.36 |
| net8.0 | 8.0.11 |
| net9.0 | 9.0.0 |
| net10.0 | 10.0.0 |
| net461 | 2.1.14 |

## Pasos para Aplicar en Otros Proyectos

1. **Cambiar TargetFramework a TargetFrameworks**:
   - Usa plural: `<TargetFrameworks>netcoreapp3.1;net5.0;net6.0;net8.0;net10.0</TargetFrameworks>`
   - **IMPORTANTE**: Si el proyecto ya tiene otros frameworks, AGREGA los que falten, no REEMPLACES.

2. **Actualizar versiones de testing**:
   - Usa Microsoft.NET.Test.Sdk 17.11.1
   - Usa MSTest.TestAdapter 3.2.2
   - Usa MSTest.TestFramework 3.2.2

3. **Condicionar dependencias por framework**:
   - Agrupa frameworks con la misma versión de dependencia
   - Usa `Condition="'$(TargetFramework)'=='framework1' or '$(TargetFramework)'=='framework2'"`

4. **Respetar frameworks existentes**:
   - Si el proyecto tenía net461, net7.0 o net9.0, mantenlos
   - No elimines frameworks sin autorización explícita

## Verificación

Después de los cambios, verifica:

```powershell
# Compilar para todos los frameworks
dotnet build qckdev.Data.csproj

# Ejecutar tests para todos los frameworks
dotnet test qckdev.DataTest.csproj

# Ver frameworks compilados
dotnet test qckdev.DataTest.csproj --list-tests
```

## Referencias

- [Official .NET Target Frameworks](https://learn.microsoft.com/en-us/dotnet/standard/frameworks)
- [MSTest Documentation](https://github.com/microsoft/testfx)
- [Entity Framework Core Version Compatibility](https://learn.microsoft.com/en-us/ef/core/what-is-new/)
