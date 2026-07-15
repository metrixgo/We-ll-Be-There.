using UnityEngine;

public class SewerPlanks : MonoBehaviour
{
    [SerializeField] private CrowBarTrigger cb;
    [SerializeField] private AudioClip picking;

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
            GetComponent<Rigidbody>().AddForce(0, 1.5f, 0, ForceMode.Impulse);
            if (cnt == 5) cb.PutAway();
        }
    }
}
