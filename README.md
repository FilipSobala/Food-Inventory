# Food Inventory

Aplikacja internetowa służąca do zarządzania domowymi zapasami żywności, stworzona w ramach projektu laboratoryjnego w technologii **ASP.NET Core 8.0 MVC** oraz **REST API**. System pozwala na kontrolę dat ważności produktów, zarządzanie listami zakupów oraz inteligentne sugerowanie przepisów na podstawie składników dostępnych w lodówce.

## Metric projektu
* **Uczelnia:** Akademia Górniczo-Hutnicza w Krakowie (AGH)
* **Wydział:** Elektrotechniki, Automatyki, Informatyki i Inżynierii Biomedycznej (EAIiIB)
* **Kierunek:** Informatyka i Systemy Inteligentne
* **Autor:** Filip Sobala
* **Prowadzący:** Tomasz Hachaj
* **Rok realizacji:** 2026
* **Termin oddania:** Laboratorium 12

## Funkcje systemu i kryteria formalne
* **Architektura MVC & REST API:** Kompletna aplikacja webowa uzupełniona o końcówki API (CRUD) dla systemów zewnętrznych.
* **Bezpieczeństwo (Sesja i Hashowanie):** Logowanie oparte na mechanizmie sesji. Hasła użytkowników są zabezpieczone w bazie przy użyciu bezpiecznego skrótu (hash-u).
* **Baza danych SQLite:** Trwałość danych zapewnia baza SQLite (`food_new.db`). Komunikacja odbywa się w całości poprzez **Entity Framework Core (Code-First)** – brak natywnych zapytań SQL.
* **Struktura relacyjna:** System operuje na 7 powiązanych relacjami tabelach (`Users`, `Products`, `Categories`, `UserProducts`, `ShoppingList`, `Recipes`, `RecipeProducts`).
* **Autoryzacja REST API:** Dostęp do API zabezpieczony jest za pomocą unikalnego nagłówka z nazwą użytkownika oraz klucza autoryzacyjnego (`ApiToken`).
* **Automatyczna inicjalizacja:** Przy pierwszym uruchomieniu aplikacja automatycznie tworzy bazę danych, uzupełnia słowniki (seeding) oraz generuje konto administratora.

## Struktura projektu
* `FoodInventory/` – Główny projekt webowy (Kontrolery MVC, Widoki Razor, Modele, Migracje).
* `FoodInventory.ConsoleClient/` – Program konsolowy demonstrujący poprawne działanie, autentykację i zapytania CRUD do interfejsu REST API.


