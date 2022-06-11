#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(EnumFlagAttribute))]
public class EnumFlagsAttributeDrawer : PropertyDrawer
{
    public bool open = true;

    public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
    {
        open = EditorGUI.Foldout(new Rect(_position.x, _position.y, EditorGUIUtility.labelWidth, 10), open, _label);

        if (open)
        {
            int buttonsIntValue = 0;
            int enumLength = _property.enumNames.Length - 2;
            bool[] buttonPressed = new bool[enumLength];
            float buttonWidth = (_position.width - EditorGUIUtility.labelWidth);
            float buttonHeight = 20;
            _position.height = buttonHeight * enumLength;
            EditorGUI.LabelField(new Rect(_position.x, _position.y, EditorGUIUtility.labelWidth, _position.height), _label);

            EditorGUI.BeginChangeCheck();

            for (int i = 0; i < enumLength; i++)
            {
                GUI.color = Color.white;
                // Check if the button is/was pressed 
                if ((_property.intValue & (1 << i)) == 1 << i)
                {
                    GUI.color = Color.green;
                    buttonPressed[i] = true;
                }

                Rect buttonPos = new Rect(_position.x + EditorGUIUtility.labelWidth, _position.y + (i * buttonHeight), buttonWidth, buttonHeight);

                buttonPressed[i] = GUI.Toggle(buttonPos, buttonPressed[i], _property.enumNames[i+1], "Button");
                GUI.color = Color.white;

                if (buttonPressed[i])
                {
                    buttonsIntValue += (1 << i);
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                _property.intValue = buttonsIntValue;
            }
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return open ? (property.enumNames.Length -2) * 20 : 20;
    }
}

#endif