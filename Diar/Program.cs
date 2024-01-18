using Diar;
using System.Diagnostics;
using System.Security.Cryptography;
using Newtonsoft.Json;

//Zkontroluje, jestli json je, pokud ne, vytvoří ho
if (!File.Exists("Udalosti.json"))
{
    File.Create("Udalosti.json");
}

//Vytvoří diář
diar diary = new diar();

//Vloží vše z jsonfile do diáře (viz v Diar.cs)
List<Udalost> data = diary.json_load();

//Pokud jsonfile neni prazdny, tak vse v nem hodi do listu
if (data.Count != 0) {
    foreach (var item in data)
    {
        diary.veci.Add(new Udalost()
        {
            nazev = item.nazev,
            popis = item.popis,
            date = item.date
        });
    }
}

//procedůra na přidávání událostí
void Add()
{
    bool check;
    string title;
    string descript;
    string date;
    DateTime date2;


    //titulek udalosti
    Console.WriteLine("Název události:");
    do
    {
        check = true;
        title = Console.ReadLine();
        if (String.IsNullOrEmpty(title)) // pokud to uzivatel necha prazdny, tak nic nemeni
        {
            Console.WriteLine($"Titulek musí obsahovat alespoň jeden znak");
            check = false;
        }
    } while (check == false);

    //popisek udalosti
    Console.WriteLine("Popis Události (pokud nechcece, nechte prázdný)");
    descript = Console.ReadLine();

    //datum udalosti
    Console.WriteLine("Datum a čas. Pište ve formátu: den/čislo měsíce/rok hodina:minuta:sekunda");
    do
    {
        date = Console.ReadLine();
        check = DateTime.TryParse(date, out date2);
        if (check == false)
        {
            Console.WriteLine($"Špatně napsaný datum");
        }
    } while (check == false);

    //prida do listu novou udalost
    diary.veci.Add(new Udalost()
    {
        nazev = title,
        popis = descript,
        date = date2
    });
    Console.WriteLine("Přidáno!");
    Console.WriteLine($"------------------------------");
    Console.WriteLine($"Stiskněte libovolne tlačítko pro vrácení do menu");
    Console.ReadKey();
}

//procedůra na upravů událostí
void Edit()
{
    int a = -1;
    string input;
    int id;
    bool check;
    bool loop = true;
    string date;
    DateTime date2;

    if (diary.veci.Count == 0) //zkontroluje, jestli diar neni prazdny
    {
        loop = false;
        Console.WriteLine($"Žádné události jsou zapsané, zkuste nějaké vytvořit");
    }
    else
    {
        Console.WriteLine($"Napište číslo vedle udalosti, které chcete upravit");
    }

    while (loop == true)
    {
        a = -1;
        foreach (var item in diary.veci) //vypise vse z listu 
        {
            a++;
            if (item.popis == "")
            {
                Console.WriteLine($"{a} || {item.nazev} {item.date}");
            }
            else
            {
                Console.WriteLine($"{a} || {item.nazev} {item.popis} {item.date}");
            }
        }
        input = Console.ReadLine();

        Console.Clear();

        check = int.TryParse(input, out id); // zkontroluje jestli, to co uzivatel zadal je cislo

        if (check == false)
        {
            Console.WriteLine($"Pište pouze číslice");
        }
        else if (id > a || id < 0) // a jestli to neni nesmyslny cislo
        {
            Console.WriteLine($"Číslo se neschodovalo s žádním id");
        }
        else
        {
            loop = false;
            Console.WriteLine("Název události:"); // zmena titulku
            string title = Console.ReadLine();
            if (String.IsNullOrEmpty(title)) // pokud to uzivatel necha prazdny, tak nic nemeni
            {                                       
                title = diary.veci[id].nazev;
            }

            Console.WriteLine("Popis Události (pokud nechcece, nechte prázdný)");
            string descript = Console.ReadLine(); // zmena titulku
            if (String.IsNullOrEmpty(descript))
            { // pokud to uzivatel necha prazdny, tak nic nemeni
                descript = diary.veci[id].popis;
            }

            Console.WriteLine("Datum a čas. Pište ve formátu: den/čislo měsíce/rok hodina:minuta:sekunda");
            do
            {
                date = Console.ReadLine(); // zmena datumu
                check = DateTime.TryParse(date, out date2);
                if(date == "") // pokud to uzivatel necha prazdny, tak nic nemeni
                {
                    check = true;
                    date2 = diary.veci[id].date;
                }
                else if (check == false) // kontroluje jestli zadany datum je validny
                {
                    Console.WriteLine($"Špatně napsaný datum");
                }
            } while (check == false);

            //zmena udalosti
            diary.veci[id].nazev = title;
            diary.veci[id].popis = descript;
            diary.veci[id].date = date2;

            Console.WriteLine($"Událost upravena");
        }
    }

    Console.WriteLine($"------------------------------");
    Console.WriteLine($"Stiskněte libovolne tlačítko pro vrácení do menu");
    Console.ReadKey();
}

//procedůra na mazání událostí
void Delete()
{
    int a;
    string input;
    int id;
    bool check;
    bool loop = true;

    if (diary.veci.Count == 0) //zkontroluje, jestli diar neni prazdny
    {
        loop = false;
        Console.WriteLine($"Žádné události jsou zapsané, zkuste nějaké vytvořit");
    } else
    {
        Console.WriteLine($"Napište číslo vedle udalosti, které chcete upravit");
    }

    while (loop == true)
    {
        a = -1;
        foreach (var item in diary.veci) //vypise vse z listu
        {
            a++;
            if (item.popis == "")
            {
                Console.WriteLine($"{a} || {item.nazev} {item.date}");
            }
            else 
            {
                Console.WriteLine($"{a} || {item.nazev} {item.popis} {item.date}");
            }
        }

        input = Console.ReadLine();
        check = int.TryParse(input, out id);

        if (check == false) // zkontroluje jestli, to co uzivatel zadal je cislo
        {
            Console.WriteLine($"Pište pouze číslice");
        }
        else if (id > a || id < 0) // a jestli to neni nesmyslny cislo
        {
            Console.WriteLine($"Číslo se neschodovalo s žádním id");
        }
        else //vymaze udalost z diare
        {
            loop = false;
            Console.WriteLine($"{diary.veci[id].nazev} byl vymazaní z diáře");
            diary.veci.RemoveAt(id);
        }
    }
    Console.WriteLine($"------------------------------");
    Console.WriteLine($"Stiskněte libovolne tlačítko pro vrácení do menu");
    Console.ReadKey();
}

//procedůra na ukončení a uložení diáře do jsonfile (viz v Diar.cs)
void Kill()
{
    diary.json_ins();
    System.Environment.Exit(1);
}

//procedůra, která vezme vše z diáře a uživateli ukáže
void Select()
{
    if (diary.veci.Count == 0) //zkontroluje, jestli diar neni prazdny
    {
        Console.WriteLine($"Žádné události jsou zapsané, zkuste nějaké vytvořit");
    }
    else
    {
        foreach (var item in diary.veci) //vypise vse z listu
        {
            if (item.popis == "")
            {
                Console.WriteLine($"{item.nazev} {item.date}");
            }
            else
            {
                Console.WriteLine($"{item.nazev} {item.popis} {item.date}");
            }
        }
    }
    Console.WriteLine($"------------------------------");
    Console.WriteLine($"Stiskněte libovolne tlačítko pro vrácení do menu");
    Console.ReadKey();
}

//menu
string answer;

Console.WriteLine($"Vítejte, co chcete dneska udělat");

while (true)
{
    Console.WriteLine($"(P)řidat událost");
    Console.WriteLine($"(S)mazat událost");
    Console.WriteLine($"(U)pravit událost");
    Console.WriteLine($"(Z)obrazit události");
    Console.WriteLine($"(K)onec");

    answer = Console.ReadLine(); //odpoved uzivatele
    Console.Clear();
    switch (answer)
    {
        case "p" or "P":
            Add();
            break;
        case "s" or "S":
            Delete();
            break;
        case "u" or "U":
            Edit();
            break;
        case "z" or "Z":
            Select();
            break;
        case "k" or "K":
            Kill();
            break;
        default:
            break;
    }
    Console.Clear();
    Console.WriteLine("Chcete dneska ještě něco udělat?");
}
