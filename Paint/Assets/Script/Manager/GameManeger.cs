using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManeger : MonoBehaviour
{
    public GameObject clearUI;

    private void Update()
    {
        Checklear();
    }

    void Checklear()
    {
        int count = GameObject.FindGameObjectsWithTag("PaintingObject").Length;

        if(count ==0)
        {
            OnstageClear();
        }
    }

    void OnstageClear()
    {
        Debug.Log("스테이지 클리어");

        if (clearUI !=null)
            clearUI.SetActive(false);
    }
}
