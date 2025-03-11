using UnityEngine;
using UnityEngine.UI;

public class Window : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private Button windowButton;
    [SerializeField] private Button turnOffButton;
    [SerializeField] private Button turnOffButton2;
    [SerializeField] private Button hi;

    [Header("GameObject")]
    [SerializeField] private GameObject windowPanel;
    [SerializeField] private GameObject turnOffPanel;

    private void Awake()
    {
        windowButton.onClick.AddListener(() => { Popup(windowPanel); });
    }

    private void Popup(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
    }
}
