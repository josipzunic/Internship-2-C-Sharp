static void writeMainMenu()
{
    Console.WriteLine("1 - Korisnici");
    Console.WriteLine("2 - Putovanja");
    Console.WriteLine("0 - Izlaz iz aplikacije");
    Console.Write("Odaberite opciju na izborniku upisivanjem broja uz željenu opciju: ");
}

static void writeUsersMenu()
{
    Console.WriteLine("1 - Unos novog korisnika");
    Console.WriteLine("2 - Brisanje korisnika");
    Console.WriteLine("3 - Uređivanje korisnika");
    Console.WriteLine("4- Pregled svih korisnika");
    Console.WriteLine("0 - Povrataka na glavni izbornik");
    Console.Write("Odaberite opciju na izborniku upisivanjem broja uz željenu opciju: ");
}

static void writeJourneyMenu()
{
    Console.WriteLine("1 - Unos novog putovanja");
    Console.WriteLine("2 - Brisanje putovanja");
    Console.WriteLine("3 - Uređivanje postojećeg putovanja");
    Console.WriteLine("4 - Pregled svih putovanja");
    Console.WriteLine("5 - Izvještaji i analize");
    Console.WriteLine("0 - Povratak na glavni izbornik");
    Console.Write("Odaberite opciju na izborniku upisivanjem broja uz željenu opciju: ");
}
/*do
{
    writeMainMenu();
    string input = Console.ReadLine();
    if (input == "0")
        break;
    else if (input == "1")
    {
        writeUsersMenu();
        break;
    }
} while (true);*/

bool onMainMenu = true;
bool onUsersMenu = false;
bool onJourneyMenu = false;

while (true)
{
    if  (onMainMenu)
        writeMainMenu();
    else if (onUsersMenu)
        writeUsersMenu();
    else if(onJourneyMenu)
        writeJourneyMenu();
    
    string input = Console.ReadLine();

    if (input == "0" && onMainMenu)
    {
        Console.WriteLine("Izlazak iz aplikacije");
        break;
    }

    else if (input == "1" && onMainMenu)
    {
        onUsersMenu = true;
        onMainMenu = false;
        Console.WriteLine("\nUlazak na korisnici menu\n");
    }
    
    else if (input == "2" && onMainMenu)
    {
        onUsersMenu = false;
        onMainMenu = false;
        onJourneyMenu = true;
        Console.WriteLine("\nUlazak na meni s putovanjima\n");
    }
    
    else if (onUsersMenu && input == "0" || onJourneyMenu && input == "0")
    {
        onUsersMenu = false;
        onMainMenu = true;
        Console.WriteLine("\nPovratak na main menu\n");
    }

}