# EbookLibrary

## Opis biznesowy
Firma chce udostępnić wewnętrzną platformę do zarządzania biblioteką e-booków.  
System powinien pozwalać na:
- dodawanie nowych książek (tytuł, autor, opis, tagi, plik PDF/EPUB),
- wyszukiwanie książek (po tytule, tagach, autorze),
- zarządzanie użytkownikami (rola: `User` / `Admin`),
- wystawienie API (np. dla klienta front-end).

## Cel projektu
Stworzenie back-endu w **.NET** wystawiającego **REST API** do obsługi systemu zarządzania e-bookami.  
System powinien umożliwiać:
- użytkownikom wyszukiwanie i pobieranie książek,  
- administratorom dodawanie nowych zasobów.  

Architektura musi być elastyczna, zgodna z zasadami **SOLID** oraz z dobrymi praktykami bezpieczeństwa.

---

## Wymagania biznesowe

### Użytkownik (`User`)
- może przeglądać listę książek,
- może filtrować po autorze i tagach,
- może pobrać książkę.

### Administrator (`Admin`)
- posiada wszystkie możliwości użytkownika,
- dodatkowo może dodawać nowe książki (tytuł, autor, opis, tagi, plik PDF/EPUB).

### System
- przechowuje **metadane** o książkach w bazie SQL (`SQLite` / `Postgres` / `MSSQL` – do wyboru),
- przechowuje **pliki** w systemie plików (lokalnie).

---

## Wymagania techniczne

### API i architektura
- REST API w dwóch wersjach:
  - **v1**: podstawowe CRUD + wyszukiwanie,
  - **v2**: rozszerzone wyszukiwanie (np. autor + tag + rok wydania).
- Obsługa **CORS** (klient z innej domeny).
- Autoryzacja: **JWT** (role: User/Admin).
- Walidacja danych wejściowych: **FluentValidation**  
  (np. tytuł nie może być pusty, plik max 50 MB).
- Zabezpieczenia:
  - XSS (filtrowanie opisów książek),
  - CSRF (middleware z anty-CSRF tokenem dla POST/PUT/DELETE).

### Warstwa domenowa
- DTO zwracane przez API → `record`.
- Lekki obiekt typu value (np. `Tag`) → `record struct`.
- Abstrakcyjne klasy, np. `FileStorageService`.
- Serwisy zgodne z zasadami **SOLID**.

### Entity Framework Core
- Komunikacja z bazą: **EF Core**.
- Wyszukiwanie z `AsNoTracking()`.
- Indeksy (np. na Author i Tags).
- Uzasadnione użycie **lazy loading** i **eager loading**.

### Zarządzanie pamięcią
- Walidacja nagłówka pliku przy uploadzie  
  (np. czy plik zaczyna się od `%PDF`) z użyciem `Span<byte>` / `Memory<byte>`  
  zamiast kopiowania całego bufora.

### Kolekcje
- Serwis zawiera dwie metody:
  - `SearchBooks(...)` → `IEnumerable<Book>` (filtrowanie, lazy evaluation),
  - `GetAllBooksForUser(userId)` → `IList<Book>` (dostęp po indeksie).

---

## Etapy realizacji

### Tydzień 1–2
- Projekt architektury, modele domenowe i DTO (`record`/`record struct`),
- Implementacja API v1: CRUD + wyszukiwanie,
- EF Core + indeksy + optymalizacje zapytań.

### Tydzień 3–4
- Autoryzacja JWT, role (User/Admin),
- FluentValidation,
- Obsługa CORS,
- Zabezpieczenia (XSS, CSRF).

### Tydzień 5
- Upload plików + walidacja nagłówka z użyciem `Span`/`Memory`,
- Abstrakcja `FileStorageService` + implementacja lokalna,
- Różnice `IList` vs `IEnumerable` w serwisach.

### Tydzień 6
- API v2 (rozszerzone filtrowanie).

---

## Kryteria akceptacji
1. API działa w wersjach **v1** i **v2**, zabezpieczone JWT.
2. Walidacja i bezpieczeństwo działają (XSS, CSRF, CORS).
3. EF Core zoptymalizowane (`AsNoTracking`, indeksy).
4. W kodzie obecne są:
   - `record`,
   - `record struct`,
   - `Span`/`Memory`,
   - `IList` / `IEnumerable`,
   - klasy abstrakcyjne,
   - zasady **SOLID**.
