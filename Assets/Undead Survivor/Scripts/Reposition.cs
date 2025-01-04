using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
        {
            return;
        }

        Vector2 playerPos = GameManager.instance.player.transform.position;
        Vector2 myPos = transform.position;

        float distanceX = Mathf.Abs(playerPos.x - myPos.x);
        float distanceY = Mathf.Abs(playerPos.y - myPos.y);

        Vector2 playerDir = GameManager.instance.player.inputVec;
        float dirX = playerDir.x < 0 ? -1 : 1;
        float dirY = playerDir.y < 0 ? -1 : 1;

        switch(transform.tag)
        {
            case "Ground":
                if(distanceX > distanceY)
                {
                    transform.Translate(Vector3.right * dirX * 60);
                }
                else if (distanceX < distanceY)
                {
                    transform.Translate(Vector3.up * dirY * 40);
                }
                break;
            case "Enemy":
                break;
        }
    }
}
