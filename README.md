CRM Light – Guide d’installation
Présentation

CRM Light est une application desktop développée en C# WinForms permettant de gérer simplement :

les clients
les interactions
les opportunités
les relances et notifications

Elle est conçue pour les TPE, freelances et petites agences.

Prérequis

Avant toute installation :

Visual Studio 2022 ou plus récent
Workload : .NET Desktop Development
SQL Server Express ou LocalDB
SQL Server Management Studio (SSMS) (recommandé)
.NET Framework 4.8 (ou compatible)

Installation du projet
1. Extraire le projet
Décompresse CRMLight.zip
Ouvre le dossier
2. Ouvrir le projet
Lance Visual Studio
Clique sur Open a project
Sélectionne :
CRMLight.sln

Configuration de la base de données

Étape 1 : Créer la base

Dans SSMS :

CREATE DATABASE CRMLightDB;
GO
Étape 2 : Créer les tables (Schema)
Ouvre le fichier :
  Database/createSchema.sql
  Ajoute en haut si ce n’est pas déjà fait :
  USE CRMLightDB;
  GO
  Exécute tout le script

Cela va créer :
  Users
  Clients
  Interactions
  Opportunities
  Relances
  Notifications
  
Étape 3 : Insérer les données (Seed)
Ouvre :
Database/SeedDemo.sql
Exécute le script

Cela va insérer :
un utilisateur admin, éventuellement des données de démonstration

Lancement de l’application

  Dans Visual Studio :
    Appuie sur F5 ou clique sur Start
    
  L’écran de connexion s’affiche, entrez les identifiants par défaut
  
  Identifiant de connexion :
    Nom d’utilisateur : admin ,mot de passe : Admin123!

Profitez de l'application !
