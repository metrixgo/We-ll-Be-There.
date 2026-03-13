using UnityEngine;

public class LoopStart : MonoBehaviour
{
    [SerializeField] private Bicycle bicycle;

    private int cnt = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Bicycle")) return ;
        cnt++;
        if (cnt == 1) MainManager.instance.AddTrigger("flashdialogue;You;I think I'm not feeling well...;3");
        else if (cnt == 2) MainManager.instance.AddTrigger("flashdialogue;You;I still remember that scene I saw at school...;3");
        else if (cnt == 3) MainManager.instance.AddTrigger("flashdialogue;You;Was is just my illusion...? But everything feels so real...;3");
        else if (cnt == 4) MainManager.instance.AddTrigger("flashdialogue;You;I still have a lot of work to do... I'm so tired...;6");
        else if (cnt == 7)
        {
            bicycle.Accelerate();
            MainManager.instance.AddTrigger("flashdialogue;You;Alright I need to go faster. I want to get home now. Now.;3");
        }
        else if (cnt == 11)
        {
            Destroy(gameObject);
            bicycle.LoseControl();
        }
    }

}
