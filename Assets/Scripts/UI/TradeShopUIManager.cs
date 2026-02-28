using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TradeShopUIManager : MonoBehaviour
{
    [Header("Trade Shop UI")]
    public GameObject TradeShopPanel;
    public Button tradePanelOffButton;


    [Header("Pistol Panel")]
    public Button PistolPanelOnButton;
    public GameObject PistolShopPanel;
    public Button PistolPanelOffButton;

    [Header("Shotgun Panel")]
    public Button ShotgunPanelOnButton;
    public GameObject ShotgunShopPanel;
    public Button ShotgunPanelOffButton;

    [Header("Smg Panel")]
    public Button SmgPanelOnButton;
    public GameObject SmgShopPanel;
    public Button SmgPanelOffButton;

    [Header("Assault Rifle")]
    public Button AssaultRiflePanelOnButton;
    public GameObject AssaultRifleShopPanel;
    public Button AssaultRiflePanelOffButton;

    [Header("Durdurma Ekraný")]
    public Button Durdurmabuton;
    public GameObject DurdurmaEkraný;

    [Header("Settings")]
    public GameObject SettingsPanel;
    public Button SettingsPanelOffButton;
    public Button musicbutton;
    public Button sfxbutton;
    public GameObject musicOnIcon;
    public GameObject musicOffIcon;
    public GameObject SfxOnIcon;
    public GameObject SfxOffIcon;

    [Header("DurdurmaEkranButonlarý")]
    public Button Continue;
    public Button Setting;
    public Button Exit;
    [Header("Uprage")]
    public GameObject UpragePanel;
    public Button UpragePanelOnButton;
    public Button UpragePanelOffButton;

    public ZombieSpawner zombieSpawner;

    public AudioSource music;
    public static bool isSfxMuted = false;

    private bool isMusicOn = true;

    void Start()
    {
        PistolShopPanel.SetActive(false);
        ShotgunShopPanel.SetActive(false);
        SmgShopPanel.SetActive(false);
        AssaultRifleShopPanel.SetActive(false);
        DurdurmaEkraný.SetActive(false);
        SettingsPanel.SetActive(false);
        UpragePanel.SetActive(false);
        zombieSpawner = FindFirstObjectByType<ZombieSpawner>();
    }
    private void Update()
    {
        if (zombieSpawner.waveActive)
        {
            TradeShopPanel.SetActive(false);
            PistolShopPanel.SetActive(false);
            ShotgunShopPanel.SetActive(false);
            SmgShopPanel.SetActive(false);
            AssaultRifleShopPanel.SetActive(false);
        }
        AudioSource[] audioSources = Object.FindObjectsByType<AudioSource>(FindObjectsSortMode.None);

        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.mute = isSfxMuted;
        }
        SfxOnIcon.SetActive(!isSfxMuted);
        SfxOffIcon.SetActive(isSfxMuted);

    }
    #region Trade Shop Panel
    public void TradePanelOff()
    {
        TradeShopPanel.SetActive(false);
    }
    #endregion
    #region Pistol Panel

    public void PistolPanelOn()
    {
        TradeShopPanel.SetActive(false);
        PistolShopPanel.SetActive(true);
    }
    public void PistolPanelOff()
    {
        PistolShopPanel.SetActive(false);
        TradeShopPanel.SetActive(true);
    }
    #endregion
    #region Shotgun Panel
    public void ShotGunShopPanelOn()
    {
        TradeShopPanel.SetActive(false);
        ShotgunShopPanel.SetActive(true);
    }
    public void ShotGunShopPanelOff()
    {
        ShotgunShopPanel.SetActive(false);
        TradeShopPanel.SetActive(true);
    }
    #endregion
    #region Smg Panel
    public void SmgPanelOn()
    {
        TradeShopPanel.SetActive(false);
        SmgShopPanel.SetActive(true);
    }
    public void SmgPanelOff()
    {
        SmgShopPanel.SetActive(false);
        TradeShopPanel.SetActive(true);
    }
    #endregion

    #region Assault Rifle Panel

    public void AssaultRiflePanelOn()
    {
        TradeShopPanel.SetActive(false);
        AssaultRifleShopPanel.SetActive(true);
    }

    public void AssaultRiflePanelOff()
    {
        AssaultRifleShopPanel.SetActive(false);
        TradeShopPanel.SetActive(true);
    }
    #endregion 

    public void OyunDurdur()
    {
        DurdurmaEkraný.SetActive(true);
        Time.timeScale = 0f; 
        music.Pause();
    }
    public void OyunDevam()
    {
        DurdurmaEkraný.SetActive(false);
        Time.timeScale = 1f;
        music.UnPause();
    }
    public void OyunAyarlarý()
    {
        SettingsPanel.SetActive(true);
        DurdurmaEkraný.SetActive(false);
    }
    public void OyunÇýkýþ()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("GiriþScene");
    }
    public void Music()
    {
        isMusicOn = !isMusicOn; // Deðeri tersine çevirir

        if (isMusicOn)
        {
          music.volume = 1f; 
          musicOnIcon.SetActive(true);
          musicOffIcon.SetActive(false);
        }
        else
        {
          music.volume = 0f;  
          musicOffIcon.SetActive(true);
          musicOnIcon.SetActive(false);
        }

    }
    public void ToggleSfx()
    {
        isSfxMuted = !isSfxMuted; 
    }
    public void settingpaneloff()
    {
        SettingsPanel.SetActive(false);
        DurdurmaEkraný.SetActive(true);
    }

    public void UpragePanelOn()
    {
        UpragePanel.SetActive(true);
        TradeShopPanel.SetActive(false);
    }
    public void UpragePanelOff()
    {
        UpragePanel.SetActive(false);
        TradeShopPanel.SetActive(true);
    }
}
