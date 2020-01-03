Shader "Exobius/Unlit Texture Tint (with Transparency)"
{
	Properties
	{
		_Color("Tint (with Transparency)", Color) = (0, 0, 0, 1)
		_MainTex("Base (RGB) Alpha (A)", 2D) = "white"
	}

	SubShader
	{
		Lighting Off
		ZWrite Off
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		Tags {"Queue" = "Transparent"}

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
