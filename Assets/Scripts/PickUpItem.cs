using UnityEngine;
using UnityEngine.Events;

public class PickUpItem : MonoBehaviour
{
    [SerializeField] private AudioClip soundEffect;
    [SerializeField] private GameObject hold;
    [SerializeField] private Vector3 pos;
    [SerializeField] private Vector3 angle;
    [SerializeField] private GameObject putBack;
    [SerializeField] private UnityEvent additionalEffect;

    public void PickUp()
    {
        if (MainManager.instance.gameState != 1) return;

        MainManager.instance.AddItem(name);
        MainManager.instance.PlayEffect(soundEffect);
        if (hold != null)
        {
            if (hold.transform.childCount > 0)
            {
                MainManager.instance.AddTrigger("dialogue;You;I don't want to hold two items at the same time...");
            }
            else
            {
                transform.SetParent(hold.transform);
                transform.localPosition = pos;
                transform.localRotation = Quaternion.Euler(angle);
                tag = "Untagged";
                additionalEffect.Invoke();
                if(putBack != null)
                {
                    putBack.SetActive(true);

                }
            }
        }
        else
        {
            additionalEffect.Invoke();
            Destroy(gameObject);
        }
    }
}
