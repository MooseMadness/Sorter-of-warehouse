using UnityEngine;

//класс реализующий бонус, который добавляет очки при срабатывании
public class ScoreBonusScript : BonusScript
{
    //кол-во добавляемых очков
    public int scoreAmount;

    public override void UseBonus(CellScript targetCell)
    {
        BoxScript box = targetCell.cellObject as BoxScript;
        if (box != null)
        {
            box.boxScore += scoreAmount;
        }
        else
        {
            throw new UnityException("Бонусу передана ссылка на клетку не содержащую ящика");
        }
    }
}
