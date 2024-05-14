using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CelestialBody;

/*
Geodescription	Atmosphereless	SuperHot	    Greenhouse
Atmosphere	    Geodescription	Geodescription	Geodescription
Water	        Water		
Quirk/Life	    Quirk/Life	    Quirk/Life	    Quirk/Life
*/

public class TextComponent
{
    public string Title;
    public string Text;
    public string GasName;
    public string WaterSourceName;
    public float TempMax;
    public float TempMin;
    public float PressMax;
    public float PressMin;
    public int WaterMax;
    public int WaterMin;
    public int BiosphereLevelMax;
    public int BiosphereLevelMin;
    public int Priority;

    public string Type;
    public string SurfaceType;

    public TextComponent()
    {
        Title = "None";
        Text = "None";
        Type = "ANY";
        GasName = "ANY";
        WaterSourceName = "ANY";
        SurfaceType = "ANY";
        TempMax = 9999f;
        TempMin = -9999;
        PressMax = 99999;
        PressMin = -9999;
        WaterMax = 99999;
        WaterMin = -999;
        BiosphereLevelMax = 99;
        BiosphereLevelMin = -99;
        Priority = 0;
    }
  
}
class TextComponents
{
    static readonly TextComponent empty = new TextComponent { Text = "s" };
    /*
static readonly TextComponent exoA = new TextComponent { Text = "The barren, atmosphereless planet.", WaterMax = 1, PressMax = 0.01f, BiosphereLevelMax = 0 };
static readonly TextComponent exoB = new TextComponent { Text = "A frozen, lifeless world", WaterMin = 2, BiosphereLevelMax = 0, TempMax = -80f };
static readonly TextComponent exoC = new TextComponent { Text = "A desolate, airless void", WaterMax = 1, PressMax = 0.01f, BiosphereLevelMax = 0 };
static readonly TextComponent exoD = new TextComponent { Text = "A bleak, icy wasteland.", WaterMin = 2, TempMax = -80f };
static readonly TextComponent exoE = new TextComponent { Text = "A frozen, airless world.", WaterMin = 2, PressMax = 0.01f, TempMax = -80f };
static readonly TextComponent exoF = new TextComponent { Text = "A uncomforably cold world.",  TempMax = -80f };
static readonly TextComponent exoG = new TextComponent { Text = "A sterile, airless world of barren rock.",  PressMax = 0.01f, BiosphereLevelMax = 0 };
static readonly TextComponent exoH = new TextComponent { Text = "A scorched, lifeless world of intense heat.",  TempMin = 200f, BiosphereLevelMax = 0 };
static readonly TextComponent exoI = new TextComponent { Text = "A rocky wasteland of sun-baked desolation.", WaterMax = 2, TempMin = 200f, };
static readonly TextComponent exoJ = new TextComponent { Text = "A hellscape of molten rock and dust.", TempMin = 200f, };
static readonly TextComponent exoK = new TextComponent { Text = "A barren, cratered world with no atmosphere or signs of life.", WaterMax = 1, PressMax = 0.01f, BiosphereLevelMax = 0 };

static readonly TextComponent rockA = new TextComponent { Text = "A fairly typical mix of rock and ice.", WetnessMax = 0.3f, PressMax = 0.01f, BiosphereLevelMax = 0 };
static readonly TextComponent rockB = new TextComponent { Text = "a chilly rock world.", WetnessMax = 0.3f, PressMax = 0.01f, BiosphereLevelMax = 0 };
static readonly TextComponent rockC = new TextComponent { Text = "A barren, cratered world with no atmosphere or signs of life.", WetnessMax = 0.3f, PressMax = 0.01f, BiosphereLevelMax = 0 };


    static readonly TextComponent AtmOxygenNitrogenA = new TextComponent { Text = "A stable and temperate atmosphere of breathable gasses. ",                           WaterMin = 2, WaterMax = 2, PressMin = 0.5f, PressMax = 1.5f, TempMin = 5, TempMax = 50, GasName = "Nitrogen-Oxygen" };
    static readonly TextComponent AtmOxygenNitrogenB = new TextComponent { Text = "The highly oxygenic atmosphere is breathable.",                                      PressMin = 2, PressMax = 2, TempMin = 5, TempMax = 50, GasName = "Nitrogen-Oxygen" };
    static readonly TextComponent AtmOxygenNitrogenC = new TextComponent { Text = "The air is breathable but causes headaches under long exposure.",                    PressMin = 2, PressMax = 2, TempMin = 5, TempMax = 50, GasName = "Nitrogen-Oxygen" };
    static readonly TextComponent AtmOxygenNitrogenD = new TextComponent { Text = "Has a thick humid atmosphere with constant rainfall and mist.",                      WaterMin = 2, WaterMax = 3, PressMin = 0.7f, PressMax = 1.5f, TempMin = 5, TempMax = 50, GasName = "Nitrogen-Oxygen" };
    static readonly TextComponent AtmOxygenNitrogenE = new TextComponent { Text = "The damp misty air permiates everywhere.",                                           WaterMin = 2, WaterMax = 3, PressMin = 0.7f, PressMax = 1.5f, TempMin = 5, TempMax = 50, GasName = "Nitrogen-Oxygen" };
    static readonly TextComponent AtmOxygenNitrogenF = new TextComponent { Text = "Breatable but uncomforably hot and wet atmosphere.",                                 WaterMin = 2, WaterMax = 3, PressMin = 0.7f, PressMax = 1.5f, TempMin = 35, TempMax = 80, GasName = "Nitrogen-Oxygen" };
    static readonly TextComponent AtmOxygenNitrogenG = new TextComponent { Text = "Frequent rainfall, with heavy rain showers or thunderstorms occurring frequently.",  WaterMin = 2, WaterMax = 3, PressMin = 0.5f, PressMax = 1.5f, TempMin = 5, TempMax = 50, GasName = "Nitrogen-Oxygen" };
    static readonly TextComponent AtmOxygenNitrogenH = new TextComponent { Text = "High levels of atmospheric moisture keeps the planet mist-shrouded and cloudy.",     WaterMin = 2, WaterMax = 3, PressMin = 0.7f, PressMax = 1.5f, TempMin = 5, TempMax = 50, GasName = "Nitrogen-Oxygen" };
    static readonly TextComponent AtmOxygenNitrogenI = new TextComponent { Text = "The climate is dangerously warm unsheltered during daylight hours.",                 PressMin = 0.5f, PressMax = 1.5f, TempMin = 35, TempMax = 80, GasName = "Nitrogen-Oxygen" };

    static readonly TextComponent AtmColdDryA = new TextComponent { Text = "Cold and dry atmosphere, snow and surface ice being only source of water",                  PressMin = 0.4f, PressMax = 1.5f, TempMax = 0, WaterSourceName = "Surface Ice"};
    static readonly TextComponent AtmColdDryB = new TextComponent { Text = "All the surface water stays frozen throughout the year.",                                   PressMin = 0.4f, PressMax = 1.5f, TempMax = 0, WaterSourceName = "Surface Ice" };
    static readonly TextComponent AtmColdDryC = new TextComponent { Text = "Thin atmosphere rarely rises high enough for liquid water to exist on the icy surface.",    PressMin = 0.1f, PressMax = 0.5f, TempMax = 0, WaterSourceName = "Surface Ice" };
    static readonly TextComponent AtmColdDryD = new TextComponent { Text = "All the water exist as miniscule ice crystals in the upper atmosphere.",                    PressMin = 0.1f, PressMax = 10f, TempMax = 300, WaterSourceName = "Water Vapor" };
    static readonly TextComponent AtmColdDryE = new TextComponent { Text = "The planet is completely dry, with only traces of atmospheric water vapor.",                PressMin = 0.1f, PressMax = 10f, TempMax = 300, WaterSourceName = "Water Vapor" };
    static readonly TextComponent AtmColdDryF = new TextComponent { Text = "Planet's polar regions contains massive ice caps consisting of water ice.",                 PressMin = 0.1f, PressMax = 10f, TempMax = 100, WaterSourceName = "Polar Ice Caps" };
    static readonly TextComponent AtmColdDryG = new TextComponent { Text = "Polar ice caps are only constant source of frozen water.",                                  PressMin = 0.1f, PressMax = 10f, TempMax = 100, WaterSourceName = "Polar Ice Caps" };
    static readonly TextComponent AtmColdDryH = new TextComponent { Text = "Planet's polar zones have a large formations of water and carbon dioxide ice",              PressMin = 0.1f, PressMax = 4f, TempMax = -100, WaterSourceName = "Polar Ice Caps" };
    static readonly TextComponent AtmColdDryI = new TextComponent { Text = "The planet has a complex hydrosphere of liquid water.",                                     PressMin = 0.4f, PressMax = 4f, TempMax = 100, WaterSourceName = "Fresh Water" };


*/
    /*


            static readonly HydrosphereSource NoWater = new HydrosphereSource { Name = "None", Priority = 1 };
       static readonly HydrosphereSource WaterVapor = new HydrosphereSource { Name = "Water Vapor", TempMax = 800, PressMin = 0.3f, WaterMax = 0, Priority = 2 };
       static readonly HydrosphereSource PolarIceCaps = new HydrosphereSource { Name = "Polar Ice Caps", TempMax = 30, WaterMin = 1, Priority = 3 };
       static readonly HydrosphereSource SurfaceIce = new HydrosphereSource { Name = "Surface Ice", TempMax = 0, WaterMin = 2,  Priority = 4 };
       static readonly HydrosphereSource SeaWater = new HydrosphereSource { Name = "Sea Water", TempMin = -4, TempMax = 100, WaterMin = 3, PressMin = 0.1f, Priority = 5 };
       static readonly HydrosphereSource FreshWater = new HydrosphereSource Gaseous{ Name = "Fresh Water", TempMin = 0, TempMax = 100, WaterMin = 2, PressMin = 0.2f, Priority = 7 };

           */
    /*

 static readonly TextComponent variedRock1 = new TextComponent { Text = "Silicatic", SurfaceType = "silicate" };
 static readonly TextComponent barrenSilicate1 = new TextComponent { Text = "Silicatic", SurfaceType = "silicate" };
 static readonly TextComponent iceRock1 = new TextComponent { Text = "Water-ice", TempMax = 0, SurfaceType = "ice" };
 static readonly TextComponent iceRock2 = new TextComponent { Text = "Dry ice", TempMax = -80, SurfaceType = "ice" };
 static readonly TextComponent iceRock3 = new TextComponent { Text = "Methane hydrate", SurfaceType = "ice" };
 static readonly TextComponent iceRock4 = new TextComponent { Text = "Methane ice",TempMax = -180, SurfaceType = "ice" };
 static readonly TextComponent pnictogen1 = new TextComponent { Text = "Metalloid", SurfaceType = "pnictogen" };
 static readonly TextComponent gas1 = new TextComponent { Text = "Gaseous",SurfaceType = "gas" };
 static readonly TextComponent metal1 = new TextComponent { Text = "Metallic",  SurfaceType = "metal" };
 static readonly TextComponent dust1 = new TextComponent { Text = "Silicates",  SurfaceType = "dust" };
 static readonly TextComponent lava1 = new TextComponent { Text = "Melting Rock",  SurfaceType = "lava" };
 static readonly TextComponent carbide1 = new TextComponent { Text = "Carbidic", SurfaceType = "carbide" };
 static readonly TextComponent chromic1 = new TextComponent { Text = "Metallic", SurfaceType = "chromic" };
 static readonly TextComponent salt1 = new TextComponent { Text = "Salty", SurfaceType = "salt" };
 static readonly TextComponent slime1 = new TextComponent { Text = "Slimy", SurfaceType = "slime" };*/

    /*
    static readonly TextComponent water5 = new TextComponent { Text = "Glacial Ice", WaterSourceName = "Surface Ice" };
    static readonly TextComponent water1 = new TextComponent { Text = "Lots of water", WaterSourceName = "Fresh Water" };
    static readonly TextComponent water2 = new TextComponent { Text = "Salt-Water Oceans", TempMax = 0, SurfaceType = "Sea Water" };
    static readonly TextComponent water4 = new TextComponent { Text = "Watery", WaterSourceName = "Fresh Water" };
    static readonly TextComponent random1 = new TextComponent { Text = "Subsurface Oceans", SurfaceType = "ice" };
    static readonly TextComponent random2 = new TextComponent { Text = "Unstable Tectonics" };
    static readonly TextComponent random4 = new TextComponent { Text = "High Gravity" };
    static readonly TextComponent random5 = new TextComponent { Text = "Low Gravity" };
    static readonly TextComponent random6 = new TextComponent { Text = "Irregular Magnetics" };
    static readonly TextComponent random7 = new TextComponent { Text = "Geothermal Vents" };
    static readonly TextComponent random8 = new TextComponent { Text = "Irradiated Terrain" };
    static readonly TextComponent random9 = new TextComponent { Text = "Diamond Outcrops" };
    static readonly TextComponent random10 = new TextComponent { Text = "Sticky surface" };
    static readonly TextComponent random11 = new TextComponent { Text = "Graphite Dust" };
    static readonly TextComponent random12 = new TextComponent { Text = "Super Fluids" };
    static readonly TextComponent random14 = new TextComponent { Text = "Glowing Rocks" };
    static readonly TextComponent random15 = new TextComponent { Text = "Quicksilver Puddles" };
    static readonly TextComponent random16 = new TextComponent { Text = "Liquid crystals" };
    static readonly TextComponent random17 = new TextComponent { Text = "Glassy terrain" };
    static readonly TextComponent random18 = new TextComponent { Text = "Slimy terrain" };
    static readonly TextComponent random20 = new TextComponent { Text = "Hydrocarbons" };
    static readonly TextComponent random21 = new TextComponent { Text = "Toxic soil" };*/
    /*
     *     public static readonly SurfaceType Silicate = new SurfaceType(name: "Silicate", metalLevel: 1, radioactivesLevel: 1, organicsLevel: 0);
    public static readonly SurfaceType Pnictogen = new SurfaceType(name: "Metallic", metalLevel: 3, radioactivesLevel: 2, organicsLevel: 0);
    public static readonly SurfaceType Dust = new SurfaceType(name: "Silicate", metalLevel: 1, radioactivesLevel: 1, organicsLevel: 0);
    public static readonly SurfaceType Ice = new SurfaceType(name: "Ice", metalLevel: 0, radioactivesLevel: 0, organicsLevel: 1);
    public static readonly SurfaceType Gas = new SurfaceType(name: "Gas", metalLevel: 0, radioactivesLevel: 0, organicsLevel: 0);
    public static readonly SurfaceType Lava = new SurfaceType(name: "Silicate", metalLevel: 2, radioactivesLevel: 2, organicsLevel: 0);
    public static readonly SurfaceType Metallic = new SurfaceType(name: "Metallic", metalLevel: 3, radioactivesLevel: 2, organicsLevel: 0);
    public static readonly SurfaceType Carbide = new SurfaceType(name: "Carbide", metalLevel: 1, radioactivesLevel: 1, organicsLevel: 1);
    public static readonly SurfaceType Chromic = new SurfaceType(name: "Metallic", metalLevel: 1, radioactivesLevel: 1, organicsLevel: 0);
    public static readonly SurfaceType Salt = new SurfaceType(name: "Salt", metalLevel: 1, radioactivesLevel: 1, organicsLevel: 1);
    public static readonly SurfaceType Slime = new SurfaceType(name: "Slime", metalLevel: 0, radioactivesLevel: 0, organicsLevel: 3);
    public static readonly SurfaceType Hydrocarbon = new SurfaceType(name: "Hydrocarbon", metalLevel: 1, radioactivesLevel: 1, organicsLevel: 3);
    public static readonly SurfaceType Transuranic = new SurfaceType(name: "Transuranic", metalLevel: 2, radioactivesLevel: 3, organicsLevel: 0);*/


    //Primary surface elements
    static readonly TextComponent rock1 = new TextComponent { Title = "Graphite Rocks", Text = "Occasional outcroppings of graphite and diamond.", SurfaceType = "Carbide" };
    static readonly TextComponent rock2 = new TextComponent { Title = "Salt Flats", Text = "Sterile featureless salt flats", Type = "Nitrate", BiosphereLevelMax = 0 };
    static readonly TextComponent rock3 = new TextComponent { Title = "Hydrocloric Liquids", Text = "Surface covered in lakes of hydrochloric acid and slurry of dissolved minerals.", Type = "Acid" };
    static readonly TextComponent rock4 = new TextComponent { Title = "Slippery terrain", Text = "The ground is slick with oily sludge and muck.", Type = "Asphalt", TempMin = 40 };
    static readonly TextComponent rock5 = new TextComponent { Title = "Unstable Tectonics", Text = "Terrain is shaky and prone to massive earthquakes." };
    static readonly TextComponent rock6 = new TextComponent { Title = "Collapsing ecosystem", Text = "Undergoing a mass extinction event." };
    static readonly TextComponent rock7 = new TextComponent { Title = "Basalt environment", Text = "The surface dominated by basaltic lava rocks and volcanic ash.", SurfaceType = "Silicate" };

    static readonly TextComponent rock8 = new TextComponent { Title = "Super-Volcanic", Text = "Volcanic eruptions constantly reshape the landscape, sending clouds of ash and gas high into the atmosphere" };
    static readonly TextComponent rock9 = new TextComponent { Title = "Melting surface", Text = "Surface temperatures high enough to melt rock.", TempMin = 900 };

    static readonly TextComponent rock10 = new TextComponent { Title = "Lots of Water", Text = "Surface has nearly 100 % constant relative humidity with lakes, rives and puddles everywhere.", PressMin = 0.5f, WaterMin = 2 };
    static readonly TextComponent aliens1 = new TextComponent { Title = "Primordial Soups", Text = "Mysterious concoctions of wiggly organic molecules.", BiosphereLevelMin = 1, Priority = 1 };
    static readonly TextComponent aliens2 = new TextComponent { Title = "Microbial Slurry", Text = "Translucent amorphous blobs of organic matter can be seen floating on planet's warm oceans.", BiosphereLevelMin = 1, Priority = 1, WaterMin = 2 , TempMin = 20};
    static readonly TextComponent aliens3 = new TextComponent { Title = "Predatory Amoeboids", Text = "Transparent ooze-like predatory ameboids lurks in murky waters and swamps",   BiosphereLevelMin = 1, Priority = 1, WaterMin = 2, TempMin = 0 };
    static readonly TextComponent aliens4 = new TextComponent { Title = "Sea Sponges", Text = "These chemosynthesis cell colonies can reach dozen of meters high form the deep sea floor. ", BiosphereLevelMin = 1, Priority = 1, WaterMin = 2};
    static readonly TextComponent aliens5 = new TextComponent { Title = "Ferocious bugs", Text = "Lifeforms on this planet are ruthless in their quest for resources, using any means necessary to outcompete their neighbors and survive.", BiosphereLevelMin = 1, Priority = 1};
    static readonly TextComponent aliens6 = new TextComponent { Title = "Pink Slime", Text = "All life on the planet is composed of a pink, gelatinous slime. ", BiosphereLevelMin = 1, Priority = 1,};
    static readonly TextComponent aliens7 = new TextComponent { Title = "Lobster Activity", Text = "The ecology of a planet solely inhabited by alien lobster-like creatures. ", BiosphereLevelMin = 2, WaterMin = 2 };

/*
                  "A gas giant with a fiery atmosphere, glowing with intense heat and radiation. Its surface is bathed in a searing glow, making it a perilous place to explore.",

                "A gas giant with a massive, global ocean, warmed by tidal forces and geothermal activity.",
                "A gas giant with an incredibly strong magnetic field that pulls in charged particles from its surroundings, creating spectacular auroras.",
                "A gas giant's lower atmosphere consist of foamy and bubbly substance despite the high pressure environment.",
                "A gas giant mostly composed of liquid metallic hydrogen.",
                "A gas giant under a constant storm and rain fo high speed molten glass shards .",
                "A gas giant with a massive and persistent cyclonic storms that never dissipates.",
                "A eerie whistling and low rumbling eminates deep from hidden, lower atmosphere.",
                "A puffy planet, coreless low density planet of heated gas and floating dust"
                */

    /*
    public readonly PlanetaryFeature PrimordialSoup = new PlanetaryFeature { Name = "Primordial Soup", Rarity = 1, TemperatureRange = new float[] { -5f, 150f }, BiosphereLevelRange = new int[] { 1, 1 } };
    public readonly PlanetaryFeature MicrobialSlurry = new PlanetaryFeature { Name = "Microbial Slurry", Rarity = 1, BiosphereLevelRange = new int[] { 1, 1 } };
    public readonly PlanetaryFeature WigglyPolymers = new PlanetaryFeature { Name = "Wiggly Polymers", Rarity = 1, BiosphereLevelRange = new int[] { 1, 1 } };
    public readonly PlanetaryFeature GiantAmoeboids = new PlanetaryFeature { Name = "Giant Amoeboids", BiosphereLevelRange = new int[] { 1, 1 } };
    public readonly PlanetaryFeature StickyJelly = new PlanetaryFeature { Name = "Sticky Jelly", Rarity = 1, BiosphereLevelRange = new int[] { 1, 1 } };
    public readonly PlanetaryFeature ReplicatingProteins = new PlanetaryFeature { Name = "Replicating Proteins", Rarity = 1, BiosphereLevelRange = new int[] { 1, 1 } };
    public readonly PlanetaryFeature AminoAcids = new PlanetaryFeature { Name = "Amino Acids", Rarity = 1, BiosphereLevelRange = new int[] { 1, 1 } };
    public readonly PlanetaryFeature SeaSnot = new PlanetaryFeature { Name = "Sea Snot", Rarity = 1, BiosphereLevelRange = new int[] { 1, 1 } };
    public readonly PlanetaryFeature MicrobialMat = new PlanetaryFeature { Name = "Microbial Mat", Rarity = 1, BiosphereLevelRange = new int[] { 1, 1 } };
    public readonly PlanetaryFeature PredatorySponges = new PlanetaryFeature { Name = "Predatory Sponges", Rarity = 1, BiosphereLevelRange = new int[] { 1, 1 } };
    public readonly PlanetaryFeature ToxicSpores = new PlanetaryFeature { Name = "Toxic Spores", Rarity = 1, BiosphereLevelRange = new int[] { 1, 1 } };
    public readonly PlanetaryFeature RadiotrophicYeast = new PlanetaryFeature { Name = "Radiotrophic Yeast", Rarity = 1, BiosphereLevelRange = new int[] { 1, 1 } };
    public readonly PlanetaryFeature FermentingLiquids = new PlanetaryFeature { Name = "Fermenting Liquids", Rarity = 1, BiosphereLevelRange = new int[] { 1, 1 } };
    public readonly PlanetaryFeature ParasiticSpiralloid = new PlanetaryFeature { Name = "Parasitic Spiralloids", Rarity = 1, BiosphereLevelRange = new int[] { 1, 1 } };
    public readonly PlanetaryFeature PinkSlime = new PlanetaryFeature { Name = "Pink Slime", Rarity = 1, BiosphereLevelRange = new int[] { 1, 1 } };
    public readonly PlanetaryFeature BubblingSwamp = new PlanetaryFeature { Name = "Bubbling Swamp", Rarity = 1, BiosphereLevelRange = new int[] { 1, 1 }, LocationType = "Land" };
    public readonly PlanetaryFeature BlueGreenDiazotrophs = new PlanetaryFeature { Name = "Blue-Green Diazotrophs", Rarity = 1, BiosphereLevelRange = new int[] { 1, 1 } };
    public readonly PlanetaryFeature GiantViruses = new PlanetaryFeature { Name = "Giant Viruses", Rarity = 1, BiosphereLevelRange = new int[] { 1, 1 } };
    public readonly PlanetaryFeature GelNodules = new PlanetaryFeature { Name = "Gel Nodules", Rarity = 1, BiosphereLevelRange = new int[] { 1, 1 } };
    public readonly PlanetaryFeature SiliconMicrobes = new PlanetaryFeature { Name = "Silicon Microbes", Rarity = 1, BiosphereLevelRange = new int[] { 1, 1 } };
*/

    //Secondary surface elements, these arent actually that important for the colonization.
    static readonly TextComponent random12 = new TextComponent { Title = "Toxic microparticles", Text = "Planet's winds glittering silver dust that seemingly gets everywhere.", PressMin = 0.1f };

    static readonly TextComponent random11 = new TextComponent { Title = "Glass terrain", Text = "Sometimes on equatorial regions sand melts into hardened pointy glass shard.", TempMin = 600 };
    static readonly TextComponent random37 = new TextComponent { Title = "Explosive Minerals", Text = "Some compounds on the surface can react violently with heat, pressure, or other forms of energy, leading to explosive chemical reactions.", };
    static readonly TextComponent random35 = new TextComponent { Title = "Radon gas", Text = "Surface full of deep crevices emitting highly concentrated radioactive fumes. ", PressMin = 0.1f };
    static readonly TextComponent random34 = new TextComponent { Title = "Fluorecent minerals", Text = "Bands of fluorecent mineral ores run like veins through these exposed cliff faces." };
    static readonly TextComponent random3 = new TextComponent { Title = "Weak magnetosphere", Text = "The planet's magnetic Field is almost non-existent.", SurfaceType = "silicate" };
    static readonly TextComponent random4 = new TextComponent { Title = "Strange patterns", Text = "Strange geometric patterns etched into the planet's surface.", SurfaceType = "silicate" };
    static readonly TextComponent random6 = new TextComponent { Title = "Global Dust Storms", Text = "Has giant sandstorms that can last for months and cover whole continents", PressMin = 0.1f, SurfaceType = "silicate" };
    static readonly TextComponent random8 = new TextComponent { Title = "Mineral water", Text = "Mineralized soil gives the planet's water sources a distinct, metallic taste.", WaterMin = 2 };
    static readonly TextComponent random9 = new TextComponent { Title = "Sinkhole terrain", Text = "Porous rock surface that is riddled with countless sinkholes." };
    static readonly TextComponent random10 = new TextComponent { Title = "Changing landsapce", Text = "Sand terrain shift constantly due to the planet's relentless high winds.", PressMin = 0.5f, SurfaceType = "silicate" };
    static readonly TextComponent random16 = new TextComponent { Title = "Toxic Mist", Text = "A hazy, toxic mist covers the land during the cold seasons.", PressMin = 0.1f };
    static readonly TextComponent random32 = new TextComponent { Title = "Shiny pebbles", Text = "Glistening silvery and greenish metallic rocks covering the surface. " };
    static readonly TextComponent random33 = new TextComponent { Title = "Precious metals", Text = "Ores rich in gold, palladium, plantinum and other such elements litter the surface." };
    static readonly TextComponent random28 = new TextComponent { Title = "Bitter dust", Text = "On windy days,the air is thick with the choking, acrid dust that can cause chemical burns.", PressMin = 0.1f };
    static readonly TextComponent random19 = new TextComponent { Title = "Gas fields", Text = "Planet's low-lying wetlands are rich in high-energy hydrocarbons.", WaterMin = 2 };
    static readonly TextComponent random22 = new TextComponent { Title = "Geothermal springs", Text = "Geothermal processes beneath the crust release contant stream of steam and gases.", WaterMin = 1 };
    static readonly TextComponent random23 = new TextComponent { Title = "Metallic liquids", Text = "Huge swathes of land are covered in metallic puddles." };
    static readonly TextComponent random24 = new TextComponent { Title = "Fish smell", Text = "The atmosphere has unpleasant smell of old fish.", PressMin = 0.1f };
    static readonly TextComponent random25 = new TextComponent { Title = "Oily subsurface", Text = "Abundant subsurface reserves of crude oil and natural gas." };
    static readonly TextComponent random36 = new TextComponent { Title = "Alkaline rocks", Text = "Dry desertscape interspersed with slab-like alkaline rocks.", };
    static readonly TextComponent random17 = new TextComponent { Title = "Super dust storms", Text = "Strong dust storms have stripped the surface of any loose matter.", PressMin = 0.4f };
    static readonly TextComponent random38 = new TextComponent { Title = "Strawberry flavor", Text = "Strawberry tasting ethyl gas compounds in the atmosphere.", PressMin = 0.1f };
    static readonly TextComponent random39 = new TextComponent { Title = "Minty flavor", Text = "a pleasant world where the skies are always crystal-clear and the air is infused with the scent of mint.", PressMin = 0.1f };
    static readonly TextComponent random40= new TextComponent { Title = "Sleeping Gas", Text = "Anesthetic gasses on planet's surface can make anyone breathing them fall asleep within seconds", PressMin = 0.1f };
    static readonly TextComponent random42 = new TextComponent { Title = "Migrainogenic", Text = "The air is breathable but causes headaches under long exposure.", PressMin = 0.5f, PressMax = 5.5f, GasName = "Nitrogen-Oxygen" };
    static readonly TextComponent random41 = new TextComponent { Title = "Tunnels and burrows", Text = "Now barren world's surface is littered with giant animal burrows and holes", PressMin = 0.1f };
    static readonly TextComponent random43 = new TextComponent { Title = "Supercritical fluids", Text = "Misty supercritical fluid mixtures flow freely, seeping into every corner of the planet's landscape", PressMin = 50f, TempMin = 500 };







    readonly TextComponent[] PrimaryGeneralElementList = new TextComponent[] {
           aliens1,aliens2,aliens3,aliens4, aliens5, aliens6, aliens7, rock1, rock2, rock3,rock4,rock5,rock6,rock7,rock8,rock9,rock10
    };

    readonly TextComponent[] SecondaryGeneralElementList = new TextComponent[] {
           random37,random3 ,random4  ,random6  ,random8 ,random9 ,random10 ,random11 ,random12,random35,random32,random16,random17,random33,random19,random22,random23,random24,random28,random25,random36,random17,random39,random40,random41,random42
    };
    //Second row in the planet surface panel

    //First row in the planet surface panel


    /*
    public readonly Gas NitrogenCarbonDioxide = new Gas { Name = "Nitrogen-Carbon Dioxide", Breathable = false, Toxic = false, Rarity = 1 };
    public readonly Gas NitrogenMethane = new Gas { Name = "Nitrogen-Methane", Breathable = false, Toxic = false, Rarity = 1 };
    public readonly Gas NitrogenOxygen = new Gas { Name = "Nitrogen-Oxygen", Breathable = true, Toxic = false, Rarity = 1 };
    public readonly Gas NitrogenSulfurDioxide = new Gas { Name = "Nitrogen-Sulfur Dioxide", Breathable = false, Toxic = true, Rarity = 1 };
    public readonly Gas NitrogenChlorine = new Gas { Name = "Nitrogen-Chlorine", Breathable = false, Toxic = true, Rarity = 1 };
    public readonly Gas CarbonDioxideNitrogen = new Gas { Name = "Carbon Dioxide-Nitrogen", Breathable = false, Toxic = false, Rarity = 1 };
    public readonly Gas CarbonDioxideOxygen = new Gas { Name = "Carbon Dioxide-Oxygen", Breathable = true, Toxic = false, Rarity = 1 };
    public readonly Gas CarbonDioxideMethane = new Gas { Name = "Carbon Dioxide-Methane", Breathable = false, Toxic = false, Rarity = 1 };
    public readonly Gas HeliumHydrogen = new Gas { Name = "Helium-Hydrogen", Breathable = false, Toxic = false, Rarity = 1 };
    public readonly Gas HydrogenHelium = new Gas { Name = "Hydrogen-Helium", Breathable = false, Toxic = false, Rarity = 1 };
    */

    public TextComponent GetPrimaryGeneralDescription(Planet planet)
    {
        TextComponent textComponent;
        textComponent = PickRandomTextComponent(PrimaryGeneralElementList, planet);
        return textComponent;
    }

    public TextComponent GetSecondaryGeneralDescription(Planet planet)
    {
        TextComponent textComponent;
        textComponent = PickRandomTextComponent(SecondaryGeneralElementList, planet);
        return textComponent;
    }

    TextComponent PickRandomTextComponent(TextComponent[] components, Planet planet)
    {
        float planetTemp = planet.Atm.Temperature;
        float planetPress = planet.Atm.Pressure;
        int  planetWetness = planet.Hydrosphere.GetWaterLevel();
        int highestPriority = 0;
        float planetBiosphereLevel = planet.Biosphere.GetBiosphereLevel();
        string planetSurfaceType = planet.Lithosphere.GetSurfaceType().GetName();
        string planetGasName = planet.Atm.PrimaryGas.GetGasName();
        string planetWaterSourceName = planet.Hydrosphere.GetWaterSourceName();
        TextComponent randomFilteredTextComponent = new TextComponent() ;

        UnityEngine.Random.InitState(planet.Seed);

        TextComponent undefined = new TextComponent { Text = "Undefined" };

        List<TextComponent> filteredTextComponents = new List<TextComponent>();

        //checks if the feature can exist in the planet
        foreach (TextComponent component in components)
        {

            if (planetTemp > component.TempMax || planetTemp < component.TempMin) continue;
            if (planetPress > component.PressMax || planetPress < component.PressMin) continue;
            if (planetWetness > component.WaterMax || planetWetness < component.WaterMin) continue;
            if (planetBiosphereLevel > component.BiosphereLevelMax || planetBiosphereLevel < component.BiosphereLevelMin) continue;
            if (planetSurfaceType != component.SurfaceType && "ANY" != component.SurfaceType) continue;
            if (planetGasName != component.GasName && "ANY" != component.GasName) continue;
            if (planetWaterSourceName != component.WaterSourceName && "ANY" != component.WaterSourceName) continue;

            if (component.Priority > highestPriority) { highestPriority = component.Priority; };
            

            filteredTextComponents.Add(component);
        }

        //filter out low priority text components
        foreach (TextComponent component in components)
        {
            if (component.Priority < highestPriority)
            {
                filteredTextComponents.Remove(component);
            };
        }

        if (filteredTextComponents.Count > 0)
        {
            int random = UnityEngine.Random.Range(0, filteredTextComponents.Count);
            randomFilteredTextComponent = filteredTextComponents[random];
        } else
        {
            randomFilteredTextComponent = undefined;

        }
        

        return randomFilteredTextComponent;
    }
};

public class TextGenerator 
{

    string PickRandomString(int seed, string[] stringList)
    {
        string possibleWord = "";
        UnityEngine.Random.InitState(seed);
        int random = UnityEngine.Random.Range(0, stringList.Length);

        if (stringList.Length != 0) {
            possibleWord = stringList[random];
        }

        return possibleWord;
    }






}
