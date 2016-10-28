﻿using UnityEngine;
using System.Collections;

//Базовый класс для объектов которые могут перемещаться по клеткам
public class CellObjectScript : MonoBehaviour
{
    //ссылка на клетку в которой находится объект
    public CellScript currCell;
    //смещение координат объекта относительно координат клетки
    public Vector3 offset;

    //истина если объект движется
    protected bool moving { get; private set; }

    //корутина перемещающая объект в другую клетку
    public IEnumerator MoveToCell(CellScript cell, float moveSpeed)
    {
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
    protected virtual void EndMoveAction() { }

    private void Start()
    {
        moving = false;
        if (currCell != null)
            transform.position = currCell.transform.position + offset;
    }
}
