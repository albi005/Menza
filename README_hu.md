# Menza

Az alábbi lépéseket követve futtathatod a szervert a saját gépeden:

1. Töltsd le a [Visual Studio 2022 Community-t](https://visualstudio.microsoft.com/downloads/).
   - Telepítés közben válaszd ki az *ASP.NET and web development* workload-ot.
2. Indítsd el a Visual Studio-t, válaszd ki a *Clone a repository* opciót, *Repository location*-nek add meg, hogy `https://github.com/albi005/Menza.git`, majd kattints a *Clone* gombra.
3. Miután megnyílt a szerkesztő, oldalt, a *Solution Explorer* ablakban jobb-kattints a *Menza.Server* projektre majd kattints a *Manage User Secrets* opcióra.
4. A megnyílt fájlban add meg a belépési adataidat a `https://tata.eny.hu` oldalhoz, a következő mintához hasonlóan. Ha Google bejelentkezést használsz, az elfelejtett jelszó gombra kattintva állíthatsz be egy jelszót.
   ```json
   {
      "EnyCredentials:Username": "valaki@example.com",
      "EnyCredentials:Password": "12345678"
   }
   ```
5. Felül, a startup project legyen a szerver, majd kattints a Run Server melletti *Start Without Debugging* gombra. ![](visual-studio.webp)
6. Az oldal a böngésződben a `http://localhost:7180` címen lesz elérhető. Ha a menü még nem látható, várj pár másodpercet, majd frissítsd az oldalt.
