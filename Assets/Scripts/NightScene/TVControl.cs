using UnityEngine;
public class TVControl : MonoBehaviour
{
    [SerializeField] private GameObject control;
    [SerializeField] private AudioClip turn;

    private BoxCollider bc;

    private void Start()
    {
        bc = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        Ray ray = new Ray(control.transform.position, control.transform.forward);
        if (MainManager.instance.HasItem("Control") && Physics.Raycast(ray, out RaycastHit hit) && hit.transform == transform)
        {
            tag = "Interactable";
            bc.enabled = true;
        }
        else
        {
            tag = "Untagged";
            bc.enabled = false;
        }
    }

    public void TurnOff()
    {
        MainManager.instance.PlayEffect(turn);
        Destroy(gameObject);
    }
}
