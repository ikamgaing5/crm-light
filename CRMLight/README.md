# CRM Light

Mini CRM WinForms pour TPE, freelances et petites agences.

## Modules

- Authentification
- Clients
- Interactions
- Opportunités
- Relances
- Notifications

## Prérequis

- Visual Studio 2022
- .NET 8 SDK
- SQL Server / LocalDB

## Installation

1. Crée la base de données avec `Database/01_CreateSchema.sql`
2. (Optionnel) Exécute `Database/02_SeedDemo.sql`
3. Ouvre `CRMLight.sln`
4. Vérifie la chaîne de connexion dans `Data/Db.cs`
5. Lance l'application

## Identifiants par défaut

Au premier lancement, l'application crée automatiquement :

- **Utilisateur** : `admin`
- **Mot de passe** : `Admin123!`

## Points clés

- UI simple et professionnelle
- CRUD de base sur les clients
- Gestion des interactions
- Relances avec notifications automatiques
- Architecture simple à maintenir
