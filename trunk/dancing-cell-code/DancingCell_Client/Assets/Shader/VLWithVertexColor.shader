Shader "Vertex/VLWithVertexColor" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB)", 2D) = "white" {}
}


// ------------------------------------------------------------------
// Dual texture cards - draw in two passes

SubShader {
	LOD 100
	Tags { "RenderType"="Opaque" }

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
			combine constant * primary
		}
		SetTexture [_MainTex] {
			combine texture * previous, texture * primary
		}
	}
	
	// Vertex lights: add lighting on top of base pass
	Pass {
		Name "BASE"
		Tags {"LightMode" = "Vertex"}
		Blend One One ZWrite Off Fog { Color (0,0,0,0) }
		Material {
			Ambient [_Color]
			Diffuse [_Color]
		}

		Lighting On
		
		ColorMask RGB

		SetTexture [_MainTex] {
			combine texture * primary , texture
		}
	}
}


Fallback "VertexLit"
}
