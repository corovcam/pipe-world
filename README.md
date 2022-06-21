# Pipe World 
- Názov hry: Pipe World
- Jazyk skriptov: C# 
- Game Engine: Unity 2022
- Platforma: Webový prehliadač
- Autor: Martin Čorovčák 

## Špecifikácia
Hra Pipe World je nástupcom populárneho druhu puzzle hier na štýl [Pipe Mania](https://en.wikipedia.org/wiki/Pipe_Mania) či [Water Pipes](https://play.google.com/store/apps/details?id=com.mobiloids.waterpipespuzzle), kde spoločným cieľom hry je prepraviť tekutinu z jedného konca systému potrubí na druhý koniec. 

### Herná mechanika a varianty
V *Arcade* hernom móde má hráč pred sebou náhodne vygenerované statické herné pole v tvare mriežky s pootočenými potrubiami rôzneho druhu (rovné, vpravo/vľavo otočené a krížové). Po kliknutí na potrubie sa potrubie pootočí vpravo. Hráč má vytvoriť spojenie s vedľajším potrubím, tak že sa tekutina dostane z jedného konca, nachádzajucého sa na náhodnom mieste na okraji plochy (typicky vľavo hore), do druhého (typicky vpravo dole). Hráč má časový limit, počas ktorého môže pootáčať potrubia, a čím skôršie puzzle vyrieši tým získa vyššie skóre. Po každom druhom leveli sa zväčší herná plocha alebo zníži časový limit.

Vo *Free World* móde dostane hráč na výber niekoľko možností potrubí rovnakého druhu ako v Arcade móde + ich drevené a kovové varianty. Tieto potrubia musí hráč vložiť (pomocou drag-and-drop) na hraciu plochu, tak, aby vytvoril naväzujúce spojenie s nejakým vedľajším potrubím (na začiatku iba so štartovným potrubím = *zdroj*). Na hracej ploche budú 1-2 typy tekutín (s ich danými zdrojmi/potrubiami): voda a láva. Drevené potrubia môžu prepravovať iba vodu, zatiaľ čo kovové iba lávu. Cieľom je prepraviť dané typy tekutín do správnych cieľových potrubí v časovom limite. V prvých leveloch (1. až 2. level) bude na hracej ploche len voda, následne sa zvýši počet štartov, a po 4. levely sa pridá láva. Láva tečie o polovicu jednotky času pomalšie ako voda (prietok vody z 1. potrubia do druhého = 1 sekunda), a teda hráč musí vytvoriť kratšiu cestu pre lávu ako pre vodu. Po kliknutí na *ŠTART* sa spustí tok zo všetkých *zdrojov* a začne sa odpočítavanie. Ak sa akákoľvek tekutina nedostane za časový limit do svojho *cieľa*, musí hráč začať od začiatku. 

V oboch módoch bude mať hráč možnosť nápovedy po kliknutí na tlačítko (po určitej dobe nečinnosti). 

### Spoločné grafické rozhranie
- Herné pole
- Tlačidlá:
    - ŠTART
    - Reset
    - Nápoveda
    - Hlavné menu
- Časový limit
    - Arcade: Odpočítavanie
    - Free World: Čas toku vody, resp. lávy

### Rozdiely od existujúcich hier
Oproti [Pipe Mania](https://en.wikipedia.org/wiki/Pipe_Mania) a  [Pipes](https://store.steampowered.com/app/755890/Pipes/) implementácia **Pipe World** obsahuje naviac: 
- rôzne typy tekutín s rôznymi rýchlosťami toku
- rozličné typy potrubí

Oproti hre [Water Pipes](https://play.google.com/store/apps/details?id=com.mobiloids.waterpipespuzzle):
- rôzne rýchlosti toku tekutín
- drag-and-drop varianta
- rozličné typy potrubí

### Knižnice a vývojové prostredie
Vývojové prostredie bude herný engine Unity 2022 a IDE Visual Studio 2022 na tvorbu a debugovanie skriptov. Na vývoj budú použité štandardné Unity a .NET knižnice s využitím dodatočných knižníc podľa potreby. Na testovanie funkčnosti v prehliadačoch bude použitá knižnica WebGL.
