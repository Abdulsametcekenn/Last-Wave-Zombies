using UnityEngine;
using UnityEngine.UI;

public class HUDCharacterImage : MonoBehaviour
{
    public Sprite karakter1Sprite;
    public Sprite karakter2Sprite;
    public Image hudImage;
    public Image hudImage1Karakter;

    void Start()
    {
        int selectedIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);

        if (selectedIndex == 0)
        {
            hudImage1Karakter.sprite = karakter1Sprite;
            hudImage.gameObject.SetActive(false);
        }
        else if (selectedIndex == 1)
        {
            hudImage.sprite = karakter2Sprite;
            hudImage1Karakter.gameObject.SetActive(false);
        }
    }
}
