using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region SocietyPreceptTypes
public class SocietyPreceptType
{
    public string Name { get; set; } = "EmptyType";
    public string GetName() { return Name; }
    public void SetName(string name) { Name = name; }

}


class SocietyFormation
{

    public List<SocietyPreceptType> PopulateSocietyPreceptTypeList()
    {

        List<SocietyPreceptType> EverySocietyPreceptTypeList = new List<SocietyPreceptType>();

        SocietyPreceptType test = new SocietyPreceptType
        {
            Name = "SocietyPreceptType TEST",

        };
        EverySocietyPreceptTypeList.Add(test);

        return EverySocietyPreceptTypeList;
    }
}

#endregion