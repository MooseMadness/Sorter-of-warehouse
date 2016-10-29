using UnityEngine;

//Класс реализующий управление персонажем
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
        MoveLeft,
        MoveRight,
        Fall,
        Stay
    }

    //текущее движене
    private MoveType currMove = MoveType.Stay;
    //движение которое будет выполнено после завершения текущего
    private MoveType nextMove = MoveType.Stay;

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

                    case MoveType.MoveLeft:
                    {
                        if (currCell.leftNeighbor.cellObject == null)
                        {
                            StartCoroutine(MoveToCell(currCell.leftNeighbor, moveSpeed));
                            currMove = MoveType.MoveLeft;
                        }
                        break;
                    }

                    case MoveType.MoveRight:
                    {
                        if (currCell.rightNeighbor.cellObject == null)
                        {
                            StartCoroutine(MoveToCell(currCell.rightNeighbor, moveSpeed));
                            currMove = MoveType.MoveRight;
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

    //движение влево
    public void MoveLeft()
    {
        if (moving)
        {
            //если во время прыжка поступит команда движение влево, то игрок сместится влево после прыжка
            if (currMove == MoveType.Jump && currCell.leftNeighbor != null)
            {
                nextMove = MoveType.MoveLeft;
            }
            //если во время движения поступила команда прыжка а затем команда движения, то прыжок отменится 
            else if (nextMove == MoveType.Jump)
            {
                nextMove = MoveType.Stay;
            }
        }
        else if (currCell.leftNeighbor != null && currCell.leftNeighbor.cellObject == null)
        {
            StartCoroutine(MoveToCell(currCell.leftNeighbor, moveSpeed));
            currMove = MoveType.MoveLeft;
        }
    }

    //движение вправо
    public void MoveRight()
    {
        if(moving)
        {
            //если во время прыжка поступит команда движение вправо, то игрок сместится вправо после прыжка
            if (currMove == MoveType.Jump && currCell.rightNeighbor != null)
            {
                nextMove = MoveType.MoveRight;
            }
            //если во время движения поступила команда прыжка а затем команда движения, то прыжок отменится 
            else if (nextMove == MoveType.Jump)
            {
                nextMove = MoveType.Stay;
            }
        }
        else if(currCell.rightNeighbor != null && currCell.rightNeighbor.cellObject == null)
        {
            StartCoroutine(MoveToCell(currCell.rightNeighbor, moveSpeed));
            currMove = MoveType.MoveRight;
        }
    }

    //в будующем здесь будет происходить переключение анимаций
    protected override void EndMoveAction()
    {
        
    }
}
