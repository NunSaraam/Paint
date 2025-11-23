using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{

    public static StageManager instance;

    PaintingObject[] paintingObjects;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        paintingObjects = FindObjectsOfType<PaintingObject>();
    }

    public void CheckStageClear()
    {
        foreach(var obj  in paintingObjects)
        {
            if (!obj.painted)
                return;    //아직 안 칠한 오브젝트 있음
        }

         Debug.Log("스테이지 클리어");
    }

}
