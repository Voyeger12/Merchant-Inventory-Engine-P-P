# Merchant Inventory Engine

> Desktop-Anwendung zur Verwaltung und Berechnung eines dynamischen Händler-Inventars für Pen-and-Paper-/Fantasy-Setups.

## Projektstatus

- Status: `Prototyp / Schulprojekt`
- Technologie-Stack: `C#`, `.NET (Windows Forms)`, `SQLite`, `MSTest`

## Hinweis zum Projektkontext

Dieses Repository wurde im Rahmen einer **Umschulung zum Fachinformatiker für Anwendungsentwicklung (FIAE)** erstellt.

## Features

- Persistente Datenhaltung mit lokaler `SQLite`-Datenbank
- MVC-nahe Struktur (`Model`, `Controller`, `View`)
- Dynamische Preisberechnung über gewichtete Multiplikatoren
  - Händlerpersönlichkeit
  - Standortfaktoren
  - politische Rahmenbedingungen
- Inventar-Darstellung in `DataGridView`
- Filter/Suche nach Name, Kategorie und Preisbereich
- Export als `CSV`/`TXT` (inkl. robustem CSV-Escaping)
- Logging (`logs/app.log`)
- Datenbank-Health-Check (`PRAGMA integrity_check`, `foreign_key_check`)
- DB-Versionierung/Migration über `SchemaVersion`
- Unit-Tests für Kernlogik und Export

## Architektur

- **Model**: Entitäten (`Item`, `Category`, `Modifier`, `InventoryItem`)
- **Controller**: Berechnungslogik, Filterung, Export
- **View**: Windows-Forms-Oberfläche (`Form1`)
- **Data**: SQLite-Zugriff und Initialisierung (`DatabaseHelper`)
- **Services**: Preisberechnung und Logging

## Datenbankstruktur

Tabellen:

- `Categories(Id, Name)`
- `Items(Id, Name, BasePrice, CategoryId)`
- `Modifiers(Id, Name, Multiplier, Type)`
- `SchemaVersion(Id, Version)`

Relevante Relationen/Regeln:

- `Items.CategoryId` → `Categories.Id` (Foreign Key)
- Aktivierte Fremdschlüsselprüfung (`PRAGMA foreign_keys = ON`)
- Eindeutige Indizes für Stammdatenkonsistenz

## Projekt lokal starten

### Voraussetzungen

- `.NET SDK` (passend zum Projekt-Target)
- Windows (für `WinForms`)

### Build & Test

```powershell
dotnet build
dotnet test
```

### App starten

```powershell
dotnet run --project MerchantInventoryEngine/MerchantInventoryEngine.csproj
```

Alternativ über die bereitgestellte `start.bat`:

```powershell
.\start.bat
```

## Qualitätssicherung

- Unit-Tests (`MerchantInventoryEngine.Tests`)
- Testfälle u. a. für:
  - mathematische Preisberechnung
  - CSV-Export inkl. Sonderzeichen
  - Multiplikator-Grenzwerte
  - DB-Integrität / korruptionsnahe Zustände

## Geplante Erweiterungen

- Konfigurationsdialog (Exportoptionen, Log-Level)
- Optionaler Installer/Deployment-Workflow
- Weiterer UI-Feinschliff und Usability-Verbesserungen

## Lizenz

Dieses Projekt steht unter der `MIT License`. Siehe Datei `LICENSE`.
