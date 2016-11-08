using UnityEngine;
using UnityEngine.UI;

//Класс управляющий игрой
//в сцене может существовать только один экземпляр данного класса
[RequireComponent(typeof(GameOverMenuScript))]
public class GameManagerScript : MonoBehaviour
{
    //ссылка на объект данного класса в сцене 
    public static GameManagerScript instance = null;
    //ссылка на текст для вывода очков
    public Text scoreText;
    //ссылка на левую клетку для респавна кранов
    public CellScript leftCraneCell;
    //ссылка на правую клетку для респавна кранов
    public CellScript rightCraneCell;
    //ссылка на префаб крана
    public GameObject cranePrefab;
    //ссылки на префабы ящиков
    public GameObject[] boxPrefabs;
    //текущее время между спавнами кранов
    public float craneSpawnTime;
    //минимальное время между спавнами кранов
    public float minSpawnTime;
    //скорость уменьшения времени между спавнами
    public float spawnTimeDecreaseSpeed;
    //ширина игрового поля (в клетках)
    public int gameFieldWidth;
    //массив используеммых боннусов
    public BonusScript[] bonuses;

    //текущее кол-во очков
    public int score { get; private set; }

    //таймер используемый для отсчета времени между спавнами кранов
    private float craneTimer = 0;
    //ссылка на компонент отвечающий за меню конца игры
    private GameOverMenuScript gameOverMenuScript;

    private void Awake()
    {
        //если объект с данным компонентом уже есть в сцене, то этот объект будет уничтожен
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        if(!CheckPrefabsType())
        {
            throw new UnityException("У префабов не найдены нужные компоненты");
        }
        if(craneSpawnTime < 0 || minSpawnTime < 0)
        {
            throw new UnityException("Время между спавнами кранов не может быть отрицательным");
        }
        if(gameFieldWidth <= 0)
        {
            throw new UnityException("Ширина игрового поля должна быть больше 0");
        }
        gameOverMenuScript = GetComponent<GameOverMenuScript>();
    }

    //создаёт необходимые объекты бонусов
    private void CheckBonusesProbs()
    {
        float sumProb = 0;
        foreach(BonusScript bonus in bonuses)
        {
            sumProb += bonus.bonusProbability;
        }
        if(sumProb > 1f)
        {
            throw new UnityException("Суммарная вероянтность появления бонуса не может быть больше 1");
        }
    }

    //проверяет на наличие у префабов необходимых компонентов
    //true - все компоненты присутствуют
    //false - у какого-либо префаба отсутствует нужный компонент
    private bool CheckPrefabsType()
    {
        CraneScript craneScript = cranePrefab.GetComponent<CraneScript>();
        if (craneScript == null)
            return false;
        foreach(GameObject boxPrefab in boxPrefabs)
        {
            BoxScript boxScript = boxPrefab.GetComponent<BoxScript>();
            if (boxScript == null)
                return false;
        }
        return true;
    }

    //вызывается когда игрок проиграл
    public void GameOver()
    {
        gameOverMenuScript.ShowMenu();
        Time.timeScale = 0;
        enabled = false;
    }   
    
    //изменяет счет на величину указанную в amount
    public void ChangeScore(int amount)
    {
        score += amount;
        scoreText.text = "Score: " + score;
    } 

    private void Update()
    {
        craneTimer += Time.deltaTime;
        if (craneSpawnTime > minSpawnTime)
        {
            if(craneTimer >= craneSpawnTime)
            {
                SpawnCrane();
                craneSpawnTime -= spawnTimeDecreaseSpeed;
                craneTimer = 0;
            }
        }
        else
        {
            if(craneTimer >= minSpawnTime)
            {
                SpawnCrane();
                craneTimer = 0;
            }
        }
    }

    //спавн крана
    private void SpawnCrane()
    {
        int cellToDrop = Random.Range(1, gameFieldWidth + 1);
        int boxPrefabIndex = Random.Range(0, boxPrefabs.Length);
        MoveDirection craneMoveDir = (MoveDirection)Random.Range(0, 2);
        CraneScript crane;

        //создание крана двигающегося в нужном напрвалении
        if(craneMoveDir == MoveDirection.Left)
        {
            crane = ((GameObject)Instantiate(cranePrefab, rightCraneCell.transform.position, Quaternion.identity)).GetComponent<CraneScript>();
            crane.currCell = rightCraneCell;
            rightCraneCell.cellObject = crane;
        }
        else
        {
            crane = ((GameObject)Instantiate(cranePrefab, leftCraneCell.transform.position, Quaternion.identity)).GetComponent<CraneScript>();
            crane.currCell = leftCraneCell;
            leftCraneCell.cellObject = crane;
        }
        crane.moveDirection = craneMoveDir;

        crane.cellCount = cellToDrop;

        //создание нужного ящика
        BoxScript box = ((GameObject)Instantiate(boxPrefabs[boxPrefabIndex], crane.transform.position, Quaternion.identity)).GetComponent<BoxScript>();
        crane.box = box;
        box.bonus = SpawnBonus();
        if (box.bonus != null)
            box.InitBonusGraphic();
    }

    //спавн бонуса
    //бонус может и не заспавниться
    //в этом случае метод вернет null
    private BonusScript SpawnBonus()
    {
        float randomNumber = Random.Range(0, 1f);
        float bonusProb = 0f;
        foreach(BonusScript bonus in bonuses)
        {
            bonusProb += bonus.bonusProbability;
            if (randomNumber <= bonusProb)
                return bonus;
        }
        return null;
    }
}