using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/GameConfig", order = 7)]
public class GameConfig : ScriptableObject
{
    public float WinPercentsMin;
    public float WinPercentsMax;
}
