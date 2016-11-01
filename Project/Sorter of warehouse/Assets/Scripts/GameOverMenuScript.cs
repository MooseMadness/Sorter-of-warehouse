using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Класс отвечающий за меню появляющееся после окончания игры
public class GameOverMenuScript : MonoBehaviour
{
    //ссылка на игровое меню
    public GameObject gameMenuRoot;
    //ссылка на меню конца игры
    public GameObject gameOverMenuRoot;
    //ссылка на подменю нового рекорда
    public GameObject newHighscoreRoot;
    //ссылка на поле для ввода имени для нового рекорда
    public InputField highscoreNameIF;
    //ссылка на текст с кол-вом набранных очков
    public Text scoreText;
    //название сцены с главным меню
    public string mainMenuSceneName = "MainMenuScene";

    //отображает меню конца игры
    public void ShowMenu()
    {
        gameMenuRoot.SetActive(false);
        gameOverMenuRoot.SetActive(true);
        scoreText.text = GameManagerScript.instance.scoreText.text;
    }

    //кнопка новой игры
    public void OnNewGameClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //кнопка перехода в главное меню
    public void OnMainMenuClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
