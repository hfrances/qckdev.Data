# Documentation

## Layered Model

- **Capa 1 (base):** checklist minima para cambios seguros.
  - [Test Dependencies Quick Reference](TEST_DEPENDENCIES_QUICK_REFERENCE.md)
- **Capa 2 (Capa 1 + detalle):** reglas completas de mantenimiento del `.Test.csproj`.
  - [Test Dependencies Update](TEST_DEPENDENCIES_UPDATE.md)
- **Capa 3 (Capa 1 + 2 + contexto legacy):** comportamiento especial de build por `AssemblyName` en `net35`.
  - [Legacy AssemblyName Build](LEGACY_ASSEMBLYNAME_BUILD.md)

## Notes

- `qckdev.DataTest` usa doble stack de testing (moderno + `net461`).
- Este repositorio no usa `Test.Common`.
