using System;
using System.Linq;
using PurpleFlameCode.Others;
using TMPro;
using UnityEngine;

public class TMP_Repeat : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] Gradient grad;
    [SerializeField] Vector3 offset;
    [SerializeField] int repetitions;
#pragma warning restore 0649
    TMP_Text _textComponent;
    TMP_Text textComponent
    {
        get
        {
            if (_textComponent == null)
                _textComponent = GetComponent<TMP_Text>();
            return _textComponent;
        }
    }

    TMP_MeshInfo[] originalMeshInfo;
    void Start()
    {
        originalMeshInfo = new TMP_MeshInfo[textComponent.textInfo.meshInfo.Length];
        Array.Copy(textComponent.textInfo.meshInfo, originalMeshInfo, textComponent.textInfo.meshInfo.Length);
        UpdateMesh();
    }

    void Update()
    {
        UpdateMesh();
    }

    void UpdateMesh()
    {
        textComponent.textInfo.meshInfo = new TMP_MeshInfo[originalMeshInfo.Length]; 
        Array.Copy(originalMeshInfo, textComponent.textInfo.meshInfo, originalMeshInfo.Length);
        textComponent.ForceMeshUpdate();
        
        TMP_TextInfo textInfo = textComponent.textInfo;

        Vector2[] uvs0 = new Vector2[0];
        Vector2[] uvs2 = new Vector2[0];
        Vector3[] normals = new Vector3[0];
        Vector4[] tangents = new Vector4[0];
        int[] triangles = new int[0];
        Vector3[] vertices = new Vector3[0];
        Color32[] colors32 = new Color32[0];
        
        for (int repetition = 0; repetition < repetitions; repetition++)
        {
            Color32 repetitionColor = grad.Evaluate((repetition - 1)/(float)repetitions);
            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                int verticesLengthBeforeAdd = vertices.Length;
                vertices = vertices.AppendArray(textInfo.meshInfo[i].vertices.Select(v => v + offset * repetition)
                    .ToArray());
                uvs0 = uvs0.AppendArray(textInfo.meshInfo[i].uvs0);
                uvs2 = uvs2.AppendArray(textInfo.meshInfo[i].uvs2);
                normals = normals.AppendArray(textInfo.meshInfo[i].normals);
                tangents = tangents.AppendArray(textInfo.meshInfo[i].tangents);
                
                if(repetition == 0)
                    colors32 = colors32.AppendArray(textInfo.meshInfo[i].colors32);
                else
                    colors32 = colors32.AppendArray(textInfo.meshInfo[i].colors32.Select(v => repetitionColor).ToArray());
                
                triangles = triangles.AppendArray(textInfo.meshInfo[i].triangles
                    .Select(v => v + verticesLengthBeforeAdd).ToArray());
            }
        }


        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            TMP_MeshInfo meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = vertices;
            meshInfo.mesh.triangles = RevertTriangleOrder(triangles);
            meshInfo.mesh.uv = uvs0;
            meshInfo.mesh.uv2 = uvs2;
            meshInfo.mesh.normals = normals;
            meshInfo.mesh.tangents = tangents;
            meshInfo.mesh.colors32 = colors32;
            textComponent.UpdateGeometry(meshInfo.mesh, i);
        }
    }

    int[] RevertTriangleOrder(int[] triangles)
    {
        int[] newTriangles = new int[triangles.Length];
        for (int i = 0; i < triangles.Length/3; i++)
        {
            int a = triangles[i * 3 + 0];
            int b = triangles[i * 3 + 1];
            int c = triangles[i * 3 + 2];
            newTriangles[triangles.Length - 3 - (i * 3) + 0] = a;
            newTriangles[triangles.Length - 3 - (i * 3) + 1] = b;
            newTriangles[triangles.Length - 3 - (i * 3) + 2] = c;
        }

        return newTriangles;
    }
    
}
