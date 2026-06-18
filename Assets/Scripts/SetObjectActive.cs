using UnityEngine;

public class SetObjectActive : MonoBehaviour
{
    [SerializeField] private bool setActive = true;
    [SerializeField] private GameObject[] objects;

    public void SetObjects()
    {
        foreach(GameObject o in objects)
        {
            if(o != null) o.SetActive(setActive);
        }
    }
}
