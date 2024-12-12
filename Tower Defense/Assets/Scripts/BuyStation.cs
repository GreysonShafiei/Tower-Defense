using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyStation : MonoBehaviour
{
    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance; ;
    }

    public void buyBasicTurret()
    {
        gameManager.setBuildTurret(gameManager.basicTurretPrefab);
    }

    public void buyAdvancedTurret()
    {
        gameManager.setBuildTurret(gameManager.AdvancedTurretPrefab);
    }
}
