using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCombatEvent : MonoBehaviour
{

    public GameObject greatSword;
    public GameObject katana;
    public GameObject backGraetSword;

    private void Awake()
    {
        greatSword.SetActive(false);
        
    }

    void HideGS()
    {
        greatSword.SetActive(false);
        backGraetSword.SetActive(true);
        katana.SetActive(true);
    }

    void ShowGS()
    {
        greatSword.SetActive(true);
        backGraetSword.SetActive(false);
        katana.SetActive(false);
    }
}
