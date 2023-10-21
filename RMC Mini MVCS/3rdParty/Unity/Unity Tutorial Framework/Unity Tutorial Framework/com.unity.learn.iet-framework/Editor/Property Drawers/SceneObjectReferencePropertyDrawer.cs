using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Unity.Tutorials.Core.Editor
{
    [CustomPropertyDrawer(typeof(SceneObjectReference))]
    class SceneObjectReferencePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var sor = new SceneObjectReference(property);

            var origColor = GUI.color;
            if (!sor.ReferenceResolved)
            {
                label.text = "(Not resolved) " + label.text;
                GUI.color = Color.red;
            }

            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);
            GUI.color = origColor;

            Object obj = sor.ReferencedObject;
            if (!sor.ReferenceResolved)
            {
                obj = sor.ReferenceScene;
            }

            EditorGUI.BeginChangeCheck();
            var newObj = EditorGUI.ObjectField(position, obj, typeof(Object), true);
            if (EditorGUI.EndChangeCheck())
            {
                sor.Update(newObj);
            }

            EditorGUI.EndProperty();
        }
    }
}
