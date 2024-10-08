using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class MultiImageTracker : MonoBehaviour
{
    // 여러 이미지를 호출하는 방법
    [SerializeField] private ARTrackedImageManager imageManager;
    [SerializeField] private GameObject jojoPrefab;
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private Vector3 prevPos;
    [SerializeField] private Vector3 nextPos;


    private void OnEnable()
    {
        // 트래킹하는 이미지가 활성화시.
        Debug.Log("이미지 활성화 ON");
        imageManager.trackedImagesChanged += OnImageChange;
    }

    private void OnDisable()
    {
        // 트래킹하는 이미지가 비활성화시.
        Debug.Log("이미지 활성화 OFF");
        imageManager.trackedImagesChanged -= OnImageChange;
    }

    private void OnImageChange(ARTrackedImagesChangedEventArgs args)
    {
        // 새로운 이미지가 추적 되었을때.
        foreach (ARTrackedImage trackedImage in args.added)
        {
            // 이미지 라이브러리에서 이미지의 이름을 확인.
            string imageName = trackedImage.referenceImage.name;
            Debug.Log($"새로운 이미지의 이름은 {imageName}");

            Quaternion quaternion = Quaternion.Euler(new Vector3(0, 0, 0));

            // 새로운 게임 오브젝트를 트래킹한 이미지의 자식으로 생성.
            switch (imageName)
            {
                case "Jojo":
                    GameObject jojo = Instantiate(jojoPrefab, trackedImage.transform.position, quaternion);
                    jojo.transform.parent = trackedImage.transform;
                    break;
                case "Enemy":
                    GameObject enemy = Instantiate(enemyPrefab, trackedImage.transform.position, trackedImage.transform.rotation);
                    enemy.transform.parent = trackedImage.transform;
                    break;
            }
        }

        // 기존의 이미지가 변경(이동, 회전) 되었을때
        foreach (ARTrackedImage trackedImage in args.updated)
        {
            Quaternion quaternion = Quaternion.Euler(new Vector3(0, 0, 0));

            Debug.Log($"기존 이미지의 이름은 {trackedImage}");

            //if (prevPos != trackedImage.transform.position)
            {
               // Debug.Log($"기존 이미지의 이전 위치는 {prevPos}");
               // Debug.Log($"기존 이미지의 현재 위치는 {trackedImage.transform.position}");
            }

            // 이미지의 변경사항이 있는 경우 자식으로 있던 게임 오브젝트를 위치와 회전을 갱신
            trackedImage.transform.GetChild(0).position = trackedImage.transform.position;
            trackedImage.transform.GetChild(0).rotation = quaternion;

            //prevPos = trackedImage.transform.position;
        }

        // 기존의 이미지가 사라졌을때
        foreach (ARTrackedImage trackedImage in args.removed)
        {
            // 이미지가 사라진 경우 자식으로 있었던 게임 오브젝트를 삭제.
            Debug.Log($"삭제된 이미지의 이름은 {trackedImage}");
            Destroy(trackedImage.transform.GetChild(0).gameObject);
        }
    }

}
