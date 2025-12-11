# Reglas de Proyecto - Cursor IDE

Este directorio contiene las reglas de proyecto para Cursor IDE, organizadas por tema.

## Estructura

Las reglas están organizadas en archivos `.mdc` (Markdown con frontmatter) según la [documentación oficial de Cursor](https://docs.cursor.com/en/context/rules):

- `anclas_canvaslayer.mdc` - **MÁXIMA PRIORIDAD**: Sistema de anclas con CanvasLayer y expand mode
- `principios_codigo.mdc` - Principios fundamentales: SOLID, KISS, SRP, DRY
- `convenciones_codigo.mdc` - Convenciones: nomenclatura, nombres de nodos, documentación
- `godot_ui_layout.mdc` - UI y Layout en Godot: CanvasLayer, contenedores, márgenes
- `godot_escenas_nodos.mdc` - Escenas y nodos en Godot
- `estructura_archivos.mdc` - Estructura de archivos del proyecto
- `patrones_comunes.mdc` - Patrones comunes de código
- `contexto_proyecto.mdc` - Contexto del proyecto (motor, lenguaje, resolución)

## Formato

Cada archivo `.mdc` sigue este formato:

```yaml
---
description: "Descripción de la regla"
globs: ["**/*.cs"]  # Patrones de archivos donde aplicar
alwaysApply: true   # Aplicar siempre o solo en contexto relevante
---

# Contenido Markdown con las reglas
```

## Migración desde .cursorrules

El archivo `.cursorrules` está en desuso. Las reglas ahora están en `.cursor/rules/` organizadas por tema para mejor mantenimiento y aplicación contextual.

## Prioridad

**MÁXIMA PRIORIDAD**: Las reglas sobre anclas (`anclas_canvaslayer.mdc`) son críticas cuando se trabaja con `CanvasLayer` y `expand` mode.

