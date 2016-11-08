using UnityEngine;

//Класс клетки на которые разбито игровое поле
[System.Serializable]
public class CellScript : MonoBehaviour
{
    //истина если при попадании в ячейку ящика игра должна заканчиваться
    public bool isGameOverCell = false;
    //ссылка на объект находящийся в ячейке
    public CellObjectScript cellObject = null;

    //ссылки на соседние ячейки
    public CellScript topNeighbor = null;
    public CellScript bottomNeighbor = null;
    public CellScript leftNeighbor = null;
    public CellScript rightNeighbor = null;

    //ширина ячейки (ячейки имеют форму квадрата)
    public float width;

    //отрисовывает ячейку в редакторе
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        float widthDiv2 = width / 2;
        Vector3 pos = transform.position;
        Gizmos.DrawLine(new Vector3(pos.x - widthDiv2, pos.y + widthDiv2, pos.z),
                        new Vector3(pos.x + widthDiv2, pos.y + widthDiv2, pos.z));
        Gizmos.DrawLine(new Vector3(pos.x - widthDiv2, pos.y - widthDiv2, pos.z),
                        new Vector3(pos.x + widthDiv2, pos.y - widthDiv2, pos.z));
        Gizmos.DrawLine(new Vector3(pos.x - widthDiv2, pos.y - widthDiv2, pos.z),
                        new Vector3(pos.x - widthDiv2, pos.y + widthDiv2, pos.z));
        Gizmos.DrawLine(new Vector3(pos.x + widthDiv2, pos.y - widthDiv2, pos.z),
                        new Vector3(pos.x + widthDiv2, pos.y + widthDiv2, pos.z));
    }
}
