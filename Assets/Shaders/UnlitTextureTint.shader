Shader "Exobius/Unlit Texture Tint"
{
	Properties
	{
		_Color("Tint", Color) = (0, 0, 0, 1)
		_MainTex("Base (RGB)", 2D) = "white"
	}

	SubShader
	{
		Lighting Off
		ZWrite Off
		Cull Back

		Pass {
			SetTexture[_MainTex]
			{
				ConstantColor[_Color]
				Combine Texture * constant
			}
		}
	}

	FallBack "Unlit/Texture"
}
