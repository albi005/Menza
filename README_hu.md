# Menza

Az alábbi lépéseket követve futtathatod a szervert a saját gépeden:

1. Töltsd le a [Visual Studio 2022 Community-t](https://visualstudio.microsoft.com/downloads/).
   - Telepítés közben válaszd ki az *ASP.NET and web development* workload-ot.
2. Indítsd el a Visual Studio-t, válaszd ki a *Clone a repository* opciót, *Repository location*-nek add meg, hogy `https://github.com/albi005/Menza.git`, majd kattints a *Clone* gombra.
3. A startup project legyen a szerver, majd kattints a Run Server melletti *Start Without Debugging* gombra. ![](visual-studio.webp)
4. A menü betöltéséhez szükséged lesz egy élő session ID-ra:
   1. Jelentkezz be a https://tata.eny.hu oldalon.
   2. Nyisd meg a Developer Tools-t a böngésződben (F12 billentyűvel).
   3. Kattints az Application (Alkalmazás) fülre.
   4. Oldalt, a Storage (Tárhely) menüpont alatt nyisd le a Cookies (Sütik) fület.
   5. Kattints a `https://tata.eny.hu`-ra.
   6. A `PHPSESSID` nevű süti értéke lesz a session ID. Valahogy így néz ki: `bcai5c8n5hog6o1g65sob3o3snd`.
5. A menü letöltéséhez írd be a böngésződbe a `http://localhost:7180/update?sessionid=<SESSIONID>` címet, a `<SESSIONID>` helyére a korábban megkapott session ID-t írd.  
   Például: `http://localhost:7180/update?sessionid=bcai5c8n5hog6o1g65sob3o3snd`.
6. Nyisd meg a böngésződben a http://localhost:7180 címet.
