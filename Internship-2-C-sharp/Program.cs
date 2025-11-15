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

static void waitOnKeyPress()
{
    Console.WriteLine("Pritisnite bilo koju tipku za nastavak");
    Console.ReadKey();
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

static void editUser(List<Dictionary<string, object>> users, string dataToChange, string newValue, string id)
{
    int index = users.FindIndex(dict => dict.ContainsKey("id") && (string)dict["id"] == id);
    users[index][dataToChange] = newValue;
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
        dataToChange = Console.ReadLine().ToLower().Replace(")", "");

    } while (dataToChange != "a" 
             && dataToChange != "b"
             && dataToChange != "c"
             && dataToChange != "d");

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

                string affirmation = confirmationMessage("izmijeniti");
                editActionOnAffirmation(users, affirmation, int.Parse(id), writeUsersMenu, dataToChange, newValue, 
                    "user");
                break;
            }
        } while (true);
    }
}

static void editTrip(List<Dictionary<string, object>> users, string dataTocChange, string change, double id)
{
    if (dataTocChange == "a") dataTocChange = "dot";
    else if (dataTocChange == "b") dataTocChange = "mileage";
    else if(dataTocChange == "c") dataTocChange = "spentFuel";
    else if(dataTocChange == "d") dataTocChange = "pricePerLiter";

    int index = (int)id;
    
    var wantedTrip =
        users
            .Where(d => d.ContainsKey("listOfTrips") && d["listOfTrips"] is List<Dictionary<string, string>>)
            .SelectMany(d => (List<Dictionary<string, string>>)d["listOfTrips"])
            .FirstOrDefault(inner =>
                inner.ContainsKey("id") &&
                int.Parse(inner["id"]) == index
            );
    wantedTrip[dataTocChange] = change;
    wantedTrip["priceOfTrip"] = (double.Parse(wantedTrip["spentFuel"]) * 
                                 double.Parse(wantedTrip["pricePerLiter"])).ToString();
}

static void editActionOnAffirmation(List<Dictionary<string, object>> users, string affirmation, 
    int index , Action writeMenu, string dataToChange, string newValue, string userOrTrip)
{
    if (affirmation == "da")
    {
        if (userOrTrip == "user") editUser(users, dataToChange, newValue, index.ToString());
        else if (userOrTrip == "trip") editTrip(users, dataToChange, newValue, index);
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

static void deleteUserByName(List<Dictionary<string, object>> users)
{
    var userIndexAndCount = fetchUserByName(users);
    int index = userIndexAndCount[0];
    int count = userIndexAndCount[1];
    
    string name = users[index]["name"].ToString();
    string surname = users[index]["surname"].ToString();

    
    if (count > 1)
    {
        findSameNameUsers(users, name, surname);
        deleteUserById(users);
    }
    else
    {
        var message = confirmationMessage("izbrisati");
        deleteActionOnAffirmation(users, message, index, writeUsersMenu, 0, 0, "user");
    }
    
}

static void deleteTripAndMessage(List<Dictionary<string, object>> users, int indexOfUser, int indexOfTrip)
{
    Console.WriteLine("Putovanje je uspješno izbrisano");
    var listOfTrips = (List<Dictionary<string, string>>)users[indexOfUser]["listOfTrips"];
    listOfTrips.RemoveAt(indexOfTrip);
}

static void deleteUserAndMessage(List<Dictionary<string, object>> users, int index)
{
    Console.WriteLine($"Korisnik {users[index]["name"]} {users[index]["surname"]} je uspješno izbrisan");
    users.RemoveAt(index);
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

static void deleteTripsByPrice(List<Dictionary<string, object>> users, string condition, double value)
{
    if (condition == "expensive")
        foreach (var user in users)
        {
            var TempList = (List<Dictionary<string, string>>)user["listOfTrips"];
            TempList.RemoveAll(dict => double.Parse(dict["priceOfTrip"]) >= value);
        }
    else if (condition == "cheap")
        foreach (var user in users)
        {
            var TempList = (List<Dictionary<string, string>>)user["listOfTrips"];
            TempList.RemoveAll(dict => double.Parse(dict["priceOfTrip"]) <= value);
        }
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

static List<int> fetchUserByName(List<Dictionary<string, object>> users)
{
    string nameAndSurname, name, firstSurname, secondSurname = "", surname;
    do
    {
        Console.Write("Upišite ime i prezime korisnika: ");
        nameAndSurname = Console.ReadLine();
        if (string.IsNullOrEmpty(nameAndSurname))
        {
            Console.WriteLine("Ovo polje ne može biti prazno, unesite ime");
            continue;
        }
        else if (nameAndSurname.Split(" ").Length < 2)
        {
            Console.WriteLine("Unesite ime i prezime");
            continue;
        }
            
        
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
    
    var returnList = new List<int>();
    returnList.Add(index);
    returnList.Add(count);

    return returnList;
}

static int fetchUserById(List<Dictionary<string, object>> users)
{
    int id;
    do
    {
        Console.Write("Unesite id: ");
        string input = Console.ReadLine();
        if (string.IsNullOrEmpty(input))
            Console.WriteLine("Ovo polje ne smije biti prazno");
        else if(!int.TryParse(input, out int number))
            Console.WriteLine("Unos mora biti cijeli pozitivni broj");
        else if(!users.Any(dict => (string)dict["id"] == input))
            Console.WriteLine("Korisnik s unesenim id-om ne postoji");
        else
        {
            users.FindIndex(dict => dict["id"] == input);
            id = int.Parse(input);
            break;
        }
    } while (true);

    return id;
}

static int fetchTripById(List<Dictionary<string, object>> users)
{
    int id;
    var trips = ExtractTripsFromUsers(users);
    do
    {
        Console.Write("Unesite id: ");
        string input = Console.ReadLine();
        if (string.IsNullOrEmpty(input))
            Console.WriteLine("Ovo polje ne smije biti prazno");
        else if(!int.TryParse(input, out int number))
            Console.WriteLine("Unos mora biti cijeli pozitivni broj");
        else if(!trips.Any(dict => dict["id"] == input))
            Console.WriteLine("Putovanje s unesenim id-om ne postoji");
        else
        {
            trips.FindIndex(dict => dict["id"] == input);
            id = int.Parse(input);
            break;
        }
    } while (true);

    return id;
}

static void findSameNameUsers(List<Dictionary<string, object>> users, string name, string surname)
{
    var indices = users
        .Select((dict, i) => new { dict, i })
        .Where(x =>
            x.dict.ContainsKey("name") && (string)x.dict["name"] == name &&
            x.dict.ContainsKey("surname") && (string)x.dict["surname"] == surname)
        .Select(x => x.i) 
        .ToList();
        
    var usersWithSameName = new List<Dictionary<string, object>>();
        
    foreach(var index in indices)
        usersWithSameName.Add(new()
            {{"id", users[index]["id"]}, {"name", name}, {"surname", surname}, {"dob", users[index]["dob"]}}
        );
    writeUsersList(usersWithSameName);
    Console.WriteLine("Postoji više korisnika s tim imenom, prusmjeravanje prema id-u");
}

static List<Dictionary<string, string>> findSameTrips(List<Dictionary<string, string>> trips, string key, string value)
{
    var indices = trips
        .Select((dict, i) => new { dict, i })
        .Where(x =>
            x.dict.ContainsKey(key) && x.dict[key] == value) 
        .Select(x => x.i) 
        .ToList();
    var tripsWithSamevalue = new List<Dictionary<string, string>>();
    foreach (var index in indices)
        tripsWithSamevalue.Add(new()
            {
                {"id", trips[index]["id"]}, {"dot", trips[index]["dot"]}, {"mileage", trips[index]["mileage"]}, 
                {"spentFuel",  trips[index]["spentFuel"]}, {"pricePerLiter", trips[index]["pricePerLiter"]}, 
                {"priceOfTrip", trips[index]["priceOfTrip"]}
            }
        );
    return tripsWithSamevalue;
}

static string confirmationMessage(string action)
{
    string input;
    do
    {
        Console.Write($"Želite li stvarno {action} ovog korisnika/putovanje (da/ne): ");
        input =  Console.ReadLine();
        if(input == "da" || input == "ne") return  input;
        else Console.WriteLine("Molim unesite da ili ne");
    } while (true);
    
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
        else return dateTime.ToString("yyyy-MM-dd");
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

static List<Dictionary<string, string>> ExtractTripsFromUsers(List<Dictionary<string, object>> users)
{
    var trips = new List<Dictionary<string, string>>();
    foreach (var user in users)
        foreach (var trip in (List<Dictionary<string, string>>)user["listOfTrips"])
            trips.Add(trip);
    return trips;
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
            string condition;
            do
            {
                Console.WriteLine("a) abecedni ispis");
                Console.WriteLine("b) korisnici stariji od 20 godina");
                Console.WriteLine("c) korisnici s najmanje 2 putovanja");
                Console.WriteLine("d) povratak");
                Console.Write("Unesite odabir za ispis: ");
                condition = Console.ReadLine().ToLower().Replace(")", "");
                
            } while (condition != "a" && condition != "b" && condition != "c" &&  condition != "d");
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
                case "d":
                {  
                    Console.Clear();
                    writeUsersMenu();
                    break;
                }
                default:
                {
                    Console.WriteLine("Unesite a, b ili c");
                    break;
                }
                    
            }
            if (condition != "d" )waitOnKeyPress();
            break;
        }
    }
}

static void tripMenuFunctionality(string input, int idCounterTrip, List<Dictionary<string, object>> users)
{
    switch (input)
    {
        case "1":
        {
            string condition;
            int index = -1;
            
            do
            {
                Console.WriteLine("Odaberite metodu upisivanjem slova ispred tražene metode");
                Console.WriteLine("a) id");
                Console.WriteLine("b) ime i prezime");
                Console.WriteLine("c) povratak");
                Console.Write("Vaš odabir: ");
                condition =  Console.ReadLine().ToLower().Replace(")", "");
            } while (condition != "a" && condition != "b" &&  condition != "c");

            if (condition == "a")
                index = fetchUserById(users);
            else if (condition == "b")
            {
                var indexAndCount = fetchUserByName(users);
                int count = indexAndCount[1];
                index = indexAndCount[0];
                string name = users[index]["name"].ToString();
                string surname = users[index]["surname"].ToString();

                if (count > 1)
                {
                    Console.WriteLine("Postoji više korisnika s istim imenom");
                    findSameNameUsers(users, name, surname);
                    index = fetchUserById(users);
                }
            }
            else if (condition == "c")
                break;
            
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
                pricePerLiter, priceOfTrip, ref idCounterTrip);
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

            if (condition.ToLower().Replace(")", "") == "b")
            {
                double price = checkNumberValidity("Unesite cijenu: ");
                deleteTripsByPrice(users, "expensive", price);
                var leftOverTrips = ExtractTripsFromUsers(users);
                Console.WriteLine("Preostala putovanja:\n");
                writeListOfTrips(leftOverTrips);
                waitOnKeyPress();
            }
            else if (condition.ToLower().Replace(")", "") == "c")
            {
                double price = checkNumberValidity("Unesite cijenu: ");
                deleteTripsByPrice(users, "cheap",  price);
                var leftOverTrips = ExtractTripsFromUsers(users);
                Console.WriteLine("Preostala putovanja:\n");
                writeListOfTrips(leftOverTrips);
                waitOnKeyPress();
            }
            else if (condition.ToLower().Replace(")", "") == "a")
            {
                double id = checkNumberValidity("Unesite id: ");
                deleteTripById(users, id);
                var leftOverTrips = ExtractTripsFromUsers(users);
                Console.WriteLine("Preostala putovanja:\n");
                writeListOfTrips(leftOverTrips);
                waitOnKeyPress();
            }
            break;
        }
        case "3":
        {
            string condition;
            do
            {
                Console.WriteLine("Upisivanjem slova ispred opcije birate koji podatak mijenjate");
                Console.WriteLine("a) datum");
                Console.WriteLine("b) kilometraža");
                Console.WriteLine("c) potrošeno gorivo");
                Console.WriteLine("d) cijena po litri");
                Console.WriteLine("e) povratak");
                Console.Write("Vaš odabir: ");
                condition = Console.ReadLine().ToLower().Replace(")", "");

            } while (condition != "a" && condition != "b" && condition != "c" && condition != "d" 
                     && condition != "e");
            if (condition == "a")
            {
                double id = fetchTripById(users);
                string change = checkDateValidity("Unesite datum: ");
                string affirmation = confirmationMessage("izmijeniti");
                editActionOnAffirmation(users, affirmation, (int)id, writeTripMenu, condition,
                    change, "trip");
            }
            else if (condition == "b")
            {
                double id = fetchTripById(users);
                double change = checkNumberValidity("Unesite kilometražu: ");
                string affirmation = confirmationMessage("izmijeniti");
                editActionOnAffirmation(users, affirmation, (int)id, writeTripMenu, condition,
                    change.ToString(), "trip");
            }
            else if (condition == "c")
            {
                double id = fetchTripById(users);
                double change = checkNumberValidity("Unesite potrošeno gorivo: ");
                string affirmation = confirmationMessage("izmijeniti");
                editActionOnAffirmation(users, affirmation, (int)id, writeTripMenu, condition,
                    change.ToString(), "trip");
            }
            else if (condition == "d")
            {
                double id = fetchTripById(users);
                double change = checkNumberValidity("Unesite cijenu po litri: ");
                string affirmation = confirmationMessage("izmijeniti");
                editActionOnAffirmation(users, affirmation, (int)id, writeTripMenu, condition,
                    change.ToString(), "trip");
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
        case "5":
        {
            string choice;
            do
            {
                Console.WriteLine("Odabereite željeni podatak upisivanjem slova ispred odabira");
                Console.WriteLine("a) analiza potrošnje goriva");
                Console.WriteLine("b) putovanje s najvećom potrošnjom goriva");
                Console.WriteLine("c) pregled putovanja po datumu");
                Console.WriteLine("d) povratak");
                Console.Write("Vaš odabir: ");
                choice =  Console.ReadLine().ToLower().Replace(")", "");
            } while (choice != "a" && choice != "b" && choice != "c" && choice != "d");

            var userIndexAndCount = fetchUserByName(users);
            var user = users[userIndexAndCount[0]];
            var userToList = new List<Dictionary<string, object>>();
            userToList.Add(user);
            var tripsOfUser = ExtractTripsFromUsers(userToList);
            
            if (choice == "a")
            {
                double totalFuelSpent = tripsOfUser.Sum(dict => double.Parse(dict["spentFuel"]));
                double totalFuelPrice = tripsOfUser.Sum(dict =>  double.Parse(dict["priceOfTrip"]));
                double totalMileage =  tripsOfUser.Sum(dict => double.Parse(dict["mileage"]));
                double averageFuelSpent = (totalFuelSpent / totalMileage) * 100;
                Console.WriteLine($"Prosječna potrošnja goriva na putovanjima je {averageFuelSpent} L/km");
                Console.WriteLine($"Potrošeno je ukupno {totalFuelPrice} eura na gorivo");
                Console.WriteLine($"Potošeno je ukupno {totalFuelSpent} litara goriva");
                waitOnKeyPress();
            }
            else if (choice == "b")
            {
                double maxFuel = tripsOfUser.Max(dict => double.Parse(dict["spentFuel"]));
                int numberOfMax = tripsOfUser.Count(dict => double.Parse(dict["spentFuel"]) == maxFuel);
                if (numberOfMax > 1)
                {
                    var tripsWithSameValue = findSameTrips(tripsOfUser, "spentFuel", maxFuel.ToString());
                    writeListOfTrips(tripsWithSameValue);
                }
                else
                {
                    int index = tripsOfUser.FindIndex(dict => double.Parse(dict["spentFuel"]) == maxFuel);
                    var trip = tripsOfUser[index];
                    var tempListTrips = new List<Dictionary<string, string>>();
                    tempListTrips.Add(trip);
                    writeListOfTrips(tempListTrips);
                }
                waitOnKeyPress();
            }
            else if(choice == "c")
            {
                var date = checkDateValidity("Unesite željeni datum: ");
                int numberOfSameDates = tripsOfUser.Count(dict => 
                    DateTime.Parse(dict["dot"]) == DateTime.Parse(date));
                if (numberOfSameDates > 1)
                {
                    var tripsWithSameDate = findSameTrips(tripsOfUser, "dot", date);
                    writeListOfTrips(tripsWithSameDate);
                }
                else if (numberOfSameDates == 1)
                {
                    int index = tripsOfUser.FindIndex(dict => dict["dot"] == date);
                    var trip = tripsOfUser[index];
                    var tempListTrips = new List<Dictionary<string, string>>();
                    tempListTrips.Add(trip);
                    writeListOfTrips(tempListTrips); 
                }
                else Console.WriteLine("Putovanja danog datuma ne postoje.");
                waitOnKeyPress();
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
