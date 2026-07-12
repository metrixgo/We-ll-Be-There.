using UnityEngine;

public class SetObjectActive : MonoBehaviour
{
    [SerializeField] private bool isPhysical = false;
    [SerializeField] private bool selfDestructs = false;
    [SerializeField] private bool setActive = true;
    [SerializeField] private GameObject[] objects;

    private void OnTriggerEnter(Collider other)
    {
        if (isPhysical) SetObjects();
    }

    public void SetObjects()
    {
        foreach(GameObject o in objects)
        {
            if(o != null) o.SetActive(setActive);
        }
        if (selfDestructs) Destroy(gameObject);
    }
}
