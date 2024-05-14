// Universal Atmosphere Light Scattering Effect
// Based on sunlight direction (0,0,0) and viewer's direction


Shader "Custom/StarCoronaShader" {
	Properties{

		_RimPower("Rim Power", Range(0,10.0)) = 0.1
		_RimOuterEdge("Outer Edge", Range(0,1.0)) = 0.0
		_Emission("Emission", color) = (0,0,0)
		_Intensity("Sun Intensity", Range(0, 54)) = 1.0
		_DotOffset("Dot Intensity", Range(-1, 1)) = 0.1 //
		_AlphaOffset("Alpha Offset", Range(0,22)) = 0.0

		[Toggle(USE_NOISE)]
		_USE_EMISSIONS("Use Noise", Float) = 0

	}
		SubShader{

		Tags {"Queue" = "Transparent"  "RenderType" = "Transparent"}

		Blend One OneMinusSrcAlpha


		CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma surface surf Standard  alpha  
			
			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			sampler2D _MainTex;

			struct Input {
				float4 color : COLOR;
				float3 worldPos;
				float3 viewDir;
				float2 uv_BumpMap;
			};

			float _RimPower;
			float _RimOuterEdge;
			half3 _Emission;
			half _Intensity;
			half _AlphaOffset;
			half _DotOffset;

			half _USE_EMISSIONS;

			float rand(float3 myVector, float seed) {
				return frac(sin(dot(myVector, float3(12.9898, seed, 45.5432))) * 43758.5453);
			}

			void surf(Input IN, inout SurfaceOutputStandard o) {

			fixed4 c = IN.color;
			o.Albedo = c.rgb * _Emission;

		
			float fullIntensity =  _Intensity;
			half rim = saturate(1 - (dot(normalize(IN.viewDir), o.Normal)));




			half edgeFade = 1 - saturate(pow( rim, _RimPower));



			float pixelRandom = 1;

			if (_USE_EMISSIONS)
			 pixelRandom = 0.5 + rand(round(IN.worldPos * 25), _Time[0] * 0.004);



			o.Alpha = ((c.a* pixelRandom * fullIntensity) * edgeFade) - _AlphaOffset ;
			o.Emission = ((c.rgb* pixelRandom * fullIntensity) * edgeFade) - _AlphaOffset;

			}
			ENDCG
		}
			FallBack "Diffuse"
}

