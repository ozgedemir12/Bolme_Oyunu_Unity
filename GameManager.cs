using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject karelerPrefab;


    [SerializeField]
    private Transform karelerPaneli;


    [SerializeField]
    private Text soruText;


    private GameObject[] karelerDizisi = new GameObject[25];


    [SerializeField]
    private Transform soruPaneli;

    [SerializeField]
    private Sprite[] kareSprites;


    List<int> bolumDegerleriListesi = new List<int>();


    int bolenSayı, bolunenSayı;
    int kacincisoru;
    int ButonDegeri;
    int dogrusonuç;
    int kalanhak;
    string SorununZorlukDerecesi;

    bool butonaBasilsinmi;

    KalanHakManager kalanHakManager;

    PuanManager puanManager;

    GameObject gecerliKare;

    [SerializeField]
    GameObject sonucPaneli;

    [SerializeField]
    AudioSource audioSource;

    public AudioClip butonsesi;

    private void Awake()
    {
        kalanhak = 3;

        audioSource = GetComponent<AudioSource>();

        sonucPaneli.GetComponent<RectTransform>().localScale = Vector3.zero;

        kalanHakManager = Object.FindObjectOfType<KalanHakManager>();
        puanManager = Object.FindObjectOfType<PuanManager>();

        kalanHakManager.KalanHaklariKontrolEt(kalanhak);

                                                                                                  
    }

    void Start()

       
    {
        butonaBasilsinmi = false;

        soruPaneli.GetComponent<RectTransform>().localScale = Vector3.zero;

        kareleriOlustur();
    }
    


     public void kareleriOlustur()
     {
        for(int i=0; i<25; i++)
        {
            GameObject kare = Instantiate(karelerPrefab, karelerPaneli);
            kare.transform.GetChild(1).GetComponent<Image>().sprite = kareSprites[Random.Range(0, kareSprites.Length)];   
            kare.transform.GetComponent<Button>().onClick.AddListener(() => ButonaBasildi());
            karelerDizisi[i] = kare;
        }

        BolümDegerleri();

        StartCoroutine(DoFadeRoutine());
        Invoke("SoruPaneliniAc", 2f);
     }



    void ButonaBasildi()
    {
        if (butonaBasilsinmi)
        {
            audioSource.PlayOneShot(butonsesi);

            ButonDegeri = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text);
            gecerliKare = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

            SonucuKontrolEt();
        }
     
    }

    void SonucuKontrolEt()
    {
        if (ButonDegeri == dogrusonuç)
        {
            gecerliKare.transform.GetChild(1).GetComponent<Image>().enabled = true;
            gecerliKare.transform.GetChild(0).GetComponent<Text>().text = "";
            gecerliKare.transform.GetComponent<Button>().interactable = false;

            puanManager.PuaniArtir(SorununZorlukDerecesi);

            bolumDegerleriListesi.RemoveAt(kacincisoru);

           

            if (bolumDegerleriListesi.Count>0)
            {
                SoruPaneliniAc();
            }
            else
            {
                OyunBitti();
            }

            SoruPaneliniAc();
        }
        else
        {
            kalanhak--;
            kalanHakManager.KalanHaklariKontrolEt(kalanhak);
        }
        if(kalanhak <= 0)
        {
            OyunBitti();
        }
    }

    void OyunBitti()
    {
        butonaBasilsinmi = false;
        sonucPaneli.GetComponent<RectTransform>().DOScale(1, 0.3f).SetEase(Ease.OutBack);
    }

    IEnumerator DoFadeRoutine()
    {
        foreach(var kare in karelerDizisi)
        {
            kare.GetComponent<CanvasGroup>().DOFade(1, 0.2f);

            yield return new WaitForSeconds(0.08f);
        }
    }



    void BolümDegerleri()
    {
        foreach (var kare in karelerDizisi)
        {
            int rastgeleDeger = Random.Range(1, 13);
            bolumDegerleriListesi.Add(rastgeleDeger);

            kare.transform.GetChild(0).GetComponent<Text>().text = rastgeleDeger.ToString();
        }

    }



    void SoruPaneliniAc()
    {
        SoruyuSor();
        butonaBasilsinmi = true;
        soruPaneli.GetComponent<RectTransform>().DOScale(1, 0.3f).SetEase(Ease.OutBack);
    }



    void SoruyuSor()
    {
        bolenSayı = Random.Range(2, 11);

        kacincisoru = Random.Range(0, bolumDegerleriListesi.Count);

        dogrusonuç = bolumDegerleriListesi[kacincisoru];

        bolunenSayı = bolenSayı * dogrusonuç;



        if(bolunenSayı<= 40)
        {
            SorununZorlukDerecesi = "kolay";
        }else if (bolunenSayı>40 && bolunenSayı <= 80)
        {
            SorununZorlukDerecesi = "orta";
        }
        else
        {
            SorununZorlukDerecesi = "zor";
        }


        soruText.text = bolunenSayı.ToString() + ":" + bolenSayı.ToString();
    }
}
