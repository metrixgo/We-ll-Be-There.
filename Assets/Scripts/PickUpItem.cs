using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    [SerializeField] private AudioClip soundEffect;
    [SerializeField] private GameObject hold;
    [SerializeField] private Vector3 pos;
    [SerializeField] private Vector3 angle;

    public void PickUp()
    {
        if (MainManager.instance.gameState != 1) return ;

        MainManager.instance.AddItem(name);
        if (soundEffect != null) MainManager.instance.PlayEffect(soundEffect);
        if (hold != null)
        {
            transform.SetParent(hold.transform);
            transform.localPosition = pos;
            transform.localRotation = Quaternion.Euler(angle);
            tag = "Untagged";
        }
        else Destroy(gameObject);
    }
}
