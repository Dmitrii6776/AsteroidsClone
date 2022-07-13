using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameState", menuName = "scriptableObjects", order = 1)]
public class GameState : ScriptableObject
{
    public bool isGameStarted;
}
