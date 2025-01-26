using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldMapScript : MonoBehaviour
{
    [Header("Can Go?")]
    [SerializeField] private bool area1;
    [SerializeField] private bool area2;
    [SerializeField] private bool area3;
    [SerializeField] private bool area4;

    public void TravelArea1()
    {
        if (area1 == true)
        {
            Debug.Log("Going to area 1");

            SceneManager.LoadScene(sceneName: "Factory");
        }
    }

    public void TravelArea2()
    {
        if (area2 == true)
        {
            Debug.Log("Going to area 2");

            SceneManager.LoadScene(sceneName: "TestingScene");
        }
    }

    public void TravelArea3()
    {
        if (area3 == true)
        {
            Debug.Log("Going to area 3");
        }
    }

    public void TravelArea4()
    {
        if (area4 == true)
        {
            Debug.Log("Going to area 4");
        }
    }
}
