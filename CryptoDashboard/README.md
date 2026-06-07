# Crypto Dashboard

An application for monitoring cryptocurrency prices. Written in C# using Blazor Server. The
application fetches cryptocurrency prices from Kraken's public REST API, stores them as
timestamped snapshots in a local SQLite database and displays the price history using apexcharts.

## Setting up the database (EF Core)

The app stores prices in a local SQLite database (`crypto.db`).

1. Install the EF Core CLI tool:
   `dotnet tool install --global dotnet-ef`
2. To create `crypto.db`:
   `dotnet ef database update --project CryptoDashboard`

Run this when starting the app for the first time.

## Running the project

1. Unzip the project and open a terminal in the repository root (where
   `CryptoDashboard.sln` is located).
2. Restore dependencies:
   `dotnet restore`
3. Run the web project:
   `dotnet run --project CryptoDashboard`
4. The app should now be running at http://localhost:5249.

## Running the tests

1. Run the tests:
   `dotnet test`

## Known issues

- No retry on the Kraken API.
- Data in `crypto.db` is never deleted.
- Hardcoded configuration.
- Hardcoded timezone (Europe/Copenhagen).
