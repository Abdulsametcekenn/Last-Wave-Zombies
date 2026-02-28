using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Karakterölümuı : MonoBehaviour
{
    Animator anim;

    public Button restartButton;
    public Button exitButton;
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetTrigger("KarakterÖldü");
    }
    public void RestartGame()
    {
        string selectedMap = PlayerPrefs.GetString("SelectedMap", "DefaultScene");
        SceneManager.LoadScene(selectedMap);
    }
    public void ExitGame()
    {
        SceneManager.LoadScene("GirişScene");
    }
}
