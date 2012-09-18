Shader "Vertex/Vertex Color CullOff" {
Properties {
    _Color ("Main Color", Color) = (1,1,1,1)
    _MainTex ("Base (RGB)", 2D) = "white" { }
}

SubShader {

    Pass {
    	Tags {"LightMode" = "Always" "IgnoreProjector"="True"}
        Lighting Off
        Cull Off

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
        
}
}
