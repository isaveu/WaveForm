using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace WaveFormRenderer
{
	public class UIPrimitiveBaseExtended : UIPrimitiveBase
	{

		protected UIVertex[] SetVbo(Vector2[] vertices, Vector2[] uvs, Color startColor, Color endColor)
		{
			UIVertex[] vbo = new UIVertex[4];
			for (int i = 0; i < vertices.Length; i++)
			{
				var vert = UIVertex.simpleVert;
				vert.color = i > 1 ? startColor : endColor;
				vert.position = vertices[i];
				vert.uv0 = uvs[i];
				vbo[i] = vert;
			}
			return vbo;
		}
	}
}
