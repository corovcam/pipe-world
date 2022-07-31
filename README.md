# Zápočtový projekt C#/Adv. C#
- Názov hry: Plumber
- Jazyk skriptov: C# 
- Game Engine: Unity 2021.3.5f1
- Platforma: Windows, HTML5

## Špecifikácia
Hra Plumber je nástupcom populárneho druhu puzzle hier na štýl [Pipe Mania](https://en.wikipedia.org/wiki/Pipe_Mania) či [Water Pipes](https://play.google.com/store/apps/details?id=com.mobiloids.waterpipespuzzle), kde spoločným cieľom hry je prepraviť tekutinu z jedného konca systému potrubí na druhý koniec. 

### Herná mechanika
Hráč pred sebou statické herné pole v tvare mriežky s pootočenými potrubiami rôzneho druhu (rovné, vpravo/vľavo otočené a krížové). Po kliknutí na potrubie sa potrubie pootočí vpravo. Hráč má vytvoriť spojenie s vedľajším potrubím, tak že sa tekutina dostane z jedného konca, nachádzajucého sa na okraji plochy (typicky vľavo hore), do druhého (typicky vpravo dole). Hráč má časový limit, počas ktorého môže pootáčať potrubia, a čím skôršie puzzle vyrieši tým získa vyššie skóre. Po každom druhom leveli sa zväčší herná plocha alebo zníži časový limit. 

### Grafické rozhranie
- Herné pole
- Tlačidlá:
    - ŠTART
    - Reset
    - Hlavné menu
- Časový limit

### Knižnice a vývojové prostredie
Vývojové prostredie bude herný engine Unity 2021 a IDE Visual Studio 2022 na tvorbu a debugovanie skriptov. Na vývoj budú použité štandardné Unity a .NET knižnice. Grafické elementy ako potrubia a herný plán budú tvorené v Adobe Photoshop. Hlavné menu bude obsahovať niektoré grafické prvky (tlačidlá, zvuky, particle efekty) z Unity Asset store.
V rámci použitých technológií bude súčasťou projektu použitá funkcia Unity Coroutines, čiže implementácia enumerátorových metod využitím yield return, ktorá bude použitá pri implementácii časového limitu a terstovania prepojenia jednotlivých potrubí. Taktiež bude využitá knižnica LINQ na hľadanie potrubí s chybnou rotáciou.
