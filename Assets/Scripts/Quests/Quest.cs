using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quests/Quest", order = 1)]
public class Quest : ScriptableObject
{
    public string QuestKey;
    public float TargetKeyValue;
    public EQuestType QuestType;
    [Range(0, 20)]
    public int OrderIndex;
    public string Text;
    public bool ShowProgress;
    public List<ActionStruct> resultActions;
}
