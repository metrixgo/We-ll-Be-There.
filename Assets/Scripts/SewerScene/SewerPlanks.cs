using UnityEngine;

public class SewerPlanks : MonoBehaviour
{
    [SerializeField] private CrowBarTrigger cb;
    [SerializeField] private AudioClip picking;
    [SerializeField] private SewerMetalDoor door;

    private static int cnt = 0;

    public void Pick()
    {
        if (!MainManager.instance.HasItem("Crowbar"))
        {
            MainManager.instance.AddTrigger("dialogue;You;I need to use a crowbar to remove these planks.");
        }
        else
        {
            cnt++;
            tag = "Untagged";
            MainManager.instance.PlayEffect(picking);
            gameObject.AddComponent<Rigidbody>();
            GetComponent<Rigidbody>().AddForce(0, 1.5f, 0.5f, ForceMode.Impulse);
            GetComponent<Rigidbody>().AddTorque(Vector3.right * 2.0f, ForceMode.Impulse);

            if (cnt == 5)
            {
                cb.PutAway();
                door.SetState(1);
            }
            Destroy(gameObject, 4.0f);
        }
    }
}
