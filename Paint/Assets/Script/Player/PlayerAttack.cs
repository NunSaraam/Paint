using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject paintBallPrefab;
    public Transform firePoint;
    public GaugeUI gaugeUI;

    private float minAngle = 10f;
    private float maxAngle = 80f;
    private float aimValue;

    public float chargeSpeed = 1f;
    public float paintBallSpeed = 10f;

    private float gaugeValue = 0f;
    private bool isDragging = false;

    private void Start()
    {
        
    }

    private void Update()
    {
        AttackInput();

    }

    void AttackInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartDrag();
        }
        else if (Input.GetMouseButton(0))
        {
            ContinuneDrag();
        }
        else if (Input.GetMouseButtonUp(0))
        {
                FirePaintBall();
        }
    }

    void StartDrag()
    {
        isDragging = true;
        gaugeValue = 0f;
        gaugeUI.ShowGauge(true);
    }
    
    void ContinuneDrag()
    {
        float mouseY = Input.GetAxis("Mouse Y");

        gaugeValue += mouseY * chargeSpeed * Time.deltaTime;
        gaugeValue = Mathf.Clamp01(gaugeValue);
        gaugeUI.GaugeValue(gaugeValue);             //UI ¿¬°á
    }

    void FirePaintBall()
    {
        isDragging = false;
        gaugeUI.ShowGauge(false);

        float fireAngle = Mathf.Lerp(minAngle, maxAngle, gaugeValue);
        Vector2 direction = Quaternion.Euler(0, 0, fireAngle) * Vector2.right;

        GameObject paintBall = Instantiate(paintBallPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = paintBall.GetComponent<Rigidbody2D>();

        rb.velocity = direction * paintBallSpeed;
    }
}