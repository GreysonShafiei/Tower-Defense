using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerList : MonoBehaviour
{
    public static Transform[] towerCoor;

    private void Awake()
    {
        towerCoor = new Transform[transform.childCount];
        for (int i = 0; i < towerCoor.Length; i++)
        {
            towerCoor[i] = transform.GetChild(i);
        }
    }
}
