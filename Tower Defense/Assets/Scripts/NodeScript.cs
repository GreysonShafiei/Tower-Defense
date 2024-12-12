using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class NodeScript : MonoBehaviour
{
    public Color hoverColor;
    public Vector3 positionOffset;

    private GameObject turret;

    private Renderer rend;
    private Color startColor;

    GameManager gameManager;
    void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        gameManager = GameManager.Instance;
    }

    void OnMouseDown()
    {
        if (gameManager.getBuildTurret() == null)
        {
            return;
        }

        GameObject turretToBeBuilt = gameManager.getBuildTurret();

        if (gameManager.cash < gameManager.getTurretCost(turretToBeBuilt))
        {
            Debug.Log("Not Enough Cash!");
            return;
        }

        turret = (GameObject)Instantiate(turretToBeBuilt, transform.position + positionOffset, Quaternion.identity);
        GameManager.Instance.cash -= (int) gameManager.getTurretCost(turretToBeBuilt);
    }

    void OnMouseEnter()
    {
        if (gameManager.getBuildTurret() == null)
        {
            return;
        }
        rend.material.color = hoverColor;
    }

    void OnMouseExit()
    {
        rend.material.color = startColor;
    }
}
