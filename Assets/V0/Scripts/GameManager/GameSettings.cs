using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings", order = 1)]
public class GameSettings : ScriptableObject
{
    //GameResourceData
    [Header("Base Required Resources per Day")]
    public int MinFood = 5;
    public int MinWood = 5;
    public int MinStone = 5;

    [Header("Maximum Required Resources per Day")]
    public int MaxFood = 15;
    public int MaxWood = 15;
    public int MaxStone = 15;

    [Header("Daily Difficulty Scaling")]

    public int FoodIncreasePerDay = 1;
    public int WoodIncreasePerDay = 1;
    public int StoneIncreasePerDay = 1;
}