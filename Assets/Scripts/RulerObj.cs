using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RulerObj : MonoBehaviour
{
    [SerializeField] private List<Transform> objList = new List<Transform>();
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform textObj;
    [SerializeField] private TextMesh text;
    public Transform mainCamTransform;

    [SerializeField] private Canvas worldSpaceCanvas;
    [SerializeField] private Button deleteButtonPrefab;
    private Button deleteButton;

    public void SetInit(Vector3 pos)
    {
        objList[0].transform.position = pos;
        lineRenderer.SetPosition(0,pos);

        CreateDeleteButton();
    }

    public void SetObj(Vector3 pos)
    {
        objList[1].transform.position = pos;
        lineRenderer.SetPosition(1, pos);
    }

    private void Update()
    {
        Vector3 textDirVec = objList[1].position - objList[0].transform.position;
        textObj.position = objList[0].position + textDirVec * 0.5f;

        float textDistance = textDirVec.magnitude;
        string textDistanceText = string.Format($"{textDistance:F2}m");
        text.text = textDistanceText;

        textObj.LookAt(mainCamTransform);

        // Delete Button의 위치를 지속적으로 갱신
        if (deleteButton != null)
        {
            deleteButton.transform.position = textObj.position + new Vector3(0.05f, 0, 0);
            deleteButton.transform.LookAt(mainCamTransform);
            Debug.Log($"업데이트 회전 {deleteButton.transform.rotation}");

        }
    }

    // 삭제 버튼을 생성하고 배치
    private void CreateDeleteButton()
    {
        if (deleteButtonPrefab != null && mainCamTransform != null)
        {
            deleteButton = Instantiate(deleteButtonPrefab);
            Debug.Log($"회전 {worldSpaceCanvas.transform.rotation}");
            //deleteButton.GetComponent<Button>().onClick.AddListener(() => DeleteRulerMarker());
        }
    }

    // 삭제 로직
    public void DeleteRulerMarker()
    {
        Debug.Log("삭제");
        Destroy(deleteButton);
        Destroy(this.gameObject);
    }
}
