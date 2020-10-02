using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class AR15 : Weapon
{
    /// <summary>
    /// shootSound - dzwiek strzalu
    /// isUnlucked - czy bron jest obecnie uzywana
    /// name - zawiera nazwe broni
    /// parcivle - referencja do prefabrykatu efektu strzalu
    /// emission - referncja do modułu komponentu efektów cząsteczkowych
    /// </summary>
    [SerializeField] AudioClip shootSound;
    bool isUnlocked = true;
    int damage = 10;
    string name = "AR15";
    GameObject particle;
    ParticleSystem emission;

    public override void SetActive(bool active)
    {
        isUnlocked = active;
    }

    public override bool Getactive()
    {
        return isUnlocked;
    }

    private void Start()
    {
        particle = transform.Find("Particle System").gameObject;
        emission = particle.GetComponent<ParticleSystem>();
    }

    public override void Shoot()
    {

        StartCoroutine(FullAuto());

    }
    public override string GetName()
    {
        return name;

    }
    public override int GetDamage()
    {
        return damage;
    }
    IEnumerator FullAuto() // Coroutine realizujacy strzelanie seria
    {
        for (int i = 0; i < 3; i++)
        {
            // funkcja emituje 1 particle co 0.1s;
            emission.Emit(1); 
            yield return new WaitForSeconds(.1f);
        }
    }
    public override AudioClip GetShootSound()
    {
        return shootSound;
    }

}
