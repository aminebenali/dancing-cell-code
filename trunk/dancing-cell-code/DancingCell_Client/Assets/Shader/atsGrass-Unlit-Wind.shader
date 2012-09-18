// - Unlit

Shader "atsGrass-Unlit-Wind" {
Properties {
	_MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
}

SubShader {
	Tags {"Queue"="Transparent+1" "IgnoreProjector"="True" "RenderType"="Transparent" "LightMode"="ForwardBase"}
	LOD 100
	Cull Off
	ZWrite Off
	Blend SrcAlpha OneMinusSrcAlpha 
	
	CGINCLUDE
	#include "UnityCG.cginc"
	sampler2D _MainTex;
	half4 _MainTex_ST;
	
	float4 _GrassWind; //is not defined in terrainengine.cginc
	float4 _GrassBlend;
	
	struct v2in {
	    float4 vertex : POSITION;
	    fixed4 color : COLOR;
	    float4 texcoord : TEXCOORD0;
	};
	
	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
		float3 color : TEXCOORD1;
	};

inline float4 AnimateGrass(float4 pos, float animParams)
{	

	half ratio = saturate((pos.y - _GrassBlend.w)/pos.y);
	// animParams stored in color
	// animParams = primary factor = blue
	// Primary bending
	// Displace position
	pos.xyz += animParams * _GrassWind.xyz * _GrassWind.w * (ratio*_GrassBlend.xyz); // controlled by vertex color blue
	return pos;
}


	
	v2f vert (v2in v)
	{
		v2f o;
		// call vertex animation
		float4 mdlPos = AnimateGrass(v.vertex, v.color.a);
		o.pos = mul(UNITY_MATRIX_MVP,mdlPos);
		o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
		o.color = v.color;
		return o;
	}
	ENDCG

	Pass {
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma fragmentoption ARB_precision_hint_fastest		
		fixed4 frag (v2f i) : COLOR
		{
			fixed4 tex = tex2D (_MainTex, i.uv);
			
			fixed4 c;
			c.rgb = tex.rgb*i.color.rgb;
			c.a = tex.a;
			
			return c;
		}
		ENDCG 
	}	
}
}


