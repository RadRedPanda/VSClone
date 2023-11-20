using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseNode : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
	public Vector3Int localHexPosition;
	public HexGrid parentGrid;
	public BaseAttackData AttackData;

	private Vector3 dragOffset;
	private HashSet<Hexagon> highlightedHexagons = new HashSet<Hexagon>();

	public void OnBeginDrag(PointerEventData eventData)
	{
		dragOffset = transform.position - Input.mousePosition;
	}

	public void OnDrag(PointerEventData eventData)
	{
		transform.position = dragOffset + Input.mousePosition;
		Hexagon gotHex = parentGrid.GetHexagon(HexGrid.CartesianToHex(transform.localPosition / parentGrid.Distance));
		if (gotHex)
			highlightHexagons(gotHex);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		Hexagon gotHex = parentGrid.GetHexagon(HexGrid.CartesianToHex(transform.localPosition / parentGrid.Distance));
		if (gotHex != null)
			snapToHexagon(gotHex);
		foreach (Hexagon hexagon in highlightedHexagons)
			hexagon.Unhighlight();
		highlightedHexagons.Clear();
	}

	private void highlightHexagons(Hexagon hexagon)
	{
		HashSet<Hexagon> newHexagons = new HashSet<Hexagon>();

		hexagon.Highlight();
		newHexagons.Add(hexagon);

		foreach (Vector3Int hexPosition in AttackData.Nodes)
		{
			Hexagon newHexagon = parentGrid.GetHexagon(HexGrid.HexAdd(hexagon.HexCoordinate, hexPosition));
			if (newHexagon != null)
			{
				newHexagon.Highlight();
				newHexagons.Add(newHexagon);
			}
		}

		highlightedHexagons.ExceptWith(newHexagons);
		foreach (Hexagon highlightedHexagon in highlightedHexagons)
			highlightedHexagon.Unhighlight();

		highlightedHexagons = newHexagons;
	}

	private void snapToHexagon(Hexagon hexagon)
	{
		transform.localPosition = hexagon.transform.localPosition;
	}
}
