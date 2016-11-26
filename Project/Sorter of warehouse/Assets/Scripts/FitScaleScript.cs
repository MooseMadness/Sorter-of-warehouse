using UnityEngine;

//изменяет с помощью скейла ширину объекта
//таким образом чтобы пользователи с разным соотношением сторон
//экрана видели один и ту же часть сцены
public class FitScaleScript : MonoBehaviour
{
    //ширина при которой тестировалась игра
    public int editorWidth;
    //высота при которой тестировалась игра
    public int editorHeight;

    private void Start()
    {
        float targetaspect = editorWidth / (float)editorHeight;
        float windowaspect = Screen.width / (float)Screen.height;
        float scaleWidth = windowaspect / targetaspect;
        Vector3 newScale = transform.localScale;
        newScale.x *= scaleWidth;
        transform.localScale = newScale;
    }
}
