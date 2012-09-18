// Upgrade NOTE: replaced 'glstate.matrix.modelview[0]' with 'UNITY_MATRIX_MV'
// Upgrade NOTE: replaced 'glstate.matrix.mvp' with 'UNITY_MATRIX_MVP'
// Upgrade NOTE: replaced 'glstate.matrix.projection' with 'UNITY_MATRIX_P'

Shader "Outlined/VertexColor" {
Properties 
   { 
      _Color ("Main Color", Color) = (1,1,1,1) 
      _OutlineColor ("Outline Color", Color) = (0.18,0.0784,0.00784,1) 
      _Outline ("Outline width", Range (0.000, 0.03)) = 0.0007 
      _MainTex ("Base (RGB)", 2D) = "white" { } 
   } 
   
CGINCLUDE
#include "UnityCG.cginc"

struct appdata {
    float4 vertex : POSITION;
    float3 normal : NORMAL;
};

struct v2f {
    float4 pos : POSITION;
    float4 color : COLOR;
};

uniform float _Outline;
uniform float4 _OutlineColor;

v2f vert(appdata v) {
    // just make a copy of incoming vertex data but scaled according to normal direction
    v2f o;
    o.pos = mul(UNITY_MATRIX_MVP, v.vertex);

    float3 norm   = mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal);
    float2 offset = TransformViewToProjection(norm.xy);

    o.pos.xy += offset * o.pos.z * _Outline;
    o.color = _OutlineColor;
    return o;
}
ENDCG

   SubShader 
   { 
      Tags { "RenderType"="Opaque" } 
	  Pass {
		Tags {"LightMode" = "Always" "IgnoreProjector"="True"}
		Lighting Off
		
		BindChannels {
		    Bind "Vertex", vertex
		    Bind "TexCoord", texcoord
		    Bind "Color", color
		} 
		
		SetTexture [_MainTex] {
			constantColor [_Color]
			combine constant * primary
		}
		SetTexture [_MainTex] {
			combine texture * previous, texture * primary
		}		
	  }
      Pass {
            Name "OUTLINE"
            Tags { "LightMode" = "Always" }
            Cull Front
            ZWrite On
            ColorMask RGB
            Blend SrcAlpha OneMinusSrcAlpha
            //Offset 50,50

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            half4 frag(v2f i) :COLOR { return i.color; }
            ENDCG
      }
   } 
    
   Fallback "Diffuse" 
}
