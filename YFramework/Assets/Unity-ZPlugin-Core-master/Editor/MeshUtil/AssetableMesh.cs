using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AssetableMesh : MonoBehaviour {
    private void OnEnable() {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = meshFilter.mesh;
        AssetDatabase.CreateAsset(mesh, "Assets/" + mesh.name.Split(' ')[0] + ".asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
