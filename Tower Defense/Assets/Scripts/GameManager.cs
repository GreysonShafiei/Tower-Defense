using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public TextMeshProUGUI currentCash;
    public TextMeshProUGUI health;
    public int cash = 500;
    EndNodeScript endNodeScript;

    public GameObject basicTurretPrefab;
    public GameObject AdvancedTurretPrefab;
    private GameObject turretToBeBuilt;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate GameManagers
        }
    }
    private void Update()
    {
        currentCash.text  = Mathf.Round(cash).ToString();
        if (EndNodeScript.Instance != null)
        {
            health.text = Mathf.Round(EndNodeScript.Instance.health).ToString();
        }
    }

    public GameObject getBuildTurret()
    {
        return turretToBeBuilt;
    }

    public float getTurretCost(GameObject turret)
    {
        Turret turretComponent = turret.GetComponent<Turret>();
        if (turretComponent != null)
        {
            return turretComponent.cost;
        }
        else
        {
            Debug.LogError("The provided GameObject does not have a Turret component.");
            return 0f; // Return 0 or handle the error as needed
        }
    }

    public void setBuildTurret(GameObject turret)
    {
        Debug.Log("Turret has been set");
        turretToBeBuilt = turret;
    }
}
