using UnityEngine;
using UnityEngine.UI;

public class Hexagon : MonoBehaviour
{
	public Vector3Int HexCoordinate;

	private Image _image;
	private Color _defaultColor;
	[SerializeField]
	private Color _highlightColor;

	void Awake()
	{
		_image = GetComponent<Image>();
		_defaultColor = _image.color;
	}

	public void Highlight()
	{
		_image.color = _highlightColor;
	}

	public void Unhighlight()
	{
		_image.color = _defaultColor;
	}
}
