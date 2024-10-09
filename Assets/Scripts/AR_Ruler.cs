using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class AR_Ruler : MonoBehaviour
{
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private GameObject startMarkerPrefab;
    [SerializeField] private GameObject endMarkerPrefab;
    [SerializeField] private LineRenderer lineRenderer;

    [SerializeField] private GameObject startMarker;
    [SerializeField] private GameObject endMarker;

    [SerializeField] private Text distanceText;
    [SerializeField] private Vector3 startPoint;
    [SerializeField] private Vector3 endPoint;
    [SerializeField] private bool isSearching;

    private void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // 터치 시작
            if(touch.phase == TouchPhase.Began)
            {
                SetPoint(touch.position, true);
            }
            // 터치 끝
            else if(touch.phase == TouchPhase.Ended)
            {
                SetPoint(touch.position, false);
            }
        }

        if(isSearching)
        {
            PointDistance(startPoint, endPoint);
            DrawLineBetweenPoints();
        }
    }

    private void SetPoint(Vector2 touchPos, bool isStartPoint)
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        if(raycastManager.Raycast(touchPos, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            if(isStartPoint)
            {
                startPoint = hitPose.position;
                isSearching = false;

                if (startMarker != null) Destroy(startMarker);
                startMarker = Instantiate(startMarkerPrefab, startPoint, Quaternion.identity);

            }
            else
            {
                endPoint = hitPose.position;
                isSearching = true;

                if (endMarker != null) Destroy(endMarker);
                endMarker = Instantiate(endMarkerPrefab, endPoint, Quaternion.identity);

            }
        }
    }

    private void PointDistance(Vector3 startPos, Vector3 endPos)
    {
        float distance = Vector3.Distance(startPos,endPos);
        distanceText.text = $"거리는 : {distance:F2}m 입니다.";
    }

    private void DrawLineBetweenPoints()
    {
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.SetPosition(1, endPoint);
        }
    }
}
