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

- **No retry / backoff on API failures or rate limiting**: if a fetch from Kraken fails
  (network error, rate limit, malformed response), the failure is logged as a warning and
  the background service simply waits for the next scheduled 30-second tick — there is no
  retry, exponential backoff, or explicit handling of HTTP 429 responses.
- **No data retention policy**: snapshots are never deleted, so `crypto.db` grows
  indefinitely the longer the app runs.
- **Hardcoded configuration**: the polling interval (30s), the tracked currency pairs, and
  the Kraken base URL are hardcoded in `KrakenService` / `CryptoPriceBackgroundService`
  rather than being read from `appsettings.json`.
- **No automated tests**: the `CryptoDashboard.Tests` project exists in the solution but
  currently contains no tests.
- **UI updates via polling, not push**: the page refreshes itself from the database with a
  client-side timer every 30 seconds rather than receiving a push notification (e.g. via
  SignalR) the moment a new snapshot is saved — so there can be a brief delay between a
  snapshot being stored and it appearing on screen.
- **Hardcoded timezone**: timestamps are converted to the `Europe/Copenhagen` timezone by
  ID; running the app on a system without that timezone in its tz database (rare, but
  possible on minimal Linux containers) will throw at startup of the background service.
