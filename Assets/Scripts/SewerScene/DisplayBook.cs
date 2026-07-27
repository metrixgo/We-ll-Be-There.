using TMPro;
using UnityEngine;

public class DisplayBook : MonoBehaviour
{
    public static DisplayBook instance;

    [SerializeField] private GameObject book;
    [SerializeField] private TextMeshProUGUI leftPage;
    [SerializeField] private TextMeshProUGUI rightPage;
    [SerializeField] private TextMeshProUGUI leftNum;
    [SerializeField] private TextMeshProUGUI rightNum;
    [SerializeField] private TextMeshProUGUI bigPage;
    [SerializeField] private GameObject leftTurn;
    [SerializeField] private GameObject rightTurn;
    [SerializeField] private AudioClip bookFlip;

    private string[] pages = {
    "",
    "Once upon a time, there was a little boy who lived with a lovely family.",
    "One day, his parents suddenly decided to move away. The little boy was so scared of being left alone.",
    "He asked his parents for the reason they were moving away, but all his parents said was, \"We promise that when you are able to buy a bike on your own, we will be back home.\" The little boy stopped, and then nodded.",
    "After his parents left, he started to earn money by doing house chores in the neighborhood. It was tough, but he managed to save a lot of money in his piggy bank. The little boy was so proud of himself!",
    "After three months of hard work, he finally had enough money to buy a bicycle. He carried his piggy bank to the bicycle store. \"I'll have a nice and big bicycle with a head light and all the decorations, please,\" he said to the store manager.",
    "The store manager stared at him. \"It'll cost a tidy bit,\" the manager replied. \"I understand. And a metal basket on the back of the bicycle, please,\" the little boy commanded with a sense of determination.",
    "The little boy exited the store with a brand new bicycle. He rode home happily, humming songs along the way and looking around curiously. He knew his parents must be at home in no time!",
    "Eight years later, the little boy went into high school. He lived in a big, empty house. His parents were still nowhere to be found. He was lonely. Sad. Helpless. He still waited in front of his house every day to see if his parents had come back. Life was rough, but he still lived happily every day. He believed that as long as he worked hard, everything would be fine.",
    "But one day the boy was so exhausted at school and accidentally fell asleep since he was extremely tired illusions of him being at school collecting stupid books to escape overwhelmed his mind and when he managed to get rid of that illusion only to find that he was already outside of school so he decided to go home but he was so tired and distracted so he accidentally crashed into the mayor's son on the ride home at night and killed him and the boy went crazy he did not know what to do so he decided to hide the body the boy managed to hide most of them but there were still traces of evidence left behind that night the boy had a terrible dream and when he woke up he noticed that a group of police will come soon he thought about lots of places and managed to clean up all the traces before the police came but something was not right and the boy looked to be manipulated was strangely lured down to a sewer that should not exist and now the boy is likely still reading books inside his illusion but he still does not know what to do he did not know that <size=20><color=red>the escape code was simply 0000</color></size> and he will likely be hunted down by a killer next so the destiny of the boy is determined and nothing can be changed and his life is ruined because he made a mistake a really stupid mistake that cost his life.",
    "\"Oh, sorry everyone. I made a mistake. These texts were added by a strange kid. The story was not like that. The boy bought his bike and went home. His parents were waiting for him. They hugged together. His mom said, 'I knew you could do this! You know, as long as you persist, everything can be solved! We are so proud of you!' Then, the family lived happily ever after. The End. Okay everyone, now go back to your seats. Story time is over.\" \n\"Ms. Bartlett!!! <size=20><color=red>The real escape code is 0419!!! The real escape code is 0419!!!</size></color> I saw it with my eyes!!! The little boy got tricked!!! Ms.-\" \n\"Enough of that, Eric. If you say this nonsense again, I'm going to take away all your stars for this week! Now everyone please be quiet and look at the whiteboard.\"",
    };

    private int unlockedNum = 0;
    private int curPage = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for(int i = 0; i < pages.Length; i++)
        {
            pages[i] = MainManager.instance.Translate(pages[i]);
        }
    }

    private void Update()
    {
        if (!book.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.Escape)) book.SetActive(false);

        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && leftTurn.activeSelf)
        {
            if (curPage <= 8) DisplayPage(curPage - 2);
            else DisplayPage(curPage - 1);
        }
        else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && rightTurn.activeSelf)
        {
            if (curPage < 7) DisplayPage(curPage + 2);
            else DisplayPage(curPage + 1);
        }
    }

    public void DisplayPage(int p)
    {
        unlockedNum = Mathf.Max(unlockedNum, p);
        curPage = p;
        book.SetActive(true);
        MainManager.instance.PlayEffect(bookFlip);

        if (p <= 8)
        {
            bigPage.text = "";
            if (p % 2 == 0)
            {
                leftNum.text = p.ToString();
                rightNum.text = (p + 1).ToString();
                leftPage.text = pages[p];
                if (unlockedNum >= p + 1 && p != 8) rightPage.text = pages[p + 1];
                else rightPage.text = "";
            }
            else
            {
                leftNum.text = (p - 1).ToString();
                rightNum.text = p.ToString();
                leftPage.text = pages[p - 1];
                rightPage.text = pages[p];
            }

            leftTurn.SetActive(p > 1);
            rightTurn.SetActive(unlockedNum > p);
        }
        else if (p == 9)
        {
            leftNum.text = "9999999999999999999999999999999999999999999999999999999999999999";
            rightNum.text = "";
            leftTurn.SetActive(true);
            rightTurn.SetActive(unlockedNum > 9);
            leftPage.text = "";
            rightPage.text = "";
            bigPage.text = pages[9];

        }
        else if (p == 10)
        {
            leftNum.text = "1010101010101010101010101010101010101010101010101010101010101010";
            rightNum.text = "";
            leftTurn.SetActive(true);
            rightTurn.SetActive(false);
            leftPage.text = "";
            rightPage.text = "";
            bigPage.text = pages[10];
        }
    }
}