using UnityEngine;

//Класс представляющий игровое поле
[ExecuteInEditMode]
public class GameFieldScript : MonoBehaviour
{
    //Префаб клетки
    public GameObject cellPrefab;
    //Кол-во горизонтальных клеток
    public int width;
    //Кол-во вертикальных клеток
    public int height;

    //Массив клеток игрового поля
    //используется 1-мерный массив т.к. Unity не поддерживает 
    //сериализацию 2-мерных массивов
    [SerializeField, HideInInspector]
    private CellScript[] cells;

    //ссылка на объект данного класса в сцене 
    public static GameFieldScript instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            throw new UnityException("Не поддерживается больше 1-го экземпляра игрового поля");
        }
    }

    //индексатор позволяющий работать с 1-мерным массивом клеток
    //как с 2-мерным
    public CellScript this[int i, int j]
    {
        get
        {
            return cells[i * width + j];
        }

        private set
        {
            cells[i * width + j] = value;
        }
    }

    //Строит поле из клеток
    //Может вызываться в редакторе с помощью кнопки
    public void BuildGameField()
    {
        if (cellPrefab == null)
        {
            throw new MissingReferenceException("Не задан префаб для клетки");
        }
        else
        {
            if (height < 0 || width < 0)
            {
                throw new UnityException("Кол-во клеток должно быть положительным числом");
            }
            else
            {
                cells = new CellScript[height * width];
                float cellWidth = cellPrefab.GetComponent<CellScript>().width;
                for (int i = 0; i < height; i++)
                {
                    GameObject row = new GameObject("Row " + i);
                    Vector3 nextPos = transform.position;
                    nextPos.y += cellWidth * i + cellWidth / 2;
                    row.transform.position = nextPos;
                    row.transform.SetParent(transform);
                    nextPos.x += cellWidth / 2;
                    for (int j = 0; j < width; j++)
                    {
                        GameObject cell = (GameObject)Instantiate(cellPrefab, nextPos, transform.rotation);
                        cell.transform.SetParent(row.transform);
                        nextPos.x += cellWidth;
                        CellScript cellScript = cell.GetComponent<CellScript>();
                        this[i, j] = cellScript;
                        if (i != 0)
                        {
                            cellScript.bottomNeighbor = this[i - 1, j];
                            cellScript.bottomNeighbor.topNeighbor = cellScript;
                        }
                        if (j != 0)
                        {
                            cellScript.leftNeighbor = this[i, j - 1];
                            cellScript.leftNeighbor.rightNeighbor = cellScript;
                        }
                    }
                }
            }
        }
    }
}
