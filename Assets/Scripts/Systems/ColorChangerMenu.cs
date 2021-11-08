using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorChangerMenu : MonoBehaviour
{
    public GameObject pausePanel;

    [SerializeField] List<PartInfo> parts;
    [SerializeField] List<Color> colors;
    [SerializeField] private Text stateText;
    [SerializeField] private Text messagetext;
        [SerializeField] private int[] currentState = new int[6];


    private int currentPartIndex;

    public void SetPart(int part) => currentPartIndex = part;

    public void SetColorForPart(int colorIndex)
    {
        parts[currentPartIndex].SetColor(colors[colorIndex]);
        currentState[currentPartIndex] = colorIndex;
        CheckState();
    }

    private void Awake()
    {
        messagetext.CrossFadeAlpha(0, 0, true);
        CharacterInteractionReactor.SaveStateEvent += AddState;
        CharacterInteractionReactor.SaveStateEvent += CheckState;
        ColorSecuritySystem.stateSavedEvent += ShowMessage; ;
        for (int i = 0; i < currentState.Length; i++)
        {
            SetPart(i);
            SetColorForPart(0);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            pausePanel.SetActive(!pausePanel.activeSelf);
            CharacterMove.Blocked = pausePanel.activeSelf;
        }
    }

    private void AddState() => ColorSecuritySystem.AddState(currentState);

    private void CheckState() => stateText.text = ColorSecuritySystem.CheckState(currentState);

    public void ShowMessage(string messageText)
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
