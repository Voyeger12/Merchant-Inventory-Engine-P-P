# Merchant Inventory Engine

> Desktop-Anwendung zur Verwaltung und Berechnung eines dynamischen Händler-Inventars für Pen-and-Paper-/Fantasy-Setups.

![Status](https://img.shields.io/badge/status-v1.0-success)
![Platform](https://img.shields.io/badge/platform-Windows-informational)
![Framework](https://img.shields.io/badge/.NET-WinForms-purple)
![Database](https://img.shields.io/badge/database-SQLite-003B57)
![Tests](https://img.shields.io/badge/tests-MSTest-success)
![License](https://img.shields.io/badge/license-MIT-green)

---

## Inhaltsverzeichnis

- [Projektüberblick](#projektüberblick)
- [Projektkontext (Umschulung)](#projektkontext-umschulung)
- [Release und Download](#release-und-download)
- [Ziele und Nicht-Ziele](#ziele-und-nicht-ziele)
- [Funktionsumfang (v1.0)](#funktionsumfang-v10)
- [Technologie-Stack](#technologie-stack)
- [Systemarchitektur](#systemarchitektur)
  - [Schichtenmodell](#schichtenmodell)
  - [Start- und Laufzeitfluss](#start--und-laufzeitfluss)
  - [Verzahnung der Komponenten](#verzahnung-der-komponenten)
- [Datenbankdesign](#datenbankdesign)
  - [Schema](#schema)
  - [Migrationen](#migrationen)
  - [Normalisierung](#normalisierung)
  - [Integrität und Konsistenz](#integrität-und-konsistenz)
- [UI/UX-Konzept](#uiux-konzept)
- [Startmodi (Endnutzer vs. Entwicklung)](#startmodi-endnutzer-vs-entwicklung)
- [Build, Test, Publish](#build-test-publish)
- [Logging, Diagnose und Fehlerbehandlung](#logging-diagnose-und-fehlerbehandlung)
- [Qualitätssicherung](#qualitätssicherung)
- [Known Limitations](#known-limitations)
- [Roadmap](#roadmap)
- [Lizenz](#lizenz)

---

## Projektüberblick

Die `Merchant Inventory Engine` ist eine lokale Desktop-Anwendung auf Basis von `Windows Forms`, die Item-Stammdaten in einer `SQLite`-Datei verwaltet und auf Basis mehrerer Multiplikatoren dynamische Endpreise berechnet.

Die Anwendung ist auf **transparente Preislogik**, **hohe Bedienbarkeit** und **stabile lokale Laufzeit** ausgelegt.

---

## Projektkontext (Umschulung)

Dieses Repository wurde im Rahmen einer **Umschulung zum Fachinformatiker für Anwendungsentwicklung (FIAE)** erstellt.

Der Fokus lag auf:

- strukturierter Softwarearchitektur,
- Datenmodellierung und Migrationen,
- sauberer Fehlerbehandlung,
- Testbarkeit,
- sowie praxisnaher UI-Entwicklung.

---

## Release und Download

- Aktueller Release: `v1.0`
- Download (GitHub Releases):
  - <https://github.com/Voyeger12/Merchant-Inventory-Engine-P-P/releases>
  - Direkt zum Tag `v1.0`: <https://github.com/Voyeger12/Merchant-Inventory-Engine-P-P/releases/tag/v1.0>

---

## Ziele und Nicht-Ziele

### Ziele

- Stabile lokale Desktop-Anwendung ohne Serverabhängigkeit
- Nachvollziehbare Preisbildung
- Einfache Erweiterbarkeit (z. B. weitere Modifier-Typen)
- Benutzerfreundliche Darstellung und Filterung der Inventardaten
- Klare Trennung zwischen Endnutzer-Start und Entwickler-Workflow

### Nicht-Ziele (v1.0)

- Mehrbenutzer-/Netzwerkbetrieb
- Rollen-/Rechtesystem
- Vollständiges Event-/Makrosystem mit zeitabhängigen Weltzuständen
- Cloud-Synchronisierung

---

## Funktionsumfang (v1.0)

- Persistente Datenhaltung über lokale `SQLite`-Datenbank
- Dynamische Preisberechnung auf Basis von:
  - Händlerpersönlichkeit
  - Region
  - politischer Lage
  - Fraktions-/Reputationsstatus (`Allied`, `Friendly`, `Neutral`, `Hostile`, `Enemy`)
- Preis-Erklärmodus pro Item (transparente Formel)
- Inventaransicht in `DataGridView`
- Filterung nach Name, Kategorie, Min/Max-Preis
- Export als `CSV`/`TXT` (inkl. robustem CSV-Escaping)
- Startup-Smoke-Checks (DB erreichbar, Integrität OK)
- Diagnosefunktionen über Menü:
  - `Tools > Logs öffnen`
  - `Tools > Diagnose`
- Responsive, DPI-aware UI mit dynamischer Typografie

---

## Technologie-Stack

- `C#`
- `.NET` (`Windows Forms`, `net10.0-windows`)
- `SQLite` (`Microsoft.Data.Sqlite`)
- `MSTest`

---

## Systemarchitektur

### Schichtenmodell

- **View** (`Form1`, `SplashForm`)
  - UI-Darstellung
  - User-Interaktionen
  - Responsive Layout/Typografie
- **Controller** (`MerchantController`)
  - Use-Case-Logik
  - Preisberechnung orchestrieren
  - Filterung und Export
- **Data** (`DatabaseHelper`)
  - SQLite Zugriff
  - Schema-Migrationen
  - Seed-Daten
  - Integritätschecks
- **Services** (`PriceCalculator`, `AppLogger`)
  - reine Formelberechnung
  - Logging
- **Startup-Orchestrierung** (`Program.cs`)
  - Splash + Startstatus
  - Smoke-Checks
  - Fehlerbehandlung vor UI-Start

### Start- und Laufzeitfluss

1. `Program.Main()` registriert globale Exception-Handler.
2. `SplashForm` wird angezeigt.
3. `DatabaseHelper` initialisiert DB (`SchemaVersion`, Migrationen, Seed).
4. `IsDatabaseHealthy()` prüft Integrität.
5. `Form1` startet mit bereits geprüftem `DatabaseHelper`.
6. `Form1` lädt Modifier und initialisiert UI.

### Verzahnung der Komponenten

- `Form1` ruft ausschließlich den `MerchantController` für Fachlogik auf.
- `MerchantController` verwendet `DatabaseHelper` (Daten) + `PriceCalculator` (Formel).
- `AppLogger` wird von Startup, Data, Controller und UI genutzt.
- `Program.cs` trennt klar technische Startprüfung von Nutzeroberfläche.

---

## Datenbankdesign

### Schema

- `Categories(Id, Name)`
- `Items(Id, Name, BasePrice, CategoryId)`
- `Modifiers(Id, Name, Multiplier, Type)`
- `SchemaVersion(Id, Version)`

### Migrationen

- `v1`: Tabellenstruktur
- `v2`: Unique-Indizes
- `v3`: Fraktions-Modifier (`Faction`)

### Normalisierung

- `Categories` und `Items` sind sauber über FK getrennt.
- `Items` enthält nur kategoriefremde Attribute (`Name`, `BasePrice`, `CategoryId`).
- `Modifiers` nutzt derzeit `Type` als Klassifikationsfeld (pragmatisch, erweiterbar).

**Einordnung:**

- Für den Projektumfang solide normalisiert (mind. 1NF/2NF, praktisch 3NF-nahe Struktur).
- Optionaler weiterer Schritt: `ModifierType` als Referenztabelle auslagern.

### Integrität und Konsistenz

- `PRAGMA foreign_keys = ON`
- Integritätsprüfung:
  - `PRAGMA integrity_check`
  - `PRAGMA foreign_key_check`
- Eindeutige Indizes:
  - `UX_Categories_Name`
  - `UX_Items_Name`
  - `UX_Modifiers_Name_Type`

---

## UI/UX-Konzept

- Mittelalterliches Farbschema und Typografie
- Menüstruktur für produktive Nutzung und Diagnose
- Preis-Erklärmodus ein-/ausblendbar
- Responsive Layoutlogik (`UpdateResponsiveLayout`) statt starrem Pixel-Layout
- DPI-aware Autoscaling (`AutoScaleMode = Dpi`)
- Mindestgrenzen zur Vermeidung von Überlappungen bei kleinen Fenstern

---

## Startmodi (Endnutzer vs. Entwicklung)

### Endnutzer

- Datei: `start.bat`
- Verhalten:
  - schneller App-Start
  - kein Unit-Testlauf
  - professionelle Endnutzer-Experience

### Entwicklung/Debug

- Datei: `start-dev.bat`
- Verhalten:
  - führt zuerst `dotnet test` aus
  - startet nur bei erfolgreichem Testlauf
  - ideal für lokale Qualitätssicherung

---

## Build, Test, Publish

### Build

```powershell
dotnet build
```

### Tests

```powershell
dotnet test
```

### Direktstart

```powershell
dotnet run --project MerchantInventoryEngine/MerchantInventoryEngine.csproj
```

### EXE (Single File, self-contained)

```powershell
dotnet publish MerchantInventoryEngine/MerchantInventoryEngine.csproj -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
```

Ausgabe:

`MerchantInventoryEngine/bin/Release/net10.0-windows/win-x64/publish/`

---

## Logging, Diagnose und Fehlerbehandlung

- Logdatei: `logs/app.log`
- Zentrale Logger-Klasse: `AppLogger`
- Globale Exception-Handler in `Program.cs`
- Startup-Fehler: technisch ins Log, nutzerfreundliche Meldung im Dialog
- Laufzeitdiagnose über `Tools > Diagnose`:
  - DB-Health
  - Fenstergröße
  - DPI
  - Skalierungsfaktor
  - Logpfad

---

## Qualitätssicherung

- Testprojekt: `MerchantInventoryEngine.Tests`
- Aktueller Stand: `12/12` Tests erfolgreich
- Abgedeckte Bereiche:
  - Preisberechnung
  - Multiplier-Grenzwerte
  - Fraktions-Einfluss
  - Export & CSV-Escaping
  - DB-Integrität

---

## Known Limitations

- Lokal/Single-User ausgelegt
- Kein serverseitiges Event-/History-System
- Keine rollenbasierte Benutzerverwaltung
- Kein Installer in v1.0 (Release-Download + Publish-Ordner)

---

## Roadmap

- Installer/Deployment-Workflow
- Erweiterte Balancing-Konfiguration (UI-Settings)
- Optionales Event-/Zeit-Simulationssystem
- Weitere UX-Verbesserungen und Accessibility-Optimierungen

---

## Lizenz

Dieses Projekt steht unter der `MIT License`.
Details: siehe Datei `LICENSE`.
