using UnityEngine;
using System;
using System.Collections.Generic;

//Касс реализующий механику ящика:
//- толкание и падение
//- уничтожение при совпадении цвета
public class BoxScript : CellObjectScript
{
    //перечесление возможных цветов ящика
    public enum BoxColor
    {
        ArrowBox,
        HorizLineBox,
        JustBox,
        StorageBox,
        VertLineBox
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
    //бонус, который содержит ящик
    public BonusScript bonus = null;

    //текущее движение ящика
    private BoxMoveType currMove = BoxMoveType.Stay;
    //истина если ящик уничтожен
    private bool destroyed = false;

    //метод производящий попытку толкнуть ящик
    //dir - направление толчка
    //moveSpeed - скорость толкания
    //возвращает true если толкнуть получилось иначе false
    public bool TryPush(MoveDirection dir, float moveSpeed)
    {
        if (moving)
        {
            return false;
        }
        else
        {
            if (dir == MoveDirection.Left)
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
        if(canFall && !moving && currCell.bottomNeighbor != null && !(currCell.bottomNeighbor.cellObject is BoxScript))
        {
            StartCoroutine(MoveToCell(currCell.bottomNeighbor, fallSpeed));
            currMove = BoxMoveType.Fall;
        }
    }

    //вызывается для уничтожения ящика
    public void DestroyBox()
    {
        if (!destroyed)
        {
            destroyed = true;
            if (bonus != null)
                bonus.UseBonus(currCell);
            currCell.cellObject = null;
            GameManagerScript.instance.ChangeScore(boxScore);
            Destroy(gameObject);
        }
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
        //проверка на совпадение цветов проводится если падение не будет прододжаться
        if (currCell.bottomNeighbor == null || currCell.bottomNeighbor.cellObject is BoxScript)
        {
            int count = 0;
            //подсчёт ящиков с одинаковым цветом
            FindBoxWithSameColor(currCell, (x) => count++);
            if (count >= 3)
            {
                //уничтожение ящиков с одинаковым цветом
                FindBoxWithSameColor(currCell, (x) => x.DestroyBox());
            }
        }
    }

    //проверяет совпадает ли цвет данного ящика 
    //с цветом ящика в клетке переданного в аргументе cell
    //возвращает true если цвет совпадает
    //возвращает false если:
    //- цвет не совпадает 
    //- в клетке нет ящика
    //- клетки нет (cell равен null)
    //цвет сравнивается с цветом ящика для которого вызван метод
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
    //passedCells - список уже проверенных клеток. используется для избежания зацикливания    
    private void FindBoxWithSameColor(CellScript cell, Action<BoxScript> action, List<CellScript> passedCells = null)
    {
        BoxScript box = cell.cellObject as BoxScript;
        if (box == null)
        {
            throw new UnityException("Попытка найти ящики с одинаковы цветом для клетки без ящика");
        }
        else
        {
            if (passedCells == null)
            {
                passedCells = new List<CellScript>();
            }
            passedCells.Add(cell);
            if (CheckCell(cell.leftNeighbor, passedCells))
                FindBoxWithSameColor(cell.leftNeighbor, action, passedCells);
            if (CheckCell(cell.rightNeighbor, passedCells))
                FindBoxWithSameColor(cell.rightNeighbor, action, passedCells);
            if (CheckCell(cell.topNeighbor, passedCells))
                FindBoxWithSameColor(cell.topNeighbor, action, passedCells);
            if (CheckCell(cell.bottomNeighbor, passedCells))
                FindBoxWithSameColor(cell.bottomNeighbor, action, passedCells);
            action(box);
        }
    }

    //проверка клетки на совпадение по цвету и на принадлежность к пройденным клеткам
    //cell - клетка на проверку
    //passedCells - массив пройденных клеток
    //если клетка не пройденна и подходит по цвету возвращает true
    //иначе возвращает false    
    private bool CheckCell(CellScript cell, List<CellScript> passedCells)
    {
        if(CompareColor(cell))
        {
            foreach(CellScript passedCell in passedCells)
            {
                if(passedCell == cell)
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        //игра заканчивается если игрок соприкосается с падающим ящиком
        if(coll.tag == playerTag && currMove == BoxMoveType.Fall)
        {
            GameManagerScript.instance.GameOver();
        }
    }

    //добавляет к ящику иконку бонуса
    public void InitBonusGraphic()
    {
        if (bonus == null)
        {
            throw new UnityException("Попытка добавить графику бонуса к ящику без бонуса");
        }
        else
        {
            GameObject bonusGO = new GameObject(gameObject.name + "Bonus", typeof(SpriteRenderer));
            bonusGO.transform.position = transform.position;
            SpriteRenderer bonusRender = bonusGO.GetComponent<SpriteRenderer>();
            bonusRender.sprite = bonus.bonusGraphic;
            bonusRender.sortingOrder = 1;
            bonusGO.transform.SetParent(transform);
        }
    } 
}