using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private InputField inputFieldChairs;

    [SerializeField] private InputField inputFieldVisitors;

    [SerializeField] private Text textCurrentChairsCount;

    [SerializeField] private Text textCurrentVisitorsCount;

    [SerializeField] private Text messageBox;

    [SerializeField] private SetVisitors setVisitors;

    [SerializeField] private float messageDelay;

    [SerializeField] private string tooMuchVisitorsMessage;

    [SerializeField] private GameObject panelBeforeStart;

    [SerializeField] private GameObject panelAfterStart;

    private void Start()
    {
        inputFieldChairs.onEndEdit.AddListener(InputChairsCount);
        inputFieldVisitors.onEndEdit.AddListener(InputVisitorsCount);
        textCurrentChairsCount.text = "Всего стульев:" + setVisitors.ChairsCount;
        textCurrentVisitorsCount.text = "Всего посетителей:" + setVisitors.VisitorsCount;
    }

    private void InputChairsCount(string text)
    {
        int result = Convert.ToInt32(text);
        textCurrentChairsCount.text ="Всего стульев:" + result;
        setVisitors.ChairsCount = result;
        inputFieldChairs.text = "";
    }

    private void InputVisitorsCount(string text)
    {
        messageBox.enabled = false;
        int result = Convert.ToInt32(text);
        if (result>setVisitors.ChairsCount)
        {
            int visitorsOut = result - setVisitors.ChairsCount;
            result = setVisitors.ChairsCount;
            StartCoroutine(DrawMessage(tooMuchVisitorsMessage+setVisitors.ChairsCount));
        }

        textCurrentVisitorsCount.text = "Всего посетителей:" + result;
        setVisitors.VisitorsCount = result;
        inputFieldVisitors.text = "";
    }

    private IEnumerator DrawMessage(string text)
    {
        messageBox.text = text;
        messageBox.enabled = true;
        yield return new WaitForSeconds(messageDelay);
        messageBox.enabled = false;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame()
    {
        setVisitors.Initialization();
        panelBeforeStart.SetActive(false);
        panelAfterStart.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
