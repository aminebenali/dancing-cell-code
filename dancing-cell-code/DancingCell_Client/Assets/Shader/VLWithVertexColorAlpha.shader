Shader "Vertex/VLWithVertexColorAlpha" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB)", 2D) = "white" {}
}


// ------------------------------------------------------------------
// Dual texture cards - draw in two passes

SubShader {
	LOD 100
	
	Alphatest Greater 0
	ZWrite Off
	Blend SrcAlpha OneMinusSrcAlpha 
	ColorMask RGB
	Tags { "Queue"="Transparent" "RenderType"="Transparent" }

	// Vertex lights: add lighting on top of base pass
	Pass {
		Name "BASE"
		Tags {"LightMode" = "Vertex"}

		Material {
			Ambient [_Color]
			Diffuse [_Color]
		}

		Lighting On
		
		//ColorMask RGB

		SetTexture [_MainTex] {
			combine texture * primary
		}
	}
	// Always drawn base pass: texture * vertex color
	Pass {
		Name "BASE"
		Tags {"LightMode" = "Always"}
		Lighting Off
		BindChannels {
            Bind "Vertex", vertex
            Bind "TexCoord", texcoord
            Bind "Color", color
		}
		SetTexture [_MainTex] {
			constantColor [_Color]
			combine texture * primary
		}
		SetTexture [_MainTex] {
			combine texture * previous, texture * primary
		}
	}

}


Fallback "VertexLit"
}
