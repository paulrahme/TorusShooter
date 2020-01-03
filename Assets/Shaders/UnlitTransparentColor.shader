Shader "Exobius/Unlit Color (with Transparency)"
{
	Properties
	{
		_Color("Color (with Transparency)", Color) = (0, 0, 0, 1)
	}
		SubShader{
			Lighting Off
			ZWrite Off
			Cull Back
			Blend SrcAlpha OneMinusSrcAlpha
			Tags {"Queue" = "Transparent"}
			Color[_Color]
			Pass {
			}
	}
		FallBack "Unlit/Transparent"
}
