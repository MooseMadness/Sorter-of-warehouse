using UnityEngine;

//Класс реализующий бонус, который уничтожает все прилегающие,
//к данному ящику, ящики
public class ExplosionBonusScript : BonusScript
{
    public override void UseBonus(CellScript targetCell)
    {
        if (targetCell.cellObject is BoxScript)
        {
            CheckCell(targetCell.topNeighbor);
            CheckCell(targetCell.bottomNeighbor);
            CheckCell(targetCell.leftNeighbor);
            CheckCell(targetCell.rightNeighbor);
        }
        else
        {
            throw new UnityException("Бонусу передана ссылка на клетку не содержащую ящика");
        }
    }

    //истина - яейка существует и в ней есть ящик
    //иначе ложь
    private bool HasBox(CellScript cell)
    {
        return cell != null && cell.cellObject is BoxScript;
    }

    //проверяет клетку на наличие в ней ящика
    //если ящик есть, то он уничтожается
    private void CheckCell(CellScript cell)
    {
        bool hasBox = cell != null && cell.cellObject is BoxScript;
        if (hasBox)
            ((BoxScript)cell.cellObject).DestroyBox();
    }
}
