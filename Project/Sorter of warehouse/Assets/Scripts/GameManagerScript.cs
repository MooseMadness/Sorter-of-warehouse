using UnityEngine;

//Класс управляющий состоянием игры
//в сцене может существовать только один экземпляр данного класса
public class GameManagerScript : MonoBehaviour
{
    //ссылка на объект данного класса в сцене 
    public static GameManagerScript instance = null;

    private void Awake()
    {
        //если объект с данным компонентом уже есть в сцене, то этот объект будет уничтожен
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    //вызывается когда игрок проиграл
    public void GameOver()
    {

    }   
    
    //изменяет счет на величину указанную в amount
    public void ChangeScore(int amount)
    {

    } 
}
