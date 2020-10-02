using System;
using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour
{
    /// <summary>
    /// player - referencja gracza
    /// enemyArray - tablica przechowujaca liczbe potworow w danej fali
    /// spawnTime - czas miedzy pojawianiem sie nowych potworów
    /// spawnPoints - tablica przechowujące współrzędne punktów, w ktorych pojawiac sie będą potwory 
    /// waveArray - tablica przechowująca obiekty typu Wave, które zawierają informacje o falach wrogów
    /// remainingMonsters - tablica z liczbą pozostałych wrogów
    /// remainingEnemies - łaczna liczba pozostałych przeciwników
    /// sceneLoader - uchwyt do obiektu SceneLoader odpowiadającego za zmiane poziomów
    /// </summary>
    public Player player;       // Reference to the player's heatlh.
    public GameObject[] enemyArray;
   // The enemy prefab to be spawned.
    public float spawnTime = 3f;            // How long between each spawn.
    public Transform[] spawnPoints;         // An array of the spawn points this enemy can spawn from.
    [SerializeField] Wave[] waveArray;
    [SerializeField] int waveNumber = 0;
    [SerializeField] int[] remainingMonster = {0, 0, 0, 0};
    [SerializeField] int remaingEnemies;
    SceneLoader sceneLoader;

    // Start jest wywoływany przed pierwszym Update, wczytuje pierwszą falę, przydziela zmiennym sceneLoader oraz player odpowiednie obiekty 
    void Start()
    {
        LoadWave(0);
        sceneLoader = FindObjectOfType<SceneLoader>();
        player = FindObjectOfType<Player>();
    }

    // Update jest wywoływany co każdą klatkę. Jeżeli nie ma już wrogów, wczytuje kolejną falę lub kolejny poziom, jeśli była to ostatnia fala
    private void Update()
    {
        if (remaingEnemies <= 0 && waveNumber < waveArray.Length) 
        {
            LoadWave(waveNumber);
        }
        else if (remaingEnemies <= 0 && waveNumber == waveArray.Length)
        {
            player.SaveStats();
            print("Load Next Scene Called from" + gameObject.name);
            sceneLoader.LoadNextScene();
        }
        
    }


    //Metoda odpowiedzialna za obsługę fal. Wywołuje metody Spawn dla różnych typów przeciwników. Odstęp czasowy między stworzeniem przeciwnika jest zależny od typu przeciwnika.
    private void HandleWave()
    {
           StartCoroutine(Spawn(1, 0, remainingMonster[0]));
           StartCoroutine(Spawn(2f, 1,remainingMonster[1]));
           StartCoroutine(Spawn(5f, 2,remainingMonster[2]));
           StartCoroutine(Spawn(100f, 3, remainingMonster[3]));
    }
    
    //Metoda zajmująca się wczytywaniem fal. Ustawia liczbę pozostałych przeciwników, inkrementuje wartość waveNumber, by przy następnym wywołaniu wczytać kolejną falę i wywołuje metodę obsługi fali.
    private void LoadWave(int waveToLoad)
    {
        remainingMonster[0] = waveArray[waveNumber].getmonster1Number(); 
        remainingMonster[1] = waveArray[waveNumber].getmonster2Number();
        remainingMonster[2] = waveArray[waveNumber].getmonster3Number();
        remainingMonster[3] = waveArray[waveNumber].getmonster4Number();
        remaingEnemies = remainingMonster[0] + remainingMonster[1] + remainingMonster[2] + remainingMonster[3];
        waveNumber++;
        HandleWave();
    }
    

    //Tworzy wskazaną ilość przeciwników w losowym miejsciu spośród określonych w edytorze spawnPointów. Każdy przeciwnik zostaje stworzony co wskazany w zmiennej timeInterval czas
    IEnumerator Spawn(float timeInterval,int enemyType, int amount)
    {
        int x = 0;
        for (int i = amount; i > 0; i--)
        {
            
            if (enemyType!=3)
            {   int spawnPointIndex = UnityEngine.Random.Range(0, spawnPoints.Length-2);
                if (enemyType == 0) { x = UnityEngine.Random.Range(0, 3); }
                else if (enemyType == 1) { x = UnityEngine.Random.Range(3, 5); }
                else if (enemyType == 2) { x = UnityEngine.Random.Range(5, 7); } 

                    Instantiate(enemyArray[x], spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
            }
            else
            {
                
                //int spawnPointIndex = spawnPoints.Length;
                Instantiate(enemyArray[7], spawnPoints[spawnPoints.Length-1].position, spawnPoints[spawnPoints.Length-1].rotation);
            }

            yield return new WaitForSeconds(timeInterval);

        }
    }

    //Pobiera obecny numer fali
    public int GetWaveNumber()
    {
        return waveNumber;
    }
    //Zmniejsza licznik pozostałych wrogów
    public void decreaseEnemiesLeft()
    {
        remaingEnemies--;
    }
    //Zwraca liczbę pozostałych wrogów
    public int GetEnemiesLeft()
    {
        return remaingEnemies;
    }
}