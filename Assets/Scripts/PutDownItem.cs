using UnityEngine;

public class PutDownItem : MonoBehaviour
{
    [SerializeField] private GameObject item;

    public void Putdown()
    {
        item.transform.SetParent(null);
        item.transform.position = transform.position;
        item.transform.rotation = transform.rotation;
        gameObject.SetActive(false);
    }
}
