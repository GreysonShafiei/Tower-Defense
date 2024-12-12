using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    SurvivalCounter counter;
    public void loadScene(string scene)
    {
        if (scene == "Gameplay")
        {
            SurvivalCounter.Instance.startSurvivalTimer(); // Start the survival timer
            Debug.Log("Counter Started");
        }
        else if (scene == "GameOver")
        {
            SurvivalCounter.Instance.endSurvivalTimer(); // Stop and calculate survival time
            Debug.Log("Counter Ended");
        }

        SceneManager.LoadScene(scene); // Load the specified scene
    }
}
