using UnityEngine;

[CreateAssetMenu(fileName = "Resource Data", menuName = "Resources/Resource Data")]
public class ResourceData : ScriptableObject
{
    public string ResourceName;
    public GameObject Prefab;
    public int PoolSize = 10;
    public ParticleSystem collectionEffect;
    public AudioClip collectionSound;
}