using TMPro;
using UnityEngine;

public class KeyPad : MonoBehaviour
{
    [SerializeField] private AudioClip type;
    [SerializeField] private GameObject[] keys;
    [SerializeField] private TextMeshPro[] displays;

    private bool flg = false;
    private int numsSize = 0;
    private int[] nums = new int[4];
    
    private void Update()
    {
        flg = false;

        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i) || Input.GetKeyDown(KeyCode.Keypad0 + i) || ClickedOn(keys[i]))
            {
                flg = true;
                if(numsSize < 4) nums[numsSize++] = i;
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
            if(numsSize > 0) numsSize--;
        }

        for(int i = 0; i < numsSize; i++)
        {
            displays[i].text = nums[i].ToString();
        }
        for(int i=numsSize; i < 4; i++)
        {
            displays[i].text = "";
        }

        if (flg) MainManager.instance.PlayEffect(type);

    }

    private bool ClickedOn(GameObject o)
    {
        return false;
    }
}
