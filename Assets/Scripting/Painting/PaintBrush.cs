using System;
using UnityEngine;


public class PaintBrush : MonoBehaviour
{


  [BoxGroup("Painting Properties"), SerializeField, Range(0.1f, 50f)] float      brushSize;
	[BoxGroup("Painting Properties"), SerializeField, Range(0,    360)] float      brushAngle;
  
  
	[BoxGroup("Painting Properties"), SerializeField]                   Material   targetMaterial;
  [BoxGroup("Painting Properties"), SerializeField]                   Texture2D  originalTexture;
	[BoxGroup("Painting Properties"), SerializeField]                   Texture2D  duplicateTexture;


[BoxGroup("Hidden Properties"), SerializeField] const float OneEighty = 180f;



void Start()
	{
	
		
		// make a new Texture
		duplicateTexture = new Texture2D(originalTexture.width, originalTexture.height, TextureFormat.RGBA32, false);
		var colorData = originalTexture.GetPixels();
		duplicateTexture.SetPixels(colorData);
		duplicateTexture.name = "Duplicated Texture";
		duplicateTexture.Apply();
		targetMaterial.SetTexture("Name Of Property", duplicateTexture);
    
    }
    
    public void UsePainting()
	{ 
   RaycastHit hit;
		if (!Physics.Raycast(brushModel.transform.position, (brushModel.transform.forward * rayLength), out hit, 1f, hitMask))
		{
			isHitting = false;
			return;
		}

		// AdjustModel(hit.point, hit.normal);
		isHitting = true;
		Vector2 pixelUV  = hit.textureCoord;
    
    int     centerX  = (int) Mathf.Round(pixelUV.x * duplicateTexture.width);
		int     centerY  = (int) Mathf.Round(pixelUV.y * duplicateTexture.height);
    
    for (int j = 0; j <= brushSize; j++)
		{
    
			for (int i = 0; i < brushAngle; i++)
			{
				
				float angle = (float) (i                                    * System.Math.PI) / OneEighty;
				int   x     = (int) (centerX + j                            * System.Math.Cos(angle));
				int   y     = (int) (centerY + j * Mathf.Sin(angle)         / 2f);

				if (duplicateTexture.GetPixel(x, y).r < color.r)
				{
					duplicateTexture.SetPixel(x, y, Your Color );
				}
      }
  }
  
  duplicateTexture.Apply();
  
  }
    
}


