using UnityEngine;
public class TalkNPC : MonoBehaviour, IInteract
{
    public void Interact()
    {
        Debug.Log("talking...");
    }
    public bool CanInteract()
    {
        return true;
    }

}
