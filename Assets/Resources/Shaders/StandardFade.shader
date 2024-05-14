Shader "My Shaders/ColorAdditiveShader" {
	Properties{
		_Color("Main Color, Alpha", Color) = (1,1,1,1)
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
	}
		Category{
			ZWrite Off
			Lighting Off
			Tags {Queue = Transparent}
			Blend One One
			Color[_Color]
			SubShader {
				Pass {
					Cull Back
				}
			}
	}
}
