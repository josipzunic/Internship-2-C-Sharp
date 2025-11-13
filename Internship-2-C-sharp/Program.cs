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
    Console.WriteLine($"Korisnik {users[index]["name"]} {users[index]["surname"]} je uspješno izbrisan");
    users.RemoveAt(index);
}

static Dictionary<string, string> addUser(string name, string surname, string DoB, ref int idCounter)
{
    var user = new Dictionary<string, string>();
    user["id"] = idCounter.ToString();
    user["name"] = name;
    user["surname"] = surname;
    user["dob"] = DoB;
    idCounter++;

    return user;
}

static void writeUsersList(List<Dictionary<string, string>> users)
{
    foreach (var user in users)
    {
        var name = user["name"];
        var surname = user["surname"];
        var DoB = user["dob"];
        var id =  user["id"];
        if(users.Count == 0) Console.WriteLine("Popis korisnika je prazan");
        else Console.WriteLine($"{id} - {name} - {surname} - {DoB}");
        
    }
}

static void writeDeletionOptionsMenu()
{
    Console.WriteLine("Upišite slovo ispred načina brisanja korisnika: ");
    Console.WriteLine("a) id");
    Console.WriteLine("b) ime i prezime");
    Console.WriteLine("c) povratak");
    Console.Write("Vaš odabir: ");
}

static void alphabeticalSortList(List<Dictionary<string, string>> users)
{
    users.Sort((surname1, surname2) =>
    {
        string firstSurname = surname1.ContainsKey("surname") ? surname1["surname"] : String.Empty;
        string secondSurname = surname2.ContainsKey("surname") ? surname2["surname"] : String.Empty;
        return string.Compare(firstSurname, secondSurname, StringComparison.OrdinalIgnoreCase);
    });
}

static List<Dictionary<string, string>> returnUsersOver20(List<Dictionary<string, string>> users)
{
    List<Dictionary<string, string>> usersOver20 = new List<Dictionary<string, string>>();
    foreach (var user in users)
    {
        var dateOfBirth = user["dob"].Split("-");
        int yearOfBirth = int.Parse(dateOfBirth[0]);
        int monthOfBirth = int.Parse(dateOfBirth[1]);
        int dayOfBirth = int.Parse(dateOfBirth[2]);
        
        var currentDate = DateTime.Now;
        var DoB = new DateTime(yearOfBirth, monthOfBirth, dayOfBirth);
        
        int age = currentDate.Year - DoB.Year;
        if (DoB > currentDate.AddYears(-age)) age--;
        
        if(age > 20) usersOver20.Add(user);
    }
    return usersOver20;
}

static void editUser(List<Dictionary<string, string>> users, string dataToChange, string newValue, string id)
{
    int index = users.FindIndex(dict => dict.ContainsKey("id") && dict["id"] == id);
    users[index][dataToChange] = newValue;
}


static void deleteById(List<Dictionary<string, string>> users)
{
    string id;
    do
    {
        Console.Write("Upišite id korisnika kojeg želite obrisati: ");
        id = Console.ReadLine();
        var condition = users.Any(dict => dict.ContainsKey("id") && dict["id"] == id);
        if (!condition) Console.WriteLine($"Korisnik s id-om {id} nije pronađen");
        else break;
    } while (true);
    var message = confirmationMessage("izbrisati");
    int index = users.FindIndex(dict => dict.ContainsKey("id") && dict["id"] == id);
    deleteActionOnAffirmation(users, message, index);
}

static void deleteActionOnAffirmation(List<Dictionary<string, string>> users, string affirmation, int index)
{
    if (affirmation == "da")
    {
        deleteUserAndMessage(users, index);
        waitOnKeyPress();
    }
    else
    {
        Console.WriteLine("Odustajanje od brisanja korisnika, pritiskom bilo koje tipke vratit ćete se na meni s korisnicima");
        Console.ReadKey();
        Console.Clear();
        writeUsersMenu();
    }
}

static void deleteByName(List<Dictionary<string, string>> users)
{
    string nameAndSurname, name, surname;
    do
    {
        Console.Write("Upišite ime i prezime korisnika kojeg želite obrisati: ");
        nameAndSurname = Console.ReadLine();
        
        var arrayWithNameAndSurname = nameAndSurname.Split(" ");
        name =  arrayWithNameAndSurname[0];
        surname = arrayWithNameAndSurname[1];
        
        var condition = users.Any(dict => 
            (dict.ContainsKey("name") && dict["name"] == name)
            && dict.ContainsKey("surname") && dict["surname"] == surname);
        
        if (!condition) Console.WriteLine($"Korisnik imena {name} {surname} nije pronađen");
        else break;
    } while (true);
    
    int index = users.FindIndex(dict => 
        (dict.ContainsKey("name") && dict["name"] ==  name)
        && dict.ContainsKey("surname") && dict["surname"] == surname);
    int count = users.Count(dict =>
        dict.ContainsKey("name") && dict["name"] == name &&
        dict.ContainsKey("surname") && dict["surname"] == surname);
    
    if (count > 1)
    {
        var indices = users
            .Select((dict, i) => new { dict, i })
            .Where(x =>
                x.dict.ContainsKey("name") && x.dict["name"] == name &&
                x.dict.ContainsKey("surname") && x.dict["surname"] == surname)
            .Select(x => x.i) 
            .ToList();
        
        var usersWithSameName = new List<Dictionary<string, string>>();
        
        for(int i = 0; i < indices.Count; i++)
            usersWithSameName.Add(new()
                {{"id", users[i]["id"]}, {"name", name}, {"surname", surname}, {"dob", users[i]["dob"]}}
        );
        
        writeUsersList(usersWithSameName);
        Console.WriteLine("Postoji više korisnika s tim imenom, preusmjeravanje na brisanje prema id-u: ");
        confirmationMessage("izbrisati");
        deleteById(users);
    }
    else
    {
        var message = confirmationMessage("izbrisati");
        deleteActionOnAffirmation(users, message, index);
    }
    
}

static void editUserById(List<Dictionary<string, string>> users)
{
    do
    {
        //ovdje trebas dovrsit uredivanje
        Console.WriteLine("a) Ime: ");
        Console.WriteLine("b) Prezime: ");
        Console.WriteLine("c) Datum rođenja");
        Console.WriteLine("d) Povratak");
        Console.Write("Unesite podatak koji želite izmijeniti: ");
        string dataToChange = Console.ReadLine();
        Console.Write("Upišite id korisnika kojeg želite obrisati: ");
        string id = Console.ReadLine();
        
        var dict = new Dictionary<string,string>();
        dict["a)"] = "ime";
        dict["b)"] = "prezime";
        dict["c)"] = "datum rođenja";
                
        Console.Write($"Unesite novu vrijednost za {dict[dataToChange]}: ");
                
        if (dataToChange == "a)") dataToChange = "name";
        else if (dataToChange == "b)") dataToChange = "surname";
        else dataToChange = "dob";
                
        string newValue = Console.ReadLine();
        editUser(users, dataToChange, newValue, id);
        waitOnKeyPress();
        
        var condition = users.Any(dict => dict.ContainsKey("id") && dict["id"] == id);
        if (!condition) Console.WriteLine($"Korisnik s id-om {id} nije pronađen");
        else break;
    } while (true);
}

static string confirmationMessage(string action)
{
    string input;
    do
    {
        Console.Write($"Želite li stvarno {action} ovog korisnika (da/ne): ");
        input =  Console.ReadLine();
        if(input == "da" || input == "ne") return  input;
        else Console.WriteLine("Molim unesite da ili ne");
    } while (true);
    
    
}

static void waitOnKeyPress()
{
    Console.WriteLine("Pritisnite bilo koju tipku za nastavak");
    Console.ReadKey();
}

static bool checkInputValidity(string name, string nameOrSurname)
{
    bool validity = true;
    if (string.IsNullOrEmpty(name)) Console.WriteLine($"{nameOrSurname} ne može biti prazno");
    else if (!name.All(c => char.IsLetter(c) || c == ' ' || c == '-'))
        Console.WriteLine($"{nameOrSurname} može sadržavati samo slova ili crticu");
    else validity = false;
    
    return validity;
}

static bool checkDateValidity(string date)
{
    bool validity = true;
    var currentDate = DateTime.Now;
    var dateTimeValidity = DateTime.TryParse(date, out DateTime dateTime);
    if(string.IsNullOrEmpty(date)) Console.WriteLine("Datum rođenja ne može biti prazan");
    else if (!dateTimeValidity)
        Console.WriteLine("Unesite datum rođenja u formatu yyyy-mm-dd");
    else if(currentDate.Year < dateTime.Year)
        Console.WriteLine("Unesena godina ne može biti kasnija od trenutne");
    else validity = false;
    return validity;
}

static void mainMenuFunctionality(string input,  List<Dictionary<string, string>> users, int idCounter)
{
    switch (input)
    {
        case "1":
        {
            string name, surname, DoB;
            do
            {
                Console.Write("Unesite ime novog korisnika: ");
                name = Console.ReadLine();
            } while (checkInputValidity(name, "ime"));
            do
            {
                Console.Write("Unesite prezime novog korisnika: ");
                surname = Console.ReadLine();
            } while (checkInputValidity(surname, "prezime"));

            do
            {
                Console.Write("Unesite datum rođenja novog korisnika u formatu yyyy-mm-dd: ");
                DoB = Console.ReadLine();
            } while (checkDateValidity(DoB));

            var newUser = addUser(name, surname, DoB, ref idCounter);
            users.Add(newUser);
            Console.WriteLine($"Korisnik {name} {surname} je uspješno dodan");
            waitOnKeyPress();
            break;
        }
        case "2":
        {
            string parameter;
            do
            {
                writeDeletionOptionsMenu();
                parameter = Console.ReadLine();
            }
            while (parameter.ToLower().Replace(")", "") != "a" 
                   && parameter.ToLower().Replace(")", "") != "b"
                   && parameter.ToLower().Replace(")", "") != "c");
            
            if (parameter == "a") deleteById(users);
            else if (parameter == "b") deleteByName(users);
            break;
        }
        case "3":
        {
            editUserById(users);
            break;
        }
        case "4":
        {
            Console.WriteLine("a) abecedni ispis");
            Console.WriteLine("b) korisnici stariji od 20 godina");
            Console.WriteLine("c) korisnici s najmanje 2 putovanja");
            Console.Write("Unesite odabir za ispis: ");
            string condition = Console.ReadLine();
            switch (condition)
            {
                case "a)":
                {
                    alphabeticalSortList(users);
                    writeUsersList(users);
                    break;
                }
                case "b)":
                {
                    var usersOver20 = returnUsersOver20(users);
                    alphabeticalSortList(usersOver20);
                    writeUsersList(usersOver20);
                    break;
                }
                case "c)":
                {
                    //vratit cemo se odi kad rijesim putovanja
                    break;
                }
                    
            }
            waitOnKeyPress();
            break;
        }
    }
}

bool onMainMenu = true;
bool onUsersMenu = false;
bool onJourneyMenu = false;

var users = new List<Dictionary<string, string>>();
int idCounter = 0;

var user1 = addUser("ante", "antic", "2002-05-20", ref idCounter);
var user2 = addUser("mate", "matic", "2020-06-04", ref idCounter);

users.Add(user1);
users.Add(user2);

do
{
    string input;

    if (onMainMenu)
    {
        Console.Clear();
        writeMainMenu();
        input = Console.ReadLine();
    }
    else if (onUsersMenu)
    {
        Console.Clear();
        writeUsersMenu();
        input = Console.ReadLine();
        mainMenuFunctionality(input, users, idCounter);
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
    }else Console.WriteLine("Unesite broj koji odgovara brojevima na izborniku");
} while (true);
