# .NET_lab2

# Sprawozdanie z laboratorium nr 2
## Platformy Programistyczne .NET i Java

**Autor:**

**Data:** 8 kwietnia 2025

**Temat:** Projekt aplikacji bazodanowej .NET

**Prowadzący:** Dr inż. Radosław Idzikowski, mgr inż. Michał Jaroszczuk

---

## Cel laboratorium
Celem laboratorium było zapoznanie się z podstawami projektowania aplikacji bazodanowych w technologii .NET oraz wykorzystanie interfejsu API do pobierania, deserializacji i przechowywania danych.

---

## Opis wykonanych zadań

### Zadanie 1 - Pobranie i deserializacja danych z API

W ramach pierwszego zadania stworzona została aplikacja konsolowa w technologii .NET 8.0, służąca do pobierania i wyświetlania danych pogodowych z zewnętrznego API (OpenWeather API). Program pobiera dane o temperaturze oraz opisie pogody dla wskazanego przez użytkownika miasta.

#### Kluczowe aspekty implementacji:
- Wykorzystanie klasy `HttpClient` do komunikacji z API.
- Deserializacja JSON na klasy C# przy użyciu `JsonSerializer`.
- Obsługa wyjątków związanych z błędami połączenia lub przetwarzania danych JSON.

### Zadanie 2 - Obsługa bazy danych

W drugim zadaniu program został rozszerzony o funkcjonalność bazodanową z wykorzystaniem Entity Framework i bazy danych SQLite.

#### Funkcjonalności:
- Utworzono model danych w klasie `WeatherInfo`, zawierający informacje o mieście, temperaturze oraz opisie pogody.
- Dodano klasę kontekstową `WeatherDbContext`, która definiuje strukturę bazy danych.
- Implementacja sprawdzania bazy danych przed wykonaniem zapytania do API, co zapobiega pobieraniu danych, które są już zapisane w bazie.

#### Struktura bazy danych:
- `Id` - klucz główny
- `Name` - nazwa miasta
- `Temp` - temperatura w stopniach Celsjusza
- `Description` - opis warunków pogodowych

---

## Kluczowe klasy i metody:

- `Program.cs`
  - `Main(string[] args)` - główna metoda aplikacji, obsługa interakcji z użytkownikiem.
  - `GetWeatherDataFromApi(string city, WeatherDbContext dbContext)` - metoda pobierająca dane z API, zapisująca je do bazy danych.

- `WeatherInfo.cs`
  - Klasa `WeatherInfo` - reprezentuje informacje pogodowe w bazie danych.
  - Klasa `ApiResponse` - model danych zwracany przez API.

---

## Użycie aplikacji
1. Użytkownik podaje nazwę miasta.
2. Aplikacja sprawdza obecność danych w lokalnej bazie.
   - Jeśli dane są dostępne, wyświetlane są bezpośrednio z bazy.
   - Jeśli brak danych, aplikacja pobiera je z API, wyświetla użytkownikowi i zapisuje w bazie.

---

## Technologie
- .NET 8.0
- Entity Framework Core
- SQLite
- OpenWeather API

---
