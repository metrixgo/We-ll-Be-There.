using UnityEngine;

public class TriggerSequences : MonoBehaviour
{
    [SerializeField] private bool selfDestructs = true;
    [SerializeField] private string[] triggers;

    private void OnTriggerEnter(Collider other)
    {
        AddTriggers();
    }

    public void AddTriggers()
    {
        if (MainManager.instance.gameState != 1) return;

        foreach (string trigger in triggers)
        {
            MainManager.instance.AddTrigger(trigger);
        }
        if (selfDestructs) Destroy(gameObject);
    }
}
