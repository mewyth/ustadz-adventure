using UnityEngine;
using UnityEngine.SceneManagement;
public class uimanager : MonoBehaviour
{
    public GameObject menu, option;
    public void openoption()
    {
        menu.SetActive(false);
        option.SetActive(true);
    }
    public void openmenu()
    {
        menu.SetActive(true);
        option.SetActive(false);
    }
    public void startgame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void ExitButton()
    {
        Application.Quit();
        Debug.Log("Game Closed");
    }
}
