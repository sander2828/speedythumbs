
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public GameObject itemtodelete;

    private void Update()
    {
       foreach (Touch touch in Input.touches)
        {
            

        }
    }


    public void StartGame()
    {
        Debug.Log("StartGame");
    }

    public void showHighscores()
    {

    }


    public void showInstructions()
    {

    }
}
