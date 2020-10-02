using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsStorage : MonoBehaviour
{
    [SerializeField] bool blood;
    [SerializeField] int difficulty;
    [SerializeField] int startinglevel;
    Options options;

    public void Awake() //Signleton pattern, ktory zapewnia jedna instacje danego obiektu na caly projekt
    {
        int numberOfObjects = FindObjectsOfType<OptionsStorage>().Length;
        if (numberOfObjects > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        GetOptionsReference();
    }

    public bool GetBlood()
    {
        return blood;
    }

    public int GetDifficulty()
    {
        return difficulty;
    }

    public int GetStartingLevel()
    {
        return startinglevel;
    }

    public void LoadOptions()
    {
        blood = options.GetBlood();
        difficulty = options.GetDifficulty();
        startinglevel = options.GetStartLevel();
    }

    public void GetOptionsReference()
    {
        options = FindObjectOfType<Options>();
    }
}
