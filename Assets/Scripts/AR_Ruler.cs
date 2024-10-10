using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class AR_Ruler : MonoBehaviour
{
    [Header("AR 세팅")]
    [SerializeField] private ARRaycastManager raycastManager;
    private static List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();
    [SerializeField] private Vector2 screenCenter;

    [Header("Ruler 세팅")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform pivot;
    [SerializeField] private Transform rulerPool;
    [SerializeField] private GameObject rulerObj;
    [SerializeField] private Text distanceText;
    [SerializeField] private GameObject deleteButtonPrefab;
    [SerializeField] private Canvas worldSpaceCanvas;

    private RulerObj activeRulerObj;
    private List<RulerObj> rulerObjList = new List<RulerObj>();
    private bool rulerEnable;
    private Vector3 lastRulerPos;


    private void Start()
    {
        screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
    }

    private void Update()
    {
        raycastHits.Clear();
        if (raycastManager.Raycast(screenCenter, raycastHits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = raycastHits[0].pose;
            rulerEnable = true;
            lastRulerPos = hitPose.position;
            pivot.rotation = Quaternion.Lerp(pivot.rotation, hitPose.rotation, 0.2f);

            // 카메라와 평면 사이의 거리 계산
            float distance = Vector3.Distance(cameraTransform.position, hitPose.position);
            distanceText.text = $"거리: {distance.ToString("F2")} m";


            if (activeRulerObj)
            {
                activeRulerObj.SetObj(hitPose.position);
            }
        }
        else
        {
            rulerEnable = false;
            Quaternion textRotation = Quaternion.Euler(90f, 0f, 0f);
            pivot.rotation = Quaternion.Lerp(pivot.rotation, textRotation, 0.5f);

            // 평면을 찾지 못한 경우 거리 텍스트 초기화
            distanceText.text = "평면을 찾고 있습니다...";
        }
    }

    public void MakeRulerMarker()
    {
        if (rulerEnable)
        {
            if (activeRulerObj == null)
            {
                GameObject obj = Instantiate(rulerObj) as GameObject;
                obj.transform.SetParent(rulerPool);
                obj.transform.position = Vector3.zero;
                obj.transform.localScale = Vector3.one;

                RulerObj rulerObjs = obj.GetComponent<RulerObj>();
                rulerObjs.mainCamTransform = cameraTransform;
                rulerObjs.SetInit(lastRulerPos);
                rulerObjList.Add(rulerObjs);
                activeRulerObj = rulerObjs;

                GameObject deleteButton = Instantiate(deleteButtonPrefab, worldSpaceCanvas.transform);
                deleteButton.transform.position = rulerObjs.transform.position + new Vector3(0.01f, 0.01f, 0);
                deleteButton.GetComponent<Button>().onClick.AddListener(() => DeleteRulerMarker(rulerObjs, deleteButton));
            }
            else
            {
                activeRulerObj = null;
            }
        }
    }

    // RulerObj 삭제 메서드
    private void DeleteRulerMarker(RulerObj rulerObj, GameObject deleteButton)
    {
        rulerObjList.Remove(rulerObj);
        Destroy(rulerObj.gameObject);
        Destroy(deleteButton);
    }
}
