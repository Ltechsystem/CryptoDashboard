# Crypto Dashboard

An application for monitoring cryptocurrency prices. Written in C# using Blazor Server. The
application fetches cryptocurrency prices from Kraken's public REST API, stores them as
timestamped snapshots in a local SQLite database and displays the price history using apexcharts.

## Running the project

1. Unzip the project and open a terminal in the repository root (where
   `CryptoDashboard.sln` is located).
2. Restore dependencies:
   `dotnet restore`
3. Run the web project:
   `dotnet run --project CryptoDashboard`
4. The app should now be running at http://localhost:5249.

## Known issues

- No retry on the Kraken API.
- Data in `crypto.db` is never deleted.
- Hardcoded configuration.
- UI updates via polling, not push.
- Hardcoded timezone (Europe/Copenhagen).
