using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.UI;

public class ExtendBoxHallway : MonoBehaviour
{
    [SerializeField] private GameObject end;
    [SerializeField] private GameObject unit;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerHead;
    [SerializeField] private NavMeshSurface surface;
    [SerializeField] private AudioClip die;
    [SerializeField] private AudioClip shift;
    [SerializeField] private Image screen;

    private int idx = 0;
    private Vector3 pos = new Vector3(-20.2310162f, -9.0305233f, 54.156929f);
    private Quaternion rot = Quaternion.Euler(-90.0f, 90.0f, 0);

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        idx++;
        if (idx >= 4)
        {
            playerHead.SetActive(true);
            playerHead.transform.parent = null;
            playerHead.GetComponent<Rigidbody>().AddForce(0, 0.5f, 0, ForceMode.Impulse);
            playerHead.GetComponent<Rigidbody>().AddTorque(Vector3.up / 3.0f, ForceMode.Impulse);
            player.SetActive(false);
            MainManager.instance.PlayEffect(die);
            screen.color = Color.red / 2.0f;
            MainManager.instance.AddTrigger("wait;3");
            MainManager.instance.AddTrigger("loadscene;SewerScene;3");
        }
        else
        {
            MainManager.instance.PlayEffect(shift);
            end.transform.Translate(Vector3.left * 12.0f, Space.World);
            Instantiate(unit, pos, rot);
            pos += Vector3.left * 12.0f;
            surface.BuildNavMesh();
            MainManager.instance.AddTrigger("flashscreen;#FF000000;#FF000055;0.2");
            MainManager.instance.AddTrigger("flashscreen;#FF000055;#FF000000;1");
        }
    }
}
