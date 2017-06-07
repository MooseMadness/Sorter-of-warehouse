using UnityEngine;

//Класс реализующий бонус, который уничтожает все прилегающие,
//к данному ящику, ящики
public class ExplosionBonusScript : BonusScript
{
    public GameObject explosionPrefab;

    public override void UseBonus(CellScript targetCell)
    {
        BoxScript bonusBox = targetCell.cellObject as BoxScript;
        if (bonusBox != null)
        {

            CheckCell(targetCell.topNeighbor);
            if (targetCell.topNeighbor != null)
            {
                CheckCell(targetCell.topNeighbor.leftNeighbor);
                CheckCell(targetCell.topNeighbor.rightNeighbor);
            }

            CheckCell(targetCell.bottomNeighbor);
            if (targetCell.bottomNeighbor != null)
            {
                CheckCell(targetCell.bottomNeighbor.leftNeighbor);
                CheckCell(targetCell.bottomNeighbor.rightNeighbor);
            }

            CheckCell(targetCell.leftNeighbor);
            CheckCell(targetCell.rightNeighbor);

            Instantiate(explosionPrefab, targetCell.transform.position, Quaternion.identity);
        }
        else
        {
            throw new UnityException("Бонусу передана ссылка на клетку не содержащую ящика");
        }
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
