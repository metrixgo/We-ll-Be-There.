using UnityEngine;

public class ExtendBoxHallway : MonoBehaviour
{
    [SerializeField] private GameObject end;
    [SerializeField] private GameObject[] units;
    [SerializeField] private AudioClip shift;
    
    private int idx = 0;

    private void OnTriggerEnter(Collider other)
    {
        MainManager.instance.PlayEffect(shift);
        end.transform.Translate(Vector3.left * 12.0f, Space.World);
        units[idx].SetActive(true);
        idx++;
        if (idx == 3) Destroy(gameObject);
    }
}
