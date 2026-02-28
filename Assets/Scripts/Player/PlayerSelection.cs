using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelection : MonoBehaviour
{
    private GameObject[] Players;
    private int index = 0;

    private void Start()
    {
        // Oyuncu listesi
        Players = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            Players[i] = transform.GetChild(i).gameObject;
        }

        // Kayýtlý karakteri al
        index = PlayerPrefs.GetInt("SelectedCharacter", 0);

        // Hepsini kapat
        foreach (GameObject go in Players)
            go.SetActive(false);

        // Seçili olaný aç
        if (index >= 0 && index < Players.Length)
            Players[index].SetActive(true);
        else
        {
            index = 0;
            Players[index].SetActive(true);
        }
    }

    public void ToggleLeft()
    {
        Players[index].SetActive(false);
        index--;
        if (index < 0)
            index = Players.Length - 1;
        Players[index].SetActive(true);
    }

    public void ToggleRight()
    {
        Players[index].SetActive(false);
        index++;
        if (index >= Players.Length)
            index = 0;
        Players[index].SetActive(true);
    }

    public void SelectPlayer()
    {
        PlayerPrefs.SetInt("SelectedCharacter", index);
        PlayerPrefs.Save();

        // Seçilen haritayý yükle
        string selectedMap = PlayerPrefs.GetString("SelectedMap", "Map1");
        SceneManager.LoadScene(selectedMap);
    }
}
