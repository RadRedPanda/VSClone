using System;
using UnityEngine;

public class AttackNode : BaseNode
{
	private BaseAttackData _attackData;
	private BaseNode _nodePrefab;

	public void SetUpNodes(BaseAttackData attackData)
	{
		_attackData = attackData;
		Vector3Int previousNode = Vector3Int.zero;
		foreach (Vector3Int node in _attackData.Nodes)
		{
			setUpNode(_nodePrefab, node);
			connectNodes(previousNode, node);
		}
	}

	private BaseNode setUpNode(BaseNode nodePrefab, Vector3Int hexCoordinate)
	{
		Vector2 localPosition = HexGrid.HexToCartesian(hexCoordinate);
		BaseNode newNode = Instantiate(nodePrefab, transform);
		newNode.localHexPosition = hexCoordinate;
		newNode.transform.localPosition = localPosition;
		return newNode;
	}

	private void connectNodes(Vector3Int previousNode, Vector3Int node)
	{
		throw new NotImplementedException();
	}
}
