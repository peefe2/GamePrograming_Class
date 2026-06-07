using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("BGM")]
    public AudioClip bgmClip;
    [Range(0f, 1f)] public float bgmVolume = 0.4f;

    [Header("SFX")]
    public AudioClip dashClip;
    public AudioClip gameOverClip;
    public AudioClip enemyHitClip;
    public AudioClip itemPickupClip;
    [Range(0f, 1f)] public float sfxVolume = 0.8f;

    private AudioSource bgmSource;
    private AudioSource sfxSource;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.volume = bgmVolume;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.volume = sfxVolume;
    }

    void Start()
    {
        PlayBGM();
    }

    public void PlayBGM()
    {
        if (bgmClip == null) return;
        bgmSource.clip = bgmClip;
        bgmSource.Play();
    }

    public void StopBGM() => bgmSource.Stop();

    public void PlayDash()      => PlaySFX(dashClip);
    public void PlayGameOver()  => PlaySFX(gameOverClip);
    public void PlayEnemyHit()  => PlaySFX(enemyHitClip);
    public void PlayItemPickup() => PlaySFX(itemPickupClip);

    private void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip, sfxVolume);
    }
}
