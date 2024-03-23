using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelDescriptor", order = 1)]
public class LevelDescriptor : ScriptableObject
{
    public string levelName = "OutdoorScene";
    public float timeToComplete = 90;
    public int scoreToBeat = 0;
}
