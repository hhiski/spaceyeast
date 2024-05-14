using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#region lithosphere classes
public class Lithosphere
{
    SurfaceType SurfaceType { get; set; }

    public SurfaceType GetSurfaceType()
    {
        return SurfaceType;
    }

    public Lithosphere(SurfaceType surfaceType)
    {
        SurfaceType = surfaceType;
    }
}


public class SurfaceType
{
    public static readonly SurfaceType Silicate = new SurfaceType(name: "Silicate", metalLevel: 1, radioactivesLevel: 1, organicsLevel: 0);
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
    public static readonly SurfaceType Transuranic = new SurfaceType(name: "Transuranic", metalLevel: 2, radioactivesLevel: 3, organicsLevel: 0);
    public static readonly SurfaceType Water = new SurfaceType(name: "Liquid", metalLevel: 0, radioactivesLevel: 0, organicsLevel: 1);

    private string Name { get; set; }
    private int MetalLevel { get; set; }
    private int RadioactivesLevel { get; set; }
    private int OrganicsLevel { get; set; }

    public string GetName()
    {
        return Name;
    }
    public int GetMetalLevel()
    {
        return MetalLevel;
    }
    public int GetRadioactivesLevel()
    {
        return RadioactivesLevel;
    }
    public int GetOrganicsLevel()
    {
        return OrganicsLevel;
    }

    /*
     0 - None
     1 - Traces
     2 - Deposits 
     3 - Everywhere
    */

    public SurfaceType(SurfaceType surfaceType)
    {
        Name = surfaceType.GetName();
        MetalLevel = GetMetalLevel();
        RadioactivesLevel = GetRadioactivesLevel();
        OrganicsLevel = GetOrganicsLevel();
    }

    public SurfaceType(string name = "Undefined", int metalLevel = 0, int radioactivesLevel = 0, int organicsLevel = 0)
    {
        Name = name;
        MetalLevel = metalLevel;
        RadioactivesLevel = radioactivesLevel;
        OrganicsLevel = organicsLevel;
    }
}



#endregion