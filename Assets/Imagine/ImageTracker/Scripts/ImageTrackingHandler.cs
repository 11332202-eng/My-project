using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Video;
using System.Collections.Generic;

public class ImageTrackingHandler : MonoBehaviour
{
    [Header("AR 影片 Prefab")]
    public GameObject videoPrefab; // 將剛才做好的影片 Prefab 拖到這裡

    private Dictionary<string, GameObject> spawnedVideos = new Dictionary<string, GameObject>();
    private ARTrackedImageManager arTrackedImageManager;

    void Awake()
    {
        arTrackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        arTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        arTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // 1. 圖片剛被辨識到時
        foreach (var trackedImage in eventArgs.added)
        {
            if (videoPrefab != null)
            {
                // 在圖片的位置與旋轉角度生成影片物件，並設為圖片的子物件（讓影片跟著圖片跑）
                GameObject newVideoObj = Instantiate(videoPrefab, trackedImage.transform);
                newVideoObj.transform.localPosition = Vector3.zero; // 貼齊圖片中心
                // 如果影片躺在地上，可能需要調整旋轉角度，例如：newVideoObj.transform.localRotation = Quaternion.Euler(90, 0, 0);

                spawnedVideos.Add(trackedImage.referenceImage.name, newVideoObj);
            }
        }

        // 2. 圖片持續被追蹤或移動時
        foreach (var trackedImage in eventArgs.updated)
        {
            if (spawnedVideos.ContainsKey(trackedImage.referenceImage.name))
            {
                GameObject videoObj = spawnedVideos[trackedImage.referenceImage.name];
                VideoPlayer videoPlayer = videoObj.GetComponentInChildren<VideoPlayer>();

                if (trackedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
                {
                    videoObj.SetActive(true);

                    // 如果影片暫停了，可以讓它繼續播（依需求調整）
                    if (videoPlayer && !videoPlayer.isPlaying)
                    {
                        // videoPlayer.Play();
                    }
                }
                else
                {
                    // 當相機移開、圖片遺失時，暫停播放或隱藏影片
                    videoObj.SetActive(false);
                }
            }
        }

        // 3. 圖片徹底離開畫面/被移除時
        foreach (var trackedImage in eventArgs.removed)
        {
            if (spawnedVideos.ContainsKey(trackedImage.referenceImage.name))
            {
                Destroy(spawnedVideos[trackedImage.referenceImage.name]);
                spawnedVideos.Remove(trackedImage.referenceImage.name);
            }
        }
    }
}