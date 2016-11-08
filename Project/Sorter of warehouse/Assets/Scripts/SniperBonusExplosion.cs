using UnityEngine;

//Класс реализующий бонус, который при срабатывании 
//уничтожает все ящики того же цвета что и ящик в переданной ячейке
public class SniperBonusExplosion : BonusScript
{
    //ссылка на скрипт игрового поля
    public GameFieldScript gameField;

    protected override void Awake()
    {
        base.Awake();
        if(gameField == null)
        {
            throw new UnassignedReferenceException("Не установлена ссылка на игровое поле в объекте бонуса");
        }
    }

    public override void UseBonus(CellScript targetCell)
    {
        BoxScript targetBox = targetCell.cellObject as BoxScript;
        if(targetBox == null)
        {
            throw new UnityException("Бонусу передана ссылка на клетку не содержащую ящика");
        }
        else
        {
            for(int i = 0; i < gameField.height; i++)
            {
                for(int j = 0; j < gameField.width; j++)
                {
                    BoxScript cellBox = gameField[i, j].cellObject as BoxScript;
                    if(cellBox != null && cellBox.boxColor == targetBox.boxColor)
                    {
                        cellBox.DestroyBox();
                    }
                }
            }
        }
    }
}
