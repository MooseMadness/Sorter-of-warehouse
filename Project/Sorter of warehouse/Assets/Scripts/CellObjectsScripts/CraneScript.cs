using UnityEngine;

public class CraneScript : CellObjectScript
{
    //кол-во клеток которое должен проехать кран перед тем как сбросить ящик
    public int cellCount;
    //ссылка на ящик который везёт кран
    public BoxScript box;
    //направление движения крана
    public MoveDirection moveDirection;
    //скорость движения крана
    public float moveSpeed;

    //кол-во проеханых клеток
    private int count = 0;

    private void Update()
    {        
        if(!moving)
        {
            //продолжение движения или начало уничтожения крана
            CellScript nextCell = moveDirection == MoveDirection.Left ? currCell.leftNeighbor : currCell.rightNeighbor;
            if (nextCell != null)
            {
                StartCoroutine(MoveToCell(nextCell, moveSpeed));
            }
            else
            {
                StartDestroy();
            }
        }
    }

    //начало движения крана
    public void StartMove()
    {
        if (moveDirection == MoveDirection.Left)
            StartCoroutine(MoveToCell(currCell.leftNeighbor, moveSpeed));
        else
            StartCoroutine(MoveToCell(currCell.rightNeighbor, moveSpeed));
    }

    protected override void EndMoveAction()
    {
        count++;
        if (count == cellCount)
        {
            DropBox();
        }
    }

    //сброс ящика
    private void DropBox()
    {
        box.currCell = currCell;
        currCell.cellObject = box;
        box.canFall = true;
        box.transform.SetParent(transform.parent);
        box = null;
    }

    //функция начинающее уничтожение крана
    //в будующем будут добавлены нужные действия
    private void StartDestroy()
    {
        Destroy(gameObject);
    }
}
