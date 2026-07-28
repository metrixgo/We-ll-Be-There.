using System.Collections;
using TMPro;
using UnityEngine;

public class KeyPad : MonoBehaviour
{
    [SerializeField] private RectTransform rawImage;
    [SerializeField] private RenderTexture renderTexture;
    [SerializeField] private AudioClip type;
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject flashLight;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerCam;
    [SerializeField] private SewerFlashlight playerFlashLight;
    [SerializeField] private GameObject[] keys;
    [SerializeField] private TextMeshPro[] displays;

    private bool tried = false;
    private int state = 0;
    private int numsSize = 0;
    private int[] nums = new int[4];
    private Camera padCam;
    private AudioSource doorAd;
    private Vector3 camPos = new Vector3(15.6794491f, -7.95501661f, -4.65597343f);
    private Quaternion camRot = Quaternion.Euler(0, -90.0f, 0);

    private void Start()
    {
        doorAd = door.GetComponent<AudioSource>();
        padCam = cam.GetComponent<Camera>();
    }

    private void Update()
    {
        if (state != 1) return;

        if (state == 1 && Input.GetKeyDown(KeyCode.Escape))
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
            if (numsSize == 4)
            {
                Debug.Log("Correct");
            }
            else
            {
                numsSize = 0;
                Debug.Log("Incorrect");
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
        if (state != -1) StartCoroutine(FocusOnPad(b));
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

    public void TryOpen()
    {
        if (state == 0) StartCoroutine(TryLocked());
    }

    private IEnumerator TryLocked()
    {
        if (!tried)
        {
            tried = true;
            MainManager.instance.AddTrigger("dialogue;You;It's locked.");
        }
        doorAd.Play();
        state = -1;
        float rot = 0;
        Vector3 angles = door.transform.eulerAngles;
        float goal = angles.y;
        while (rot < 4.0f)
        {
            rot += 60.0f * Time.deltaTime;
            door.transform.Rotate(0, 60.0f * Time.deltaTime, 0, Space.World);
            yield return null;
        }
        rot = 0;
        while (rot < 4.0f)
        {
            rot += 20.0f * Time.deltaTime;
            door.transform.Rotate(0, -20.0f * Time.deltaTime, 0, Space.World);
            yield return null;
        }
        door.transform.rotation = Quaternion.Euler(angles.x, goal, angles.z);
        state = 0;
    }
}