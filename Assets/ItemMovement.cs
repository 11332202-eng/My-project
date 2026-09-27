using UnityEngine;

public class ItemMovement : MonoBehaviour
{
    public float speed = 2.0f;

    void Update()
    {
        // 沿著父物件的「下方」移動
        transform.Translate(Vector3.down * Time.deltaTime * speed);

        // 掉太遠就刪除
        if (transform.localPosition.y < -15f)
        {
            Destroy(gameObject);
        }
    }
}