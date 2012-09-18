Shader "Vertex/Vertex Color Alpha" {
Properties {
    _Color ("Main Color", Color) = (1,1,1,1)
    _MainTex ("Base (RGB)", 2D) = "white" { }
}

SubShader {
    Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        
    Pass {
    	Blend SrcAlpha OneMinusSrcAlpha
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
			combine texture * previous
		}
    }
}
}
