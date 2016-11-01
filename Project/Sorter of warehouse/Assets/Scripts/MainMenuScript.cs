using UnityEngine;
using UnityEngine.SceneManagement;

//Класс управляющий главным меню
public class MainMenuScript : MonoBehaviour
{
    //ссылка на главное меню
    public GameObject mainMenuRoot;
    //ссылка на таблицу рекордов
    public GameObject highscoreRoot;
    //название сцены с игрой
    public string gameSceneName = "GameScene";

    //клик по кнопке запуска игры
    public void OnStartGameClick()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    //клик по кнопке перехода между главным меню и меню с таблицой рекордов
    public void OnSwitchMenu()
    {
        mainMenuRoot.SetActive(!mainMenuRoot.activeSelf);
        highscoreRoot.SetActive(!mainMenuRoot.activeSelf);
    }

    //клик по кнопке выхода
    public void OnExitClick()
    {
        Application.Quit();
    }
}
