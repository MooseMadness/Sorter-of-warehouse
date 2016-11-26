using UnityEngine;

//базовый класс для бонусов
public abstract class BonusScript : MonoBehaviour
{
    //вероятность появление бонуса
    //от 0 до 1
    public float bonusProbability;
    //изображение бонуса
    public Sprite bonusGraphic;

    protected virtual void Start()
    {
        if (bonusProbability < 0 || bonusProbability > 1f)
        {
            throw new System.ArgumentOutOfRangeException("bonusProbability");
        }
        if(bonusGraphic == null)
        {
            throw new MissingReferenceException("Бонусы без графики не поддерживаются");
        }
    }

    //при вызове бонус активируется
    public abstract void UseBonus(CellScript targetCell);
}
