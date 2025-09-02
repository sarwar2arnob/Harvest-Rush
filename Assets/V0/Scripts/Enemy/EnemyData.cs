using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Data", menuName = "Enemies/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string EnemyName;
    public GameObject Prefab;
    public int PoolSize = 5;
    public ParticleSystem BloodEffect;
}