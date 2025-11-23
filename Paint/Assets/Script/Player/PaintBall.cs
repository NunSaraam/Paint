using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBall : MonoBehaviour
{
    [Header("페인트 스플래시 프리팹들 (랜덤)")]
    public GameObject[] paintSplashPrefabs;

    [Header("페인트 색상")]
    public Color paintColor = Color.yellow;

    private float lifeTime = 3f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PaintingObject"))
        {
            CreatePaintSplash(collision);
            Destroy(gameObject);
        }
    }

    void CreatePaintSplash(Collider2D collision)
    {
        if (paintSplashPrefabs.Length == 0)
        {
            Debug.Log("프리팹이 없음");  //페인트 볼
            return;
        }

        // 스플래시 선택
        int index = Random.Range(0, paintSplashPrefabs.Length);

        // 충돌 지점 계산
        Vector2 hitPoint = collision.ClosestPoint(transform.position);

        // 스플래시 생성
        GameObject splash = Instantiate(paintSplashPrefabs[index], hitPoint, Quaternion.identity);

        // 색 적용
        SpriteRenderer sr = splash.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = paintColor;
        }

        // 랜덤 회전 추가
        splash.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));

        // 랜덤 스케일
        float scale = Random.Range(0.8f, 1.2f);
        splash.transform.localScale = new Vector3(scale, scale, 1);

        // 페인트를 충돌한 오브젝트에 붙이기
        splash.transform.SetParent(collision.transform);
    }
}
