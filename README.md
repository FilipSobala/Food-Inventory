# Food Inventory

Web app for managing home food supplies: track what is in your fridge and pantry, get alerts before products expire, manage a shopping list and get recipe suggestions based on the ingredients you already have.

Built with **ASP.NET Core 8 MVC**, **Entity Framework Core** and **SQLite**, plus a **REST API** with a console client. University project at AGH Kraków.

## Features
- **Fridge view** — products grouped by category, with visual alerts for items close to their expiry date.
- **Inventory management** — add products with quantity, unit, notes and expiry date; edit, remove or mark as used.
- **Shopping list** — add items manually; bought items move straight into the inventory.
- **Recipe suggestions** — matches recipes to what is in the fridge and adds missing ingredients to the shopping list in one click.
- **Admin panel** — manage users, products, categories and recipes.

## Architecture
- **MVC** web app with Razor views and Bootstrap.
- **Authentication:** session-based login, passwords stored as salted hashes (`PasswordHasher`), two roles: User and Admin.
- **Data layer:** EF Core Code-First with migrations, no raw SQL. 7 related tables: `Users`, `Products`, `Categories`, `UserProducts`, `ShoppingList`, `Recipes`, `RecipeProducts`.
- **REST API** (`/api/products`): `GET`, `POST`, `DELETE` on the user's products, authorized with `X-Username` + `X-Api-Token` headers (each user gets their own token).
- **Auto-initialization:** on first run the app creates the database, seeds dictionaries and creates an admin account.

## Project structure
```
FoodInventory/                 # web app: Controllers, Models, Views, Migrations, wwwroot
FoodInventory.ConsoleClient/   # console client calling the REST API
```

## Run
Requires the [.NET 8 SDK](https://dotnet.microsoft.com/download).

```bash
cd FoodInventory
dotnet run
```
The app starts at `http://localhost:5140`. The database is created automatically on first run.

Console client (in a second terminal, while the app is running):
```bash
cd FoodInventory.ConsoleClient
dotnet run
```

## Stack
C# · ASP.NET Core 8 MVC · Entity Framework Core · SQLite · Razor · Bootstrap · REST API
