using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RPGSystem
{
    public class ItemTypeEditor : EditorWindow
    {
        Dictionary<string, int> _enum = new Dictionary<string, int>();
        string enumTypeName;
        int enumTypeValue;

        [MenuItem("Window/Item Types")]
        public static void ShowWindow()
        {
            GetWindow<ItemTypeEditor>("Item Types");
        }
        private void OnGUI()
        {
            GUILayout.Space(10);

            foreach (KeyValuePair<string, int> value in _enum)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(20);
                EditorGUILayout.TextField(value.Key, GUILayout.Width(150));
                EditorGUILayout.IntField(value.Value, GUILayout.Width(150));
                EditorGUILayout.EndHorizontal();
            }

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            enumTypeName = EditorGUILayout.TextField("", enumTypeName, GUILayout.Width(150));
            enumTypeValue = EditorGUILayout.IntField("Bit Mask", enumTypeValue); 
            if (GUILayout.Button("+"))
            {
                try
                {
                    _enum.Add(enumTypeName, enumTypeValue);
                }
                catch (System.Exception ex)
                {

                }   
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);
            if (GUILayout.Button("Update"))
            {
                System.IO.StreamWriter writer = new System.IO.StreamWriter("Assets/Scripts/RPGSystem/Items/ItemTypes.cs");
                writer.WriteLine("namespace RPGSystem");
                writer.WriteLine("{");
                writer.WriteLine("\tpublic enum ItemTypes");
                writer.WriteLine("\t{");
                foreach (KeyValuePair<string, int> value in _enum)
                    writer.WriteLine("\t\t" + value.Key + " = " + value.Value + ",");
                writer.WriteLine("\t}");
                writer.WriteLine("}");
            }
        }
    }
}
