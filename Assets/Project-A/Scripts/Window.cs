using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using JetBrains.Annotations;
using System.Collections;
using TMPro;

public class Window : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private Button windowButton;
    [SerializeField] private Button turnOffButton;
    [SerializeField] private Button turnOffButton2;
    [SerializeField] private Button hibernateButton;
    [SerializeField] private Button restartButton;

    [Header("GameObject")]
    [SerializeField] private GameObject windowPanel;
    [SerializeField] private GameObject turnOffPanel;
    [SerializeField] private GameObject turnOffScreenPanel;

    [Header("Check")]
    [SerializeField] private bool isHibernate = false;

    [Header("Random")]
    [SerializeField] private float randomBegin;
    [SerializeField] private float randomEnd;

    [Header("Text")]
    [SerializeField] private TMP_Text turnOffScreenText;

    private void Awake()
    {
        Init();
        windowButton.onClick.AddListener(() => { Popup(windowPanel); });
        turnOffButton.onClick.AddListener( () => { Popup(turnOffPanel); });
        turnOffButton2.onClick.AddListener(() => { TurnOff(); });
        hibernateButton.onClick.AddListener( () => { Hibernate(); });
        restartButton.onClick.AddListener( () => { Restart(); });
    }

    private void Update()
    {
        if (isHibernate)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isHibernate = false;
                OnDemandRendering.renderFrameInterval = 1;
            }
        }
    }

    private void Init()
    {
        windowPanel.SetActive(false);
        turnOffPanel.SetActive(false);
        turnOffScreenPanel.SetActive(false);

        Application.targetFrameRate = 60;
        OnDemandRendering.renderFrameInterval = 1;
    }

    private void Popup(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
    }

    private void TurnOff()
    {
        float rand = Random.Range(randomBegin, randomEnd);
        turnOffScreenText.text = "종료 중";
        StartCoroutine(TurnOffCoolTime(rand));
    }

    private IEnumerator TurnOffCoolTime(float value)
    {
        turnOffScreenPanel.SetActive(true);
        yield return new WaitForSeconds(value);
        Application.Quit();
    }

    private void Hibernate()
    {
        isHibernate = true;
        OnDemandRendering.renderFrameInterval = 3;
    }

    public void Restart()
    {
        turnOffScreenText.text = "재시작 중";

        string[] endings = new string[]
        {
            "exe", "x86", "x86_64", "app"
        };
        string executablePath = Application.dataPath + "/../";
        foreach (string file in System.IO.Directory.GetFiles(executablePath))
        {
            foreach (string ending in endings)
            {
                if (file.ToLower().EndsWith("." + ending))
                {
                    System.Diagnostics.Process.Start(file);
                    Application.Quit();
                    return;
                }
            }
        }
    }
}
