using TMPro;
using UnityEngine;

public class TMP_Wobble : MonoBehaviour
{
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

    void Update()
    {
        textComponent.ForceMeshUpdate();
        TMP_TextInfo textInfo = textComponent.textInfo;
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible)
            {
                continue;
            }

            Vector3[] verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
            for (int j = 0; j < 4; j++)
            {
                Vector3 orig = verts[charInfo.vertexIndex + j];
                verts[charInfo.vertexIndex + j] = orig + new Vector3( Mathf.Cos(Time.time*3f)*5f, Mathf.Sin(Time.time*3f)*10f,0);
            }
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            TMP_MeshInfo meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            textComponent.UpdateGeometry(meshInfo.mesh, i );
        }
    }
}
