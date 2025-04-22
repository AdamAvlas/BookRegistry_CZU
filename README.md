# BookRegistry

Semestrální projekt na předmět programování.
1. Přehled projektu
BookRegistry_CZU je konzolová CRUD (Create, Read, Update, Delete) aplikace určená k evidenci knih. Projekt je napsán v C# (.NET) a umožňuje uživateli vytvářet nové položky knih, prohlížet existující záznamy, upravovat je a mazat. Data jsou ukládána prostřednictvím třídy DatabaseHandler, která zajišťuje inicializaci i manipulaci s databází.

2. Architektura a tok programu
Spuštění programu zajišťuje třída Program: v metodě Main se vytvoří a inicializuje instance DatabaseHandler a poté se předá do metody ConsoleFunctions.MainMenu, odkud uživatel ovládá celou aplikaci. Všechny interakce s uživatelem probíhají v této statické třídě, která nabízí přehledné menu s volbami pro různé operace nad záznamy knih.

3. Zpracování chyb a logování
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

Databáze:
