using UnityEngine;
using UnityEngine.UI;

public class SeparatePlayerHead : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject cam;
    [SerializeField] private AudioClip die;
    [SerializeField] private Image screen;

    private bool dead = false;
    private Collider coll;
    private Rigidbody rb;

    private void Start()
    {
        coll = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    public void Die()
    {
        if (dead) return;
        dead = true;
        coll.enabled = true;
        rb.isKinematic = false;
        cam.SetActive(true);
        gameObject.transform.SetParent(null);
        MainManager.instance.PlayEffect(die);
        screen.color = Color.red * 0.7f;
        MainManager.instance.AddTrigger("wait;3");
        MainManager.instance.AddTrigger("loadscene;SewerScene;3");
        rb.AddForce(0, 0.5f, 0, ForceMode.Impulse);
        rb.AddTorque(Vector3.up / 3.0f, ForceMode.Impulse);
        player.SetActive(false);
    }

    public bool IsDead()
    {
        return dead;
    }
}
