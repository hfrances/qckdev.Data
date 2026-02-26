# Actualizacion de Dependencias de Tests Unitarios para Multi-Framework

## Objetivo

Ejecutar pruebas unitarias en multiples frameworks de .NET, incluso cuando la libreria principal solo soporta `netstandard2.0`.

## Regla Principal (Decision Rapida)

Usa esta regla antes de tocar el `.Test.csproj`:

1. Si **todas** las versiones de `Microsoft.NET.Test.Sdk`, `MSTest.*` y `coverlet.*` son iguales para todos los frameworks del proyecto:
   - Usa `ItemGroup` **sin `Condition`**.
2. Si existe al menos un framework con versiones distintas (ej: `net461`):
   - Separa por `ItemGroup` **con `Condition`** para cada grupo de versiones.

Esto evita condiciones innecesarias y tambien evita mezclar frameworks incompatibles.

## Estructura de Cambios

### 1. TargetFrameworks (Plural)

```xml
<!-- ANTES -->
<TargetFramework>net6.0</TargetFramework>

<!-- DESPUES -->
<TargetFrameworks>netcoreapp3.1;net5.0;net6.0;net8.0;net10.0</TargetFrameworks>
```

Importante:
- Respeta siempre los frameworks existentes.
- Agrega frameworks nuevos solo si se solicita.
- No elimines frameworks sin autorizacion explicita.

### 2. Dependencias Condicionadas por Framework

Solo condiciona paquetes cuando cambie la version por framework:

```xml
<ItemGroup Condition="'$(TargetFramework)'=='netcoreapp3.1' or '$(TargetFramework)'=='net5.0'">
  <PackageReference Include="SomePackage" Version="3.1.32" />
</ItemGroup>

<ItemGroup Condition="'$(TargetFramework)'=='net6.0' or '$(TargetFramework)'=='net8.0'">
  <PackageReference Include="SomePackage" Version="8.0.0" />
</ItemGroup>
```

### 3. Versiones de Testing Recomendadas

Frameworks modernos (`netcoreapp3.1`, `net5.0+`):
- `Microsoft.NET.Test.Sdk`: `17.11.1`
- `MSTest.TestAdapter`: `3.2.2`
- `MSTest.TestFramework`: `3.2.2`
- `coverlet.msbuild`: `6.0.0`
- `coverlet.collector`: `6.0.0`

`net461` (si existe en el proyecto):
- `Microsoft.NET.Test.Sdk`: `17.11.0`
- `MSTest.TestAdapter`: `2.2.10`
- `MSTest.TestFramework`: `2.2.10`
- `coverlet.msbuild`: `3.1.2`
- `coverlet.collector`: `1.2.0`

Importante:
- No actualices versiones de `net461` sin una validacion especifica.
- Si el proyecto no tiene `net461`, no hace falta crear bloques legacy.

### 4. Patrones Validos para Testing/Coverlet

#### Patron A: sin condiciones (recomendado cuando no hay diferencias)

```xml
<ItemGroup>
  <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.11.1" />
  <PackageReference Include="MSTest.TestAdapter" Version="3.2.2" />
  <PackageReference Include="MSTest.TestFramework" Version="3.2.2" />
  <PackageReference Include="coverlet.msbuild" Version="6.0.0">
    <PrivateAssets>all</PrivateAssets>
    <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
  </PackageReference>
  <PackageReference Include="coverlet.collector" Version="6.0.0" />
</ItemGroup>
```

#### Patron B: con condiciones (cuando hay diferencias, ej. `net461`)

```xml
<ItemGroup Condition="'$(TargetFramework)'=='netcoreapp3.1' or '$(TargetFramework)'=='net5.0' or '$(TargetFramework)'=='net6.0' or '$(TargetFramework)'=='net8.0' or '$(TargetFramework)'=='net9.0' or '$(TargetFramework)'=='net10.0'">
  <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.11.1" />
  <PackageReference Include="MSTest.TestAdapter" Version="3.2.2" />
  <PackageReference Include="MSTest.TestFramework" Version="3.2.2" />
  <PackageReference Include="coverlet.msbuild" Version="6.0.0">
    <PrivateAssets>all</PrivateAssets>
    <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
  </PackageReference>
  <PackageReference Include="coverlet.collector" Version="6.0.0" />
</ItemGroup>

<ItemGroup Condition="'$(TargetFramework)'=='net461'">
  <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.11.0" />
  <PackageReference Include="MSTest.TestAdapter" Version="2.2.10" />
  <PackageReference Include="MSTest.TestFramework" Version="2.2.10" />
  <PackageReference Include="coverlet.msbuild" Version="3.1.2">
    <PrivateAssets>all</PrivateAssets>
    <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
  </PackageReference>
  <PackageReference Include="coverlet.collector" Version="1.2.0" />
  <Reference Include="Microsoft.CSharp" />
</ItemGroup>
```

## Pasos para Aplicar en Otros Proyectos

1. Identifica frameworks actuales y preservalos.
2. Define si el bloque de testing/coverlet sera condicionado o no (regla principal).
3. Actualiza dependencias de runtime (EF, Sqlite, etc.) por framework.
4. Ejecuta build/test en todos los frameworks.

## Verificacion

```powershell
dotnet build qckdev.Data.csproj
dotnet test qckdev.DataTest.csproj
dotnet test qckdev.DataTest.csproj --list-tests
```

## Regla de Oro

- Agregar frameworks o actualizar versiones: SI.
- Eliminar frameworks existentes sin autorizacion: NO.

