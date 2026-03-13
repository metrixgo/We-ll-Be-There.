using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    [SerializeField] private AudioClip soundEffect;

    public void PickUp()
    {
        if (MainManager.instance.gameState != 1) return ;
        if (soundEffect != null) MainManager.instance.PlayEffect(soundEffect);
        MainManager.instance.AddItem(gameObject.name);
        Destroy(gameObject);
    }
}
