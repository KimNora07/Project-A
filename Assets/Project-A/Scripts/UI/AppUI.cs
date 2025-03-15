using UnityEngine;
using UnityEngine.EventSystems;

public class AppUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject appScreenUI;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount == 2)
        {
            // 특정화면 열기
            appScreenUI.SetActive(true);
        }
    }
}
