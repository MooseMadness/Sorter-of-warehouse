using UnityEngine;

//Класс реализующий управление персонажем
[RequireComponent(typeof(Animator))]
public class CharacterControlScript : CellObjectScript
{
    //скорость движения (лево/право)
    public float moveSpeed;
    //скорость падения
    public float fallSpeed;
    //скорость прыжка
    public float jumpSpeed;

    //перечесление возможных видов передвежения персонажа
    private enum MoveType
    {
        Jump,
        Move,
        Fall,
        Stay
    }

    //ссылка на компонент аниматор
    private Animator animControl;
    //текущее движене
    private MoveType currMove = MoveType.Stay;
    //движение которое будет выполнено после завершения текущего
    private MoveType nextMove = MoveType.Stay;
    //клетка в которую будет выполнен переход после завершения текущего
    //(при условии что nextMove == MoveType.Move)
    private CellScript nextTarget;

    private void Awake()
    {
        animControl = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!moving)
        {
            //проверка на следующее движение не проводится в EndMoveAction
            //чтобы избежать вызова корутины из самой себя (приведет к некорректному значению moving)
            if (nextMove == MoveType.Stay)
            {
                //падение если под ногами ничего нет то игрок падает
                if (currCell.bottomNeighbor != null && currCell.bottomNeighbor.cellObject == null)
                {
                    StartCoroutine(MoveToCell(currCell.bottomNeighbor, fallSpeed));
                    currMove = MoveType.Fall;
                }
            }
            else
            {
                //выбор следующего движения если во время предыдущего была нажата кнопка
                switch (nextMove)
                {
                    case MoveType.Jump:
                    {
                        if (currCell.bottomNeighbor == null || currCell.bottomNeighbor.cellObject != null)
                        {
                            StartCoroutine(MoveToCell(currCell.topNeighbor, jumpSpeed));
                            currMove = MoveType.Jump;
                        }
                        break;
                    }

                    case MoveType.Move:
                    {
                        //игрок не может толкать ящики в прыжке
                        if (nextTarget.cellObject == null)
                        {
                            StartMove(nextTarget);
                        }
                        break;
                    }
                }
                nextMove = MoveType.Stay;
            }
        }
    }

    //прыжок игрока
    public void Jump()
    {
        if (!currCell.isGameOverCell)
        {
            if (!moving)
            {
                if (currCell.bottomNeighbor == null || currCell.bottomNeighbor.cellObject != null)
                {
                    StartCoroutine(MoveToCell(currCell.topNeighbor, jumpSpeed));
                    currMove = MoveType.Jump;
                }
            }
            else if(currMove != MoveType.Fall) //если "прыгнуть" во время движения, то игрок прыгнет после завершения движения
            {
                nextMove = MoveType.Jump;
            }
        }
    }

    //команда движения игрока в указаном направлении
    //dir - направление движения
    public void Move(MoveDirection dir)
    {
        CellScript targetCell;
        MoveDirection targetDir;
        if(dir == MoveDirection.Left)
        {
            targetCell = currCell.leftNeighbor;
            targetDir = MoveDirection.Left;
        }
        else
        {
            targetCell = currCell.rightNeighbor;
            targetDir = MoveDirection.Right;
        }
        if (moving)
        {
            //если во время прыжка поступит команда движение влево, то игрок сместится влево после прыжка
            if (currMove == MoveType.Jump && targetCell != null)
            {
                nextMove = MoveType.Move;
                nextTarget = targetCell;
            }
            //если во время движения поступила команда прыжка а затем команда движения, то прыжок отменится 
            else if (nextMove == MoveType.Jump)
            {
                nextMove = MoveType.Stay;
            }
        }
        else if (targetCell != null)
        {
            if (targetCell.cellObject == null)
            {
                StartMove(targetCell);
            }
            else //попытка толкнуть ящик влево
            {
                BoxScript box = targetCell.cellObject as BoxScript;
                if (box != null)
                {
                    if (box.TryPush(targetDir, moveSpeed))
                    {
                        StartMove(targetCell);
                    }
                }
                else
                {
                    throw new UnityException("Непредвиденный тип объекта в клетке");
                }
            }
        }
    }

    //нужен в случаях когда вызывающий скрипт не может передать направление движения
    //например UnityEngine.UI.Button при событии OnClick
    public void MoveRght()
    {
        Move(MoveDirection.Right);
    }

    //нужен в случаях когда вызывающий скрипт не может передать направление движения
    //например UnityEngine.UI.Button при событии OnClick
    public void MoveLeft()
    {
        Move(MoveDirection.Left);
    }

    //в будующем здесь будет происходить переключение анимаций
    protected override void EndMoveAction()
    {
        currMove = MoveType.Stay;
        animControl.SetBool("move", false);
    }

    //начинает движение персонажа
    private void StartMove(CellScript targetCell)
    {
        if((targetCell == currCell.leftNeighbor && transform.localScale.x > 0) ||
            (targetCell == currCell.rightNeighbor && transform.localScale.x < 0))
        {
            Flip();
        }
        StartCoroutine(MoveToCell(targetCell, moveSpeed));
        currMove = MoveType.Move;
        animControl.SetBool("move", true);
    }

    //разворачивает персонажа в сторону противоположную текущей
    private void Flip()
    {
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
        offset.x *= -1;
    }
}