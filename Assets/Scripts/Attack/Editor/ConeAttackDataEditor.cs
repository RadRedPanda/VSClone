using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ConeAttackData), true)]
public class ConeAttackDataEditor : BaseAttackDataDrawer
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		float radius = serializedObject.FindProperty("Radius").floatValue;
		float angle = serializedObject.FindProperty("Angle").floatValue;

		SerializedProperty spriteProperty = serializedObject.FindProperty("Image");
		Sprite sprite = spriteProperty.objectReferenceValue as Sprite;

		drawHitboxTexture(radius, angle, sprite);
	}

	private void drawHitboxTexture(float radius, float angle, Sprite sprite)
	{
		if (radius <= 0)
			return;

		GUILayout.Label("Hitbox Preview");

		float scale = 0.5f;

		int textureSize = Mathf.RoundToInt(radius * 2 * sprite.pixelsPerUnit * scale);

		Texture2D hitboxTexture = new Texture2D(textureSize, textureSize);
		Color[] colorArray = Enumerable.Repeat(Color.black, hitboxTexture.width * hitboxTexture.height).ToArray();

		hitboxTexture.SetPixels(colorArray);

		List<Vector2> conePoints = ConeAttackData.GetConePoints(radius, angle);
		List<Vector2Int> intConePoints = new List<Vector2Int>();

		intConePoints.Add(
			new Vector2Int(
				Mathf.RoundToInt(conePoints[0].x * sprite.pixelsPerUnit * scale + textureSize / 2),
				Mathf.RoundToInt(conePoints[0].y * sprite.pixelsPerUnit * scale + textureSize / 2)
			)
		);
		for (int i=1; i<conePoints.Count; i++)
		{
			intConePoints.Add(
				new Vector2Int(
					Mathf.RoundToInt(conePoints[i].x * sprite.pixelsPerUnit * scale + textureSize / 2),
					Mathf.RoundToInt(conePoints[i].y * sprite.pixelsPerUnit * scale + textureSize / 2)
				)
			);
			drawLine(hitboxTexture, intConePoints[i - 1], intConePoints[i], Color.green);
		}
		drawLine(hitboxTexture, intConePoints[0], intConePoints[intConePoints.Count - 1], Color.green);

		hitboxTexture.Apply();

		Rect hitboxRect = GUILayoutUtility.GetLastRect();
		hitboxRect.y += hitboxRect.height;
		hitboxRect.width = textureSize;
		hitboxRect.height = textureSize;

		GUI.Label(hitboxRect, hitboxTexture);

		Rect spriteRect = new Rect(new Vector2((textureSize - sprite.texture.width * scale) / 2 + hitboxRect.x, (textureSize - sprite.texture.height * scale) / 2 + hitboxRect.y), sprite.texture.Size() * scale);
		GUI.Label(spriteRect, sprite.texture);

		GUILayout.Space(textureSize * scale);
	}
}
