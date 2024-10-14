using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AngleObj : MonoBehaviour
{
    [SerializeField] private List<Transform> markerList = new List<Transform>();
    [SerializeField] private LineRenderer lineRenderer1;
    [SerializeField] private LineRenderer lineRenderer2;
    [SerializeField] private TextMesh angleTextMesh;
    [SerializeField] private Canvas worldSpaceCanvas;
    [SerializeField] private Button deleteButtonPrefab;
    public Transform mainCamTransform;

    private Button deleteButton;
    private int markerIndex = 0;

    // 첫 번째 마커 설정
    public void SetInit(Vector3 pos)
    {
        markerList[0].transform.position = pos;
        lineRenderer1.SetPosition(0, pos);
        markerIndex = 1;
    }
    // 마커 위치 설정 (두 번째, 세 번째 마커)
    public void SetMarkerPosition(Vector3 pos)
    {
        if (markerIndex == 1)
        {
            markerList[1].transform.position = pos;
            // 첫 번째 직선 그리기
            lineRenderer1.SetPosition(1, pos);

            // 두 번째 직선도 시작 (2-3번 마커 직선)
            lineRenderer2.SetPosition(0, pos);

            markerIndex = 2;
        }
        else if (markerIndex == 2)
        {
            markerList[2].transform.position = pos;
            // 두 번째 직선 완성
            lineRenderer2.SetPosition(1, pos);
            markerIndex++;

            CalculateAngle();
            CreateDeleteButton();
        }
    }
    public int GetMarkerCount()
    {
        return markerIndex;
    }

    // 각도 계산
    private void CalculateAngle()
    {
        // 두벡터 계산
        Vector3 dirA = markerList[1].position - markerList[0].position;
        Vector3 dirB = markerList[2].position - markerList[1].position;

        float angle = Vector3.Angle(dirA, dirB);
        angleTextMesh.text = $"{angle:F2}°";

        // 각도 텍스트 위치 설정
        angleTextMesh.transform.position = markerList[1].position;

        Vector3 directionToCamera = (mainCamTransform.position - angleTextMesh.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToCamera);
        angleTextMesh.transform.rotation = lookRotation * Quaternion.Euler(0, 180f, 0);

    }

    // 삭제 버튼 생성
    private void CreateDeleteButton()
    {
        if (deleteButtonPrefab != null && worldSpaceCanvas != null)
        {
            deleteButton = Instantiate(deleteButtonPrefab, worldSpaceCanvas.transform);
            deleteButton.transform.position = markerList[1].position + new Vector3(0.05f, 0, 0);

            // 삭제 버튼도 카메라를 바라보도록 설정
            Vector3 directionToCamera = (mainCamTransform.position - deleteButton.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(directionToCamera);
            deleteButton.transform.rotation = lookRotation * Quaternion.Euler(0, 180f, 0);

            deleteButton.GetComponent<Button>().onClick.AddListener(DeleteAngleObj);
        }
    }

    // 각도 객체 삭제
    public void DeleteAngleObj()
    {
        Debug.Log("AngleObj 삭제");
        Destroy(lineRenderer1.gameObject);
        Destroy(lineRenderer2.gameObject);
        Destroy(angleTextMesh.gameObject);
        Destroy(deleteButton.gameObject);
        Destroy(this.gameObject);
    }
}


