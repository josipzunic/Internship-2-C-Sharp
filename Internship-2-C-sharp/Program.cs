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
    Console.WriteLine("4 - Pregled svih korisnika");
    Console.WriteLine("0 - Povratak na glavni izbornik");
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

static void deleteUserAndMessage(List<Dictionary<string, string>> users, int index)
{
    if (index >= 0)
    {
        Console.WriteLine("postji");
        users.RemoveAt(index);
    }
    else Console.WriteLine("ne postoji");
}

static Dictionary<string, string> addUser(string name, string surname, string DoB)
{
    var user = new Dictionary<string, string>();
    user["id"] = "1";
    user["name"] = name;
    user["surname"] = surname;
    user["dob"] = DoB;

    return user;
}

static void mainMenuFunctionality(string input,  List<Dictionary<string, string>> users)
{
    switch (input)
    {
        case "1":
        {
            Console.Write("Unesite ime novog korisnika: ");
            string name = Console.ReadLine();
            Console.Write("Unesite prezime novog korisnika: ");
            string surname = Console.ReadLine();
            Console.Write("Unesite datum rođenja novog korisnika u formatu yyyy-mm-dd: ");
            string DoB =  Console.ReadLine();

            var newUser = addUser(name, surname, DoB);
            users.Add(newUser);
            Console.WriteLine($"Korisnik {name} {surname} je uspješno dodan");
            Console.WriteLine("Pritisnite bilo koju tipku za nastavak\n");
            Console.ReadKey();
            break;
        }
        case "2":
        {
            //rijesi i slucaj ako imaju 2 osobe istog imena i prezimena, onda pitaj id svakako
            Console.WriteLine("Upišite prema ćemu želite obrisati korisnika: "); // odi provjeri sve moguce kombinacija
            Console.WriteLine("a) id");
            Console.WriteLine("b) ime i prezime");
            Console.Write("Vaš odabir: ");
            string parameter =  Console.ReadLine();
            if (parameter == "a)")
            {
                Console.Write("Upišite id korisnika kojeg želite obrisati: ");
                string id =  Console.ReadLine();
                int index = users.FindIndex(dict => dict.ContainsKey("id") && dict["id"] == id);
                deleteUserAndMessage(users, index);
            }
            else
            {
                Console.Write("Upišite ime i prezime korisnika kojeg želite obrisati: ");
                string nameAndSurname = Console.ReadLine();
                var arrayWithNameAndSurname = nameAndSurname.Split(" ");
                string name =  arrayWithNameAndSurname[0];
                string surname = arrayWithNameAndSurname[1];
                int index = users.FindIndex(dict => 
                    (dict.ContainsKey("name") && dict["name"] ==  name)
                    && dict.ContainsKey("surname") && dict["surname"] == surname);
                deleteUserAndMessage(users, index);
            }
            break;
        }
        case "3":
        {
            Console.Write("Unesite id korisnika čije podatke želite izmijeniti: ");
            string id = Console.ReadLine();
            Console.Write("Unesite podatak koji želite izmijeniti: ");
            string dataToChange = Console.ReadLine();
            Console.WriteLine($"Unesite novu vrijednost za {dataToChange}: ");
            break;
        }
        case "4":
        {
            Console.WriteLine("");
            foreach (var user in users)
            {
                var name = user["name"];
                var surname = user["surname"];
                var DoB = user["dob"];
                var id =  user["id"];
                Console.WriteLine($"{id}-{name}-{surname}-{DoB}");
            }
            Console.WriteLine("");
            if(users.Count == 0) Console.WriteLine("Popis korisnika je prazan");
            break;
        }
    }
}

bool onMainMenu = true;
bool onUsersMenu = false;
bool onJourneyMenu = false;

var users = new List<Dictionary<string, string>>();

while (true)
{
    string input;

    if (onMainMenu)
    {
        writeMainMenu();
        input = Console.ReadLine();
    }
    else if (onUsersMenu)
    {
        writeUsersMenu();
        input = Console.ReadLine();
        mainMenuFunctionality(input, users);
    }
    else if (onJourneyMenu)
    {
        writeJourneyMenu();
        input = Console.ReadLine();
    }
    else
    {
        input = "dogodilo se nesto neocekivano";
        Console.WriteLine(input);
    }

    if (input == "0" && onMainMenu)
    {
        Console.WriteLine("\nIzlazak iz aplikacije\n");
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