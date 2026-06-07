using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PlayerBall : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 15f;
    [SerializeField] private float dashForce = 40f;
    [SerializeField] private float dashDuration = 0.2f;
    private Rigidbody rb;
    private bool isDashing = false;
    private Animator anim;

    [Header("Item Settings")]
    private float baseSpeed;
    private bool hasShield = false;
    public GameObject shieldEffect;
    public TMP_Text itemNoticeText;     // 아이템 알림 텍스트
    private Renderer playerRenderer;
    private Color originalColor;

    [Header("UI Settings")]
    public GameObject gameOverUI;
    public GameObject restartButton;
    public Text scoreText;
    public Text finalScoreText;
    public Text highScoreText;

    [Header("Atmosphere Settings")]
    public Color startGroundColor = Color.white;
    public Color endGroundColor = Color.red;
    public float maxDifficultyTime = 60f;

    private float survivedTime = 0f;
    private bool isDead = false;

    private Renderer groundRenderer;
    private Light mainLight;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        baseSpeed = speed;
        playerRenderer = GetComponentInChildren<Renderer>();
        if (playerRenderer != null) originalColor = playerRenderer.material.color;
        if (itemNoticeText != null) itemNoticeText.gameObject.SetActive(false);
        anim = GetComponentInChildren<Animator>();

        GameObject ground = GameObject.Find("Ground");
        if (ground != null) groundRenderer = ground.GetComponent<Renderer>();
        mainLight = GameObject.FindAnyObjectByType<Light>();

        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogDensity = 0.01f;

        Time.timeScale = 1f;
        isDead = false;
        survivedTime = 0f;

        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (restartButton != null) restartButton.SetActive(false);
        if (finalScoreText != null) finalScoreText.gameObject.SetActive(false);

        float best = PlayerPrefs.GetFloat("HighScore", 0f);
        if (highScoreText != null)
            highScoreText.text = "Best: " + best.ToString("F1") + "s";
    }

    void Update()
    {
        if (!isDead)
        {
            survivedTime += Time.deltaTime;
            if (scoreText != null)
                scoreText.text = "Time: " + survivedTime.ToString("F1") + "s";

            float best = PlayerPrefs.GetFloat("HighScore", 0f);
            if (survivedTime > best)
            {
                PlayerPrefs.SetFloat("HighScore", survivedTime);
                PlayerPrefs.Save();
                if (highScoreText != null)
                    highScoreText.text = "Best: " + survivedTime.ToString("F1") + "s";
            }

            UpdateAtmosphere();

            if (anim != null)
            {
                float h = Input.GetAxis("Horizontal");
                float v = Input.GetAxis("Vertical");
                float moveInput = new Vector2(h, v).magnitude;
                anim.SetBool("Walk_Anim", moveInput > 0.1f);
            }

            if (Input.GetKeyDown(KeyCode.Space) && !isDashing)
            {
                SoundManager.Instance?.PlayDash();
                StartCoroutine(Dash());
            }

            if (transform.position.y < -5f) GameOver();
        }
    }

    void UpdateAtmosphere()
    {
        float progress = Mathf.Clamp01(survivedTime / maxDifficultyTime);
        if (groundRenderer != null)
        {
            groundRenderer.material.color = Color.Lerp(startGroundColor, endGroundColor, progress);
        }
        if (mainLight != null)
        {
            mainLight.intensity = Mathf.Lerp(1.5f, 0.4f, progress);
        }
        if (survivedTime > 20f)
        {
            float fogProgress = Mathf.Clamp01((survivedTime - 20f) / (maxDifficultyTime - 20f));
            RenderSettings.fogDensity = Mathf.Lerp(0.01f, 0.08f, fogProgress);
        }
    }

    void FixedUpdate()
    {
        if (isDead || isDashing) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(h, 0, v).normalized;

        if (direction.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.fixedDeltaTime);

            Vector3 moveVelocity = direction * speed;
            moveVelocity.y = rb.linearVelocity.y;
            rb.linearVelocity = moveVelocity;
        }
        else
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 dashDir = new Vector3(h, 0, v).normalized;

        if (dashDir == Vector3.zero) dashDir = transform.forward;

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(dashDir * dashForce, ForceMode.Impulse);

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
    }

    private void HandleCollision(GameObject other)
    {
        if (isDead) return;
        bool isEnemy = false;
        Transform current = other.transform;
        for (int i = 0; i < 5; i++)
        {
            if (current == null) break;
            if (current.CompareTag("Enemy")) { isEnemy = true; break; }
            current = current.parent;
        }
        if (isEnemy)
        {
            if (hasShield)
            {
                hasShield = false;
                if (shieldEffect != null) shieldEffect.SetActive(false);
                return; // 실드로 막음
            }
            TriggerGameOverEffect();
            GameOver();
        }
    }

    void OnCollisionEnter(Collision collision) { HandleCollision(collision.gameObject); }
    void OnTriggerEnter(Collider other) { HandleCollision(other.gameObject); }

    void TriggerGameOverEffect()
    {
        SoundManager.Instance?.PlayEnemyHit();
        CameraShake shake = Camera.main.GetComponent<CameraShake>();
        if (shake != null) StartCoroutine(shake.Shake(0.3f, 0.4f));
    }

    void GameOver()
    {
        if (isDead) return;
        isDead = true;
        SoundManager.Instance?.PlayGameOver();
        SoundManager.Instance?.StopBGM();
        if (gameOverUI != null) gameOverUI.SetActive(true);
        if (restartButton != null) restartButton.SetActive(true);
        if (finalScoreText != null)
        {
            finalScoreText.gameObject.SetActive(true);
            float best = PlayerPrefs.GetFloat("HighScore", 0f);
            finalScoreText.text = "TIME: " + survivedTime.ToString("F1") + "s  ★  BEST: " + best.ToString("F1") + "s";
        }
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SoundManager.Instance?.PlayBGM();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ApplyItem(Item.ItemType itemType, float duration)
    {
        if (itemType == Item.ItemType.SpeedUp)
            StartCoroutine(SpeedUpRoutine(duration));
        else if (itemType == Item.ItemType.Shield)
            StartCoroutine(ShieldRoutine(duration));
    }

    IEnumerator SpeedUpRoutine(float duration)
    {
        speed = baseSpeed * 1.8f;
        if (playerRenderer != null) playerRenderer.material.color = new Color(0f, 1f, 1f);
        ShowItemNotice("⚡ SPEED UP!", new Color(0f, 1f, 1f));
        yield return new WaitForSeconds(duration);
        speed = baseSpeed;
        if (playerRenderer != null) playerRenderer.material.color = originalColor;
        HideItemNotice();
    }

    IEnumerator ShieldRoutine(float duration)
    {
        hasShield = true;
        if (playerRenderer != null) playerRenderer.material.color = new Color(1f, 0.84f, 0f);
        if (shieldEffect != null) shieldEffect.SetActive(true);
        ShowItemNotice("🛡 SHIELD ON!", new Color(1f, 0.84f, 0f));
        yield return new WaitForSeconds(duration);
        if (hasShield)
        {
            hasShield = false;
            if (shieldEffect != null) shieldEffect.SetActive(false);
            if (playerRenderer != null) playerRenderer.material.color = originalColor;
            HideItemNotice();
        }
    }

    void ShowItemNotice(string message, Color color)
    {
        if (itemNoticeText == null) return;
        itemNoticeText.gameObject.SetActive(true);
        itemNoticeText.text = message;
        itemNoticeText.color = color;
    }

    void HideItemNotice()
    {
        if (itemNoticeText != null) itemNoticeText.gameObject.SetActive(false);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SoundManager.Instance?.PlayBGM();
        SceneManager.LoadScene(0);
    }
}
