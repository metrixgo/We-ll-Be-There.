using UnityEngine;

public class CheckRoom : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(MainManager.instance.gameState == 1)
        {
            CheckRoomsManager.count.CheckOne();
            Destroy(gameObject);
        }
    }
}
