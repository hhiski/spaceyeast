using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Names 
{

    System.Random Random = new System.Random();
    List<string> customStarNames = new List<string>
    {
        "Alfalfa",
        "Aranyanrpati",
        "Xolotl",
        "Iskierka",
        "Lykos-Iklil",
        "Gluonfree",
        "Shechaqim Posterior",
        "Lazulum Australis",
        "Shamash 124",
        "Cat 73",
        "Apricot",
        "Carmichael",
        "Inkberry",
        "Sellafield",
        "Spurdog",
        "Ammenepthes",
        "Ouagasis",
        "Shapash",
        "Utnapishtim 42",
        "Glaze",
        "Khatlon Extremis",
        "Kayrakkum",
        "Hooghvorst",
        "Charybdis",
        "Yashilkul",
        "Kelipot-Nbu",
        "Copacati",
        "Moshueshue",
        "Inlacrimas",
        "Quibblesnoot 102",
        "Hipparchus",
        "Pancake Star",
        "Bergljot",
        "Spindlewax",
        "Kubek",
        "Tabby",
        "Kazilik",
        "Qaraqorum",
        "Shubankari 6",
        "Triptychon 156",
        "Gamma Vermis",
        "Alpha Scarabaeorum",
        "Alpha Pantherae",
        "Beta Catti",
        "Zeta Sciuri",
        "Delta Sciuri",
        "Mu Cedrorum",
        "Gamma Monocerotis",
        "Omicron Hydrae",
        "Epsilon Carotae",
        "Theta Picae",
        "Lambda Desidiae",
        "Gamma Furcinae",
        "Rho Anguium",
        "Xi Delphini",
        "Zeta Ciconiarum",
        "Zeta Struthionis",
        "Alpha Scapulae",
        "Omicron Lactucae",
        "Iota Raphani",
        "Beta Haliaeeti",
        "Epsilon Viriditatis",
        "Theta Acuarii",
        "Gamma Epinyctidum",
        "Upsilon Zygris",
        "Alpha Theagenis",
        "Delta Ziziphi",
        "Delta Saocorae",
        "Alpha Cameli",
    };
    List<string> customZoneInOrderNames = new List<string>
    {
        "Goat-fish Formation",
        "Coreward Frontier",
        "Ereshkigal Supergroup",
        "Salamandra Sector",
        "Chandrasekhar Density",
        "Psidium-Sucrose Stream",
        "Myrmecoleonic Bridge",
        "Arimaspi Sector",
        "Spider Cluster",
        "Tarellond-Ashe Trail",
        "Taklamakan Sector",
        "Banana Nebula",
        "Chicken Nest Nebula",
        "Cat's Ear Nebula",
        "Crocodilus Rift",
        "Olgoi-Khorkhoi Cloud",
    };


    public string getRandomZoneName()
    {
        string name = "unnamed";
        int namesLeft = customZoneInOrderNames.Count;

        if (namesLeft >= 1)
        {

            int randomIndex = Random.Next(0, namesLeft);
            name = customZoneInOrderNames[randomIndex];
            customZoneInOrderNames.RemoveAt(randomIndex);
        }
        else
        {
            name = getRandomStarDesignation(); // replace later okey
        }
        return name;
    }

    public string getPlanetName(string starName, int order)
    {

        int asciiLetterNumber = order + 65;
        string planetOrderLetter = ((char)asciiLetterNumber).ToString();
        string name = starName + " " + planetOrderLetter;

        return name;
    }


    public string getRandomStarDesignation()
    {
        string start = ((char)Random.Next('A', 'Z')).ToString() + ((char)Random.Next('A', 'Z')).ToString() + ((char)Random.Next('A', 'Z')).ToString();
        string middle = " " + ((int)Random.Next(000, 999)).ToString();
        string end = " " + ((int)Random.Next(1000, 9999)).ToString();
        string designation = start + middle + end;

        return designation;
    }
   
    public string getRandomStarName()
    {
        string name = "unnamed";
        int namesLeft = customStarNames.Count;

        if (namesLeft >= 1) {

            int randomIndex = Random.Next(0, namesLeft);
            name = customStarNames[randomIndex];
            customStarNames.RemoveAt(randomIndex);
        }
        else
        {
            name = getRandomStarDesignation();
        }



        return name;
    }


}
