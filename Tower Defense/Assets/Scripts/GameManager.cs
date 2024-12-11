using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int cash = 500;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Assign this instance to the static variable
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate GameManagers
        }
    }
}
