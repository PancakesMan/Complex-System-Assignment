using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RPGSystem
{
    [System.Serializable]
    public class SerializableDictionary : Dictionary<string, int>, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<string> keys = new List<string>();

        [SerializeField]
        private List<int> values = new List<int>();

        // save the dictionary to lists
        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();

            if (Count > 0)
                foreach (KeyValuePair<string, int> pair in this)
                {
                    keys.Add(pair.Key);
                    values.Add(pair.Value);
                }
        }

        // load dictionary from lists
        public void OnAfterDeserialize()
        {
            this.Clear();

            if (keys.Count != values.Count)
                throw new System.Exception("An error occurred during serialization");

            for (int i = 0; i < keys.Count; i++)
                this.Add(keys[i], values[i]);
        }
    }
    public class ItemTypeEditor : EditorWindow
    {
        [SerializeField]
        SerializableDictionary _enum = new SerializableDictionary();

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

                if (GUILayout.Button("-", GUILayout.Width(20)))
                {
                    _enum.Remove(value.Key);
                }

                GUILayout.Label(value.Key, GUILayout.Width(150));
                GUILayout.Label(value.Value.ToString(), GUILayout.Width(150));
                EditorGUILayout.EndHorizontal();
            }

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
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
                    Debug.Log(ex.Message);
                }   
            }
            GUILayout.Space(10);
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Update"))
            {
                if (_enum.Count > 0)
                {
                    System.IO.StreamWriter writer = new System.IO.StreamWriter("Assets/Scripts/RPGSystem/Items/ItemTypes.cs");
                    writer.WriteLine("namespace RPGSystem");
                    writer.WriteLine("{");
                    writer.WriteLine("\tpublic enum ItemTypes");
                    writer.WriteLine("\t{");
                    writer.Write("\t\t");

                    List<string> enumFields = new List<string>();
                    foreach (KeyValuePair<string, int> value in _enum)
                    {
                        enumFields.Add(value.Key + " = " + value.Value);
                        //writer.Write(value.Key + " = " + value.Value + ",\n\t\t");
                    }
                    string enumContents = string.Join("," + System.Environment.NewLine + "\t\t", enumFields.ToArray());
                    writer.WriteLine(enumContents);
                    writer.WriteLine("\t}");
                    writer.WriteLine("}");
                    writer.Close();
                }
            }
        }
    }
}
