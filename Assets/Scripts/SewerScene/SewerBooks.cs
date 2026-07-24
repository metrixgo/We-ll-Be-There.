using System.Collections;
using UnityEngine;

public class SewerBooks : MonoBehaviour
{
    [SerializeField] private Transform corBook;

    private static int num = 0;

    private bool alreadyRead = false;
    private string message;
    private string to;
    private string back;
    private int page;

    private void Start()
    {
        float x = corBook.position.x - transform.position.x;
        float y = corBook.position.y - transform.position.y;
        float z = corBook.position.z - transform.position.z;
        to = x + ";" + y + ";" + z;
        back = (-x) + ";" + (-y) + ";" + (-z);
    }

    public void Read()
    {
        if (!alreadyRead)
        {
            alreadyRead = true;
            num++;
            page = num;
            if (num == 1) message = "Once upon a time, there was a little boy who lived with a lovely family.";
            else if (num == 2) message = "One day, his parents suddenly decided to move away. The little boy was so scared of being left alone.";
            else if (num == 3) message = "He asked his parents for the reason they were moving away, but all his parents said was, \"We promise that when you are able to buy a bike on your own, we will be back home.\" The little boy stopped, and then nodded.";
            else if (num == 4) message = "After his parents left, he started to earn money by doing house chores in the neighborhood. It was tough, but he managed to save a lot of money in his piggy bank. The little boy was so proud of himself!";
            else if (num == 5) message = "After three months of hard work, he finally had enough money to buy a bicycle. He carried his piggy bank to the bicycle store. \"I'll have a nice and big bicycle with a head light and all the decorations, please,\" he said to the store manager.";
            else if (num == 6) message = "The store manager stared at him. \"It'll cost a tidy bit,\" the manager replied. \"I understand. And a metal basket on the back of the bicycle, please,\" the little boy commanded with a sense of determination.";
            else if (num == 7) message = "The little boy exited the store with a brand new bicycle. He rode home happily, humming songs along the way and looking around curiously. He knew his parents must be at home in no time!";
            else if (num == 8) message = "Eight years later, the little boy went into high school. He lived in a big, empty house. His parents were still nowhere to be found. He was lonely. Sad. Helpless. He still waited in front of his house every day to see if his parents had come back. Life was rough, but he still lived happily every day. He believed that as long as he worked hard, everything would be fine.";
            else if (num == 9) message = "But one day the boy was so exhausted at school and accidentally fell asleep since he was extremely tired illusions of him being at school collecting stupid books to escape overwhelmed his mind and when he managed to get rid of that illusion only to found that he was already outside of school so he decided to go home but he was so tired and distracted so he accidentally crashed onto the mayor's son on the ride home at night and killed him and the boy went crazy he did not know what to do so he decided to hide the body the boy managed to hide most of them but there were still traces of evidence left behind that night the boy had a terrible dream and when he woke up he noticed that a group of police will come soon he thinked about lots of places and managed to clean up all the traces before the police came but something was not right and the boy looked like to be manipulated was strangely lured down to a sewer that should not exist and now the boy is likely still reading books inside his illusion but he still do not know what to do he did not know that the escape code was simply 0000 and he will likely be hunted down by a killer next so the destiny of the boy is determined and nothing can be changed and his life is ruined because he made a mistake a really stupid mistake that costed his life.";
            else if (num == 10) message = "\"Oh, sorry everyone. I made a mistake. These texts were added by a strange kid. The story was not like that. The boy bought his bike and went home. His parents were waiting for him. They hugged together. His mom said, 'I knew you could do this! You know, as long as you persist, everything can be solved! We are so proud of you!' Then, the family lived happily ever after. The End. Okay everyone, now go back to your seats. Story time is over.\" \"Ms. Bartlett!!! The real escape code is 0419!!! The real escape code is 0419!!! I saw it with my eyes!!! The little boy got tricked!!! Ms.-\" \"Enough of that, Eric. If you say this nonsense again, I'm going to take away all your stars for this week! Now everyone please be quiet and look at the whiteboard.\"";
        }

        StartCoroutine(Teleport());

    }
    
    private IEnumerator Teleport()
    {
        MainManager.instance.AddTrigger("moveplayer;" + to);
        MainManager.instance.AddTrigger("dialogue;Book;" + message);
        MainManager.instance.AddTrigger("moveplayer;" + back);
        RenderSettings.fogDensity = 1.0f;
        RenderSettings.ambientIntensity = 0.6f;
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => MainManager.instance.gameState == 1);
        RenderSettings.fogDensity = 0.15f;
        RenderSettings.ambientIntensity = 0.3f;
    }
}
