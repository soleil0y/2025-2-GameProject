using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("체력 설정")]
    public float maxHp = 30f;
    public float dps = 10f;

    private SpriteRenderer sr;

    private float currentHp;

    private bool isDying = false;

    void Start()
    {
        isDying = false;
        currentHp = maxHp;

        sr = GetComponent<SpriteRenderer>();
        sr.color = Color.black; // 초기화
    }


    void Update()
    {
        if (!isDying)
        {
            if (currentHp <= 0)
            {
                StartCoroutine(DieEffect());
            }
            else
            {
                UpdateColor();
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!isDying && other.CompareTag("Flashlight"))
        {
            currentHp -= dps * Time.deltaTime * StageManager.Instance.flashlightDamage; // 일단편한대로...
            currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        }
    }

    void UpdateColor()
    {
        if (sr != null)
        {
            float t = 1f - (currentHp / maxHp);
            sr.color = Color.Lerp(Color.black, Color.white, t);
        }
    }

    IEnumerator DieEffect()
    {
        isDying = true;

        float duration = 0.4f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float intensity = 1f + Mathf.Sin(elapsed * 20f) * 0.5f;
            sr.color = Color.white * intensity;

            yield return null;
        }

        // 스테매니
        if (StageManager.Instance != null)
        {
            StageManager.Instance.EnemyDefeated();
        }

        Destroy(gameObject);
    }
}