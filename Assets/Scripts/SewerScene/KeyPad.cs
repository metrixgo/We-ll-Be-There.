using System.Collections;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class KeyPad : MonoBehaviour
{
    [SerializeField] private RectTransform rawImage;
    [SerializeField] private RenderTexture renderTexture;
    [SerializeField] private AudioClip type;
    [SerializeField] private AudioClip wrong;
    [SerializeField] private AudioClip correct;
    [SerializeField] private SewerMetalDoor door;
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject flashLight;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerCam;
    [SerializeField] private SewerFlashlight playerFlashLight;
    [SerializeField] private Light pLight;
    [SerializeField] private Renderer pRend;
    [SerializeField] private GameObject[] keys;
    [SerializeField] private TextMeshPro[] displays;

    private int state = 0;
    private int numsSize = 0;
    private int[] nums = new int[4];
    private float wrongT = 0;
    private Camera padCam;
    private AudioSource ad;
    private Vector3 camPos = new Vector3(15.6794491f, -7.95501661f, -4.65597343f);
    private Quaternion camRot = Quaternion.Euler(0, -90.0f, 0);

    private void Start()
    {
        padCam = cam.GetComponent<Camera>();
        ad = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (state != 1) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            FocusOn(false);
            return;
        }

        bool flg = false;

        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i) || Input.GetKeyDown(KeyCode.Keypad0 + i) || ClickedOn(keys[i]))
            {
                flg = true;
                if (numsSize < 4) nums[numsSize++] = i;
            }
        }

        if (ClickedOn(keys[10]) || Input.GetKeyDown(KeyCode.Return))
        {
            flg = true;
            if (numsSize == 4 && nums[0] == 0 && nums[1] == 0 && nums[2] == 0 && nums[3] == 0)
            {
                door.SetState(1);
                ad.clip = correct;
                ad.Play();
                pLight.enabled = true;
                pLight.color = Color.green;
                pRend.material.color = Color.green;
                MainManager.instance.ClearTriggers();
                FocusOn(false);
                tag = "Untagged";
            }
            else
            {
                numsSize = 0;
                ad.clip = wrong;
                ad.Play();
                wrongT = 1.0f;
                pLight.enabled = true;
                pLight.color = Color.red;
                pRend.material.color = Color.red;
            }
        }
        if (ClickedOn(keys[11]) || Input.GetKeyDown(KeyCode.Backspace))
        {
            flg = true;
            if (numsSize > 0) numsSize--;
        }

        for (int i = 0; i < numsSize; i++)
        {
            displays[i].text = nums[i].ToString();
        }
        for (int i = numsSize; i < 4; i++)
        {
            displays[i].text = "";
        }

        if (flg) MainManager.instance.PlayEffect(type);

        if(wrongT > 0)
        {
            wrongT -= Time.deltaTime;
            if(wrongT <= 0)
            {
                if (CompareTag("Interactable"))
                {
                    pLight.enabled = false;
                    pRend.material.color = Color.white;
                }
                wrongT = 0;
            }
        }

    }

    private bool ClickedOn(GameObject o)
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rawImage, Input.mousePosition, null, out Vector2 localPoint))
            {
                float x = (localPoint.x - rawImage.rect.x) / rawImage.rect.width;
                float y = (localPoint.y - rawImage.rect.y) / rawImage.rect.height;
                Vector3 texPos = new Vector3(x * renderTexture.width, y * renderTexture.height, 0);
                Ray ray = padCam.ScreenPointToRay(texPos);
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                {
                    return hit.collider.gameObject == o;
                }
            }
        }
        return false;
    }

    public void FocusOn(bool b)
    {
        if (state != -1)
        {
            if (b && SewerBooks.num < 9) MainManager.instance.AddTrigger("dialogue;You;I don't know the code yet.");
            else StartCoroutine(FocusOnPad(b));
        }
    }

    private IEnumerator FocusOnPad(bool b)
    {
        state = -1;
        float t = 0;
        float l = 0.7f;
        Vector3 pos = playerCam.transform.position;
        Quaternion rot = playerCam.transform.rotation;
        MainManager.instance.AddTrigger("wait;" + l);

        if (b)
        {
            MainManager.instance.AddTrigger("waitesc");
            if (playerFlashLight.IsOpened()) flashLight.SetActive(true);
            else flashLight.SetActive(false);
            cam.SetActive(true);
            player.SetActive(false);
            while (t < l)
            {
                cam.transform.position = Vector3.Lerp(pos, camPos, t / l);
                cam.transform.rotation = Quaternion.Slerp(rot, camRot, t / l);
                t += Time.deltaTime;
                yield return null;
            }
            cam.transform.position = camPos;
            cam.transform.rotation = camRot;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            state = 1;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            while (t < l)
            {
                cam.transform.position = Vector3.Lerp(camPos, pos, t / l);
                cam.transform.rotation = Quaternion.Slerp(camRot, rot, t / l);
                t += Time.deltaTime;
                yield return null;
            }
            cam.SetActive(false);
            player.SetActive(true);
            state = 0;
        }
    }
}