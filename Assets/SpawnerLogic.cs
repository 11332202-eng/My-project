using UnityEngine;

public class SpawnerLogic : MonoBehaviour
{
    public GameObject[] itemPrefabs;
    public float spawnRate = 1.5f;
    private float[] lanes = { -1f, 0f, 1f }; // 對應左中右三條路

    void OnEnable()
    {
        // 確保計時器在 AR 掃描到圖後啟動
        CancelInvoke("SpawnItem");
        InvokeRepeating("SpawnItem", 1.0f, spawnRate);
    }

    void SpawnItem()
    {
        if (itemPrefabs == null || itemPrefabs.Length == 0) return;

        // 隨機選一個物件和位置
        int randomIndex = UnityEngine.Random.Range(0, lanes.Length);
        int prefabIndex = UnityEngine.Random.Range(0, itemPrefabs.Length);

        // 1. 生成物件，並直接設為 Spawner 的子物件
        GameObject newItem = Instantiate(itemPrefabs[prefabIndex], transform);

        // 2. 重設位置：讓它生在 Spawner 的上方，Z 軸務必是 0
        newItem.transform.localPosition = new Vector3(lanes[randomIndex], 5.0f, 0f);

        // 3. 【最重要的視覺修正】強制放大縮放比例
        // 如果原本看不見，這裡直接設定為 50, 50, 50 看看會不會出現巨大的物體
        newItem.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        // 先找有沒有，沒有才加；但不管有沒有，都重新設定速度
        ItemMovement moveScript = newItem.GetComponent<ItemMovement>();
        if (moveScript == null)
        {
            moveScript = newItem.AddComponent<ItemMovement>();
        }
        moveScript.speed = 4f; // 在這裡填入你想要的新速度
    }
}