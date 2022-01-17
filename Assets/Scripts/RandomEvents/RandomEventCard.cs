using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomEventCard : MonoBehaviour
{
    public RandomEvent randomEvent;

    public RectTransform CardTransform;

    public Text Front_TitleText;
    public Image Front_MainImage;
    public Text Front_DesriptionText;

    public Text Back_TitleText;
    public Image Back_Image;
    public Text Back_ResultText;

    public Transform AnswersContainer;

    public GameObject AnswerPrefab;

    List<Button> buttons = new List<Button>();

    public void Init(RandomEvent randomEvent)
    {
        GameManager.singleton.AddPausingCount(1);

        this.randomEvent = randomEvent;
        Front_TitleText.text = randomEvent.Title;
        Front_MainImage.sprite = randomEvent.MainSprite;
        if (randomEvent.MainSprite == null)
        {
            Front_MainImage.gameObject.SetActive(false);
        }

        var actionText = CallREAction(randomEvent.Standart_Actions);

        Front_DesriptionText.text = randomEvent.Desription + "\n" + actionText;
        AddWeightAndDelta(randomEvent.Standart_ChangeOtherEventsWeight);

        CardTransform.sizeDelta = new Vector2(CardTransform.sizeDelta.x, 475 + randomEvent.Answers.Count * 85);

        for (var i = 0; i < randomEvent.Answers.Count; i++) {
            var answer = randomEvent.Answers[i];
            var answerButton = Instantiate(AnswerPrefab, AnswersContainer, false);
            answerButton.GetComponent<RectTransform>().localPosition = new Vector2(0, -490 - 85 * i);
            int answerId = i;
            answerButton.GetComponent<Button>().onClick.AddListener(new UnityEngine.Events.UnityAction(() => PressedAnswer(answerId)));

            var condition = GetAnswerConditions(answer);

            answerButton.transform.GetChild(0).GetComponent<Text>().text = $"{answer.AnswerText} {condition.Item2}";
            if (!condition.Item1)
                answerButton.GetComponent<Button>().interactable = false;
            buttons.Add(answerButton.GetComponent<Button>());
        }

        if (randomEvent.Answers.Count == 0)
        {
            CardTransform.sizeDelta = new Vector2(CardTransform.sizeDelta.x, 475 + 85);
            var answerButton = Instantiate(AnswerPrefab, AnswersContainer, false);
            answerButton.GetComponent<RectTransform>().localPosition = new Vector2(0, -490);
            answerButton.GetComponent<Button>().onClick.AddListener(new UnityEngine.Events.UnityAction(() => CardClosed()));
            answerButton.transform.GetChild(0).GetComponent<Text>().text = "Continue";

            buttons.Add(answerButton.GetComponent<Button>());
        }
    }

    public (bool, string) GetAnswerConditions(EventAnswer eventAnswer) {
        string result = "";
        bool isCompleted = true;

        foreach (var condition in eventAnswer.RequiredConditions) {
            var e = RE_ConditionManager.GetConditionInfo(condition.RE_Condition, condition.value);
            if (e.Item1)
            {
                if(condition.IsAddToText)
                    result += $"[{e.Item2}]";
            }
            else
            {
                isCompleted = false;
                if (condition.IsAddToText)
                    result += $"<color=\"brown\">[{e.Item2}]</color> ";
            }
        }

        return (isCompleted, result);
    }

    public void PressedAnswer(int answerId) {
        var answer = randomEvent.Answers[answerId];
        Debug.Log(answerId + ": " + answer.AnswerText);

        foreach (var e in buttons)
        {
            e.interactable = false;
        }

        Back_TitleText.text = randomEvent.Title;
        Back_Image.sprite = answer.AnswerResultSprite;
        if (answer.AnswerResultSprite == null)
        {
            Back_Image.gameObject.SetActive(false);
        }

        var resultString = CallREAction(answer.Actions);
        AddWeightAndDelta(answer.ChangeOtherEventsWeightAndRepeats);

        Back_ResultText.text = answer.AnswerResultText + "\n" + resultString;

        if (answer.CloseCardOnPress)
            CardClosed();
        else
            GetComponent<Animator>().SetTrigger("Swap");
    }

    public string CallREAction(List<ActionStruct> actionStructs) {
        string resultString = "";
        foreach (var e in actionStructs) {
            var stringValue = RE_ActionManager.CallAction(e.RE_Action, e.value);

            if (e.IsAddToText)
                resultString += stringValue + "\n";
        }
        return resultString;
    }

    public void AddWeightAndDelta(List<EventWeightAndRepeatChange> list)
    {
        foreach (var e in list)
        {
            RandomEventsManager.singleton.AddWeightAndDelta(e);
        }
    }

    public void CardClosed()
    {
        GameManager.singleton.AddPausingCount(-1);
        Destroy(gameObject);
    }
}
