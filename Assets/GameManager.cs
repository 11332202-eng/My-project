using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("遊戲數據")]
    public int score = 0;
    public int health = 3;

    [Header("UI 介面連結")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI gameOverHighScoreText;
    public GameObject mainMenuPanel;
    public GameObject spawner;
    public GameObject gameOverPanel;

    [Header("音樂播放器")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("背景音樂清單")]
    public AudioClip mainMenuMusic;   // 拖入：natsuyasuminotanken
    public AudioClip gamePlayMusic;   // 拖入：tanoshiimugibatake
    public AudioClip gameOverMusic;   // 拖入：natsuyasuminotanken (或是你準備的其他首)

    [Header("效果音效清單")]
    public AudioClip chiliSound;      // 拖入：maou_se_onepoint31
    public AudioClip heartSound;      // 拖入：maou_se_magical01
    public AudioClip meatSound;       // 拖入：maou_se_system46

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // 初始狀態：開啟選單、關閉生成、時間暫停
        mainMenuPanel.SetActive(true);
        spawner.SetActive(false);
        gameOverPanel.SetActive(false);
        Time.timeScale = 0f;

        // 載入最高分並顯示
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (highScoreText != null) highScoreText.text = "HighScore: " + highScore;

        // 讀取存檔，如果沒存過，預設值給 0.5f (一半音量)
        float savedBGM = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
        float savedSFX = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

        // 套用到播放器
        bgmSource.volume = savedBGM;
        sfxSource.volume = savedSFX;

        // 同步滑桿的顯示位置 (如果有連結的話)
        if (bgmSlider != null) bgmSlider.value = savedBGM;
        if (sfxSlider != null) sfxSlider.value = savedSFX;

        // 【加入】播放主選單音樂
        ChangeBGM(mainMenuMusic);
    }

    public void StartGame()
    {
        score = 0;
        health = 3;
        scoreText.text = "Score: 0";
        lifeText.text = "Life: 3";

        mainMenuPanel.SetActive(false);
        spawner.SetActive(true);
        Time.timeScale = 1f;

        // 【加入】切換到遊戲中音樂
        ChangeBGM(gamePlayMusic);
    }

    public void AddScore()
    {
        score += 10;
        scoreText.text = "Score: " + score;
    }

    public void AddHealth()
    {
        if (health < 3)
        {
            health += 1;
            lifeText.text = "Life: " + health;
        }
    }

    public void TakeDamage()
    {
        health -= 1;
        lifeText.text = "Life: " + health;

        if (health <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Time.timeScale = 0f;
        spawner.SetActive(false);
        gameOverPanel.SetActive(true);
        currentScoreText.text = "CurrentScore: " + score;

        int savedHighScore = PlayerPrefs.GetInt("HighScore", 0);
        if (score > savedHighScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
            savedHighScore = score;
        }
        gameOverHighScoreText.text = "HighScore: " + savedHighScore;

        // 【加入】切換到結束音樂
        ChangeBGM(gameOverMusic);
    }

    public void BackToMenu()
    {
        gameOverPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        spawner.SetActive(false);
        Time.timeScale = 0f;

        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (highScoreText != null) highScoreText.text = "HighScore: " + highScore;

        // 【加入】切換回主選單音樂
        ChangeBGM(mainMenuMusic);
    }

    // 切換背景音樂
    public void ChangeBGM(AudioClip newClip)
    {
        if (newClip == null || bgmSource.clip == newClip) return;
        bgmSource.Stop();
        bgmSource.clip = newClip;
        bgmSource.Play();
    }

    [Header("設定介面連結")]
    public GameObject settingsPanel; // 拖入 SettingsPanel
    public UnityEngine.UI.Slider bgmSlider; // 拖入 BGMSlider
    public UnityEngine.UI.Slider sfxSlider; // 拖入 SFXSlider

    // 開啟設定介面
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        // 確保打開面板時，滑桿跟現在的音量是一致的
        bgmSlider.value = bgmSource.volume;
        sfxSlider.value = sfxSource.volume;
    }

    // 關閉設定介面
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    // 給 BGM 滑桿使用的函數
    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = volume;
        PlayerPrefs.SetFloat("BGMVolume", volume); // 儲存 BGM 音量
    }

    // 給 SFX 滑桿使用的函數
    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume); // 儲存 SFX 音量
    }
}