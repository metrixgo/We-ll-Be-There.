using UnityEngine;

public class DreamNewspaper : MonoBehaviour
{
    [SerializeField] private int index;

    public static int numLooked { get; private set; } = 0;
    private bool looked = false;

    public void Interact()
    {
        if (!looked)
        {
            looked = true;
            numLooked++;
        }

        if (index == 1)
        {
            MainManager.instance.AddTrigger("dialogue;Newspaper;CRASHED BIKE FOUND: The local police discovered an abandoned bike broken into pieces on the sidewalk.");
            MainManager.instance.AddTrigger("dialogue;Newspaper;There were blood traces on the wheels and investigators believe there might have been an accident last night that no one noticed.");
        }
        else if (index == 2)
        {
            MainManager.instance.AddTrigger("dialogue;Newspaper;MAYOR'S SON WENT MISSING: The mayor announced that his son went missing last night.");
            MainManager.instance.AddTrigger("dialogue;Newspaper;The police have opened an investigation for this. Almost all the police force was involved.");
            MainManager.instance.AddTrigger("dialogue;Newspaper;The mayor said that he will use all his power and resources to find his son, and if he knows anyone who did anything to his son, they will be harshly punished.");
        }
        else
        {
            MainManager.instance.AddTrigger("dialogue;Newspaper;MYSTERIOUS FIGURE REPORTED: A 12-year-old boy named Charlie woke up last night and saw a tall, black figure outside the window.");
            MainManager.instance.AddTrigger("dialogue;Newspaper;He said he saw the figure carrying a large bag with both of his hands. When asked for more details, Charlie said he was scared at that time and ran to his mom.");
            MainManager.instance.AddTrigger("dialogue;Newspaper;When they went to the window again, the figure was gone. They then reported this incident to the police later.");
        }
    }
}
