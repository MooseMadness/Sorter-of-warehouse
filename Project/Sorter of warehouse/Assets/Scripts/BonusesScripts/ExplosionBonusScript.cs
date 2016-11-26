using UnityEngine;

//Класс реализующий бонус, который уничтожает все прилегающие,
//к данному ящику, ящики
public class ExplosionBonusScript : BonusScript
{
    public override void UseBonus(CellScript targetCell)
    {
        if(targetCell.cellObject is BoxScript)
        {
            if (targetCell.topNeighbor != null && targetCell.topNeighbor.cellObject is BoxScript)
                ((BoxScript)targetCell.topNeighbor.cellObject).DestroyBox();
            if (targetCell.bottomNeighbor != null && targetCell.bottomNeighbor.cellObject is BoxScript)
                ((BoxScript)targetCell.bottomNeighbor.cellObject).DestroyBox();
            if (targetCell.leftNeighbor != null && targetCell.leftNeighbor.cellObject is BoxScript)
                ((BoxScript)targetCell.leftNeighbor.cellObject).DestroyBox();
            if (targetCell.rightNeighbor != null && targetCell.rightNeighbor.cellObject is BoxScript)
                ((BoxScript)targetCell.rightNeighbor.cellObject).DestroyBox();
        }
        else
        {
            throw new UnityException("Бонусу передана ссылка на клетку не содержащую ящика");
        }
    }
}
