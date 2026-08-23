using UnityEngine;
using UnityEngine.Events;

public class PickUpItem : MonoBehaviour
{
    [SerializeField] private AudioClip soundEffect;
    [SerializeField] private GameObject hold;
    [SerializeField] private Vector3 pos;
    [SerializeField] private Vector3 angle;
    [SerializeField] private bool changeScale = false;
    [SerializeField] private Vector3 scale;
    [SerializeField] private GameObject putBack;
    [SerializeField] private UnityEvent additionalEffect;

    private bool pickedUpBefore = false;
    private Collider coll;

    private void Start()
    {
        coll = GetComponent<Collider>();
    }

    public void PickUp()
    {
        if (MainManager.instance.gameState != 1) return;

        if (hold != null)
        {
            if (hold.transform.childCount > 0)
            {
                MainManager.instance.AddTrigger("dialogue;You;I don't want to hold two items at the same time...");
            }
            else
            {
                MainManager.instance.AddItem(name);
                MainManager.instance.PlayEffect(soundEffect);
                transform.SetParent(hold.transform);
                transform.localPosition = pos;
                transform.localRotation = Quaternion.Euler(angle);
                if (changeScale)
                {
                    transform.localScale = scale;
                }
                tag = "Untagged";
                if (!pickedUpBefore)
                {
                    pickedUpBefore = true;
                    additionalEffect.Invoke();
                }
                if(putBack != null)
                {
                    putBack.SetActive(true);
                }
                if(coll != null)
                {
                    coll.enabled = false;
                }
            }
        }
        else
        {
            MainManager.instance.AddItem(name);
            MainManager.instance.PlayEffect(soundEffect);
            additionalEffect.Invoke();
            Destroy(gameObject);
        }
    }
}
