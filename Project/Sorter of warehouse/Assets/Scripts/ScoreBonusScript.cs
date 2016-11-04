//класс реализующий бонус, который добавляет очки при срабатывании
public class ScoreBonusScript : IBonus
{
    //кол-во добавляемых очков
    private int scoreAmount;

    public ScoreBonusScript(int _scoreAmount)
    {
        scoreAmount = _scoreAmount;
    }

    public void UseBonus(CellScript targetCell)
    {
        GameManagerScript.instance.ChangeScore(scoreAmount);
    }
}
