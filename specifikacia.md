# Pipe World 
- Názov hry: Pipe World
- Jazyk skriptov: C# 
- Game Engine: Unity 2021.3.5f1
- Platforma: WebGl
- Autor: Martin Čorovčák 

## Špecifikácia
Hra ***Pipe World*** je nástupcom populárneho druhu puzzle hier na štýl [Pipe Mania](https://en.wikipedia.org/wiki/Pipe_Mania) či [Water Pipes](https://play.google.com/store/apps/details?id=com.mobiloids.waterpipespuzzle), kde spoločným cieľom hry je prepraviť tekutinu z jedného konca systému potrubí na druhý koniec. 

### Herná mechanika a varianty
V ***Level Slect*** hernom móde má hráč pred sebou predpripravené statické herné pole v tvare mriežky s pootočenými potrubiami rôzneho druhu (rovné, vpravo/vľavo otočené a krížové). Po kliknutí na potrubie sa potrubie pootočí v smere hodinových ručičiek. Hráč má vytvoriť spojenie s vedľajším potrubím, tak že sa tekutina dostane z jedného konca, nachádzajucého sa typicky vľavo hore na kraji plochy, do druhého konca (typicky vpravo dole). Hráč má časový limit, počas ktorého môže pootáčať potrubia, a čím skôršie puzzle vyrieši tým získa vyššie skóre. Po niekoľkých leveloch sa zväčší herná plocha alebo prispôsobí časový limit podľa náročnosti.

V ***Arcade*** hernom móde má hráč, rovnako ako v *Level Select* režime, pred sebou vygenerované statické herné pole. Rozdielom je náhodné generovanie potrubí, ako aj náhodné umiestnenie *Štartu* a *Konca*. Časový limit je pevne daný, a to 30 sekúnd (je možné modifikovať v kóde).

***Free World*** mód je varianta ***Level Select*** módu, kde hráč dostane na výber niekoľko možností potrubí (tvoriace správnu cestu od zdroja do cieľa) + ich zelené a sivé varianty. Tieto potrubia musí hráč vložiť (pomocou drag-and-drop) na hraciu plochu, tak, aby vytvoril naväzujúce spojenie s nejakým vedľajším potrubím (na začiatku iba so štartovným potrubím = *zdroj*). Hráč môže presúvať už položené potrubia na iné miesta. Na hracej ploche budú 1-2 typy tekutín (s ich danými zdrojmi/potrubiami): voda a láva. Zelené potrubia môžu prepravovať iba vodu, zatiaľ čo sivé iba lávu. Cieľom je prepraviť dané typy tekutín do správnych cieľových potrubí v časovom limite. V prvých leveloch bude na hracej ploche len voda, a po 4. levely sa pridá láva. Láva tečie o polovicu jednotky času pomalšie ako voda (prietok vody z 1. potrubia do druhého = 1 sekunda). Po kliknutí na *Flow* sa spustí tok zo všetkých *zdrojov* a pozastaví sa odpočítavanie (ako v predošlých variantách).

### Grafické rozhranie
- Herné pole
- GUI:
    - Flow (Štart toku vody)
    - Settings (Pause menu)
        - Help (Nápoveda/Ovládanie hry)
        - Restart
        - Main Menu (Späť do hlavného menu)
        - Resume (Späť do hry)
    - End Game Menu (Menu na konci hry)
        - Total Score
        - Next Level / Restart
        - Main Menu
    - Skip (Na zrýchlenie animácie toku vody)
- Timelimit
    - Odpočítavanie do začiatku toku

### Rozdiely od existujúcich hier
Oproti [Pipe Mania](https://en.wikipedia.org/wiki/Pipe_Mania) a  [Pipes](https://store.steampowered.com/app/755890/Pipes/) implementácia **Pipe World** obsahuje naviac: 
- rôzne typy tekutín s rôznymi rýchlosťami toku
- rozličné typy potrubí

Oproti hre [Water Pipes](https://play.google.com/store/apps/details?id=com.mobiloids.waterpipespuzzle):
- rôzne rýchlosti toku tekutín
- drag-and-drop varianta
- rozličné typy potrubí

### Knižnice a vývojové prostredie
Vývojové prostredie je herný engine Unity 2021.3.5f1 a IDE Visual Studio 2022 na tvorbu a debugovanie skriptov. Na vývoj sú použité štandardné Unity a .NET knižnice s využitím dodatočných knižníc podľa potreby. Na testovanie funkčnosti v prehliadačoch je použitá knižnica WebGL. Hra bude prispôsobená aj čiastočne pre Android.

Viac v Dokumentácii: https://corovcam.github.io/pipe-world/.
