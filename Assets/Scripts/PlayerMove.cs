using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    // 이동
    public float speed = 5f;
    Vector2 moveVec;
    float angle;
    Vector2 target, mouse;

    // 조명
    public GameObject flashlight;
    private int currentLight;

    public float battery = 50f;
    public float maxBattery = 50f;

    // 스탯
    public int hp = 5;
    public int maxHp = 5;

    // UI 
    public Slider batterySlider;
    public Text HpUI;

    // 넉백 관련
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.2f;
    private bool isKnockback = false;

    // 컴포
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentLight = flashlight.GetComponent<Flashlight>().changeFlashlight(0);

        //target = transform.position;

        battery = maxBattery;

        if (batterySlider != null)
        {
            batterySlider.minValue = 0;
            batterySlider.maxValue = maxBattery;
            batterySlider.value = battery;
        }
    }

    void Update()
    {
        Look();
        // 배떠리 추가
        if (flashlight.activeSelf && battery > 0)
        {
            battery -= Time.deltaTime;
            battery = Mathf.Max(0, battery);
        }

        if (battery <= 0 && flashlight.activeSelf)
        {
            flashlight.SetActive(false);
        }

        if (batterySlider != null)
        {
            batterySlider.value = battery;
        }
    }    

    private void FixedUpdate()
    {
        if (!isKnockback) // 넉백 중일 땐 입력 무시
        {
            Move();
        }
    }

    void Move()
    {
        rb.linearVelocity = moveVec * speed;
    }

    private void Look()
    {
        mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        angle = Mathf.Atan2(mouse.y - target.y, mouse.x - target.x) * Mathf.Rad2Deg;
        flashlight.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

    // 충돌처리관련

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !isKnockback)
        {
            UpdateHp(-1);

            if (hp <= 0)
            {
                gameObject.SetActive(false);
            }
            else
            {
                // 넉백 실행
                Vector2 dir = (rb.position - (Vector2)collision.transform.position).normalized;
                StartCoroutine(Knockback(dir));
            }
        }
    }

    void UpdateHp(int value)
    {
        hp = Mathf.Min(hp + value, maxHp);
        string str = "";
        for(int i= 0; i < hp; i++)
        {
            str += '♡';
        }
        HpUI.text = str;

    }

    System.Collections.IEnumerator Knockback(Vector2 direction)
    {
        isKnockback = true;

        rb.linearVelocity = Vector2.zero; // 기존 속도 초기화
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockbackDuration);

        isKnockback = false;
    }

    //---인풋 콜백-

    void OnMove(InputValue value)
    {
        moveVec = value.Get<Vector2>().normalized;
    }

    public void OnKey1(InputValue value)
    {
        currentLight = flashlight.GetComponent<Flashlight>().changeFlashlight(0);
    }

    public void OnKey2(InputValue value)
    {
        currentLight = flashlight.GetComponent<Flashlight>().changeFlashlight(1);
    }

    public void OnKey3(InputValue value)
    {
        currentLight = flashlight.GetComponent<Flashlight>().changeFlashlight(2);
    }

    public void OnOnOff(InputValue value)
    {
        if (battery >= 0 && !flashlight.activeSelf) 
        {
            flashlight.SetActive(true);
        }
        else if (flashlight.activeSelf)
        {
            flashlight.SetActive(false);
        }
    }

}
