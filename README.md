# Menza

[*Ha Eötvösös vagy kattints ide!*](README_hu.md)

A simple website where students can see what's going to be the lunch and rate it.

![Thumbnail](thumbnail.webp)

The backend is an ASP.NET Core Web API that stores and serves the menus and votes. It extracts the menu from the school's lunch signup website using a session ID.

The frontend is a standalone Blazor WebAssembly app.

## Running locally

1. [Install the .NET 7 SDK](https://learn.microsoft.com/dotnet/core/install)
2. Clone the repository
3. Update the logic for loading the menus in `Menza.Server/UpdateController.cs`
4. Update email filtering in `Menza.Server/AuthService.cs` and `Menza.Client/AuthService.cs`
5. Start the backend with `dotnet run --project Menza.Server/Menza.Server.csproj`
6. Then, in another terminal, start the frontend with `dotnet run --project Menza.Client/Menza.Client.csproj`
