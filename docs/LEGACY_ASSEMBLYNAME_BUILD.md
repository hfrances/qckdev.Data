# Build de Proyectos con `AssemblyName` Diferente en `net35`

## Contexto

`qckdev.Data` usa un nombre de ensamblado diferente cuando el target es `net35`:

- `net35` -> `qckdev.Data.2.dll`
- resto de frameworks -> `qckdev.Data.dll`

Esto se define en `qckdev.Data/qckdev.Data.csproj` mediante condiciones sobre `$(TargetFramework)`.

## Problema conocido

En este tipo de proyectos multi-target (especialmente con frameworks legacy), Visual Studio puede quedar en un estado inconsistente si no se ejecuta restore/build por CLI primero.

Referencia:
- https://github.com/dotnet/sdk/issues/22469#issuecomment-1732733899

## Flujo recomendado de compilacion

Ejecutar siempre desde linea de comando antes de abrir/compilar en Visual Studio:

```powershell
cd qckdev.Data
dotnet restore
dotnet build
```

Para release/pack:

```powershell
cd qckdev.Data
dotnet build --configuration Release
dotnet pack --configuration Release
```

## Verificacion rapida

Tras compilar, revisar que existan ambos outputs esperados:

- `bin/<Configuration>/net35/qckdev.Data.2.dll`
- `bin/<Configuration>/<otro-framework>/qckdev.Data.dll`

## Nota

El archivo `qckdev.Data/_Readme.md` contiene esta misma base historica. Este documento la formaliza dentro de `docs`.

