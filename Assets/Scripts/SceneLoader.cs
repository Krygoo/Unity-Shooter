using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    /// <summary>
    /// musicPlayList - tablica przechwująca muzykę 
    /// audioSource - referencja do komponentu odpowiadającego za odtwarzanie muzyki
    /// startGameAudio - dzwiek startu gry
    /// statsy - menadżer statystyk (referencja)
    /// </summary>
    [SerializeField] AudioClip[] musicPlaylist;
    AudioSource audioSource;
    [SerializeField] AudioClip startGameAudio;
    StatsManager statsy;
    OptionsStorage options;
    // [SerializeField] GameObject bgmobject;

    //Awake jest wywoływane jednorazowo na początku działania skryptu. Ustawia ilość scen i sprawia, że obiekt nie jest niszczony przy zmianie sceny (DontDestroyOnLoad)
    private void Awake()
    {
        int numberOfObjects = FindObjectsOfType<SceneLoader>().Length;
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


    //Wczytuje kolejną scenę. Jeżeli wywoływana na ostatnim poziomie wczytywany jest poziom pierwszy.
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex != SceneManager.sceneCountInBuildSettings - 2)
            SceneManager.LoadScene(currentSceneIndex + 1);
        else
            SceneManager.LoadScene(1);

    }
    //Wywoływana przy przejściu na kolejną scenę. Wysyła komunikat, że wczytano nową scenę
    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void LoadLevelInvokable()
    {
        SceneManager.LoadScene(options.GetStartingLevel());
    }

    //Zatrzymuje muzykę z poprzedniego poziomu oraz uruchamia muzykę bieżącego poziomu, dodatkowo przydziela zmiennej statsy odpowiedni obiekt
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        audioSource = GetComponent<AudioSource>();
        statsy = FindObjectOfType<StatsManager>();
        
        audioSource.Stop();
        if (SceneManager.GetActiveScene().buildIndex != SceneManager.sceneCountInBuildSettings - 1)
        {
            AudioClip levelMusic = musicPlaylist[scene.buildIndex];
            audioSource.clip = levelMusic;
            audioSource.Play();
        }

        if (SceneManager.GetActiveScene().buildIndex == 0)
            options.GetOptionsReference();
    }

    //Wyjście do głównego menu. Zatrzymuje muzykę i przechodzi na scenę menu głównego.
    public void ExitToMainMenu()
    {
        if (audioSource)
        {
            audioSource.Stop();
        }
        SceneManager.LoadScene(0);
    }

    //Wywoływana po wciśnięciu przycisku Start Game. Ustawia podstawowe parametry StatsManagera, zatrzymuje muzykę z menu i przechodzi na pierwszy poziom
    public void StartGameButton()
    {
        //  AudioSource bgm = bgmobject.GetComponent<AudioSource>();
        //  bgm.Stop();
        options.LoadOptions();
        statsy.setDefaultStats();
        audioSource.Stop();
        audioSource.PlayOneShot(startGameAudio);
        Invoke("LoadLevelInvokable", 2f);
        
        
    }




}
