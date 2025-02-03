using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NameList", menuName = "Custom Tools/Name List")]
public class NameListScriptableObject : ScriptableObject
{
    public List<string> names = new List<string>();
}