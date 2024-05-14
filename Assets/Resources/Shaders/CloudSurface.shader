Shader "Custom/CloudSurface" {
	Properties{
		_Glossiness("Smoothness", Range(0,1)) = 1
		_Metallic("Metallic", Range(0,1)) = 0.0
		_AlphaOffset("Alpha Offset", Range(-1,1)) = -0.05
		_Color("Color", Color) = (1,1,1,1)
		_CutOff("CutOff", Range(-1,1)) = 0.0
	}

		SubShader{
			Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Transparent"}
		
		 Blend One One
		LOD 200
		Cull Off


			CGPROGRAM

		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard  alpha vertex:vert fullforwardshadows 


		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0


		struct Input
		{				
			float vertexAlpha;
			float3 normal: NORMAL;
			float4 pos : SV_POSITION;

		};

		float4 _Color;
		half _Glossiness;
		half _Metallic;
		half _AlphaOffset;
		half vertexAlpha; //Alpha channel of the vertex color will be used as a cloud coverage level. 
		half _CutOff;


		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);

			v.vertex.xyz = normalize(v.vertex.xyz);
			o.pos = v.vertex;
			v.normal = normalize(v.vertex.xyz);

			o.vertexAlpha = v.color.a;

		}

		void surf(Input IN, inout SurfaceOutputStandard o)
		{

			fixed4 c = _Color;

			half finalAlpha =  (c.a*IN.vertexAlpha + _AlphaOffset) ;
			clip( finalAlpha - _CutOff);

			o.Albedo = saturate(c.rgb*IN.vertexAlpha* c.a* + _AlphaOffset);
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = finalAlpha;
		}
		ENDCG
		}
	
}

