using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingObject : MonoBehaviour
{
    public bool painted = false;      //칠해졌는가

   public void MarkPainted()
    {
        if(!painted)   // 칠해졌다면
        {
            painted = true;
            StageManager.instance.CheckStageClear();
        }
    }
}
