using UnityEngine;

public class ItemLogic : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            if (gameObject.CompareTag("Bad")) // 辣椒
            {
                GameManager.instance.TakeDamage();
                // 播放辣椒音效
                GameManager.instance.sfxSource.PlayOneShot(GameManager.instance.chiliSound);
            }
            else if (gameObject.CompareTag("Good")) // 愛心
            {
                GameManager.instance.AddHealth();
                // 播放愛心音效
                GameManager.instance.sfxSource.PlayOneShot(GameManager.instance.heartSound);
            }
            else // 肉片
            {
                GameManager.instance.AddScore();
                // 播放肉片音效
                GameManager.instance.sfxSource.PlayOneShot(GameManager.instance.meatSound);
            }
            Destroy(gameObject);
        }
        else if (other.name == "RedLine")
        {
            // 只有肉片（沒標籤的）掉下去才扣血
            if (!gameObject.CompareTag("Bad") && !gameObject.CompareTag("Good"))
            {
                GameManager.instance.TakeDamage();
                // 既然是扣血，也可以播辣椒那個受傷音效，或者你有準備「失敗音效」
                GameManager.instance.sfxSource.PlayOneShot(GameManager.instance.chiliSound);
            }
            Destroy(gameObject);
        }
    }
}