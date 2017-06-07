using UnityEngine;

//Класс реализующий бонус, который при срабатывании 
//уничтожает все ящики того же цвета что и ящик в переданной ячейке
public class SniperBonusExplosion : BonusScript
{
    public GameObject sniperEffect;

    protected override void Start()
    {
        base.Start();
        if(GameFieldScript.instance == null)
        {
            throw new UnassignedReferenceException("В сцене нет игрового поля");
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
            for(int i = 0; i < GameFieldScript.instance.height; i++)
            {
                for(int j = 0; j < GameFieldScript.instance.width; j++)
                {
                    CellScript cell = GameFieldScript.instance[i, j];
                    BoxScript cellBox = cell.cellObject as BoxScript;
                    if(cellBox != null && cellBox.boxColor == targetBox.boxColor)
                    {
                        cellBox.DestroyBox();
                        Instantiate(sniperEffect, cell.transform.position, Quaternion.identity);
                    }
                }
            }
        }
    }
}
