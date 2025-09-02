using UnityEngine;

[CreateAssetMenu(fileName = "New Tool", menuName = "Tool/Tool")]
public class ToolData : ScriptableObject
{
    public string toolName;
    public Sprite toolIcon;
    public GameObject toolPrefab;

}