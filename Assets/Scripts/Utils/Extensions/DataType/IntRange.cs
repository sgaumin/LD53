using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Utils
{
	[Serializable]
	public struct IntRange
	{
		[SerializeField]
		private int min;

		[SerializeField]
		private int max;

		public int Min
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

		public int Max
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

		public int RandomValue
		{
			get
			{
				return UnityEngine.Random.Range(this.min, this.max);
			}
		}

		public IntRange(int min, int max)
		{
			this.min = min;
			this.max = max;
		}

		public int Clamp(int value)
		{
			return Mathf.Clamp(value, this.min, this.max);
		}

		public bool Contains(int value)
		{
			return value >= this.min && value <= this.max;
		}
	}

	public class IntRangeSliderAttribute : PropertyAttribute
	{
		public readonly int Min;
		public readonly int Max;

		public IntRangeSliderAttribute(int min, int max)
		{
			Min = min;
			Max = max;
		}
	}

#if UNITY_EDITOR

	[CustomPropertyDrawer(typeof(IntRange))]
	[CustomPropertyDrawer(typeof(IntRangeSliderAttribute))]
	public class IntRangeDrawer : PropertyDrawer
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
			IntRangeSliderAttribute minmax = attribute as IntRangeSliderAttribute ?? new IntRangeSliderAttribute(0, 1);
			position.height -= 16f;

			label = EditorGUI.BeginProperty(position, label, property);
			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
			float min = minProperty.intValue;
			float max = maxProperty.intValue;

			Rect left = new Rect(position.x, position.y, position.width / 2 - 11f, position.height);
			Rect right = new Rect(position.x + position.width - left.width, position.y, left.width, position.height);
			Rect mid = new Rect(left.xMax, position.y, 22, position.height);
			min = Mathf.Clamp(EditorGUI.IntField(left, (int)min), minmax.Min, max);
			EditorGUI.LabelField(mid, " to ");
			max = Mathf.Clamp(EditorGUI.IntField(right, (int)max), min, minmax.Max);

			position.y += 16f;
			EditorGUI.MinMaxSlider(position, GUIContent.none, ref min, ref max, minmax.Min, minmax.Max);

			minProperty.intValue = (int)min;
			maxProperty.intValue = (int)max;
			EditorGUI.EndProperty();
		}
	}

#endif
}