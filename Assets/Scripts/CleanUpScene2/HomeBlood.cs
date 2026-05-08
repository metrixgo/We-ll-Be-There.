using UnityEngine;

public class HomeBlood : MonoBehaviour
{
    [SerializeField] private AudioClip cleanEffect;
    [SerializeField] private Flashlight flashlight;

    private Material mat;
    private int layers = 3;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        if (flashlight.IsOpened()) tag = "Interactable";
        else tag = "Untagged";
    }

    public void Clean()
    {
        if (MainManager.instance.gameState != 1) return;

        if (MainManager.instance.HasItem("Mop"))
        {
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, --layers / 3.0f + 0.1f);
            MainManager.instance.PlayEffect(cleanEffect);
            MainManager.instance.AddTrigger("wait;"+(cleanEffect.length + 0.1f));
            if (layers == 0)
            {
                flashlight.FinishedOne();
                Destroy(gameObject);
            }
        }
        else
        {
            MainManager.instance.AddTrigger("dialogue;You;I can't clean these up with my hands. I need to get a mop.");
        }
    }
}
