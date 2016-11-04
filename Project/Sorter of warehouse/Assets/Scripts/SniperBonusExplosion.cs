using UnityEngine;

//Класс реализующий бонус, который при срабатывании 
//уничтожает все ящики того же цвета что и ящик в переданной ячейке
public class SniperBonusExplosion : IBonus
{
    //ссылка на скрипт игрового поля
    private GameFieldScript gameField;

    public SniperBonusExplosion(GameFieldScript _gameField)
    {
        if (_gameField == null)
        {
            throw new UnassignedReferenceException("Не установлена ссылка на игровое поле в объекте бонуса");
        }
        else
        {
            gameField = _gameField;
        }
    }

    public void UseBonus(CellScript targetCell)
    {
        BoxScript targetBox = targetCell.cellObject as BoxScript;
        if(targetBox == null)
        {
            throw new UnityException("Бонусу передана ссылка на клетку не содержащую ящика");
        }
        else
        {
            for(int i = 0; i < gameField.cells.GetLength(0); i++)
            {
                for(int j = 0; j < gameField.cells.GetLength(1); j++)
                {
                    BoxScript cellBox = gameField.cells[i, j].cellObject as BoxScript;
                    if(cellBox != null && cellBox.boxColor == targetBox.boxColor)
                    {
                        cellBox.DestroyBox();
                    }
                }
            }
        }
    }
}
