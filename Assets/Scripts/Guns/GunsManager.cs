using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GunsManager : MonoBehaviour
{
    PlayerMovement playerMovement;
    Guns guns;

    #region Pistol Prefabs
    [Header("Pistol Prefabs")]
    public GameObject pistolPrefab1;
    public GameObject pistolPrefab2;
    public GameObject pistolPrefab4;
    public GameObject pistolPrefab5;
    #endregion

    #region Shotgun Prefabs
    [Header("Shotgun Prefabs")]
    public GameObject shotgunPrefab1;
    public GameObject shotgunPrefab2;
    public GameObject shotgunPrefab3;
    public GameObject shotgunPrefab4;
    #endregion

    #region SMG Prefabs
    [Header("SMG Prefabs")]
    public GameObject smgPrefab1;
    public GameObject smgPrefab2;
    public GameObject smgPrefab3;
    public GameObject smgPrefab4;
    #endregion

    #region Assault Rifle Prefabs
    [Header("Assault Rifle Prefabs")]
    public GameObject assaultRiflePrefab1;
    public GameObject assaultRiflePrefab2;
    public GameObject assaultRiflePrefab3;
    public GameObject assaultRiflePrefab4;
    #endregion
    [Header("HealthWeapon")]
    public GameObject healthWeaponPrefab;

    private bool[] satinAlinanSilahlar = new bool[20];
    private int aktifSilahIndex = 0;

    public Transform pistolSlot;
    public Transform anaSilahSlot;
    public Transform healthWeaponSlot;


    public TMP_Text MermiSayısı;

    private GameObject mevcutPistol;
    private GameObject mevcutSilah;
    private GameObject HealthWeapon;

    private bool healthWeaponAlindi = false;

    private int silahhasararttırmasayısı=0;
    private int canarttırmasayısı=0;
    private int şarjörarttırmasayısı = 0;

    private void Start()
    {
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        guns = FindAnyObjectByType<Guns>();
        pistolSlot =GameObject.FindGameObjectWithTag("Pistolslot").transform;
        anaSilahSlot=GameObject.FindGameObjectWithTag("Gunslot").transform;
        healthWeaponSlot=GameObject.FindGameObjectWithTag("Healthslot").transform;
    }
    private void Update()
    {
        // Aktif silahı bul
        Guns aktifSilah = null;

        if (anaSilahSlot.gameObject.activeSelf && mevcutSilah != null)
        {
            aktifSilah = mevcutSilah.GetComponent<Guns>();
        }
        else if (pistolSlot.gameObject.activeSelf && mevcutPistol != null)
        {
            aktifSilah = mevcutPistol.GetComponent<Guns>();
        }
        else if (healthWeaponSlot.gameObject.activeSelf && HealthWeapon != null)
        {
            aktifSilah = HealthWeapon.GetComponent<Guns>();
        }

        // Eğer silah varsa, mermi sayısını güncelle
        if (aktifSilah != null && MermiSayısı != null)
        {
            MermiSayısı.text = $"{aktifSilah.currentAmmo} / {aktifSilah.totalAmmo}";
        }
        else
        {
            // Eğer elimizde silah yoksa mermi göstergesini temizle
            if (MermiSayısı != null)
                MermiSayısı.text = "- / -";
        }
    }

    private void SilahEkle(GameObject prefab, Transform slot, ref GameObject mevcutSilah)
    {
        if (mevcutSilah != null)
            Destroy(mevcutSilah);

        mevcutSilah = Instantiate(prefab, slot);
        mevcutSilah.transform.localPosition = Vector3.zero;
        mevcutSilah.transform.localRotation = Quaternion.identity;
    }

    public void SatinAlVeyaSec(int silahIndex, GameObject prefab, int fiyat, string tur)
    {
        if (tur == "health weapon")
        {
            if (playerMovement.coin >= fiyat)
            {
                playerMovement.coin -= fiyat;
                SilahEkle(prefab, healthWeaponSlot, ref HealthWeapon);
                pistolSlot.gameObject.SetActive(false);
                anaSilahSlot.gameObject.SetActive(false);
                healthWeaponSlot.gameObject.SetActive(true);
            }
            return; // Burada çıkıyoruz, diğer mantık çalışmıyor
        }
        if (!satinAlinanSilahlar[silahIndex])
        {
            if (playerMovement.coin >= fiyat)
            {
                playerMovement.coin -= fiyat;
                satinAlinanSilahlar[silahIndex] = true;
            }
            else
            {
                Debug.Log("Yetersiz coin.");
                return;
            }
        }

        // Satın alındı veya zaten alınmışsa silahı tak
        if (tur == "pistol")
        {
            SilahEkle(prefab, pistolSlot, ref mevcutPistol);
            healthWeaponSlot.gameObject.SetActive(false);
            anaSilahSlot.gameObject.SetActive(false);
            pistolSlot.gameObject.SetActive(true);
        }
        else if (tur == "shotgun"||tur=="smg"||tur=="assault rifle")
        {
            SilahEkle(prefab, anaSilahSlot, ref mevcutSilah);
            healthWeaponSlot.gameObject.SetActive(false);
            pistolSlot.gameObject.SetActive(false);
            anaSilahSlot.gameObject.SetActive(true);
        }
        else if (tur == "health weapon")
        {
            SilahEkle(prefab, healthWeaponSlot, ref HealthWeapon);
            pistolSlot.gameObject.SetActive(false);
            anaSilahSlot.gameObject.SetActive(false);
            healthWeaponSlot.gameObject.SetActive(true);
        }

    }

    #region Pistol
    public void SatinAlPistol1() => SatinAlVeyaSec(0, pistolPrefab1, 100, "pistol");
    public void SatinAlPistol2() => SatinAlVeyaSec(1, pistolPrefab2, 200, "pistol");
    public void SatinAlPistol4() => SatinAlVeyaSec(3, pistolPrefab4, 400, "pistol");
    public void SatinAlPistol5() => SatinAlVeyaSec(4, pistolPrefab5, 700, "pistol");
    #endregion

    #region Shotgun
    public void SatinAlShotgun1() => SatinAlVeyaSec(5, shotgunPrefab1, 900, "shotgun");
    public void SatinAlShotgun2() => SatinAlVeyaSec(6, shotgunPrefab2, 1100, "shotgun");
    public void SatinAlShotgun3() => SatinAlVeyaSec(7, shotgunPrefab3, 1200, "shotgun");
    public void SatinAlShotgun4() => SatinAlVeyaSec(8, shotgunPrefab4, 1250, "shotgun");
    #endregion

    #region SMG
    public void SatinAlSMG1() => SatinAlVeyaSec(10, smgPrefab1, 1600, "smg");
    public void SatinAlSMG2() => SatinAlVeyaSec(11, smgPrefab2, 1750, "smg");
    public void SatinAlSMG3() => SatinAlVeyaSec(12, smgPrefab3, 1900, "smg");
    public void SatinAlSMG4() => SatinAlVeyaSec(13, smgPrefab4, 2100, "smg");
    #endregion

    #region Assault Rifle
    public void SatinAlAssaultRifle1() => SatinAlVeyaSec(14, assaultRiflePrefab1, 2250, "assault rifle");
    public void SatinAlAssaultRifle2() => SatinAlVeyaSec(15, assaultRiflePrefab2, 2400, "assault rifle");
    public void SatinAlAssaultRifle3() => SatinAlVeyaSec(16, assaultRiflePrefab3, 2450, "assault rifle");
    public void SatinAlAssaultRifle4() => SatinAlVeyaSec(17, assaultRiflePrefab4, 2500, "assault rifle");
    #endregion
    public void SatinAlHealthWeapon()
    {
        if (healthWeaponAlindi)
        {
            return;
        }

        SatinAlVeyaSec(18, healthWeaponPrefab, 1000, "health weapon");
        healthWeaponAlindi = true;
    }
    public void ResetHealthWeapon()
    {
        healthWeaponAlindi = false;
    }
    public void SilahDegistir()
    {
        // Tüm silahları kapat
        anaSilahSlot.gameObject.SetActive(false);
        pistolSlot.gameObject.SetActive(false);
        healthWeaponSlot.gameObject.SetActive(false);

        // Sıradaki silahı aç
        switch (aktifSilahIndex)
        {
            case 0:
                anaSilahSlot.gameObject.SetActive(true);
                break;
            case 1:
                pistolSlot.gameObject.SetActive(true);
                break;
            case 2:
                healthWeaponSlot.gameObject.SetActive(true);
                break;
        }

        // Sıradaki index’e geç (0 → 1 → 2 → 0 …)
        aktifSilahIndex = (aktifSilahIndex + 1) % 3;
    }
    public void BuyAmmo()
    {
        Guns aktifSilahScript = null;

        if (anaSilahSlot.gameObject.activeSelf && mevcutSilah != null)
        {
            aktifSilahScript = mevcutSilah.GetComponent<Guns>();
        }
        else if (pistolSlot.gameObject.activeSelf && mevcutPistol != null)
        {
            aktifSilahScript = mevcutPistol.GetComponent<Guns>();
        }

        if (aktifSilahScript != null)
        {
            int eksikSarjor = aktifSilahScript.maxMagazine - aktifSilahScript.currentAmmo;
            int eksikToplam = aktifSilahScript.maxTotalAmmo - aktifSilahScript.totalAmmo;
            int toplamEksikMermi = eksikSarjor + eksikToplam;

            if (toplamEksikMermi > 0)
            {
                int toplamMaliyet = toplamEksikMermi * aktifSilahScript.ammoPrice;

                if (playerMovement.coin >= toplamMaliyet)
                {
                    playerMovement.coin -= toplamMaliyet;
                    aktifSilahScript.currentAmmo = aktifSilahScript.maxMagazine;
                    aktifSilahScript.totalAmmo = aktifSilahScript.maxTotalAmmo;
                }
            }
        }
    }


    public void BuyFullAmmo()
    {
        int toplamMaliyet = 0;

        Guns anaSilahScript = mevcutSilah != null ? mevcutSilah.GetComponent<Guns>() : null;
        Guns pistolScript = mevcutPistol != null ? mevcutPistol.GetComponent<Guns>() : null;

        if (anaSilahScript != null)
        {
            int eksikSarjor = anaSilahScript.maxMagazine - anaSilahScript.currentAmmo;
            int eksikToplam = anaSilahScript.maxTotalAmmo - anaSilahScript.totalAmmo;
            toplamMaliyet += (eksikSarjor + eksikToplam) * anaSilahScript.ammoPrice;
        }

        if (pistolScript != null)
        {
            int eksikSarjor = pistolScript.maxMagazine - pistolScript.currentAmmo;
            int eksikToplam = pistolScript.maxTotalAmmo - pistolScript.totalAmmo;
            toplamMaliyet += (eksikSarjor + eksikToplam) * pistolScript.ammoPrice;
        }

        if (toplamMaliyet <= 0) return;

        if (playerMovement.coin >= toplamMaliyet)
        {
            playerMovement.coin -= toplamMaliyet;

            if (anaSilahScript != null)
            {
                anaSilahScript.currentAmmo = anaSilahScript.maxMagazine;
                anaSilahScript.totalAmmo = anaSilahScript.maxTotalAmmo;
            }   

            if (pistolScript != null)
            {
                pistolScript.currentAmmo = pistolScript.maxMagazine;
                pistolScript.totalAmmo = pistolScript.maxTotalAmmo;
            }
        }
    }

    public void ReloadCurrentGun()
    {
        Guns aktifSilahScript = null;

        if (anaSilahSlot.gameObject.activeSelf && mevcutSilah != null)
        {
            aktifSilahScript = mevcutSilah.GetComponent<Guns>();
        }
        else if (pistolSlot.gameObject.activeSelf && mevcutPistol != null)
        {
            aktifSilahScript = mevcutPistol.GetComponent<Guns>();
        }

        if (aktifSilahScript != null)
        {
            StartCoroutine(aktifSilahScript.Reload());
        }
    }
    public void SilahHasarArttır()
    {
        int fiyat = 300; // Örneğin 50 coin harcanacak
        if (silahhasararttırmasayısı >= 5) return;
        if (playerMovement.coin < fiyat) return; // Yetersiz coin kontrolü

        playerMovement.coin -= fiyat;
        silahhasararttırmasayısı++;

        if (mevcutPistol != null)
            mevcutPistol.GetComponent<Guns>().bulletDamage += 10;

        if (mevcutSilah != null)
            mevcutSilah.GetComponent<Guns>().bulletDamage += 10;
    }

    public void OyuncuCanArttır()
    {
        int fiyat = 200; // Örneğin 50 coin harcanacak
        if (canarttırmasayısı >= 5) return;
        if (playerMovement.coin < fiyat) return; // Yetersiz coin kontrolü

        playerMovement.coin -= fiyat;
        canarttırmasayısı++;

        playerMovement.maxHealth += 10;
        playerMovement.health = playerMovement.maxHealth;
        playerMovement.HealthBar.fillAmount = 1f;
        playerMovement.BackGround.fillAmount = 1f;
    }

    public void ŞarjörArttırma()
    {
        int fiyat = 100; // Örneğin 50 coin harcanacak
        if (şarjörarttırmasayısı >= 5) return;
        if (playerMovement.coin < fiyat) return; // Yetersiz coin kontrolü

        playerMovement.coin -= fiyat;
        şarjörarttırmasayısı++;

        if (mevcutPistol != null)
        {
            mevcutPistol.GetComponent<Guns>().maxMagazine += 5;
            mevcutPistol.GetComponent<Guns>().totalAmmo += 5;
            mevcutPistol.GetComponent<Guns>().maxTotalAmmo += 5;
            mevcutPistol.GetComponent<Guns>().currentAmmo += 5;
        }

        if (mevcutSilah != null)
        {
            mevcutSilah.GetComponent<Guns>().maxMagazine += 5;
            mevcutSilah.GetComponent<Guns>().totalAmmo += 5;
            mevcutSilah.GetComponent<Guns>().maxTotalAmmo += 5;
            mevcutSilah.GetComponent<Guns>().currentAmmo += 5;
        }
    }
}
