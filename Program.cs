namespace BookRegistry;

internal class Program
{
    static void Main(string[] args)
    {
        //creation and inicialization of a new DBHandler object
        DatabaseHandler databaseHandler = new();
        databaseHandler.Initialize();

        //where the program is actually started from
        ConsoleFunctions.MainMenu(databaseHandler);
    }
}
