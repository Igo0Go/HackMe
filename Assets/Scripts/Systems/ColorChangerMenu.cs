using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ColorChangerMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] List<PartInfo> parts;
    [SerializeField] List<Color> colors;
    [SerializeField] private Text stateText;
    [SerializeField] private Text messagetext;
    [SerializeField] private GameObject finalPanel;
    [SerializeField] private int[] currentState = new int[6];

    private SuitSystem currentSuitSystemPoint;
    private ColorSecuritySystem colorSecuritySystem;
    private SuitSystem currentSuitPoint;
    private int currentPartIndex;

    private void Awake()
    {
        CharacterInteractionReactor characterInteractionReactor = FindObjectOfType<CharacterInteractionReactor>();
        colorSecuritySystem = FindObjectOfType<ColorSecuritySystem>();
        colorSecuritySystem.SetUp();
        messagetext.CrossFadeAlpha(0, 0, true);
        characterInteractionReactor.saveStateEvent += AddState;
        characterInteractionReactor.suitSystemPointUsed += OnSuitPointUsed;
        colorSecuritySystem.stateSavedEvent += ShowMessage;
        for (int i = 0; i < currentState.Length; i++)
        {
            SetPart(i);
            SetColorForPart(0);
        }
    }
    private void Start()
    {
        pausePanel.SetActive(false);
        finalPanel.SetActive(false);
    }

    public void CloseSuitMenu()
    {
        currentSuitPoint.openColorMenuEvent -= OpenMenu;
        currentSuitPoint.CloseSystem();
        currentSuitPoint = null;
        pausePanel.SetActive(false);
    }

    public void SetPart(int part) => currentPartIndex = part;

    public void SetColorForPart(int colorIndex)
    {
        parts[currentPartIndex].SetColor(colors[colorIndex]);
        currentState[currentPartIndex] = colorIndex;
        CheckState(false);
    }

    public void Restart()
    {
        finalPanel.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Exit()
    {
        Application.Quit();
    }

    private void AddState()
    {
        CheckState(true);
        colorSecuritySystem.AddState(currentState);
        stateText.text = "Отслеживается";
    }
    private void CheckState(bool withFinal)
    {
        if(colorSecuritySystem.IsAlienClass(currentState, out string message))
        {
            if(withFinal)
            {
                Time.timeScale = 0;
                finalPanel.SetActive(true);
                return;
            }
        }
        stateText.text = message;
    }
    private void ShowMessage(string messageText)
    {
        messagetext.text = messageText;
        StopAllCoroutines();
        StartCoroutine(ShowMessageCoroutine());
    }
    private IEnumerator ShowMessageCoroutine()
    {
        messagetext.CrossFadeAlpha(1, 1, true);
        yield return new WaitForSeconds(5);
        messagetext.CrossFadeAlpha(0, 1, true);
    }
    private void OnSuitPointUsed(SuitSystem suitSystemPoint)
    {
        currentSuitPoint = suitSystemPoint;
        currentSuitPoint.openColorMenuEvent += OpenMenu;
    }
    private void OpenMenu()
    {
        pausePanel.SetActive(true);
    }
}

[System.Serializable]
public class PartInfo
{
    public Image buton;
    public PartColorControllModule part;

    public void SetColor(Color color)
    {
        part.ChangeColor(color);
        buton.color = color;
    }
}
