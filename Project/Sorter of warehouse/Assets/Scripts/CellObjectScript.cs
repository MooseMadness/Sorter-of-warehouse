using UnityEngine;
using System.Collections;

//Базовый класс для объектов которые могут перемещаться по клеткам
public abstract class CellObjectScript : MonoBehaviour
{
    //ссылка на клетку в которой находится объект
    public CellScript currCell;
    //смещение координат объекта относительно координат клетки
    public Vector3 offset;

    //истина если объект движется
    protected bool moving { get; private set; }

    //корутина перемещающая объект в другую клетку
    //cell - клетка в которую будет производится перемещение
    //moveSpeed - скорость перемещения
    protected IEnumerator MoveToCell(CellScript cell, float moveSpeed)
    {
        if (moveSpeed <= 0)
        {
            throw new UnityException("Скорость не может быть меньшей либо равной 0 (moveSpeed = " + moveSpeed + ")");
        }
        moving = true;
        //по игровой механике объект "переходит" в клетку до того как завершится визуальный переход
        currCell.cellObject = null;
        currCell = cell;
        currCell.cellObject = this;
        while(cell.transform.position != transform.position - offset)
        {
            float deltaSpeed = moveSpeed * Time.deltaTime;
            Vector3 movement = cell.transform.position - (transform.position - offset);
            if (movement.sqrMagnitude > deltaSpeed * deltaSpeed)
            {
                movement = movement.normalized * deltaSpeed;
                transform.Translate(movement); 
            }
            else
            {
                transform.position = cell.transform.position + offset;
            }
            yield return null; //продолжить в следующем update
        }
        EndMoveAction();
        moving = false;
    }

    //чтобы выполнить доп. действия полсе окончания движения
    //потомки должны перегрузить этот метод
    protected abstract void EndMoveAction();

    private void Start()
    {
        moving = false;
        if (currCell != null)
            transform.position = currCell.transform.position + offset;
    }
}
