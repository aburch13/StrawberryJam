using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UIPopUp))]
public class UIPopUpEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        UIPopUp popupScript = (UIPopUp)target;

        GUILayout.Space(20);

        if (GUILayout.Button("Pop Up"))
        {
            popupScript.PopUp();
        }

        if (GUILayout.Button("Vanish"))
        {
            popupScript.Vanish();
        }
    }
}
  