using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;

//Структура описывающая запись в таблице рекордов
[System.Serializable]
public struct HighscoreRecord
{
    public string name;
    public int score;

    public HighscoreRecord(string name, int score)
    {
        this.name = name;
        this.score = score;
    }
}

//Класс управляющий главным меню
public class MainMenuScript : MonoBehaviour
{
    //ссылка на главное меню
    public GameObject mainMenuRoot;
    //ссылка на таблицу рекордов
    public GameObject highscoreRoot;
    //название сцены с игрой
    public string gameSceneName = "GameScene";
    //ссылка на текст для вывода названий рекордов
    public Text scoreNamesText;
    //ссылка на текст для вывода значений рекордов
    public Text scoresText;

    //ссылка на массив рекордов
    public static List<HighscoreRecord> highscores;
    //имя рекорда по умолчанию
    public static string defaultHighscoreName;

    //переменная используемая для проведения инициализации при запуске игры
    //а не при каждом переходе в сцену главного меню
    private static bool isInit = false;

    //путь к папке с данными игры
    private string dataDir;

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

    private void Awake()
    {
        if (!isInit)
        {
            dataDir = Application.persistentDataPath;
            CheckDefaultName();
            ReadHighscoreFromFile();
            isInit = true;
        }
        CreateScoreTable();
    }

    //проверяет есть ли сохранненое имя для рекорда
    //(последнее введенное в таблицу имя)
    //если нет, то подставляет имя по умолчанию
    private void CheckDefaultName()
    {
        if(!PlayerPrefs.HasKey("DefaultScoreName"))
        {
            PlayerPrefs.SetString("DefaultScoreName", "<default>");
            defaultHighscoreName = "<default>";
        }
        else
        {
            defaultHighscoreName = PlayerPrefs.GetString("DefaultScoreName");
        }
    }

    //попытка считать рекорды из файла
    private void ReadHighscoreFromFile()
    {
        if (File.Exists(dataDir + "/highscore.dat"))
        {
            BinaryFormatter binFormatter = new BinaryFormatter();
            using (FileStream dataFile = File.Open(dataDir + "/highscore.dat", FileMode.Open))
            {
                highscores = (List<HighscoreRecord>)binFormatter.Deserialize(dataFile);
            }
        }
        else
        {
            highscores = new List<HighscoreRecord>();
        }
    }

    //Заполнение таблицы рекордов
    private void CreateScoreTable()
    {
        if(highscores == null)
        {
            throw new UnityException("Попытка заполнить таблицу рекордов до их инициализации");
        }
        else
        {
            scoreNamesText.text = "";
            scoresText.text = "";
            foreach(HighscoreRecord highscore in highscores)
            {
                scoreNamesText.text += highscore.name + "\n";
                scoresText.text += highscore.score + "\n";
            }
        }
    }

    //сохраняет данные используемы между игровыми сесиями
    private void SaveData()
    {
        PlayerPrefs.SetString("DefaultScoreName", defaultHighscoreName);
        BinaryFormatter binFormater = new BinaryFormatter();
        using (FileStream dataFile = File.Open(dataDir + "/highscore.dat", FileMode.OpenOrCreate))
        {
            binFormater.Serialize(dataFile, highscores);
        }
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }
}