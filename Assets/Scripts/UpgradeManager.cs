using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UpgradeManager : MonoBehaviour
{

    // koszty ulepszen 
    [SerializeField] int speedUpgradeCost; // ulepszenie predkosci
    [SerializeField] int damageUpgradeCost; // ulepszenie modyfikatora obrazen
    [SerializeField] int maxHealthUpgradeCost; // ulepszenie maksymalnego zycia

    // bonus poziomu ulepszen 20 % za kazdy poziom
    [SerializeField] float speedUpgradeBonus = 1.2f;  // bonus predkosci poruszania
    [SerializeField] float damageUpgradeBonus = 1.2f; // bonus modyfikatora obrazen
    [SerializeField] float maxHealthUpgradeBonus = 1.2f; // bonus maksymalnego zycia

    //obecne statystyki gracza
    [SerializeField] float currentMaxHealth; //obecne maksymalne zycie
    [SerializeField] float currentDamageMod; // obecny modyfikator obrazen
    [SerializeField] float currentSpeed;  // obecne predkosc

    // aktualny poziom statystyk - potrzebny do obliczen 
    [SerializeField] int currentMaxHealthLvl = 1;
    [SerializeField] int currentDamageModlvl = 1;
    [SerializeField] int currentSpeedlvl = 1;
    //doswiadczenie gracza 
    [SerializeField] int playerExp;


    // pola tekstowe ktore beda wyswietlane  w menu
    [SerializeField] Text expText;
    [SerializeField] Text actualHP;
    [SerializeField] Text actualSpeed;
    [SerializeField] Text actualDamage;
    [SerializeField] Text nextHP;
    [SerializeField] Text nextSpeed;
    [SerializeField] Text nextDamage;
    [SerializeField] Text hpCostText;
    [SerializeField] Text speedCostText;
    [SerializeField] Text damageCostText;

    Player playerRef; //referencja do obiektu gracza
    StatsManager statistics; // referenja do menedzera statystyk




        private void Start()
    {
        //pobranie rereferencji
        statistics = FindObjectOfType<StatsManager>();
        playerRef = FindObjectOfType<Player>();
    }
 


    public void OnStartUpgradeMenu() // funkcja jest wywolywana przez gracza za pomca przycisku esc
    {
        //obliczenia po wscisnieciu przycisku
        playerExp = playerRef.GetPlayerExp();
        currentMaxHealth = playerRef.GetMaxHealth();
        currentSpeed = playerRef.GetSpeed();
        currentDamageMod = playerRef.GetDamageModifier();
        speedUpgradeCost = currentSpeedlvl * 150;
        damageUpgradeCost = currentDamageModlvl * 150;
        maxHealthUpgradeCost = currentMaxHealthLvl * 150;
        currentDamageModlvl = playerRef.GetDmgLvl();
        currentMaxHealthLvl = playerRef.GetHpLvl();
        currentSpeedlvl = playerRef.GetSpeedLvl();

        // wywolanie funkcji aktualizujacej napisy
        UpdateTexts();

    }

    private void UpdateTexts() // aktualizuje wyswietlane napisy w menu ulepszen
    {
        expText.text = playerExp.ToString();
        actualHP.text = currentMaxHealth.ToString();
        actualSpeed.text = currentSpeed.ToString();
        actualDamage.text = currentDamageMod.ToString();
        nextHP.text = (currentMaxHealth * maxHealthUpgradeBonus).ToString();
        nextDamage.text = (currentDamageMod * damageUpgradeBonus).ToString();
        nextSpeed.text = (currentSpeed * speedUpgradeBonus).ToString();
        hpCostText.text = (currentMaxHealthLvl * 150).ToString();
        speedCostText.text = (currentSpeedlvl * 150).ToString();
        damageCostText.text = (currentDamageModlvl * 150).ToString();
    }

    public void OnExitUpgradeMenu() // przy wyjsciu  z menu ulepszen statystyki sa zapisywane.
    {
        playerRef.SetDamageModifier(currentDamageMod);
        playerRef.SetMaxHealth((int)currentMaxHealth);
        playerRef.SetSpeed(currentSpeed);
        playerRef.SetExp(playerExp);
        playerRef.SetHpLVL(currentMaxHealthLvl);
        playerRef.SetSpeedLVL(currentSpeedlvl);
        playerRef.SetDamageLVL(currentSpeedlvl);
    }

    public void UpgradeHealth() //funkcja zwiekszajaca maksymalne zycie gracza
    {
        if (playerExp >= maxHealthUpgradeCost) {

            playerExp -= maxHealthUpgradeCost;
            currentMaxHealthLvl++;
            currentMaxHealth *= maxHealthUpgradeBonus;
            maxHealthUpgradeCost = currentMaxHealthLvl * 150;
            UpdateTexts();
        }

    }

    public void UpgradeSpeed() // funkcja zwiekszajaca maksymalna predkosc gracza
    {
        if (playerExp >= speedUpgradeCost)
        {
            playerExp -= speedUpgradeCost;
            currentSpeedlvl++;
            currentSpeed *= speedUpgradeBonus;
            speedUpgradeCost = currentSpeedlvl * 150;
            UpdateTexts();
        }


    }
    public void UpgradeDamage() // funkcja zwiekszajaca obrazenia gracza
    {
        if (playerExp >= damageUpgradeCost)
        {
            playerExp -= damageUpgradeCost;
            currentDamageModlvl++;
            currentDamageMod *= damageUpgradeBonus;
            damageUpgradeCost = currentDamageModlvl * 150;
            UpdateTexts();
        }
    }





}
