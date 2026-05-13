using UnityEngine;

public class PutDownItem : MonoBehaviour
{
    [SerializeField] private AudioClip soundEffect;
    [SerializeField] private GameObject item;

    private Collider coll;

    private void Start()
    {
        coll = item.GetComponent<Collider>();
    }

    public void Putdown()
    {
        if (MainManager.instance.gameState != 1) return;

        MainManager.instance.RemoveItem(item.name);
        MainManager.instance.PlayEffect(soundEffect);

        item.transform.SetParent(null);
        item.transform.position = transform.position;
        item.transform.rotation = transform.rotation;
        item.tag = "Interactable";
        if (coll != null) coll.enabled = true;
        gameObject.SetActive(false);
    }
}
