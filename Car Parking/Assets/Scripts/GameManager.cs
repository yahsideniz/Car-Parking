using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [Header("---CAR SETTINGS")]
    public GameObject[] cars;
    public int numberOfCars;
    int activeCarIndex = 0;
    //Kalan araba sayisi yazi olarak lazim olursa
    public TextMeshProUGUI remainingVehicles;
    int remainingVehiclesValue;


    [Header("---CANVAS SETTINGS")]
    public Sprite vehicleArrivedImage; // Arac basarili sekilde platforma gecince gelecek görsel
    public GameObject[] carCanvasImages;
    //Panel ayalari
    public TextMeshProUGUI[] Texts;
    public GameObject[] Panels;
    //Butonlar
    public GameObject[] TapToButtons;

    [Header ("---PLATFORM SETTINGS")]
    public GameObject platform_1;
    public GameObject platform_2;
    public float[] rotationSpeed;
    bool isPlatformRotating;

    [Header("---LEVEL SETTINGS")]
    public int numberOfDiamonds;
    //Kaza efekti
    public ParticleSystem CrashEffect;
    //Sesler
    public AudioSource[] Sounds;
    //Platform yukseliyor mu
    public bool isRisingPlatform;
    //Dokunmatik ekran
    bool touchLock; 


    void Start()
    {
        //Dokunma ekrani
        touchLock = true;

        isPlatformRotating = true;
        CheckDefaultValues();

        remainingVehiclesValue = numberOfCars;

        //Park edilen araba gorselleri icin
        for (int i = 0; i < numberOfCars; i++)
        {
           carCanvasImages[i].SetActive(true); // Dongu kac kere dönerse o kadar görsel ortaya cikacak

        }


    }

    public void NewCar()
    {
        remainingVehiclesValue--;


        if (activeCarIndex < numberOfCars)
        {
            cars[activeCarIndex].SetActive(true);
        }
        else // Araba kalmadiginda calisacak, seviye tamamlanmistir
        {
            Win();
        }


        //Araba park edilince canvastaki görsel yesile döncek
        // -1 yapma nedenimiz bu fonk. cagrilinca aktif index 1 artmýs oluyor yanlis görsel yesil olmasin
        carCanvasImages[activeCarIndex - 1].GetComponent<Image>().sprite = vehicleArrivedImage;
    }

    void Update()
    {

        if(Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0); // Ilk dokunusu aldik

            if(touch.phase == TouchPhase.Began) // Dokunmaya basladiysak
            {
                if (touchLock)
                {
                    Panels[0].SetActive(false);
                    Panels[3].SetActive(true); // Araba gorselleri olan panel ortaya cikacak
                    touchLock = false;
                }
                else
                {
                    // Arabanin ileri gitmesini saglar
                    cars[activeCarIndex].GetComponent<Car>().isForward = true;
                    activeCarIndex++; // Listedeki diger arabalarda aktif olacak
                }
            }
        }


        //Platform rotation islemleri
        if (isPlatformRotating)
        {
            platform_1.transform.Rotate(new Vector3(0, 0, -rotationSpeed[0]), Space.Self);// Self komutu sayesinde donme islemi objenin kendi ic eksenine göre yapilacak
            if(platform_2!=null)
            platform_2.transform.Rotate(new Vector3(0, 0, rotationSpeed[1]), Space.Self);

        }
       
    }

    public void GameOver()
    {
        isPlatformRotating = false;

        Texts[6].text = PlayerPrefs.GetInt("Diamond").ToString(); 
        Texts[7].text = SceneManager.GetActiveScene().name; 
        Texts[8].text = (numberOfCars - remainingVehiclesValue).ToString(); // Kac araba park edildi
        Texts[9].text = numberOfDiamonds.ToString(); // Kac elmas toplanladi

        Sounds[1].Play();
        Sounds[3].Play(); // Kaza sesi


        Panels[1].SetActive(true);
        Panels[3].SetActive(false);

        //Tap To buttonlarý için
        Invoke("RevealLostButton", 2f);
    }

    void Win()
    {
        PlayerPrefs.SetInt("Diamond", PlayerPrefs.GetInt("Diamond") + numberOfDiamonds);

        Texts[2].text = PlayerPrefs.GetInt("Diamond").ToString(); 
        Texts[3].text = SceneManager.GetActiveScene().name; 
        Texts[4].text = (numberOfCars - remainingVehiclesValue).ToString();
        Texts[5].text = numberOfDiamonds.ToString();

        Sounds[2].Play();

        Panels[2].SetActive(true);
        Panels[3].SetActive(false);


        Invoke("RevealWinButton", 2f);

    }

    //Tap To butonlari ortaya ciksin
    void RevealLostButton()
    {
        TapToButtons[0].SetActive(true);
    }
    void RevealWinButton()
    {
        TapToButtons[1].SetActive(true);
    }

    //Bellek yonetimi
    void CheckDefaultValues()
    {
        Texts[0].text = PlayerPrefs.GetInt("Diamond").ToString();
        Texts[1].text = SceneManager.GetActiveScene().name;
    }

    public void Replay()
    {
        //Ayni sahneyi tekrar yukler
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        PlayerPrefs.SetInt("Level", SceneManager.GetActiveScene().buildIndex + 1);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void EndGame()
    {
        Application.Quit();
    }
}
