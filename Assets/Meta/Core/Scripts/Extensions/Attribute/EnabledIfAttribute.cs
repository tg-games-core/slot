using System;
using UnityEditor;
using UnityEngine;

namespace Core
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    internal class EnabledIfAttribute : PropertyAttribute
    {
        public enum HideMode
        {
            Invisible,
            Disabled
        }

        public enum SwitcherType
        {
            Bool,
            Enum
        }

        public int[] enableIfValuesIs;
        public HideMode hideMode;
        public string[] switcherFieldNames;
        public SwitcherType switcherType;

        public EnabledIfAttribute(string switcherFieldName, bool enableIfValueIs, HideMode hideMode = HideMode.Invisible)
            : this(SwitcherType.Bool, new[] { switcherFieldName }, new[] { enableIfValueIs ? 1 : 0 }, hideMode)
        {
        }

        public EnabledIfAttribute(string switcherFieldName, int enableIfValueIs, HideMode hideMode = HideMode.Invisible)
            : this(SwitcherType.Enum, new[] { switcherFieldName }, new[] { enableIfValueIs }, hideMode)
        {
        }
        
        public EnabledIfAttribute(string switcherFieldName, int[] enableIfValuesIs, HideMode hideMode = HideMode.Invisible)
            : this(SwitcherType.Enum, new[] { switcherFieldName }, enableIfValuesIs, hideMode)
        {
        }
        
        public EnabledIfAttribute(string[] switcherFieldNames, int[] enableIfValuesIs, HideMode hideMode = HideMode.Invisible)
            : this(SwitcherType.Enum, switcherFieldNames, enableIfValuesIs, hideMode)
        {
        }

        public EnabledIfAttribute(string[] switcherFieldNames, bool[] enableIfValuesIs, HideMode hideMode = HideMode.Invisible)
            : this(SwitcherType.Bool, switcherFieldNames, ConvertBoolsToInts(enableIfValuesIs), hideMode)
        {
        }

        private static int[] ConvertBoolsToInts(bool[] values)
        {
            int[] result = new int[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                result[i] = values[i] ? 1 : 0;
            }
            return result;
        }

        private EnabledIfAttribute(SwitcherType switcherType, string[] switcherFieldNames, int[] enableIfValuesIs,
            HideMode hideMode)
        {
            this.switcherType = switcherType;
            this.hideMode = hideMode;
            this.switcherFieldNames = switcherFieldNames;
            this.enableIfValuesIs = enableIfValuesIs;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(EnabledIfAttribute))]
    public class EnabledIfAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var attr = attribute as EnabledIfAttribute;
            var isEnabled = GetIsEnabled(attr, property);

            if (attr.hideMode == EnabledIfAttribute.HideMode.Disabled)
            {
                GUI.enabled &= isEnabled;
            }

            if (GetIsVisible(attr, property))
            {
                EditorGUI.PropertyField(position, property, label, true);
            }

            if (attr.hideMode == EnabledIfAttribute.HideMode.Disabled)
            {
                GUI.enabled = true;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var attr = attribute as EnabledIfAttribute;
            return GetIsVisible(attr, property) ? EditorGUI.GetPropertyHeight(property) : -2;
        }

        private bool GetIsVisible(EnabledIfAttribute attribute, SerializedProperty property)
        {
            if (GetIsEnabled(attribute, property))
            {
                return true;
            }

            if (attribute.hideMode != EnabledIfAttribute.HideMode.Invisible)
            {
                return true;
            }

            return false;
        }

        private bool GetIsEnabled(EnabledIfAttribute attribute, SerializedProperty property)
        {
            bool allConditionsMet = true;

            foreach (var switcherFieldName in attribute.switcherFieldNames)
            {
                int switcherValue = GetSwitcherPropertyValue(attribute, switcherFieldName, property);
                bool conditionMet = false;

                for (int i = 0; i < attribute.enableIfValuesIs.Length; i++)
                {
                    if (attribute.enableIfValuesIs[i] == switcherValue)
                    {
                        conditionMet = true;
                        break;
                    }
                }

                if (!conditionMet)
                {
                    allConditionsMet = false;
                    break;
                }
            }
            
            return allConditionsMet;
        }
        
        private int GetSwitcherPropertyValue(EnabledIfAttribute attribute, string switcherFieldName, SerializedProperty property)
        {
            var so = property.serializedObject;

            var pathIndex = property.propertyPath.LastIndexOf(property.name, StringComparison.Ordinal);
            var candidatePath = property.propertyPath
                                    .Substring(0, pathIndex)
                                + switcherFieldName;
            var switcherProp = so.FindProperty(candidatePath);

            if (switcherProp == null)
                switcherProp = so.FindProperty(switcherFieldName);

            if (switcherProp == null)
            {
                var backingName = $"<{switcherFieldName}>k__BackingField";
                switcherProp = so.FindProperty(backingName);

                if (switcherProp == null && pathIndex > 0)
                {
                    var backingPath = property.propertyPath.Substring(0, pathIndex) + backingName;
                    switcherProp = so.FindProperty(backingPath);
                }
            }

            if (switcherProp == null)
                throw new Exception($"EnabledIf: не найдено поле-переключатель '{switcherFieldName}'");

            switch (switcherProp.propertyType)
            {
                case SerializedPropertyType.Boolean:
                    return switcherProp.boolValue ? 1 : 0;
                case SerializedPropertyType.Enum:
                    return switcherProp.intValue;
                default:
                    throw new Exception("EnabledIf: unsupported switcher type " + switcherProp.propertyType);
            }
        }
    }
#endif
}