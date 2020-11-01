using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Aggro")]
public class Aggro : ScriptableObject
{
    public Vector2 location;
    public string targetName;
}
