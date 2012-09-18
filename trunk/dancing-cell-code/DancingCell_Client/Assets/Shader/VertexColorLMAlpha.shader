Shader "Vertex/VertexColorLMAlpha" {
Properties {
    _Color ("Main Color", Color) = (1,1,1,1)
    _MainTex ("Base (RGB)", 2D) = "white" { }
    _LightMap ("Lightmap (RGB)", 2D) = "lightmap" { LightmapMode }
}

SubShader {
    Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        
    Pass {
    	Blend SrcAlpha OneMinusSrcAlpha
        Lighting Off

        BindChannels {
            Bind "Vertex", vertex
			Bind "texcoord1", texcoord0 // lightmap uses 2nd uv
			Bind "texcoord", texcoord1 // main uses 1st uv
            Bind "Color", color
        } 

		SetTexture [_LightMap] {
			constantColor [_Color]
			combine texture * constant
		}
		SetTexture [_LightMap] {
			constantColor (0.5,0.5,0.5,0.5)
			combine previous * constant + primary
		}
		SetTexture [_MainTex] {
			combine texture * previous DOUBLE, texture * primary
		}
    }
}
}
