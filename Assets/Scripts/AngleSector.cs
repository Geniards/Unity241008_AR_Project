using UnityEngine;

public class AngleSector : MonoBehaviour
{
    [SerializeField] private Material sectorMaterial;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = sectorMaterial;
    }

    // 부채꼴 그리기
    public void DrawSector(Vector3 center, float radius, float startAngle, float endAngle, int segments = 100)
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[segments + 2];
        int[] triangles = new int[segments * 3];

        // 첫 번째 점은 부채꼴의 중심점
        vertices[0] = center;

        float angleStep = (endAngle - startAngle) / segments;

        // 부채꼴 외곽선을 구성하는 점들 계산
        for (int i = 0; i <= segments; i++)
        {
            float currentAngle = startAngle + i * angleStep;
            float rad = Mathf.Deg2Rad * currentAngle;

            float x = center.x + radius * Mathf.Cos(rad);
            float z = center.z + radius * Mathf.Sin(rad);
            vertices[i + 1] = new Vector3(x, center.y, z);

            // 삼각형 설정
            if (i < segments)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        // 메쉬 데이터 설정
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }

    public void ClearSector()
    {
        if (meshFilter != null)
        {
            meshFilter.mesh.Clear();
        }
    }
}
