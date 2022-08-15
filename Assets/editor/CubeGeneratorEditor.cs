using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldGenerator))]
public class CubeGeneratorEditor : UnityEditor.Editor
{
	FieldGenerator _script;
	public override void OnInspectorGUI()
	{
		DrawBottonDie();

		DrawDefaultInspector();

		EditorGUILayout.LabelField("                              ---==  Übersicht  ==---");
	}
	private void DrawBottonDie()
	{
		if (GUILayout.Button("Generate Field"))
		{
			_script.GeneratePrefaps();
		}
	}
    private void OnEnable()
    {
		_script = target as FieldGenerator;
	}

}
