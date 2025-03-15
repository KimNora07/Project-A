using System;
using UnityEngine;
using UnityEngine.UI;

public class CloudifyUI : MonoBehaviour
{
    public Button closeButton;
    public Button maximizeButton;
    public Button minimizeButton;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        closeButton.onClick.AddListener(() => {Close(this.gameObject);});
    }

    private void Close(GameObject obj)
    {
        obj.SetActive(false);
    }
}
