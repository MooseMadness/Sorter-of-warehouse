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

    //ВРЕМЕНЫЙ КОД
    private bool first = true;

    private void Update()
    {
        //ВРЕМЕНЫЙ КОД
        if(first)
        {
            first = false;
            StartMove();
        }

        
        if (moving)
        {
            //перемещение ящика вместе с краном
            if (box != null)
                box.transform.position = transform.position;
        }
        else
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
        //сброс ящика
        if (count == cellCount)
        {
            box.currCell = currCell;
            currCell.cellObject = box;
            box.canFall = true;
            box = null;
        }
    }

    //функция начинающее уничтожение крана
    //в будующем будут добавлены нужные действия
    private void StartDestroy()
    {
        Destroy(gameObject);
    }
}
