using UnityEngine;

public class MoveRelativeToPlayer : MonoBehaviour
{
    public GameObject player;
    void Update()
    {
        transform.position = new Vector2(player.transform.position.x + 7.75f, player.transform.position.y);
    }
}
