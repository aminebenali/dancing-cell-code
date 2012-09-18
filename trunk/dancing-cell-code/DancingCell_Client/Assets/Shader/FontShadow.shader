Shader "Font/FontShadow" {
    Properties {
        _MainTex ("Font Texture", 2D) = "white" {}
        _Color ("Text Color", Color) = (1,1,1,1)
        _ShadowColor ("Shadow Color", Color) = (1,1,1,1)
        _Offset ("Offset width", Range (.0, .1)) = .05
    }
 
    SubShader {
		Tags {"Queue" = "Overlay" "RenderType"="Overlay" } 
		
		Lighting Off 
		//Blend SrcAlpha OneMinusSrcAlpha 
		Cull Off 
		ZWrite Off 
		Fog { Mode Off } 
		ZTest Always 
		
        Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag


			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			sampler2D _MainTex;

			uniform float4 _MainTex_ST;
			
			uniform float _Offset;
			
			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				float2 v2offset = (_Offset, _Offset);
				o.vertex.xy += v2offset;
				o.texcoord = v.texcoord;
				return o;
			}

			float4 _ShadowColor;
			
			float4 frag (v2f i) : COLOR
			{
				float4 col = _ShadowColor * tex2D(_MainTex, i.texcoord);
				clip(col.a - 0.4);
				return col;
			}
			ENDCG
        }
        
        
        Pass {
        	Tags {"LightMode" = "Always" "Queue" = "Transparent" }
            Blend SrcAlpha OneMinusSrcAlpha
            SetTexture [_MainTex] {
                constantColor [_Color]
                Combine texture * constant, texture * constant
            }
        }
    }
}
