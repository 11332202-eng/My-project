using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;

    void Update()
    {
        // 保留原本的鍵盤控制 (方便你在電腦測試)
        float h = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * h * Time.deltaTime * 5f);

        // 新增的手機觸控控制
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.position.x < Screen.width / 2)
            {
                // 往左移：請填入你原本左移的邏輯，例如：
                transform.Translate(Vector3.left * Time.deltaTime * 5f);
            }
            else
            {
                // 往右移：請填入你原本右移的邏輯，例如：
                transform.Translate(Vector3.right * Time.deltaTime * 5f);
            }
        }
    }

    // 讓按鈕點擊時呼叫的左移函數
    public void MoveLeft()
    {
        // 這裡的 0.5f 是移動距離，你可以根據需要調整
        transform.Translate(Vector3.left * 0.5f);
    }

    // 讓按鈕點擊時呼叫的右移函數
    public void MoveRight()
    {
        transform.Translate(Vector3.right * 0.5f);
    }
}