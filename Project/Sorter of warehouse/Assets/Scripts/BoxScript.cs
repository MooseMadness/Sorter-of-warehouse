using UnityEngine;
using System;

//Касс реализующий механику ящика:
//- толкание и падение
//- уничтожение при совпадении цвета
public class BoxScript : CellObjectScript
{
    //перечесление возможных цветов ящика
    public enum BoxColor
    {
        Red,
        White,
        Black,
        Yellow
    }

    //возможные направления для толкания яшика
    public enum PushDirection
    {
        Left,
        Right
    }

    //возможные типы движения ящика
    private enum BoxMoveType
    {
        Fall,
        Push,
        Stay
    }

    //цвет ящика
    public BoxColor boxColor;
    //true если на ящик действует гравитация и он может падать
    //яшик не падает если его держит кран
    public bool canFall = false;
    //скорость падения
    public float fallSpeed;
    //кол-во очков которое даётся за ящик
    public int boxScore;
    //тег которым отмечен игрок
    public string playerTag = "Player";

    //текущее движение ящика
    private BoxMoveType currMove = BoxMoveType.Stay;

    //метод производящий попытку толкнуть ящик
    //dir - направление толчка
    //moveSpeed - скорость толкания
    //возвращает true если толкнуть получилось иначе false
    public bool TryPush(PushDirection dir, float moveSpeed)
    {
        if (moving)
        {
            return false;
        }
        else
        {
            if (dir == PushDirection.Left)
            {
                //если слева и сверху нет ящика толчок разрешается
                if (currCell.leftNeighbor != null && currCell.leftNeighbor.cellObject == null && currCell.topNeighbor.cellObject == null)
                {
                    StartCoroutine(MoveToCell(currCell.leftNeighbor, moveSpeed));
                    currMove = BoxMoveType.Push;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                //если справа и сверху нет ящика толчок разрешается
                if (currCell.rightNeighbor != null && currCell.rightNeighbor.cellObject == null && currCell.topNeighbor.cellObject == null)
                {
                    StartCoroutine(MoveToCell(currCell.rightNeighbor, moveSpeed));
                    currMove = BoxMoveType.Push;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }

    private void Update()
    {
        //падение если под ящиком ничего нет
        if(canFall && !moving && currCell.bottomNeighbor != null && currCell.bottomNeighbor.cellObject == null)
        {
            StartCoroutine(MoveToCell(currCell.bottomNeighbor, fallSpeed));
            currMove = BoxMoveType.Fall;
        }
    }

    //вызывается для уничтожения ящика
    public void DestroyBox()
    {
        currCell.cellObject = null;
        GameManagerScript.instance.ChangeScore(boxScore);
        Destroy(gameObject);
    }

    protected override void EndMoveAction()
    {
        //конец игры если ящик упал в запретную клетку
        if(currMove == BoxMoveType.Fall && currCell.isGameOverCell && currCell.bottomNeighbor.cellObject is BoxScript)
        {
            GameManagerScript.instance.GameOver();
            return;
        }
        currMove = BoxMoveType.Stay;
        int count = 0;
        //подсчёт ящиков с одинаковым цветом
        FindBoxWithSameColor(currCell, (x) => count++);
        if(count >= 3)
        {
            //уничтожение ящиков с одинаковым цветом
            FindBoxWithSameColor(currCell, (x) => x.DestroyBox());
        }
    }

    //проверяет совпадает ли цвет данного ящика 
    //с цветом ящика в клетке переданного в аргументе cell
    //возвращает true если цвет совпадает
    //возвращает false если:
    //- цвет не совпадает 
    //- в клетке нет ящика
    //- клетки нет (cell равен null)
    private bool CompareColor(CellScript cell)
    {
        if(cell == null)
        {
            return false;
        }
        else
        {
            BoxScript box = cell.cellObject as BoxScript;
            if(box == null)
            {
                return false;
            }
            else
            {
                if(box.boxColor == boxColor)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }

    //ищет, среди соседей ящика, ящики с таким же цветом и выполняет для них действие
    //cell - клетка в которой содержится текущий ящик
    //action - делегат, который будет вызван для каждого найденого ящика (включая исходный)
    //startCell - аргумент используемый для избежания зацикливания при рекурсии (когда 2 соседние клетки будут вызывать метод друг для друга по очереди)
    //в нём передаётся ссылка на клетку, которая вызвала данный метод
    //для этой клетки метод не будет вызываться повторно
    //другие виды зацикливания при рекурсии невозможны исходя из механики игры
    private void FindBoxWithSameColor(CellScript cell, Action<BoxScript> action, CellScript startCell = null)
    {
        BoxScript box = cell.cellObject as BoxScript;
        if (box == null)
        {
            throw new UnityException("Попытка искать ящики с одинаковым цветом для клетки без ящика");
        }
        else
        {
            action(box);
            if (cell.bottomNeighbor != startCell && CompareColor(cell.bottomNeighbor))
            {
                FindBoxWithSameColor(cell.bottomNeighbor, action, cell);
            }
            if (cell.topNeighbor != startCell && CompareColor(cell.topNeighbor))
            {
                FindBoxWithSameColor(cell.topNeighbor, action, cell);
            }
            if (cell.leftNeighbor != startCell && CompareColor(cell.leftNeighbor))
            {
                FindBoxWithSameColor(cell.leftNeighbor, action, cell);
            }
            if (cell.rightNeighbor != startCell && CompareColor(cell.rightNeighbor))
            {
                FindBoxWithSameColor(cell.rightNeighbor, action, cell);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        //игра заканчивается если игрок соприкосается с падающим ящиком
        if(coll.tag == playerTag && currMove == BoxMoveType.Fall)
        {
            GameManagerScript.instance.GameOver();
        }
    }
}