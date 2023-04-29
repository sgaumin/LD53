using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Utils
{
	[Serializable]
	public struct FloatRange
	{
		[SerializeField]
		private float min;

		[SerializeField]
		private float max;

		public float Min
		{
			get
			{
				return this.min;
			}
			set
			{
				this.min = value;
			}
		}

		public float Max
		{
			get
			{
				return this.max;
			}
			set
			{
				this.max = value;
			}
		}

		public float RandomValue
		{
			get
			{
				return UnityEngine.Random.Range(this.min, this.max);
			}
		}

		public FloatRange(float min, float max)
		{
			this.min = min;
			this.max = max;
		}

		public float Clamp(float value)
		{
			return Mathf.Clamp(value, this.min, this.max);
		}

		public bool Contains(float value)
		{
			return value >= this.min && value <= this.max;
		}
	}

	public class FloatRangeSliderAttribute : PropertyAttribute
	{
		public readonly float Min;
		public readonly float Max;

		public FloatRangeSliderAttribute(float min, float max)
		{
			Min = min;
			Max = max;
		}
	}

#if UNITY_EDITOR

	[CustomPropertyDrawer(typeof(FloatRange))]
	[CustomPropertyDrawer(typeof(FloatRangeSliderAttribute))]
	public class FloatRangeDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			if (property.serializedObject.isEditingMultipleObjects)
			{
				return 0f;
			}

			return base.GetPropertyHeight(property, label) + 16f;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (property.serializedObject.isEditingMultipleObjects)
			{
				return;
			}

			SerializedProperty minProperty = property.FindPropertyRelative("min");
			SerializedProperty maxProperty = property.FindPropertyRelative("max");
			FloatRangeSliderAttribute minmax = attribute as FloatRangeSliderAttribute ?? new FloatRangeSliderAttribute(0, 1);
			position.height -= 16f;

			label = EditorGUI.BeginProperty(position, label, property);
			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
			float min = minProperty.floatValue;
			float max = maxProperty.floatValue;

			Rect left = new Rect(position.x, position.y, position.width / 2 - 11f, position.height);
			Rect right = new Rect(position.x + position.width - left.width, position.y, left.width, position.height);
			Rect mid = new Rect(left.xMax, position.y, 22, position.height);
			min = Mathf.Clamp(EditorGUI.FloatField(left, min), minmax.Min, max);
			EditorGUI.LabelField(mid, " to ");
			max = Mathf.Clamp(EditorGUI.FloatField(right, max), min, minmax.Max);

			position.y += 16f;
			EditorGUI.MinMaxSlider(position, GUIContent.none, ref min, ref max, minmax.Min, minmax.Max);

			minProperty.floatValue = min;
			maxProperty.floatValue = max;
			EditorGUI.EndProperty();
		}
	}

#endif
}