using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

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

    //максимальное кол-во рекордов
    private const int maxHighscoresCount = 10;
    //true если текущий счет рекорд
    //иначе false
    private bool isHighscore = false;
    //ссылка на скрипт паузы
    private PauseScript pauseScript;

    private void Awake()
    {
        pauseScript = GetComponent<PauseScript>();
    }

    //отображает меню конца игры
    public void ShowMenu()
    {
        gameMenuRoot.SetActive(false);
        gameOverMenuRoot.SetActive(true);
        if (pauseScript != null)
            pauseScript.enabled = false;
        scoreText.text = GameManagerScript.instance.scoreText.text;
        CheckHighscore();
        if (isHighscore)
        {
            newHighscoreRoot.SetActive(true);
            highscoreNameIF.text = MainMenuScript.defaultHighscoreName;
        }
    }

    //кнопка новой игры
    public void OnNewGameClick()
    {
        Time.timeScale = 1;
        if(isHighscore)
            SaveScore();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //кнопка перехода в главное меню
    public void OnMainMenuClick()
    {
        Time.timeScale = 1;
        if(isHighscore)
            SaveScore();
        SceneManager.LoadScene(mainMenuSceneName);
    }

    //сохраняет текущий рекорд
    private void SaveScore()
    {
        if(MainMenuScript.highscores.Count == maxHighscoresCount)
        {
            MainMenuScript.highscores.RemoveAt(MainMenuScript.highscores.Count - 1);
        }
        MainMenuScript.highscores.Add(new HighscoreRecord(highscoreNameIF.text, GameManagerScript.instance.score));
        MainMenuScript.highscores = MainMenuScript.highscores.OrderByDescending(x => x.score).ToList();
        MainMenuScript.defaultHighscoreName = highscoreNameIF.text;
    }

    //проверяет является ли текущее кол-во очков рекордом
    private void CheckHighscore()
    {
        if(MainMenuScript.highscores.Count == maxHighscoresCount)
        {
            if (GameManagerScript.instance.score > MainMenuScript.highscores[maxHighscoresCount - 1].score)
                isHighscore = true;
            else
                isHighscore = false;
        }
        else
        {
            isHighscore = true;
        }
    }
}
