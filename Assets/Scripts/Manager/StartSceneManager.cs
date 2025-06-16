using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneManager : MonoBehaviour
{
    private static StartSceneManager instance;
    public TextMeshProUGUI gameTitleTxt;
    public Button[] btns;

    void Awake()
    {
        instance = this;
        gameTitleTxt.text = string.Empty;
        foreach (Button one in btns)
        {
            one.interactable = false;
        }
    }

    IEnumerator Start()
    {
        yield return DisplayGameTitle();
        foreach (Button one in btns)
        {
            one.interactable = true;
        }
    }

    IEnumerator DisplayGameTitle()
    {
        string title = "Eclipse\nProtocol";
        char[] arrayTitle = title.ToCharArray();
        for (int i = 0; i < arrayTitle.Length; i++)
        {
            gameTitleTxt.text += arrayTitle[i];
            yield return new WaitForSeconds(0.3f);
        }
    }

    public void OnClickPlayBtn()
    {
        Debug.Log("PLAY 버튼 클릭");
        SceneManager.LoadScene("Field Scene");
    }

    public void OnClickOptionBtn()
    {
        Debug.Log("OPTION 버튼 클릭");
        //SceneManager.LoadScene("");
    }

    public void OnClickExit()
    {
#if UNITY_EDITOR
        Debug.Log("EXIT 버튼 클릭");
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
