Shader "Vertex/Vertex Color CutOut" {
Properties {
    _Color ("Main Color", Color) = (1,1,1,1)
    _MainTex ("Base (RGB)", 2D) = "white" { }
    _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
}

SubShader {

    Pass {
    	Tags {"LightMode" = "Always"}
        Lighting Off
        Alphatest Greater [_Cutoff]

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
