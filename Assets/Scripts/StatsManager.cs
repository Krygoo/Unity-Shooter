using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{

   /// <summary>
   /// playerExp - doświadczenie gracza
   /// currentMAxHealth - obecny maksymalny poziom zdrowia gracza
   /// currentDamageMod - obecny modyfikator obrażeń
   /// currentSpeed - obecna predkość poruszania się gracza
   /// currentMaxHealthlvl, currentDamageModlvl,currentSpeedlvl - ifnormacje o poziomach umiejetnosci gracza
   /// shotAmmo,ar15Ammo,grenades - informacje o posiadnej przez gracza amunicji
   /// </summary>
    [SerializeField] int playerExp; 
    [SerializeField] float currentMaxHealth;
    [SerializeField] float currentDamageMod;
    [SerializeField] float currentSpeed;
    [SerializeField] int currentMaxHealthLvl;
    [SerializeField] int currentDamageModlvl;
    [SerializeField] int currentSpeedlvl;
    [SerializeField] int shotAmmo;
    [SerializeField] int aR15Ammo;
    [SerializeField] int grenades;

    OptionsStorage options;

    public void Awake() //Signleton pattern, ktory zapewnia jedna instacje danego obiektu na caly projekt
    {
        int numberOfObjects = FindObjectsOfType<StatsManager>().Length;
        if (numberOfObjects > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
        options = FindObjectOfType<OptionsStorage>();
    }
    public void setDefaultStats() // ustawia poczatkowe statystyki gracza. Przy rozpoczeciu nowej gry
    {
        playerExp = 0;        
        currentMaxHealth = 120 - (options.GetDifficulty()*20);
        currentMaxHealthLvl = 1;
        currentDamageMod = 1;
        currentDamageModlvl = 1;
        currentSpeedlvl = 1;
        currentSpeed = 9 - (options.GetDifficulty());
        shotAmmo = 0;
        aR15Ammo = 0;
        grenades = 0;
    }

    public void SaveCurrentStats(int exp, float maxHealth, float damageMod,
        float speed, int healthlvl, int dmglvl, int speedlvl, int shotammo, int ar15ammo, int nades) // zapisuje obecne statystyki przy wcisnieciu esc
    {
        playerExp = exp;
        currentMaxHealth = maxHealth;
        currentMaxHealthLvl = healthlvl;
        currentDamageMod = damageMod;
        currentDamageModlvl = dmglvl;
        currentSpeedlvl = speedlvl;
        currentSpeed = speed;
        shotAmmo = shotammo;
        aR15Ammo = ar15ammo;
        grenades = nades;

    }

    public float[] LoadStats() // wczytuje statystyki po wyjsciu z menu
    {
        float[] stats = { playerExp, currentMaxHealth, currentDamageMod, currentSpeed,
            currentMaxHealthLvl, currentDamageModlvl, currentSpeedlvl, shotAmmo,aR15Ammo, grenades };
        return stats;
     
    }
}
