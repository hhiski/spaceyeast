// Universal Atmosphere Light Scattering Effect
// Based on sunlight direction (0,0,0) and viewer's direction


Shader "Custom/Rim" {
	Properties{

		_RimPower("Rim Power", Range(0,10.0)) = 0.1
		_RimOuterEdge("Outer Edge", Range(0,1.0)) = 0.0
		_Color("_Color", color) = (0,0,0)
		_Intensity("Sun Intensity", Range(0, 54)) = 1.0
		_DotOffset("Dot Intensity", Range(-1, 1)) = 0.1 //
		_AlphaOffset("Alpha Offset", Range(-10,10)) = 0.0


	}
		SubShader{

		Tags {"Queue" = "Transparent"  "RenderType" = "Transparent"}

		Blend One OneMinusSrcAlpha


		CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma surface surf Standard  alpha  
			
			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0
			float _AlphaOffset;
			float _Intensity;
			sampler2D _MainTex;
			float _RimPower;
			struct Input {
				float4 color : COLOR;
				float3 worldPos;
				float3 viewDir;
			};


			void surf(Input IN, inout SurfaceOutputStandard o) {

			fixed4 c = IN.color;
			o.Albedo = c.rgb ;

			half viewerPlanetDotProduct = (dot(normalize(IN.viewDir), o.Normal));
			half edgeFade = _Intensity * saturate(pow( viewerPlanetDotProduct, _RimPower));

			o.Alpha = edgeFade + _AlphaOffset;
			

			}
			ENDCG
		}
			FallBack "Diffuse"
}

