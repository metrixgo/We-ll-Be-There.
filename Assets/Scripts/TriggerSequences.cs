using UnityEngine;

public class TriggerSequences : MonoBehaviour
{
    [SerializeField] private bool selfDestructs = true;
    [SerializeField] private string appliesTo = "Player";
    [SerializeField] private string[] triggers;

    private void OnTriggerEnter(Collider other)
    {
        if (MainManager.instance.gameState != 1) return ;
        if (!other.CompareTag(appliesTo)) return ;

        foreach (string trigger in triggers)
        {
            MainManager.instance.AddTrigger(trigger);
        }
        if(selfDestructs) Destroy(gameObject);
    }
}
