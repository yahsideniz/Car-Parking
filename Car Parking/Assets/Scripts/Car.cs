using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public bool isForward;

    //Durma noktasi
    bool stopPointSituation = false;

    public GameObject[] ruts;

    public Transform parent;

    //GameManager Koduna erismek icin
    public GameManager _GameManager;

    //Kaza efekti icin pozisyon
    public GameObject CrashPoint;

    //Platform yukselirken yumusak bir gecis icin
    float RiseValue;
    bool PlatformRise;


    void Start()
    {
        
    }

    void Update()
    {
        //Durma noktasi
        if (!stopPointSituation)
        {
            transform.Translate(7f * Time.deltaTime * transform.forward); // ileri dogru guc uygula

        }

        if (isForward)
        {
            transform.Translate(15f * Time.deltaTime * transform.forward);

        }

        if(PlatformRise)
        {
            //Sonsuza kadar yükselmesin diye
            if(RiseValue > _GameManager.platform_1.transform.position.y)
            {
                //Lerp, önce vektore baslangic noktasi sonra gidecegi yeri ve en son katsayi verilir
                _GameManager.platform_1.transform.position = Vector3.Lerp(_GameManager.platform_1.transform.position,
                  new Vector3(_GameManager.platform_1.transform.position.x,
                    _GameManager.platform_1.transform.position.y + 1.3f,
                    _GameManager.platform_1.transform.position.z), .010f);
            }
            else // istedigmiz kadar yukseldiyse
            {
                PlatformRise = false;
            }
              
              
        }

    }

    private void OnCollisionEnter(Collision other)
    {
       

        if (other.gameObject.CompareTag("Parking"))
        {
            CarTechnicalProcess();

            //Araba platforma gelince durmasi icin, platformun alt nesnesi yaptik
            transform.SetParent(parent);

            //Level4 icin yukselen platform
            if (_GameManager.isRisingPlatform)
            {
                RiseValue = _GameManager.platform_1.transform.position.y + 1.3f;
                PlatformRise = true;
            }

            //GameManager koduna erisiyoruz
            _GameManager.NewCar();
        }

       

        else if (other.gameObject.CompareTag("Car"))
        {
            _GameManager.CrashEffect.transform.position = CrashPoint.transform.position;
            _GameManager.CrashEffect.Play();

            CarTechnicalProcess();

            _GameManager.GameOver();
        }

       
    }
     
    void CarTechnicalProcess()
    {
        isForward = false;

        //Teker izleri
        ruts[0].SetActive(false);
        ruts[1].SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("StopPoint"))
        {
            stopPointSituation = true;
        }

        else if (other.gameObject.CompareTag("Diamond"))
        {
            other.gameObject.SetActive(false);
            _GameManager.numberOfDiamonds++;
            _GameManager.Sounds[0].Play();
        }

        else if (other.gameObject.CompareTag("Middle"))
        {
            //Partical efektin patlayacagi nokta
            _GameManager.CrashEffect.transform.position = CrashPoint.transform.position;
            _GameManager.CrashEffect.Play();

            CarTechnicalProcess();

            _GameManager.GameOver();
        }

        else if (other.gameObject.CompareTag("FrontParking"))
        {
            other.gameObject.GetComponent<FrontParking>().Parking.SetActive(true);
        }
    }
}
