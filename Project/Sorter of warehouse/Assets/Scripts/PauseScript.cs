using UnityEngine;
using UnityStandardAssets.ImageEffects;

//Класс позволяющий поставить игру на паузу
public class PauseScript : MonoBehaviour
{
    //ссылка игровое меню
    public GameObject gameMenuRoot;
    //ссылка на меню паузы
    public GameObject pauseMenuRoot;
    //ссылка на эффект размытия
    public BlurOptimized blurEffect;

    private void Update()
    {
        //если нажата кнопка "Назад"
        if(Input.GetButton("Cancel"))
        {
            SwitchPause();
        }
    }

    //переключение в режим паузы и обратно
    public void SwitchPause()
    {
        if(gameMenuRoot == null || pauseMenuRoot == null)
        {
            throw new UnassignedReferenceException("Для PauseScript не назначены все компоненты");
        }
        else
        {
            Time.timeScale = Time.timeScale == 0f ? 1f : 0f;
            gameMenuRoot.SetActive(!gameMenuRoot.activeSelf);
            pauseMenuRoot.SetActive(!pauseMenuRoot.activeSelf);
            blurEffect.enabled = !blurEffect.enabled;
        }
    }
}
