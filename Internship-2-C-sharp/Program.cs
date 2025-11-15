using System.Globalization;
using System.Security.Cryptography;

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

static void writeTripMenu()
{
    Console.WriteLine("1 - Unos novog putovanja");
    Console.WriteLine("2 - Brisanje putovanja");
    Console.WriteLine("3 - Uređivanje postojećeg putovanja");
    Console.WriteLine("4 - Pregled svih putovanja");
    Console.WriteLine("5 - Izvještaji i analize");
    Console.WriteLine("0 - Povratak na glavni izbornik");
    Console.Write("Odaberite opciju na izborniku upisivanjem broja uz željenu opciju: ");
}

static void deleteUserAndMessage(List<Dictionary<string, object>> users, int index)
{
    Console.WriteLine($"Korisnik {users[index]["name"]} {users[index]["surname"]} je uspješno izbrisan");
    users.RemoveAt(index);
}

static Dictionary<string, object> addUser(string name, string surname, string DoB, ref int idCounterUsers, 
    List<Dictionary<string, string>> trips)
{
    var user = new Dictionary<string, object>();
    user["id"] = idCounterUsers.ToString();
    user["name"] = name;
    user["surname"] = surname;
    user["dob"] = DoB;
    user["listOfTrips"] = trips;
    idCounterUsers++;

    return user;
}

static Dictionary<string, string> addTrip(string dateOfTrip, double mileage, 
    double spentFuel, double pricePerLiter, double priceOfTrip, ref int idCounterTrip)
{
    var trip = new Dictionary<string, string>();
    trip["id"] = idCounterTrip.ToString();
    trip["dot"] = dateOfTrip;
    trip["mileage"] = mileage.ToString();
    trip["spentFuel"] = spentFuel.ToString();
    trip["pricePerLiter"] = pricePerLiter.ToString();
    trip["priceOfTrip"] = priceOfTrip.ToString();
    idCounterTrip++;

    return trip;
}

static void writeUsersList(List<Dictionary<string, object>> users)
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

static void writeDeletionOptionsMenu(string type)
{
    if (type == "user")
    {
        Console.WriteLine("Upišite slovo ispred načina brisanja korisnika: ");
        Console.WriteLine("a) id");
        Console.WriteLine("b) ime i prezime");
        Console.WriteLine("c) povratak");
        Console.Write("Vaš odabir: ");
    }
    else
    {
        Console.WriteLine("Odaberite način za obrisati putovanje");
        Console.WriteLine("a) id");
        Console.WriteLine("b) sva putovanja skuplja od određene cijene");
        Console.WriteLine("c) sva putovanja jeftinija od određene cijene");
        Console.WriteLine("d) povratak");
        Console.Write("Vaš odabir: ");
    }
}

static void alphabeticalSortList(List<Dictionary<string, object>> users)
{
    users.Sort((surname1, surname2) =>
    {
        var firstSurname = surname1.ContainsKey("surname") ? (string)surname1["surname"] : String.Empty;
        var secondSurname = surname2.ContainsKey("surname") ? (string)surname2["surname"] : String.Empty;
        return string.Compare(firstSurname, secondSurname, StringComparison.OrdinalIgnoreCase);
    });
}

static List<Dictionary<string, object>> returnUsersOver20(List<Dictionary<string, object>> users)
{
    List<Dictionary<string, object>> usersOver20 = new List<Dictionary<string, object>>();
    foreach (var user in users)
    {
        var dateOfBirth = user["dob"].ToString().Split("-");
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

static List<Dictionary<string, object>> returnUsersWithMutlipleTrips(List<Dictionary<string, object>> users)
{
    List<Dictionary<string, object>> usersWithMultipleTrips = new List<Dictionary<string, object>>();
    foreach (var user in users)
    {
        var trips = (List<Dictionary<string, string>>)user["listOfTrips"];
        if (trips.Count >= 2) usersWithMultipleTrips.Add(user);
    }
    return usersWithMultipleTrips;
}

static void editUser(List<Dictionary<string, object>> users, string dataToChange, string newValue, string id)
{
    int index = users.FindIndex(dict => dict.ContainsKey("id") && (string)dict["id"] == id);
    users[index][dataToChange] = newValue;
}


static void deleteUserById(List<Dictionary<string, object>> users)
{
    string id;
    do
    {
        Console.Write("Upišite id korisnika kojeg želite obrisati: ");
        id = Console.ReadLine();
        var condition = users.Any(dict => dict.ContainsKey("id") && (string)dict["id"] == id);
        if (!condition) Console.WriteLine($"Korisnik s id-om {id} nije pronađen");
        else break;
    } while (true);
    var message = confirmationMessage("izbrisati");
    int index = users.FindIndex(dict => dict.ContainsKey("id") && (string)dict["id"] == id);
    deleteActionOnAffirmation(users, message, index, writeUsersMenu, 0, 0, "user");
}

static void deleteActionOnAffirmation(List<Dictionary<string, object>> users, string affirmation, 
    int index , Action writeMenu, int indexOfUser, int indexOfTrip, string userOrTrip)
{
    if (affirmation == "da")
    {
        if (userOrTrip == "user") deleteUserAndMessage(users, index);
        else if (userOrTrip == "trip") deleteTripAndMessage(users, indexOfUser, indexOfTrip);
        waitOnKeyPress();
    }
    else
    {
        Console.WriteLine("Odustajanje od brisanja, pritiskom bilo koje tipke vratit ćete se na prijašnji meni");
        Console.ReadKey();
        Console.Clear();
        writeMenu();
    }
}

static void deleteUserByName(List<Dictionary<string, object>> users)
{
    string nameAndSurname, name, firstSurname, secondSurname = "", surname;
    
    do
    {
        Console.Write("Upišite ime i prezime korisnika kojeg želite obrisati: ");
        nameAndSurname = Console.ReadLine();
        
        var arrayWithNameAndSurname = nameAndSurname.Split(" ");
        name =  arrayWithNameAndSurname[0];
        firstSurname = arrayWithNameAndSurname[1];
        if (arrayWithNameAndSurname.Length > 2)
        {
            secondSurname = arrayWithNameAndSurname[2];
            surname = $"{firstSurname} {secondSurname}";
        }
        else surname = firstSurname;
        
        
        var condition = users.Any(dict => 
            (dict.ContainsKey("name") && (string)dict["name"] == name)
            && dict.ContainsKey("surname") && (string)dict["surname"] == surname);
        
        if (!condition) Console.WriteLine($"Korisnik imena {name} {surname} nije pronađen");
        else break;
    } while (true);
    
    int index = users.FindIndex(dict => 
        (dict.ContainsKey("name") && (string)dict["name"] ==  name)
        && dict.ContainsKey("surname") && (string)dict["surname"] == surname);
    int count = users.Count(dict =>
        dict.ContainsKey("name") && (string)dict["name"] == name &&
        dict.ContainsKey("surname") && (string)dict["surname"] == surname);
    
    if (count > 1)
    {
        var indices = users
            .Select((dict, i) => new { dict, i })
            .Where(x =>
                x.dict.ContainsKey("name") && x.dict["name"] == name &&
                x.dict.ContainsKey("surname") && x.dict["surname"] == surname)
            .Select(x => x.i) 
            .ToList();
        
        var usersWithSameName = new List<Dictionary<string, object>>();
        
        for(int i = 0; i < indices.Count; i++)
            usersWithSameName.Add(new()
                {{"id", users[i]["id"]}, {"name", name}, {"surname", surname}, {"dob", users[i]["dob"]}}
        );
        
        writeUsersList(usersWithSameName);
        Console.WriteLine("Postoji više korisnika s tim imenom, preusmjeravanje na brisanje prema id-u: ");
        deleteUserById(users);
    }
    else
    {
        var message = confirmationMessage("izbrisati");
        deleteActionOnAffirmation(users, message, index, writeUsersMenu, 0, 0, "user");
    }
    
}

static void editUserById(List<Dictionary<string, object>> users)
{
    string dataToChange;
    do
    {
        Console.WriteLine("a) Ime: ");
        Console.WriteLine("b) Prezime: ");
        Console.WriteLine("c) Datum rođenja");
        Console.WriteLine("d) Povratak");
        Console.Write("Unesite podatak koji želite izmijeniti: ");
        dataToChange = Console.ReadLine();

    } while (dataToChange.ToLower().Replace(")", "") != "a" 
             && dataToChange.ToLower().Replace(")", "") != "b"
             && dataToChange.ToLower().Replace(")", "") != "c"
             && dataToChange.ToLower().Replace(")", "") != "d");

    if (dataToChange.ToLower().Replace(")", "") != "d")
    {
        do
        {
            Console.Write("Upišite id korisnika kojeg želite obrisati: ");
            string id = Console.ReadLine();
            
            var condition = users.Any(dict => dict.ContainsKey("id") && (string)dict["id"] == id);
            if (!condition) Console.WriteLine($"Korisnik s id-om {id} nije pronađen");
            else
            {
                var dict = new Dictionary<string,string>();
                dict["a"] = "ime";
                dict["b"] = "prezime";
                dict["c"] = "datuma rođenja";
                    
                Console.Write($"(Ažuriranje {dict[dataToChange]}) ");
                    
                if (dataToChange == "a") dataToChange = "name";
                else if (dataToChange == "b") dataToChange = "surname";
                else dataToChange = "dob";
                
                string newValue;
                if (dataToChange == "name") newValue = checkInputValidity("ime");
                else if (dataToChange == "surname") newValue = checkInputValidity("prezime");
                else
                {
                    string message = "Unesite datum rođenja novog korisnika u formatu yyyy-mm-dd: ";
                    newValue = checkDateValidity(message);
                }

                confirmationMessage("izmijeniti");
                editUser(users, dataToChange, newValue, id);
                waitOnKeyPress();
                break;
            }
        } while (true);
    }
}

static string confirmationMessage(string action)
{
    string input;
    do
    {
        // tu mozes generalizirat da vrijedi i za putovanja 
        Console.Write($"Želite li stvarno {action} ovog korisnika/putovanje (da/ne): ");
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

static string checkInputValidity(string nameOrSurname)
{
    do
    {
        Console.Write($"Unesite {nameOrSurname} korisnika: ");
        string name = Console.ReadLine();
        if (string.IsNullOrEmpty(name)) Console.WriteLine($"{nameOrSurname} ne može biti prazno");
        else if (!name.All(c => char.IsLetter(c) || c == ' ' || c == '-'))
            Console.WriteLine($"{nameOrSurname} može sadržavati samo slova ili crticu");
        else return name;
    } while (true);
}

static string checkDateValidity(string message)
{
    var currentDate = DateTime.Now;
    do
    {
        Console.Write(message);
        string DoB = Console.ReadLine();
        var dateTimeValidity = DateTime.TryParse(DoB, out DateTime dateTime);
        if(string.IsNullOrEmpty(DoB)) Console.WriteLine("Datum ne može biti prazan");
        else if (!dateTimeValidity)
            Console.WriteLine("Unesite datum u formatu yyyy-mm-dd");
        else if(currentDate.Year < dateTime.Year)
            Console.WriteLine("Unesena godina ne može biti kasnija od trenutne");
        else return DoB;
    } while (true);
}

static double checkNumberValidity(string message)
{
    do
    {
        Console.Write(message);
        string input =  Console.ReadLine();
        if (string.IsNullOrEmpty(input))
            Console.WriteLine("Ovo polje ne može biti prazno");
        else if (!double.TryParse(input, out double number))
            Console.WriteLine("Unesite broj");
        else return number;
    } while (true);
}

static void userMenuFunctionality(string input,  List<Dictionary<string, object>> users, int idCounter)
{
    switch (input)
    {
        case "1":
        {
            string name = checkInputValidity("ime");
            string surname = checkInputValidity("prezime");
            string message = "Unesite datum rođenja novog korisnika u formatu yyyy-mm-dd: ";
            string DoB = checkDateValidity(message);

            var newUser = addUser(name, surname, DoB, ref idCounter, 
                new List<Dictionary<string, string>>());
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
                writeDeletionOptionsMenu("user");
                parameter = Console.ReadLine();
            }
            while (parameter.ToLower().Replace(")", "") != "a" 
                   && parameter.ToLower().Replace(")", "") != "b"
                   && parameter.ToLower().Replace(")", "") != "c");
            
            if (parameter == "a") deleteUserById(users);
            else if (parameter == "b") deleteUserByName(users);
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
            condition = condition.ToLower().Replace(")", "");
            switch (condition)
            {
                case "a":
                {
                    alphabeticalSortList(users);
                    writeUsersList(users);
                    break;
                }
                case "b":
                {
                    var usersOver20 = returnUsersOver20(users);
                    alphabeticalSortList(usersOver20);
                    writeUsersList(usersOver20);
                    break;
                }
                case "c":
                {
                    var usersWithMultipleTrips = returnUsersWithMutlipleTrips(users);
                    alphabeticalSortList(usersWithMultipleTrips);
                    writeUsersList(usersWithMultipleTrips);
                    break;
                }
                default:
                {
                    Console.WriteLine("Unesite a, b ili c");
                    break;
                }
                    
            }
            waitOnKeyPress();
            break;
        }
    }
}

/*static int fetchUser()
{
    do
    {
        Console.Write("Unesite id ili ime i prezime korisnika: ");
        string input = Console.ReadLine();
        
        
        if (string.IsNullOrEmpty(input))
            Console.WriteLine("Ovo polje ne može biti prazno");
        else if(input.Split(" ").Length > 1 && )
        
        
    } while (true);
    
}*/
static void deleteTripAndMessage(List<Dictionary<string, object>> users, int indexOfUser, int indexOfTrip)
{
    Console.WriteLine("Putovanje je uspješno izbrisano");
    var listOfTrips = (List<Dictionary<string, string>>)users[indexOfUser]["listOfTrips"];
    listOfTrips.RemoveAt(indexOfTrip);
}

static void deleteTripById(List<Dictionary<string, object>> users, double id)
{
    int indexOfTrip = 0, indexOfUser = 0;
    for (int i = 0; i < users.Count; i++)
    {
        var listOfTrips = (List<Dictionary<string, string>>)users[i]["listOfTrips"];
        indexOfTrip = listOfTrips.FindIndex(dict => 
            dict.ContainsKey("id") && double.Parse(dict["id"]) == id);
        indexOfUser = i;
        if (indexOfTrip != -1) break;
    }

    string message = confirmationMessage("izbrisati");
    deleteActionOnAffirmation(users, message, 0, writeTripMenu, indexOfUser, indexOfTrip, "trip");
}

static void writeListOfTrips(List<Dictionary<string, string>> trips)
{
    foreach (var trip in trips)
    {
        Console.WriteLine($"Putovanje {trip["id"]}");
        Console.WriteLine($"Datum: {trip["dot"]}");
        Console.WriteLine($"Kilometri: {trip["mileage"]}");
        Console.WriteLine($"Gorivo: {trip["spentFuel"]}");
        Console.WriteLine($"Cijena po litri: {trip["pricePerLiter"]}");
        Console.WriteLine($"Ukupno: {trip["priceOfTrip"]}\n");
    }
}

static List<Dictionary<string, string>> ExtractTripsFromUsers(List<Dictionary<string, object>> users)
{
    var trips = new List<Dictionary<string, string>>();
    foreach (var user in users)
        foreach (var trip in (List<Dictionary<string, string>>)user["listOfTrips"])
            trips.Add(trip);
    return trips;
}

static void writeListOfTripsInOrder(List<Dictionary<string, string>> trips, string sortKey, string ascOrDesc)
{
    trips.Sort((dict1, dict2) =>
    {
        if (sortKey == "dot")
        {
            DateTime idDict1 = DateTime.Parse(dict1[sortKey]);
            DateTime idDict2 = DateTime.Parse(dict2[sortKey]);
            if(ascOrDesc == "ascending") return idDict1.CompareTo(idDict2);
            else return idDict2.CompareTo(idDict1);
        }
        else
        {
            int idDict1 = int.Parse(dict1[sortKey]);
            int idDict2 = int.Parse(dict2[sortKey]);
            if(ascOrDesc == "ascending") return idDict1.CompareTo(idDict2);
            else return idDict2.CompareTo(idDict1);
        }
    });
    writeListOfTrips(trips);
}


static void tripMenuFunctionality(string input, int idCounterTrip, List<Dictionary<string, object>> users)
{
    switch (input)
    {
        case "1":
        {
            //odi povecat rigor i napravit da moze i ime
            Console.Write("Unesite id ili ime korisnika: ");
            string idOrName = Console.ReadLine();
            int index= int.Parse(idOrName);
            var user = users[index];
            
            
            string message = "Unesite datum putovanja u formatu yyyy-mm-dd: ";
            var dateOfTrip = checkDateValidity(message);
            message = "Unesite kilometražu: ";
            double mileage = checkNumberValidity(message);
            message = "Unesite potrošeno gorivo u litrama: ";
            double spentFuel =  checkNumberValidity(message);
            message = "Unesite cijenu po litri: ";
            double pricePerLiter = checkNumberValidity(message);
            double priceOfTrip = pricePerLiter * spentFuel;
            
            var newTrip = addTrip(dateOfTrip, mileage, spentFuel, 
                priceOfTrip, priceOfTrip, ref idCounterTrip);
            var tripsOfUser = (List<Dictionary<string, string>>)user["listOfTrips"];
            
            tripsOfUser.Add(newTrip);
            
            Console.WriteLine("Putovanje je uspješno dodano");
            waitOnKeyPress();
            break;
        }
        case "2":
        {
            string condition;
            do
            {
                writeDeletionOptionsMenu("trip");
                condition = Console.ReadLine();
                
            } while (condition.ToLower().Replace(")", "") != "a" 
                     && condition.ToLower().Replace(")", "") != "b"
                     && condition.ToLower().Replace(")", "") != "c"
                     && condition.ToLower().Replace(")", "") != "d");

            if (condition.ToLower().Replace(")", "") == "b" || condition.ToLower().Replace(")", "") == "c")
            {
                double price = checkNumberValidity("Unesite cijenu: ");
            }
            else if (condition.ToLower().Replace(")", "") == "a")
            {
                double id = checkNumberValidity("Unesite id: ");
                deleteTripById(users, id);
            }
            break;
        }
        case "4":
        {
            string choice;
            do
            {
                Console.WriteLine("Unesite željeni način sortiranj upisivanjem slova ispred načina sortiranja");
                Console.WriteLine("a) putovanja redom kojim su dodavana");
                Console.WriteLine("b) putovanja po trošku; uzlazno");
                Console.WriteLine("c) putovanja po trošku; silazno");
                Console.WriteLine("d) putovanja po kilometraži; uzlazno");
                Console.WriteLine("e) putovanja po kilometraži; silazno");
                Console.WriteLine("f) putovanja po datumu; uzlazno");
                Console.WriteLine("g) putovanja po datumu; silazno");
                Console.WriteLine("h) povratak");
                Console.Write("Vaš odabir: ");
                choice = Console.ReadLine().ToLower().Replace(")", "");
            } while (choice != "a" && choice != "b" && choice != "c"
                     && choice != "d" && choice != "e" && choice != "f"
                     && choice != "g" && choice != "h");
            var trips = ExtractTripsFromUsers(users);
            switch (choice)
            {
                case "a":
                {
                    writeListOfTripsInOrder(trips, "id", "ascending");
                    waitOnKeyPress();
                    break;
                }
                case "b":
                {
                    writeListOfTripsInOrder(trips, "priceOfTrip", "ascending");
                    waitOnKeyPress();
                    break;
                }
                case "c":
                {
                    writeListOfTripsInOrder(trips, "priceOfTrip", "descending");
                    waitOnKeyPress();
                    break;
                }
                case "d":
                {
                    writeListOfTripsInOrder(trips, "mileage", "ascending");
                    waitOnKeyPress();
                    break;
                }
                case "e":
                {
                    writeListOfTripsInOrder(trips, "mileage", "descending");
                    waitOnKeyPress();
                    break;
                }
                case "f":
                {
                    writeListOfTripsInOrder(trips, "dot", "ascending");
                    waitOnKeyPress();
                    break;
                }
                case "g":
                {
                    writeListOfTripsInOrder(trips, "dot", "descending");
                    waitOnKeyPress();
                    break;
                }
            }
            break;
        }
    }
}

bool onMainMenu = true;
bool onUsersMenu = false;
bool onTripMenu = false;

var users = new List<Dictionary<string, object>>();

int idCounterUsers = 0;
int idCounterTrip = 0;


var trip1User1 = addTrip("2002-05-20", 100, 100, 2, 
    100 * 2, ref idCounterTrip);
var trip2User1 = addTrip("2003-05-20", 110, 150, 2, 
    150 * 2, ref idCounterTrip);
var trip3User1 = addTrip("2004-05-20", 120, 200, 2, 
    200 * 2, ref idCounterTrip);
var listOfTripsUser1 = new List<Dictionary<string, string>>();
listOfTripsUser1.Add(trip1User1);
listOfTripsUser1.Add(trip2User1);
listOfTripsUser1.Add(trip3User1);

var trip1User2 = addTrip("2002-9-26", 100, 250, 2, 
    250 * 2, ref idCounterTrip);
var trip2User2 = addTrip("2002-9-26", 110, 300, 2, 
    300 * 2, ref idCounterTrip);
var listOfTripsUser2 = new List<Dictionary<string, string>>();
listOfTripsUser2.Add(trip1User2);
listOfTripsUser2.Add(trip2User2);


var user1 = addUser("ante", "antic", "2002-05-20", ref idCounterUsers, listOfTripsUser1);
var user2 = addUser("mate", "matic", "2020-06-04", ref idCounterUsers, listOfTripsUser2);

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
        userMenuFunctionality(input, users, idCounterUsers);
    }
    else if (onTripMenu)
    {
        Console.Clear();
        writeTripMenu();
        input = Console.ReadLine();
        tripMenuFunctionality(input, idCounterTrip, users);
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
        onTripMenu = true;
        Console.WriteLine("\nUlazak na meni s putovanjima\n");
    }

    else if (onUsersMenu && input == "0" || onTripMenu && input == "0")
    {
        onUsersMenu = false;
        onMainMenu = true;
        Console.WriteLine("\nPovratak na main menu\n");
    }else Console.WriteLine("Unesite broj koji odgovara brojevima na izborniku");
} while (true);
