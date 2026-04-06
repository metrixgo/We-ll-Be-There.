using UnityEngine;

public class NeightborJumpscare : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private PoliceWoman woman;
    [SerializeField] private AudioClip ac;

    private void OnTriggerEnter(Collider other)
    {
        woman.MoveOut();
        player.SetRotation(89.0f, -3.5f, 0.2f);
        MainManager.instance.PlayEffect(ac);
        MainManager.instance.AddTrigger("wait;7.5");
        MainManager.instance.AddTrigger("dialogue;Policewoman;Hello, sir.");
        MainManager.instance.AddTrigger("dialogue;You;H... Hello?");
        MainManager.instance.AddTrigger("dialogue;Policewoman;What are you doing out here so late?");
        MainManager.instance.AddTrigger("dialogue;You;Um... I... I just came back from school...");
        MainManager.instance.AddTrigger("dialogue;Policewoman;Are you sure? It's already 2 o'clock midnight.");
        MainManager.instance.AddTrigger("dialogue;You;Well... um... I just accidentally fell asleep in my classroom.");
        MainManager.instance.AddTrigger("dialogue;Policewoman;...Ok. I just heard a large sound somewhere, it's like a car crash. Do you know where it came from?");
        MainManager.instance.AddTrigger("dialogue;You;......");
        MainManager.instance.AddTrigger("dialogue;You;Oh. I have no idea. You must be imagining.");
        MainManager.instance.AddTrigger("dialogue;Policewoman;I don't think that's true, sir. It woke me up from my sleep.");
        MainManager.instance.AddTrigger("dialogue;You;......");
        MainManager.instance.AddTrigger("dialogue;Policewoman;Alright. Seems like you have no idea what you're doing as well. I'm going to call backups to come and check this out. I don't want anything bad going on here to disrupt my sleep.");
        MainManager.instance.AddTrigger("dialogue;You;Wait... No...");
        MainManager.instance.AddTrigger("dialogue;Policewoman;Mhm?");
        MainManager.instance.AddTrigger("dialogue;You;Well... Never mind.");
        MainManager.instance.AddTrigger("dialogue;Policewoman;It's dangerous out here. Get home now and we'll investigate this.");
        MainManager.instance.AddTrigger("dialogue;You;Okay... Thanks for the advice...");
        player.CanRun(true);
        MainManager.instance.AddTrigger("flashprompt;Press [Shift] to run");
        Destroy(gameObject);
    }
}