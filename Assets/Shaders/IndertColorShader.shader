Shader "Unlit/IndertColorShader"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "" {}
	}

	CGINCLUDE

	#include "UnityCG.cginc"

	struct v2f
	{
		float4 pos : POSITION;
		float2 uv : TEXCOORD0;
	};

	sampler2D _MainTex;

	v2f vert(appdata_img v) 
	{
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);

		o.uv.xy = v.texcoord.xy;

		return o;
	}

	half4 frag(v2f i) : COLOR
	{
		half4 color = float4 (0,0,0,0);

		color += 1 - tex2D(_MainTex, i.uv);

		return color;
	}

		ENDCG
}
