using UnityEngine;

public class NeightborJumpscare : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private PoliceWoman woman;
    [SerializeField] private AudioClip ac;

    private void OnTriggerEnter(Collider other)
    {
        woman.MoveOut();
        player.Freeze(true);
        player.SetRotation(89.0f, -3.5f, 0.2f);
        MainManager.instance.PlayEffect(ac);
        MainManager.instance.AddTrigger("wait;7.5");
        MainManager.instance.AddTrigger("dialogue;Policewoman;Hello, sir.;1");
        MainManager.instance.AddTrigger("dialogue;You;H... Hello?;1");
        MainManager.instance.AddTrigger("dialogue;Policewoman;What are you doing out here so late?;1");
        MainManager.instance.AddTrigger("dialogue;You;Um... I... I just came back from school...;1");
        MainManager.instance.AddTrigger("dialogue;Policewoman;Are you sure? It's already 2 o'clock midnight.;1");
        MainManager.instance.AddTrigger("dialogue;You;Well... um... I just accidentally fell asleep in my classroom.;1");
        MainManager.instance.AddTrigger("dialogue;Policewoman;...Ok. I just heard a large sound somewhere, it's like a car crash. Do you know where it came from?;1");
        MainManager.instance.AddTrigger("dialogue;You;......;1");
        MainManager.instance.AddTrigger("dialogue;You;Oh. I have no idea. You must be imagining.;1");
        MainManager.instance.AddTrigger("dialogue;Policewoman;I don't think that's true, sir. It woke me up from my sleep.;1");
        MainManager.instance.AddTrigger("dialogue;You;......;1");
        MainManager.instance.AddTrigger("dialogue;Policewoman;Alright. Seems like you have no idea what you're doing as well. I'm going to call backups to come and check this out. I don't want anything bad going on here to disrupt my sleep.;1");
        MainManager.instance.AddTrigger("dialogue;You;Wait... No...;1");
        MainManager.instance.AddTrigger("dialogue;Policewoman;Mhm?;1");
        MainManager.instance.AddTrigger("dialogue;You;Well... Never mind.;1");
        MainManager.instance.AddTrigger("dialogue;Policewoman;It's dangerous out here. Get home now and we'll investigate this.;1");
        MainManager.instance.AddTrigger("dialogue;You;Okay... Thanks for the advice...;1");
        player.CanRun(true);
        MainManager.instance.AddTrigger("flashprompt;Press [Shift] to run");
        Destroy(gameObject);
    }
}