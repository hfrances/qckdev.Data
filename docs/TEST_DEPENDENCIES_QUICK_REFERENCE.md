# Referencia Rapida: Dependencias de Tests Multi-Framework

## Checklist Minimo

1. Mantener todos los `TargetFrameworks` existentes.
2. No eliminar frameworks sin autorizacion.
3. Condicionar paquetes solo cuando cambian versiones por framework.
4. Ejecutar `dotnet test` para todos los frameworks al final.

## Regla de Decision para Testing/Coverlet

### Caso A: mismas versiones para todos los frameworks

Usa un unico bloque sin `Condition`:

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

### Caso B: hay frameworks con versiones distintas (ej: `net461`)

Usa bloques con `Condition` por grupo de versiones.

## Versiones Recomendadas

Para `netcoreapp3.1` y `net5.0+`:
- `Microsoft.NET.Test.Sdk`: `17.11.1`
- `MSTest.TestAdapter`: `3.2.2`
- `MSTest.TestFramework`: `3.2.2`
- `coverlet.msbuild`: `6.0.0`
- `coverlet.collector`: `6.0.0`

Para `net461`:
- `Microsoft.NET.Test.Sdk`: `17.11.0`
- `MSTest.TestAdapter`: `2.2.10`
- `MSTest.TestFramework`: `2.2.10`
- `coverlet.msbuild`: `3.1.2`
- `coverlet.collector`: `1.2.0`

## Errores Comunes

1. Condicionar por costumbre aunque no haya diferencias de version.
2. Mezclar `net461` con versiones modernas de MSTest/coverlet.
3. Eliminar `net7.0`, `net9.0` o `net461` por accidente.

## Comandos de Verificacion

```powershell
dotnet build TestProject.csproj
dotnet test TestProject.csproj
dotnet test TestProject.csproj --list-tests
```

