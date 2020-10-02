using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
 
    [SerializeField] Text hpText; // tekst wyswietla aktualne zycie
    [SerializeField] Text waveText; //tekst wyswietla aktualna fale
    [SerializeField] Text enemiesText; // tekst wyswietla tekst wrogow
    [SerializeField] Text ammoShotgun; // wyswietla liczbe amunicji do strzelby
    [SerializeField] Text ammoAR15; //tekst wyswietla aktualna liczbe amunicji do karabinu
    [SerializeField] Text expText; // aktualne doswiadczenie gracza
    Player player; //referencja gracza
    EnemyManager enemyManager; // referencja obiektu zarzadzajacego wrogami


    void Start()
    {
        //referencje
        player = FindObjectOfType<Player>();
        enemyManager = FindObjectOfType<EnemyManager>();

    }

 
    void Update()
    {
        //aktualizacja odpowiednich tekstow
        hpText.text = player.GetPlayersHealth().ToString();
        waveText.text = enemyManager.GetWaveNumber().ToString();
        enemiesText.text = enemyManager.GetEnemiesLeft().ToString();
        expText.text = player.GetPlayerExp().ToString();
        ammoAR15.text = player.GetAR15Ammo().ToString();
        ammoShotgun.text = player.GetShotgunAmmo().ToString();
    }
}
