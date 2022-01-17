using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public int OpenedBlockId;

    public RectTransform leftBorder;
    public RectTransform rightBorder;

    public List<Image> ButtonsImages;

    public Color Button_UnselectedColor;
    public Color Button_SelectedColor;

    public Text QuestsText;

    public static QuestManager singleton;

    public List<Quest>[] Quests = new List<Quest>[4];

    string[] PagesTextes = new string[4];

    Dictionary<string, List<Quest>> questsKeys;
    Dictionary<string, float> questsKeysValues;
    HashSet<Quest> completedQuests;

    public string DebugKey;
    public float DebugDelta;
    public bool DebugBool;

    private void Awake()
    {
        singleton = this;
    }

    private void Update()
    {
        if (DebugBool) {
            DebugBool = false;
            QuestKeyAddValue(DebugKey, DebugDelta);
        }
    }


    public void Start()
    {
        questsKeys = new Dictionary<string, List<Quest>>();
        questsKeysValues = new Dictionary<string, float>();
        completedQuests = new HashSet<Quest>();
        var quests = Resources.LoadAll<Quest>("Quests");

        for (var i = 0; i < 4; i++)
        {
            Quests[i] = new List<Quest>();
        }

        foreach (var e in quests)
        {
            var questType = (int)e.QuestType;
            Quests[questType].Add(e);

            if (!questsKeys.ContainsKey(e.QuestKey))
            {
                questsKeys[e.QuestKey] = new List<Quest>();
                questsKeysValues[e.QuestKey] = 0;
            }
            questsKeys[e.QuestKey].Add(e);
        }

        for (var i = 0; i < 4; i++) {
            SortQuests(Quests[i]);
        }

        for (var i = 0; i < 4; i++)
        {
            foreach (var e in Quests[i]) {
                PagesTextes[(int)e.QuestType] += e.Text + "\n";
            }
        }

        UpdateButtonColors();
        UpdateBlockText(true);
    }

    public void QuestKeyAddValue(string key, float value) {
        if (!questsKeysValues.ContainsKey(key))
            questsKeysValues[key] = 0;
        questsKeysValues[key] += value;
        CheckQuestsIsCompleted(key);
        UpdateBlockText(true);
    }

    public void QuestKeySetValue(string key, float value)
    {
        if (!questsKeysValues.ContainsKey(key))
            questsKeysValues[key] = 0;
        questsKeysValues[key] = value;
        CheckQuestsIsCompleted(key);
        UpdateBlockText(true);
    }

    public void CheckQuestsIsCompleted(string key) {
        var value = questsKeysValues[key];
        foreach (var quest in questsKeys[key]) {
            if (completedQuests.Contains(quest))
                continue;
            if (value >= quest.TargetKeyValue) {
                completedQuests.Add(quest);
                Debug.Log("Completed quest: " + quest.Text);

                foreach (var e in quest.resultActions)
                {
                    var stringValue = RE_ActionManager.CallAction(e.RE_Action, e.value);
                }
            }
        }
    }

    public void SortQuests(List<Quest> list)
    {
        for (int i = 0; i < list.Count - 1; i++)
        {
            for (int j = i + 1; j < list.Count; j++)
            {
                if (list[i].OrderIndex > list[j].OrderIndex)
                {
                    var temp = list[i];
                    list[i] = list[j];
                    list[j] = temp;
                }
            }
        }
    }

    public void OpenBlock(int blockId)
    {
        if (OpenedBlockId == blockId) return;
        leftBorder.sizeDelta = new Vector2(blockId * 125, 8);
        rightBorder.sizeDelta = new Vector2(375 - blockId * 125, 8);

        OpenedBlockId = blockId;
        UpdateButtonColors();
        UpdateBlockText(true);
    }

    void UpdateButtonColors() {

        for (var i = 0; i < ButtonsImages.Count; i++)
        {
            ButtonsImages[i].color = (i == OpenedBlockId ? Button_SelectedColor : Button_UnselectedColor);
        }
    }

    void UpdateBlockText(bool recalculate = false) {
        if (recalculate)
        {
            var result = "";
            foreach (var quest in Quests[OpenedBlockId])
            {
                var questStr = "";
                questStr += quest.Text;
                if (quest.ShowProgress && !completedQuests.Contains(quest))
                    questStr += $" {questsKeysValues[quest.QuestKey]}/{quest.TargetKeyValue}";

                if (completedQuests.Contains(quest)) {
                    var newStr = "";
                    foreach (var c in questStr)
                    {
                        newStr += "" + c + '\u0336';
                    }
                    questStr = $"<color=#575757ff>{newStr}</color>";

                    questStr += $" <color=#00b700ff>{'\u2713'}</color>";
                }
                questStr += "\n";

                result += questStr;
            }
            PagesTextes[OpenedBlockId] = result;
        }
        QuestsText.text = PagesTextes[OpenedBlockId];
    }
}
