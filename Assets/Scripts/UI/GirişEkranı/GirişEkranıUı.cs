using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GirişEkranıUı : MonoBehaviour
{
    [Header("Giriş Ekranı Butonları")]
    public GameObject OynaButonu;
    public GameObject AyarlarButonu;


    public void Oyna()
    {
        SceneManager.LoadScene("MapSeçme");
    }
}
