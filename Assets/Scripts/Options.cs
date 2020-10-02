using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Options : MonoBehaviour
{
    [SerializeField] int difficulty = 1;
    [SerializeField] Text diffText;
    [SerializeField] string[] diffArray = { "Easy", "Normal", "Hard" };
    [SerializeField] string[] onOff = { "Off", "On" };
    [SerializeField] int lvlToLoad = 1;
    [SerializeField] Text bloodText;
    [SerializeField] bool blood = true;
    [SerializeField] int startinglevel = 1;
    [SerializeField] Text startinglevelText;





    public void Start()
    {
        diffText.text = diffArray[difficulty];
        if (blood) bloodText.text = onOff[1];
        else bloodText.text = onOff[0];
    }

    public void ChangeDifficulty()
    {
        if (difficulty < 2) difficulty++;
        else difficulty = 0;
        diffText.text = diffArray[difficulty];
    }

    public void ChangeBlood()
    {
        if (blood) { blood = false; bloodText.text = onOff[0]; }
        else { blood = true; bloodText.text = onOff[1]; }
        }

    public bool GetBlood()
    {
        return blood;
    }

    public int GetDifficulty()
    {
        return difficulty;
    }
    public int GetStartLevel()
    {
        return startinglevel;
    }

    public void SelectLevel1()
    {
        startinglevel = 1;
        startinglevelText.text = startinglevel.ToString();
    }
    public void SelectLevel2()
    {
        startinglevel = 2;
        startinglevelText.text = startinglevel.ToString();
    }
    public void SelectLevel3()
    {
        startinglevel = 3;
        startinglevelText.text = startinglevel.ToString();
    }
    public void SelectLevel4()
    {
        startinglevel = 4;
        startinglevelText.text = startinglevel.ToString();
    }


}
