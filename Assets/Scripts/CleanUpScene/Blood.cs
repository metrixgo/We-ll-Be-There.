using UnityEngine;

public class Blood : MonoBehaviour
{
    [SerializeField] private AudioClip cleanEffect;
    [SerializeField] private GameObject plasticBag;
    [SerializeField] private GameObject corpse;
    [SerializeField] private GameObject trig;

    private Material mat;
    private int layers = 15;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    public void Clean()
    {
        if (MainManager.instance.gameState != 1) return ;

        if (MainManager.instance.HasItem("Mop"))
        {
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, --layers/15.0f);
            MainManager.instance.PlayEffect(cleanEffect);
            MainManager.instance.AddTrigger("wait;1.2");
            if(layers == 0)
            {
                MainManager.instance.AddTrigger("wait;1");
                MainManager.instance.AddTrigger("dialogue;You;Ok, I think this is fine now.");
                MainManager.instance.AddTrigger("dialogue;You;Now I need to grab a plastic bag from home to pack this up.");
                MainManager.instance.AddTrigger("cleartasks");
                MainManager.instance.AddTrigger("task;Get home");
                plasticBag.tag = "Interactable";
                corpse.tag = "Interactable";
                trig.SetActive(true);
                Destroy(gameObject);
            }
        }
        else
        {
            MainManager.instance.AddTrigger("dialogue;You;I can't clean these up with my hands. I need to go home to get a mop.");
        }
    }
}
