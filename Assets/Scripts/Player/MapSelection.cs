using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelection : MonoBehaviour
{
    // Harita seçim butonlarýndan çaðýrýlacak
    public void SelectMap(string mapSceneName)
    {
        PlayerPrefs.SetString("SelectedMap", mapSceneName);
        PlayerPrefs.Save();

        // Karakter seçme sahnesine git
        SceneManager.LoadScene("KarakterSeçme");
    }
}
