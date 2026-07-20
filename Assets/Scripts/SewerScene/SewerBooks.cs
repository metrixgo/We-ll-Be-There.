using UnityEngine;

public class SewerBooks : MonoBehaviour
{
    private static int num = 0;

    private bool alreadyRead = false;
    private string message;
    private int page;

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
            else if (num == 9) message = "But one day the boy was so exhausted at school and accidentally fell asleep since he was extremely tired. Illusions of him being at school collecting stupid books to escape overwhelmed his mind, and when he managed to get rid of that illusion only to find that he was already outside of school, he decided to go home. But he was so tired and distracted that he accidentally crashed into the mayor's son on the ride home at night and killed him. The boy went crazy. He did not know what to do, so he decided to hide the body. The boy managed to hide most of it, but there were still traces of evidence left behind. That night the boy had a terrible dream, and when he woke up he noticed that a group of police would come soon. He thought about lots of places and managed to clean up all the traces before the police came, but something was not right. The boy looked as though he was being manipulated and was strangely lured down to a sewer that should not exist. Now the boy is likely still reading books inside his illusion, but he still did not know what to do. He did not know that the escape code was simply 0000, and he will likely be hunted down by a killer next, so the destiny of the boy is determined and nothing can be changed. His life is ruined because he made a mistake, a really stupid mistake that cost his life.";
            else if (num == 10) message = "\"Oh, sorry everyone. I made a mistake. These texts were added by a strange kid. The story was not like this. The boy bought his bike and went home. His parents were waiting for him. They hugged together. His mom said, 'I knew you could do this! You know, as long as you persist, everything can be solved! We are so proud of you!' Then, the family lived happily ever after. The End. Okay everyone, now go back to your seats. Story time is over.\" \"Ms. Bartlett!!! The real escape code is 0419!!! The real escape code is 0419!!! I saw it with my eyes!!! The little boy got tricked!!! Ms.-\" \"Enough of that, Eric. If you say this nonsense again, I'm going to take away all your stars for this week! Now everyone please be quiet and look at the whiteboard.\"";
        }
    }
}
