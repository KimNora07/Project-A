using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using System.Collections;
using System.Collections.Generic;

public class Window : MonoBehaviour
{
    /*******************************************************************/
    /*                        Public Field                             */
    /*******************************************************************/
    [Header("PopupUI")]
    public PopupUI startMenuPopup;
    public PopupUI powerMenuPopup;
    
    [Header("PowerMenuButton")]
    public Button shutDownButton;
    public Button hibernateButton;
    public Button restartButton;
    
    [Header("GameObject")]
    public GameObject shutDownScreenPanel;
    
    [Header("RandomValue")]
    [Range(0, 10)] public float minValue;
    [Range(0, 10)] public float maxValue;

    /*******************************************************************/
    /*                        Private Field                            */
    /*******************************************************************/
    private LinkedList<PopupUI> activePopupUILList;
    private List<PopupUI> allPopupUIList;
    
    /*******************************************************************/
    /*                        Unity CallBacks                          */
    /*******************************************************************/
    private void Awake()
    {
        Init();
        InitCloseAll();
    }
    
    /*******************************************************************/
    /*                        Private Methods                          */
    /*******************************************************************/
    private void Init()
    {
        activePopupUILList = new LinkedList<PopupUI>();
        allPopupUIList = new List<PopupUI>()
        {
            startMenuPopup, powerMenuPopup
        };
        foreach (var popup in allPopupUIList)
        {
            popup.popupUIButton.onClick.AddListener(() => { ToggleOpenClosePopup(popup); });
        }
        shutDownScreenPanel.SetActive(false);
        Application.targetFrameRate = 60;
        OnDemandRendering.renderFrameInterval = 1;
        
        shutDownButton.onClick.AddListener(() => {ShutDown();});
        hibernateButton.onClick.AddListener(() => {Hibernate();});
        restartButton.onClick.AddListener(() => {Restart();});
    }

    private void InitCloseAll()
    {
        foreach (var popup in allPopupUIList)
        {
            activePopupUILList.Remove(popup);
            popup.gameObject.SetActive(false);
        }
    }

    private void ToggleOpenClosePopup(PopupUI popup)
    {
        if(!popup.gameObject.activeSelf) OpenPopup(popup);
        else ClosePopup(popup);
    }

    private void OpenPopup(PopupUI popup)
    {
        activePopupUILList.AddFirst(popup);
        popup.gameObject.SetActive(true);
        RefreshAllPopupDepth();
    }

    private void ClosePopup(PopupUI popup)
    {
        int popupIndex = popup.siblingIndex;
        var popupsToRemove = new List<PopupUI>();
        foreach (var ui in activePopupUILList)
        {
            if (ui.siblingIndex >= popupIndex)
            {
                popupsToRemove.Add(ui);
            }
        }
        foreach (var ui in popupsToRemove)
        {
            activePopupUILList.Remove(ui);
            ui.gameObject.SetActive(false);
        }
        RefreshAllPopupDepth();
    }
    
    private void RefreshAllPopupDepth()
    {
        foreach (var popup in activePopupUILList)
        {
            popup.transform.SetAsFirstSibling();
        }
    }

    private void ShutDown()
    {
        float rand = Random.Range(minValue, maxValue);       
        StartCoroutine(TurnOffCoolTime(rand));
    }

    private void Hibernate()
    {
        OnDemandRendering.renderFrameInterval = 3;
    }

    private void Restart()
    {
        float rand = Random.Range(minValue, maxValue);
        shutDownScreenPanel.SetActive(true);

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
                    StartCoroutine(RestartCoolTime(file, rand));
                    return;
                }
            }
        }
    }

    private IEnumerator RestartCoolTime(string file, float value)
    {
        yield return new WaitForSeconds(value);
        System.Diagnostics.Process.Start(file);
        Application.Quit();
    }

    private IEnumerator TurnOffCoolTime(float value)
    {  
        shutDownScreenPanel.SetActive(true);
        yield return new WaitForSeconds(value);
        Application.Quit();
    }
}
