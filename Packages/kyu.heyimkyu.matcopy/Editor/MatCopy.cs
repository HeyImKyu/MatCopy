using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class MatCopy : UnityEditor.EditorWindow
{
    public GameObject FromAvatar;
    public GameObject ToAvatar;
    public bool IncludeInactive = true;
    public bool NoLengthCheck;

    [UnityEditor.MenuItem("Tools/Kyu/MatCopy")]
    public static void ShowWindow()
    {
        GetWindow<MatCopy>("MatCopy");
    }

    private void OnGUI()
    {
        FromAvatar = (GameObject)EditorGUILayout.ObjectField("From Avatar", FromAvatar, typeof(GameObject), true);
        ToAvatar = (GameObject)EditorGUILayout.ObjectField("To Avatar", ToAvatar, typeof(GameObject), true);
        IncludeInactive = EditorGUILayout.ToggleLeft("Include inactive", IncludeInactive);
        NoLengthCheck = EditorGUILayout.ToggleLeft("Don't check material count", NoLengthCheck);

        if (GUILayout.Button("Do it!"))
            DoIt();
    }

    private void DoIt()
    {
        var fromAvatarMeshRenderers = FromAvatar.GetComponentsInChildren<SkinnedMeshRenderer>(IncludeInactive);
        var toAvatarMeshRenderers = ToAvatar.GetComponentsInChildren<SkinnedMeshRenderer>(true);


        for (int i = 0; i < fromAvatarMeshRenderers.Length; i++)
        {
            SkinnedMeshRenderer fromMeshRenderer = fromAvatarMeshRenderers[i];
            SkinnedMeshRenderer toMeshRenderer;
            if (toAvatarMeshRenderers.Length > i && toAvatarMeshRenderers[i].name == fromAvatarMeshRenderers[i].name)
                toMeshRenderer = toAvatarMeshRenderers[i];
            else
                toMeshRenderer = toAvatarMeshRenderers.FirstOrDefault(x => x.name == fromMeshRenderer.name);

            if (toMeshRenderer == null)
                continue;

            if (NoLengthCheck || toMeshRenderer.sharedMaterials.Length == fromMeshRenderer.sharedMaterials.Length)
            {
                toMeshRenderer.sharedMaterials = fromMeshRenderer.sharedMaterials;
            }
        }
    }
}
