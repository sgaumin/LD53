using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
	public static class UIExtensions
	{
		/// <summary>
		/// Assign a new color from the given TextMeshPro and a new value for its alpha.
		/// </summary>
		/// <param name="t">TextMeshPro</param>
		/// <param name="a">The new alpha value</param>
		/// <returns>A new color {t.rgb, a}</returns>
		public static TextMeshPro WithAlpha(this TextMeshPro t, float a)
		{
			t.color = t.color.WithAlpha(a);
			return t;
		}

		/// <summary>
		/// Assign a new color from the given TextMeshProUGUI and a new value for its alpha.
		/// </summary>
		/// <param name="t">TextMeshProUGUI</param>
		/// <param name="a">The new alpha value</param>
		/// <returns>A new color {t.rgb, a}</returns>
		public static TextMeshProUGUI WithAlpha(this TextMeshProUGUI t, float a)
		{
			t.color = t.color.WithAlpha(a);
			return t;
		}

		/// <summary>
		/// Assign a new color from the given SpriteRenderer and a new value for its alpha.
		/// </summary>
		/// <param name="s">SpriteRenderer</param>
		/// <param name="a">The new alpha value</param>
		/// <returns>A new color {s.rgb, a}</returns>
		public static SpriteRenderer WithAlpha(this SpriteRenderer s, float a)
		{
			s.color = s.color.WithAlpha(a);
			return s;
		}

		/// <summary>
		/// Assign a new color from the given Image and a new value for its alpha.
		/// </summary>
		/// <param name="i">Image</param>
		/// <param name="a">The new alpha value</param>
		/// <returns>A new color {i.rgb, a}</returns>
		public static Image WithAlpha(this Image i, float a)
		{
			i.color = i.color.WithAlpha(a);
			return i;
		}

		/// <summary>
		/// Create a new color from the given color and a new value for its alpha.
		/// </summary>
		/// <param name="c">The source color</param>
		/// <param name="a">The new alpha value</param>
		/// <returns>A new color {c.rgb, a}</returns>
		public static Color WithAlpha(this Color c, float a)
		{
			c.a = a;
			return c;
		}
	}
}