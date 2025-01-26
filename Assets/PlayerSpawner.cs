using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Instantiate(player, transform.position, Quaternion.identity);
        }
    }
}
