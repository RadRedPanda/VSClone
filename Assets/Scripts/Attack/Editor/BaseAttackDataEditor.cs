using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BaseAttackData), true)]
public class BaseAttackDataDrawer : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		SerializedProperty nodeProperty = serializedObject.FindProperty("Nodes");
		List<Vector3Int> nodes = new List<Vector3Int>();

		Texture2D nodeTexture = drawNodeTexture(nodeProperty);
		GUILayout.Label("Node Preview");
		GUILayout.Label(nodeTexture);
	}

	private Texture2D drawNodeTexture(SerializedProperty nodeProperty)
	{
		int startRadius = 12, nodeRadius = 10;
		int pixelDistance = 30;

		List<Vector2> cartesianNodes = new List<Vector2>();
		cartesianNodes.Add(HexGrid.HexToCartesian(new Vector3Int(0, 0, 0)));

		float minX = 0, minY = 0, maxX = 0, maxY = 0;

		for (int i = 0; i < nodeProperty.arraySize; i++)
		{
			Vector3Int node = nodeProperty.GetArrayElementAtIndex(i).vector3IntValue;
			Vector2 cartesianNode = HexGrid.HexToCartesian(node) * pixelDistance;
			cartesianNodes.Add(cartesianNode);
			minX = Mathf.Min(minX, cartesianNode.x);
			minY = Mathf.Min(minY, cartesianNode.y);
			maxX = Mathf.Max(maxX, cartesianNode.x);
			maxY = Mathf.Max(maxY, cartesianNode.y);
		}

		minX -= startRadius;
		minY -= startRadius;
		maxX += startRadius;
		maxY += startRadius;

		int x = Mathf.RoundToInt(maxX - minX);
		int y = Mathf.RoundToInt(maxY - minY);
		Texture2D newTexture = new Texture2D(x, y);

		Color[] colorArray = Enumerable.Repeat(Color.black, x * y).ToArray();
		newTexture.SetPixels(colorArray);

		drawCircle(newTexture, new Vector2Int(Mathf.RoundToInt(cartesianNodes[0].x - minX), Mathf.RoundToInt(cartesianNodes[0].y - minY)), startRadius, Color.grey);

		for (int i = 1; i < cartesianNodes.Count; i++)
		{

			drawLine(newTexture,
				new Vector2Int(Mathf.RoundToInt(cartesianNodes[i - 1].x - minX), Mathf.RoundToInt(cartesianNodes[i - 1].y - minY)),
				new Vector2Int(Mathf.RoundToInt(cartesianNodes[i].x - minX), Mathf.RoundToInt(cartesianNodes[i].y - minY)),
				Color.white
			);
			drawCircle(newTexture, new Vector2Int(Mathf.RoundToInt(cartesianNodes[i].x - minX), Mathf.RoundToInt(cartesianNodes[i].y - minY)), nodeRadius, Color.white);

		}

		newTexture.Apply();

		return newTexture;
	}

	protected void drawCircle(Texture2D texture, Vector2Int position, int radius, Color color)
	{
		for (int i=Mathf.Max(0, position.x - radius); i<Mathf.Min(position.x + radius, texture.width); i++)
			for (int j=Mathf.Max(0, position.y - radius); j<Mathf.Min(position.y + radius, texture.height); j++)
				if (Vector2.Distance(position, new Vector2(i, j)) < radius)
					texture.SetPixel(i, j, color);
	}

	protected void drawLine(Texture2D texture, Vector2Int startPosition, Vector2Int endPosition, Color color)
	{
		Vector2 t = startPosition;
		float frac = 1 / Mathf.Sqrt(Mathf.Pow(endPosition.x - startPosition.x, 2) + Mathf.Pow(endPosition.y - startPosition.y, 2));
		float ctr = 0;

		while ((int)t.x != (int)endPosition.x || (int)t.y != (int)endPosition.y)
		{
			t = Vector2.Lerp(startPosition, endPosition, ctr);
			ctr += frac;
			texture.SetPixel((int)t.x, (int)t.y, color);
		}
	}
}