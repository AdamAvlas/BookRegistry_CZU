# BookRegistry

Semestrální projekt na předmět programování.
1. Přehled projektu:
BookRegistry_CZU je konzolová CRUD (Create, Read, Update, Delete) aplikace určená k evidenci knih. Projekt je napsán v C# (.NET) a umožňuje uživateli vytvářet nové položky knih, prohlížet existující záznamy, upravovat je a mazat. Data jsou ukládána prostřednictvím třídy DatabaseHandler, která zajišťuje inicializaci i manipulaci s databází.

2. Architektura a tok programu: 
Spuštění programu zajišťuje třída Program: v metodě Main se vytvoří a inicializuje instance DatabaseHandler a poté se předá do metody ConsoleFunctions.MainMenu, odkud uživatel ovládá celou aplikaci. Všechny interakce s uživatelem probíhají v této statické třídě, která nabízí přehledné menu s volbami pro různé operace nad záznamy knih.

3. Zpracování chyb a logování: 
Při každé chybě, například nevalidním vstupu nebo výjimce při práci s databází, je zavolána třída ErrorLogger. Ta zapíše podrobnosti o chybě do logovacího souboru, aby bylo možné později snadno dohledat, co se pokazilo. Díky tomu je aplikace robustnější a přehlednější pro další ladění. Zachovává pouze 3 nejnovější log soubory, zbylé smaže.
Popis hlavních tříd

Program

    Metoda Main(string[] args): Vytvoří novou instanci DatabaseHandler, zavolá Initialize(), a spustí hlavní uživatelské menu přes ConsoleFunctions.MainMenu.

ConsoleFunctions

    Statická třída obsahující metody pro veškerou komunikaci v konzoli:

        MainMenu(DatabaseHandler): cyklicky zobrazuje hlavní nabídku a volá odpovídající CRUD operace.

        ViewAll, CreateNewBook, EditBook, RemoveBook: metody pro čtení, vytváření, úpravy a mazání knih.

        Pomocné metody UserInputCheck a MenuInputCheck validují vstupy od uživatele.

DatabaseHandler

    Zajišťuje veškeré operace s uložištěm:

        Initialize(): připraví databázi (vytvoření tabulek, naplnění výchozími daty).

        InsertNewBook, UpdateBook, DeleteBook apod.: metody pro CRUD operace nad entitou Book, Author, Category.

        Uchovává kolekce Books, Authors, Categories pro aktuální relaci aplikace.

ErrorLogger

    Jednoduchý logger pro zaznamenávání chybových zpráv:

        Při výskytu výjimky nebo chybě vstupu zapíše čas, zdroj chyby a detailní popis do externího textového logu.

        Umožňuje zpětnou analýzu problémů bez nutnosti interaktivního debugování.

Program pak dále obsahuje třídy Book, Author a Category, které reprezentují tyto objekty. Obsahují pouze pár vlastností a parametrů + konstruktor (kromě funkce Author, ta obsahuje jednoduchou funkci na vracení celého jména autora jako jeden string).

Databáze:
Databáze v projektu se skládá ze tří hlavních tabulek: Books, Authors a Categories. Tabulka Books obsahuje základní informace o knize jako název, ID autora a kategorie. Authors uchovává seznam autorů včetně jména a příjmení, zatímco Categories slouží k rozřazení knih dle žánrů nebo typu. Vztahy mezi tabulkami jsou definovány pomocí cizích klíčů (pouze vztahy typu 1:N) – každá kniha odkazuje na jednoho autora a jednu kategorii. Všechny tabulky mají auto-increment, přiřazování nových ID si tedy zařizují samy, program samotný "nemusí nic řešit".

Vstupní omezení a možné problémy:
-program omezuje jaké znaky může uživatel použít při tvorbě/editaci knih, jednak kvůli zabezpečení, dále kvůli "estetice" (nepředpokládá se, že například znak "§" nebo "&" by byl použit v názvu knihy)
-regexový pattern není tak přísný, aby uživateli odepřel vložit např. knihu s názvem složeným pouze se speciálních znaků (např. název knihy "!!??$$##@@%%" by prošel)
-ikdyž program umožňuje tvorbu nových autorů a kategorií, neumožňuje jejich smazání ani editace (je to samozřejmě možné, ale pouze přímo přes databázi, u které se nepředpokládá, že by k ní uživatel měl přístup)
-při změně databáze, při které bude buď změněn název jakékoliv tabulky, nebo zrušen auto-increment, program přestane fungovat (a bude třeba změna kódu)

Jak spustit:
Ke spuštění aplikace je krom klonování tohoto repository potřeba samotný databázový soubor a konfigurační soubor, které jsou ke stažení zde{https://drive.google.com/file/d/1kTt1kD1nARhzn6zA48QuOOBLoelP58V8/view?usp=sharing}. Prvním krokem je přidání databáze jako zdroje dat ve Visual Studiu
1) Data Connections->Add Connection
![Snímek obrazovky 2025-04-22 191825](https://github.com/user-attachments/assets/940e62a6-a12e-4e1f-93cd-f8a2dae73348)

2) Najít databázový .mdf soubor a přidat ho
3) Zkopírovat ConnectionString databáze do konfiguračního souboru
![Snímek obrazovky 2025-04-22 192250](https://github.com/user-attachments/assets/d5400042-2fff-41c0-b423-5c92330a3189)

![Snímek obrazovky 2025-04-22 192349](https://github.com/user-attachments/assets/1bc57278-9e47-44a2-825b-681a80ab1149)
