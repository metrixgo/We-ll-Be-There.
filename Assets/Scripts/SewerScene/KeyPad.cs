using System.Collections;
using TMPro;
using UnityEngine;

public class KeyPad : MonoBehaviour
{
    [SerializeField] private AudioClip type;
    [SerializeField] private AudioClip doorBang;
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerCam;
    [SerializeField] private GameObject[] keys;
    [SerializeField] private TextMeshPro[] displays;

    private bool tried = false;
    private int state = 0;
    private int numsSize = 0;
    private int[] nums = new int[4];
    private Vector3 camPos = new Vector3(15.6794491f, -7.95501661f, -4.65597343f);


    private void Update()
    {
        if (state != 1 || MainManager.instance.gameState != 1) return;

        if (state == 1 && Input.GetKeyDown(KeyCode.Escape))
        {
            state = -1;
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
        return false;
    }

    public void FocusOn(bool b)
    {
        if (state != -1) StartCoroutine(FocusOnPad());
    }

    private IEnumerator FocusOnPad()
    {
        bool b = true;
        state = -1;
        float t = 0;
        if (b)
        {
            Vector3 pos = playerCam.transform.position;
            cam.SetActive(true);
            player.SetActive(false);
            while (t < 1.5f)
            {
                cam.transform.position = Vector3.Lerp(pos, camPos, t / 1.5f);
                t += Time.deltaTime;
                yield return null;
            }
            cam.transform.position = camPos;
        }
        else
        {
            Vector3 pos = playerCam.transform.position;
            while (t < 1.5f)
            {
                cam.transform.position = Vector3.Lerp(camPos, pos, t / 1.5f);
                t += Time.deltaTime;
                yield return null;
            }
            cam.SetActive(false);
            player.SetActive(true);
        }
        state = 1;
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
        MainManager.instance.PlayEffect(doorBang);
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