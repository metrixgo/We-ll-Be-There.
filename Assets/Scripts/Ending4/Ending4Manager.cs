using UnityEngine;

public class Ending4Manager : MonoBehaviour
{
    [SerializeField] private GameObject cam;
    [SerializeField] private PlayerController pc;

    private void Start()
    {
        pc.Freeze(true);
        MainManager.instance.AddTrigger("changescreen;#000000FF;#00000000;4");
        MainManager.instance.AddTrigger("dialogue;You;......;1");
        MainManager.instance.AddTrigger("dialogue;Dad;...and I was so surprised you know, we both didn't see when that happened.;1");
        MainManager.instance.AddTrigger("dialogue;Dad;I suspect someone broke it with a hammer, or else the hole on the glass door wouldn't be so uniform.;1");
        MainManager.instance.AddTrigger("dialogue;Mom;Son, do you know who broke the glass door?;1");
        MainManager.instance.AddTrigger("dialogue;You;I... don't know...;1");
        MainManager.instance.AddTrigger("dialogue;You;Mom, is there a hole in the backyard?;1");
        MainManager.instance.AddTrigger("dialogue;Mom;......;1");
        MainManager.instance.AddTrigger("dialogue;Mom;I do think there are some traces of dirt there. But there should be no holes.;1");
        MainManager.instance.AddTrigger("dialogue;Dad;What are you saying, didn't we already see that? Like the one that's very deep into the ground, with all kinds of...;1");
        MainManager.instance.AddTrigger("dialogue;You;What? What do you mean? Dad!;1");
        MainManager.instance.AddTrigger("dialogue;Dad;N... Nothing.;1");
        MainManager.instance.AddTrigger("dialogue;You;???;1");
        MainManager.instance.AddTrigger("dialogue;Dad;Haha, got you with a joke, huh? There are no holes, what are you even worrying about.;1");
        MainManager.instance.AddTrigger("dialogue;You;......;1");
        MainManager.instance.AddTrigger("dialogue;Dad;Come on, are you not feeling well right now?");
        MainManager.instance.AddTrigger("dialogue;Dad;Let's sing together, how about that?");
        MainManager.instance.AddTrigger("dialogue;Mom;Um... he's not feeling good right now. Give him some space. Let's just eat.");
        MainManager.instance.AddTrigger("dialogue;Dad;Ok. Yeah. Sure. Let's eat.");
        MainManager.instance.AddTrigger("dialogue;You;......");
        MainManager.instance.AddTrigger("dialogue;Dad;......");
        MainManager.instance.AddTrigger("dialogue;???;......");
        MainManager.instance.AddTrigger("dialogue;???;......");
    }
}
