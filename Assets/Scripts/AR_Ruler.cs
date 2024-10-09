using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class AR_Ruler : MonoBehaviour
{
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private GameObject startMarkerPrefab;
    [SerializeField] private GameObject endMarkerPrefab;
    [SerializeField] private GameObject focusPointPrefab;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Button measureButton;


    [SerializeField] private GameObject startMarker;
    [SerializeField] private GameObject endMarker;
    [SerializeField] private GameObject focusPoint;

    [SerializeField] private Text distanceText;
    [SerializeField] private GameObject distanceTextPrefab;
    [SerializeField] private GameObject distanceTextObject;

    [SerializeField] private Vector3 startPoint;
    [SerializeField] private Vector3 endPoint;
    [SerializeField] private bool isSearching;
    [SerializeField] private bool firstPointSet;

    private void Start()
    {
        // 버튼 클릭시 마커 설정 및 거리 측정 수행
        measureButton.onClick.AddListener(OnMeasureButton);

        focusPoint = Instantiate(focusPointPrefab);
    }

    private void Update()
    {
        RaycastFromCameraCenter();
    }

    // 카메라 중앙에서 물체와의 간격 측정 메서드
    private void RaycastFromCameraCenter()
    {
        // 화면 중앙위치
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        if(raycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            // 초점 오브젝트의 위치를 감지된 평면으로 이동
            focusPoint.transform.position = hitPose.position;
            focusPoint.transform.rotation = hitPose.rotation;

            // 실시간으로 카메라와 Raycast에 해당하는 평면과의 거리계산
            float distanceToPlane = Vector3.Distance(Camera.main.transform.position, hitPose.position);
            distanceText.text = $"해당 목표물과의 거리는 {distanceToPlane:F2}m 입니다.";
        }
        else
        {
            distanceText.text = "평면을 감지하지 못했습니다.";
        }
    }

    // 마커 배치 및 거리 측정 메서드
    private void OnMeasureButton()
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        if (raycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            if(!firstPointSet)
            {
                startPoint = hitPose.position;
                if(startMarker) Destroy(startMarker);
                startMarker = Instantiate(startMarkerPrefab, startPoint, Quaternion.identity);
                Debug.Log($"첫번째 마크의 위치 : {startPoint}");
                // 첫번째 포인트 설정.
                firstPointSet = true;
            }
            else
            {
                endPoint = hitPose.position;
                if(endMarker) Destroy(endMarker);
                endMarker = Instantiate(endMarkerPrefab, endPoint, Quaternion.identity);
                Debug.Log($"두번째 마크의 위치 : {endPoint}");

                float distanceToPoint = Vector3.Distance(startPoint, endPoint);
                distanceText.text = $"두 마커와의 거리는 {distanceToPoint}m 입니다.";
                Debug.Log($"두 마커와의 거리는 : {distanceToPoint}");

                // 두 마커를 이어주는 직선 그리기
                DrawLineBetweenPoints();

                // 중간 지점에 거리 표시 텍스트 생성
                PlaceDistanceText(distanceToPoint);

                // 재측정 가능하게 첫번째포인트 리셋
                firstPointSet = false;
            }
        }
    }

    // 두 지점을 이어주는 직선 그리기
    private void DrawLineBetweenPoints()
    {
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2; // 두 지점으로 구성된 직선
            lineRenderer.SetPosition(0, startPoint); // 첫 번째 점
            lineRenderer.SetPosition(1, endPoint); // 두 번째 점
        }
    }

    // 중간 지점에 거리 표시 텍스트 배치
    private void PlaceDistanceText(float distance)
    {
        Vector3 middlePoint = (startPoint + endPoint) / 2;

        if (distanceTextObject) Destroy(distanceTextObject);

        distanceTextObject = Instantiate(distanceTextPrefab, middlePoint, Quaternion.identity);

        // 텍스트 내용 설정
        Text textComponent = distanceTextObject.GetComponentInChildren<Text>();
        if (textComponent != null)
        {
            textComponent.text = $"{distance:F2} m";
        }

        // 카메라를 향하도록 텍스트 회전 (텍스트가 항상 카메라를 바라보도록)
        distanceTextObject.transform.LookAt(Camera.main.transform);
        distanceTextObject.transform.Rotate(0, 180, 0); // 텍스트가 반대로 뒤집히지 않도록 180도 회전
    }
}
