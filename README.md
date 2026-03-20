# Merchant Inventory Engine

> Desktop-Anwendung zur Verwaltung und Berechnung eines dynamischen Händler-Inventars für Pen-and-Paper-/Fantasy-Setups.

![Status](https://img.shields.io/badge/status-prototype-blue)
![Platform](https://img.shields.io/badge/platform-Windows-informational)
![Framework](https://img.shields.io/badge/.NET-WinForms-purple)
![Database](https://img.shields.io/badge/database-SQLite-003B57)
![Tests](https://img.shields.io/badge/tests-MSTest-success)
![License](https://img.shields.io/badge/license-MIT-green)

---

## Inhaltsverzeichnis

- [Projektüberblick](#projektüberblick)
- [Projektkontext (Umschulung)](#projektkontext-umschulung)
- [Hauptfunktionen](#hauptfunktionen)
- [Technologie-Stack](#technologie-stack)
- [Architektur](#architektur)
- [Datenbankstruktur](#datenbankstruktur)
- [Schnellstart](#schnellstart)
- [Tests und Qualitätssicherung](#tests-und-qualitätssicherung)
- [Roadmap](#roadmap)
- [Lizenz](#lizenz)

---

## Projektüberblick

Die `Merchant Inventory Engine` ist eine lokale Desktop-Anwendung, die Item-Stammdaten in einer `SQLite`-Datei speichert, Preise dynamisch über Multiplikatoren berechnet und Ergebnisse übersichtlich in einer `DataGridView` darstellt.

## Projektkontext (Umschulung)

Dieses Repository wurde im Rahmen einer **Umschulung zum Fachinformatiker für Anwendungsentwicklung (FIAE)** erstellt.

## Hauptfunktionen

- Persistente Datenhaltung mit lokaler `SQLite`-Datenbank
- Struktur nach `MVC`-Prinzipien (`Model`, `Controller`, `View`)
- Dynamische Preisberechnung auf Basis von:
  - Händlerpersönlichkeit
  - Standortfaktoren
  - politischen Rahmenbedingungen
- Inventar-Darstellung in `DataGridView`
- Filter/Suche nach Name, Kategorie und Preisbereich
- Export als `CSV`/`TXT` mit robustem CSV-Escaping
- Logging in `logs/app.log`
- Datenbank-Health-Check (`PRAGMA integrity_check`, `foreign_key_check`)
- DB-Versionierung und Migration über `SchemaVersion`
- Unit-Tests für zentrale Geschäftslogik

## Technologie-Stack

- `C#`
- `.NET` (`Windows Forms`)
- `SQLite` (`Microsoft.Data.Sqlite`)
- `MSTest`

## Architektur

- **Model**: Entitäten (`Item`, `Category`, `Modifier`, `InventoryItem`)
- **Controller**: Berechnungslogik, Validierung, Filterung, Export
- **View**: Windows-Forms-Oberfläche (`Form1`)
- **Data**: SQLite-Zugriff, Migrationen, Seed (`DatabaseHelper`)
- **Services**: Preisberechnung und Logging

## Datenbankstruktur

### Tabellen

- `Categories(Id, Name)`
- `Items(Id, Name, BasePrice, CategoryId)`
- `Modifiers(Id, Name, Multiplier, Type)`
- `SchemaVersion(Id, Version)`

### Relationen und Regeln

- `Items.CategoryId` → `Categories.Id` (Foreign Key)
- Aktivierte Fremdschlüsselprüfung: `PRAGMA foreign_keys = ON`
- Integritätsprüfung via `integrity_check` und `foreign_key_check`
- Eindeutige Indizes für Stammdatenkonsistenz

## Schnellstart

### Voraussetzungen

- `Windows`
- `.NET SDK` (passend zum Target Framework)

### Build und Tests

```powershell
dotnet build
dotnet test
```

### Anwendung starten

```powershell
dotnet run --project MerchantInventoryEngine/MerchantInventoryEngine.csproj
```

Alternativ über den Launcher:

```powershell
.\start.bat
```

## Tests und Qualitätssicherung

- Unit-Tests im Projekt `MerchantInventoryEngine.Tests`
- Abgedeckte Bereiche u. a.:
  - mathematische Preisberechnung
  - CSV-Export inkl. Sonderzeichen
  - Multiplikator-Grenzwerte
  - DB-Integrität / korruptionsnahe Zustände

## Roadmap

- Konfigurationsdialog (Exportoptionen, Log-Level)
- Installer/Deployment-Workflow
- Weiterer UI-Feinschliff und Usability-Verbesserungen

## Lizenz

Dieses Projekt steht unter der `MIT License`.
Details: siehe Datei `LICENSE`.
