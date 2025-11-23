using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject[] paintBallPrefab;
    public Transform firePoint;
    public Transform effectPoint;
    public GaugeUI gaugeUI;
    public GameObject[] shotEffect;
    public PlayerPaintGauge paintGauge;

    private float minAngle = 10f;
    private float maxAngle = 80f;

    public float chargeSpeed = 1f;
    public float paintBallSpeed = 10f;

    private GameObject currentPaintBall;
   [SerializeField] private ParticleSystem currentEffect;

    private int effentIndex = 0;
    private float gaugeValue = 0f;
    private bool isDragging = false;

    public int paintCost = 10;


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
            TrtFire();
        }
    }

    void StartDrag()
    {
        isDragging = true;
        gaugeValue = 0f;
        gaugeUI.ShowGauge(true);
        SetRandomPanitBall();
    }
    
    void ContinuneDrag()
    {
        float mouseY = Input.GetAxis("Mouse Y");

        gaugeValue += mouseY * chargeSpeed * Time.deltaTime;
        gaugeValue = Mathf.Clamp01(gaugeValue);
        gaugeUI.GaugeValue(gaugeValue);             //UI 연결
    }

    void SetRandomPanitBall()
    {
        int index = Random.Range(0, paintBallPrefab.Length);
        currentPaintBall = paintBallPrefab[index];

        if (index >= 0 && index < shotEffect.Length && shotEffect[index] != null)
        {
            effentIndex = index;
            GameObject effectObj = Instantiate(shotEffect[effentIndex], effectPoint.position, Quaternion.identity);

            currentEffect = effectObj.GetComponent<ParticleSystem>();
            currentEffect.Pause();

        }
        else
        {
            Debug.LogWarning("선택된 shotEffect 가 null 이거나 범위를 벗어났습니다. 인스펙터를 확인하세요.");
            currentEffect = null;
        }
    }


    void TrtFire() //페인트 체크
    {
        if (paintGauge.UsePaint(paintCost))
        {
            FirePaintBall();
        }
        else
        {
            NotEnoughPaint();
        }
    }

    void FirePaintBall()
    {
        isDragging = false;
        gaugeUI.ShowGauge(false);

        float fireAngle = Mathf.Lerp(minAngle, maxAngle, gaugeValue);
        Vector2 direction = Quaternion.Euler(0, 0, fireAngle) * Vector2.right;

        GameObject paintBall = Instantiate(currentPaintBall, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = paintBall.GetComponent<Rigidbody2D>();

        rb.velocity = direction * paintBallSpeed;

        if (currentEffect != null)
        {


            currentEffect.transform.position = effectPoint.position;
            currentEffect.Play();

            Destroy(currentEffect.gameObject, 1f);

          

        }
    }

    void  NotEnoughPaint()     //페인트 부족
    {
        Debug.Log("페인트 부족");
            gaugeUI.ShowGauge(false);
    }
}