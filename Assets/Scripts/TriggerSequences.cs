using UnityEngine;

public class TriggerSequences : MonoBehaviour
{
    [SerializeField] private bool isPhysical = true;
    [SerializeField] private bool selfDestructs = true;
    [SerializeField] private string[] triggers;

    private void OnTriggerEnter(Collider other)
    {
        if(isPhysical) AddTriggers();
    }

    public void AddTriggers()
    {
        if (MainManager.instance.gameState != 1) return;

        foreach (string trigger in triggers)
        {
            MainManager.instance.AddTrigger(trigger);
        }
        if (selfDestructs && isPhysical) Destroy(gameObject);
        else if (selfDestructs) Destroy(this);
    }
}
