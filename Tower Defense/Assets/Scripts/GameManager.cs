using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public TextMeshProUGUI currentCash;
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
    private void Update()
    {
        currentCash.text  = Mathf.Round(cash).ToString();
    }
}
