using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPaintGauge : MonoBehaviour
{
    public int maxPaint = 100;
    public int currentPaint;

    [Header("자동 회복 설정")]
    public int recoverAmount = 1;    //회복량
    public float recoverDelay = 0.2f;    //회복 속도
    private float timer = 0f;

    private void Start()
    {
        currentPaint = maxPaint;
    }

    private void Update()
    {
        AutoRecover();
    }

    void AutoRecover()
    {
        if (currentPaint >= maxPaint)
            return;

        timer += Time.deltaTime;

        if (timer >= recoverDelay)
        {
            currentPaint += recoverAmount;
            currentPaint = Mathf.Clamp(currentPaint, 0, maxPaint);

            timer = 0f;
        }
    }

    public bool UsePaint(int amount)
    {
        if (currentPaint < amount )
            return false;

        currentPaint -= amount;
        return true;
    }

    public void AddPaint (int amount)
    {
        currentPaint = Mathf.Clamp(currentPaint + amount, 0, maxPaint);
    }

}
