# Menza

Egy egyszerű weboldal, ahol a diákok megtekinthetik, hogy mi lesz az ebéd és értékelhetik azt.

![Thumbnail](thumbnail.webp)

Az alkamazás két részből áll:
- A szerver egy ASP.NET Core Web API, amely tárolja az adatokat és kiszolgálja a klienst. A menüt a tata.eny.hu-oldalról gyűjti be.
- A kliens egy önálló Blazor WebAssembly alkalmazás.

## Futtatás

Az alábbi lépésekkel futtathatod a programot a saját gépeden:
1. [Telepítsd a .NET 7 SDK-t](https://learn.microsoft.com/dotnet/core/install).
2. Futtasd a `dotnet dev-certs https --trust` parancsot.
3. Töltsd le a forráskódot.
   1. Telepítsd a Git-et.
   2. Futtasd a `git clone https://github.com/albi005/Menza.git' parancsot.
   3. A forráskód a `Menza` mappában lesz.
4. Futtasd a `cd Menza` parancsot.
5. Indítsd el a szervert a `dotnet run --project Menza.Server/Menza.Server.csproj` parancs segítségével.
6. A menü letöltéséhez szükséged lesz egy élő session ID-ra:
   1. Jelentkezz be a https://tata.eny.hu oldalon.
   2. Nyisd meg a Developer Tools-ot a böngésződben (F12 billentyűvel).
   3. Kattints az Application (Alkalmazás) fülre.
   4. Oldalt a Storage (Tárhely) menüpont alatt nyisd le a Cookies (Sütik) fület.
   5. Kattints a `https://tata.eny.hu`-ra.
   6. A `PHPSESSID` nevű süti értéke lesz a session ID. Valahogy így néz ki: `bcai5c8n5hog6o1g65sob3o3snd`.
7. A menü letöltéséhez írd be a böngésződbe a `https://localhost:7181/update?sessionid=<sessionid>` címet, ahol a `<sessionid>` helyére a korábban megkapott session ID-t kell írnod.  
   Például: `https://localhost:7181/update?sessionid=bcai5c8n5hog6o1g65sob3o3snd`.
8. Ezután egy másik parancssorban indítsd el a klienst a `dotnet run --project Menza.Client/Menza.Client.csproj` parancs segítségével.
9. Nyisd meg a böngésződben a https://localhost:7276 címet.