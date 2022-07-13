using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    void Start()
    {
        //Elmas vs. yoksa oyun ilk kez acilmis demektir 
        //Daha önceden oyunu acildiysa anahtar olusmustur, bu bloktaki kodlara bakmadan direkt kaldigi leveli yükler
        //Elmas yoksa biz ekledik
        if (!PlayerPrefs.HasKey("Diamond"))
        {
            PlayerPrefs.SetInt("Diamond", 0);
            PlayerPrefs.SetInt("Level", 1);
        }

        //Oyuncunun kaldigi leveli yukle
        SceneManager.LoadScene(PlayerPrefs.GetInt("Level"));
    }

   
}
