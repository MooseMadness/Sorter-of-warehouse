//класс реализующий бонус, который добавляет очки при срабатывании
public class ScoreBonusScript : BonusScript
{
    //кол-во добавляемых очков
    public int scoreAmount;

    public override void UseBonus(CellScript targetCell)
    {
        GameManagerScript.instance.ChangeScore(scoreAmount);
    }
}
