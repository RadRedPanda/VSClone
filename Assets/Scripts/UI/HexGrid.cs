using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
	[SerializeField]
	private Hexagon _hexagonPrefab;
	[SerializeField]
	private BaseNode _baseNodePrefab;
	[SerializeField]
	private RectTransform _emptyNodePrefab;
	[SerializeField]
	private RectTransform _connectionPrefab;

	[SerializeField]
	private BaseAttackData TESTDATA;

	[SerializeField]
	private int _rows = 5;
	[SerializeField]
	private int _columns = 16;
	[SerializeField]
	public float Distance = 100f;

	List<List<Hexagon>> _hexagonsA = new List<List<Hexagon>>();
	List<List<Hexagon>> _hexagonsB = new List<List<Hexagon>>();

	void Start()
	{
		createHexagons();
		CreateAttackNodes(TESTDATA);
	}

	private void Update()
	{
		//Vector3Int hexPosition = CartesianToHex(Input.mousePosition / Distance);
		//Hexagon hexagon = GetHexagon(hexPosition);
		//hexagon?.Highlight();
	}

	public BaseNode CreateAttackNodes(BaseAttackData attackData)
	{
		BaseNode newBaseNode = Instantiate(_baseNodePrefab, transform);
		newBaseNode.AttackData = attackData;
		newBaseNode.parentGrid = this;
		createConnections(newBaseNode);
		createEmptyNodes(newBaseNode);
		return newBaseNode;
	}

	private void createConnections(BaseNode baseNode)
	{
		createConnection(Vector3Int.zero, baseNode.AttackData.Nodes[0], baseNode);

		for (int i=1; i<baseNode.AttackData.Nodes.Count; i++)
			createConnection(baseNode.AttackData.Nodes[i - 1], baseNode.AttackData.Nodes[i], baseNode);
	}

	private RectTransform createConnection(Vector3Int previous, Vector3Int next, BaseNode parent)
	{
		RectTransform connection = Instantiate(_connectionPrefab, parent.transform);
		Rect rect = connection.rect;
		Vector2 distanceVector = HexToCartesian(HexSubtract(previous, next));
		connection.sizeDelta = new Vector2(distanceVector.magnitude * Distance, connection.rect.height);
		connection.localPosition = HexToCartesian(HexAdd(previous, next)) / 2 * Distance;
		Vector3 newEulerAngles = connection.localEulerAngles;
		newEulerAngles.z = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
		connection.localEulerAngles = newEulerAngles;
		return connection;
	}

	private void createEmptyNodes(BaseNode baseNode)
	{
		for (int i=0; i<baseNode.AttackData.Nodes.Count; i++)
		{
			RectTransform emptyNode = Instantiate(_emptyNodePrefab, baseNode.transform);
			emptyNode.localPosition = HexToCartesian(baseNode.AttackData.Nodes[i]) * Distance;
		}
	}

	private void createHexagons()
	{
		for (int i = 0; i < _rows; i++)
		{
			List<Hexagon> hexagonAColumn = new List<Hexagon>();
			List<Hexagon> hexagonBColumn = new List<Hexagon>();
			for (int j = 0; j < _columns; j++)
			{
				Hexagon firstHex = Instantiate(_hexagonPrefab, transform);
				firstHex.HexCoordinate = new Vector3Int(i, j, 0);
				firstHex.transform.localPosition = Distance * (Vector3)HexToCartesian(firstHex.HexCoordinate);
				hexagonAColumn.Add(firstHex);

				Hexagon secondHex = Instantiate(_hexagonPrefab, transform);
				secondHex.HexCoordinate = new Vector3Int(i, j, 1);
				secondHex.transform.localPosition = Distance * (Vector3)HexToCartesian(secondHex.HexCoordinate);
				hexagonBColumn.Add(secondHex);
			}
			_hexagonsA.Add(hexagonAColumn);
			_hexagonsB.Add(hexagonBColumn);
		}
	}

	public Hexagon GetHexagon(Vector3Int hexCoordinate)
	{
		if (hexCoordinate.x < 0 || hexCoordinate.x >= _hexagonsA.Count)
			return null;
		if (hexCoordinate.y < 0 || hexCoordinate.y >= _hexagonsA[0].Count)
			return null;
		if (hexCoordinate.z < 0 || hexCoordinate.z > 1)
			return null;

		if (hexCoordinate.z == 0)
			return _hexagonsA[hexCoordinate.x][hexCoordinate.y];
		if (hexCoordinate.z == 1)
			return _hexagonsB[hexCoordinate.x][hexCoordinate.y];
		return null;
	}

	public static Vector3Int CartesianToHex(Vector2 cartesianPosition)
	{
		Vector3Int hexPosition = new Vector3Int();

		int yValue = Mathf.RoundToInt(cartesianPosition.y / Mathf.Sqrt(3) * 2);
		hexPosition.x = yValue / 2;
		hexPosition.y = Mathf.FloorToInt(cartesianPosition.x);
		hexPosition.z = yValue % 2;

		List<Vector3Int> neighborList = GetHexNeighbors(hexPosition);
		float closestDistance = Mathf.Infinity;
		Vector3Int closestNeighbor = Vector3Int.zero;
		foreach (Vector3Int neighbor in neighborList)
		{
			float newDistance = Vector2.Distance(HexToCartesian(neighbor), cartesianPosition);
			if (newDistance < closestDistance)
			{
				closestDistance = newDistance;
				closestNeighbor = neighbor;
			}
		}
		return closestNeighbor;
	}

	public static Vector2 HexToCartesian(Vector3Int hexPosition)
	{
		Vector2 cartesianPosition;
		cartesianPosition.x = (hexPosition.z / 2f) + hexPosition.y;
		cartesianPosition.y = Mathf.Sqrt(3) * ((hexPosition.z / 2f) + hexPosition.x);
		return cartesianPosition;
	}

	public static List<Vector3Int> GetHexNeighbors(Vector3Int hexPosition)
	{
		List<Vector3Int> neighbors = new List<Vector3Int>();
		int inverseA = 1 - hexPosition.z;
		neighbors.Add(new Vector3Int(hexPosition.x - inverseA, hexPosition.y - inverseA, inverseA));
		neighbors.Add(new Vector3Int(hexPosition.x - inverseA, hexPosition.y + hexPosition.z, inverseA));
		neighbors.Add(new Vector3Int(hexPosition.x, hexPosition.y - 1, hexPosition.z));
		neighbors.Add(hexPosition);
		neighbors.Add(new Vector3Int(hexPosition.x, hexPosition.y + 1, hexPosition.z));
		neighbors.Add(new Vector3Int(hexPosition.x + hexPosition.z, hexPosition.y - inverseA, inverseA));
		neighbors.Add(new Vector3Int(hexPosition.x + hexPosition.z, hexPosition.y + hexPosition.z, inverseA));
		return neighbors;
	}

	public static Vector3Int HexAdd(Vector3Int a, Vector3Int b)
	{
		Vector3Int result = new Vector3Int();
		result.x = a.x + b.x + (a.z & b.z);
		result.y = a.y + b.y + (a.z & b.z);
		result.z = a.z ^ b.z;
		return result;
	}

	public static Vector3Int HexNegate(Vector3Int a)
	{
		Vector3Int result = new Vector3Int();
		result.x = -a.x - a.z;
		result.y = -a.y - a.z;
		result.z = a.z;

		return result;
	}

	public static Vector3Int HexSubtract(Vector3Int a, Vector3Int b)
	{
		Vector3Int result = HexNegate(b);
		return HexAdd(a, result);
	}
}
