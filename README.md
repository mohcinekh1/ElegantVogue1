# ElegantVogue

Application web **e-commerce** de type vitrine / boutique mode, développée en **ASP.NET Core MVC** sur **.NET 8**. Elle propose catalogue produits, panier basé sur la session, passage de commande simplifié et un **espace d’administration** réservé aux utilisateurs avec le rôle **Admin**.

## Pile technique

| Élément | Détail |
|--------|--------|
| Runtime | .NET 8 |
| UI | ASP.NET Core MVC (Razor) |
| Données | Entity Framework Core 8 + **SQL Server** |
| Authentification | ASP.NET Core Identity (utilisateurs + rôles) |
| Packages principaux | `Microsoft.EntityFrameworkCore.SqlServer`, `Microsoft.AspNetCore.Identity.EntityFrameworkCore` |

## Fonctionnalités

- **Catalogue** : liste produits, fiche détail avec couleurs, tailles et stock (relations many-to-many `ProductColor` / `ProductSize`).
- **Panier** : articles liés à un identifiant de session (`CartId`).
- **Checkout** : validation d’une commande (sous-total, frais de port, total).
- **Comptes** : inscription, connexion, déconnexion (`AccountController`).
- **Admin** (`/Admin`, rôle `Admin`) : liste des produits, création, édition, suppression.
- **Initialisation** : au démarrage, `DbInitializer` applique les migrations (`Migrate()`), crée les rôles **Admin** / **Client**, injecte un compte administrateur et, si la base est vide, des données de démo (catégories, collections, couleurs, tailles, produits).

## Structure du dépôt

```
ElegantVogue1-master/
├── ElegantVogue.sln
└── ElegantVogue/
    ├── Controllers/     # Home, Products, Cart, Checkout, Account, Admin
    ├── Models/          # Entités + ViewModels
    ├── Data/            # ApplicationDbContext, DbInitializer
    ├── Migrations/      # Migrations EF Core
    ├── Views/           # Vues Razor
    └── wwwroot/         # Fichiers statiques (CSS, images produits, etc.)
```

## Prérequis

- [SDK .NET 8](https://dotnet.microsoft.com/download/dotnet/8.0)
- **SQL Server** (instance locale, Express ou autre) accessible depuis la machine de développement
- (Optionnel) Visual Studio 2022 pour ouvrir `ElegantVogue.sln`

## Configuration

La chaîne de connexion est définie dans `ElegantVogue/appsettings.json` sous `ConnectionStrings:DefaultConnection`.

**Important :** le dépôt contient une instance typique du type `Server=…\SQLEXPRESS;Database=ElegantVogueDB;…`. Adaptez **le nom du serveur** et le nom de la base à votre environnement avant la première exécution.

Exemple :

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=VOTRE_SERVEUR;Database=ElegantVogueDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

## Lancer le projet

À la racine du dépôt :

```bash
cd ElegantVogue
dotnet restore
dotnet run
```

Les URLs de développement par défaut figurent dans `Properties/launchSettings.json` (par ex. **http://localhost:5176** et **https://localhost:7215** selon le profil `https`).

La première exécution crée / met à jour la base via les migrations et exécute le script d’initialisation.

### Compte administrateur par défaut

Créé automatiquement si absent (voir `Data/DbInitializer.cs`) :

- **E-mail / nom d’utilisateur :** `admin@elegantvogue.com`
- **Mot de passe :** `Admin123!`

Connectez-vous avec ce compte pour accéder à l’interface **Admin**.

## Migrations Entity Framework

Les migrations sont versionnées dans `ElegantVogue/Migrations/`. En développement, l’application appelle `Database.Migrate()` au démarrage, ce qui applique les migrations en attente sans commande manuelle obligatoire.

Pour générer une nouvelle migration après modification du modèle (depuis le dossier du projet web) :

```bash
dotnet ef migrations add NomDeLaMigration --project ElegantVogue.csproj
```

## Images et assets

Les produits de démo référencent des fichiers sous `wwwroot/images/products/`. Vérifiez que ces images existent pour un affichage correct des vignettes.

## Licence et origine

Projet fourni tel quel dans ce dossier ; adaptez configuration, secrets et mots de passe avant tout déploiement public.
