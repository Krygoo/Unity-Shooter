using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName= "Wave")]
public class Wave : ScriptableObject
{
   [SerializeField] bool isBoss; // Czy fala jest bossem
   [SerializeField] int monster_01_number; // liczba  wrogow 1 typu (osmiornica)
   [SerializeField] int monster_02_number; // liczba wrogow 2 typu (czarny)
   [SerializeField] int monster_03_number; // liczba wrogow 3 typu (fioletowy)
   [SerializeField] int monster_04_number; // liczba bossow


    public bool hasBoss() //zwraca true jesli jest boos;
    {
        return isBoss;
    }


    // funkcje ponizej zwracaja liczbe wrogow danego typu

    public int getmonster1Number() 
    {
        return monster_01_number;
    }
    public int getmonster2Number()
    {
        return monster_02_number;
    }
    public int getmonster3Number()
    {
        return monster_03_number;
    }

    public int getmonster4Number()
    {
        return monster_04_number;
    }

}
