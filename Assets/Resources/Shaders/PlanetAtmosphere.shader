// Universal Atmosphere Light Scattering Effect
// Based on sunlight direction (0,0,0), viewer's direction and the surface normal.


Shader "Custom/AtmosphereLightScattering" {
	Properties{

		_RimPower("Rim Fade Power", Range(0,10)) = 4.82 
		
		_RimInPower("Rim In Fade Power", Range(0,10)) = 3.31 
		_Emission("Emission", color) = (0,0,0)
		_Albedo("Albedo", color) = (0,0,0)
		_Intensity("Sun Intensity", Range(0, 54)) = 8.2
		_DotOffset("Dot Offset", Range(-1, 1)) = 0.6 //Offsets the dot product of the sun position and the planets normal. 
		_AlphaOffset("Alpha Offset", Range(0,1)) = 0.0
		_CutOff("CutOff", Range(-1,1)) = 0.0
		[Toggle(USE_NOISE)]
		_USE_EMISSIONS("Use Noise", Float) = 0

	}
		SubShader{

        
		Tags { "RenderType" = "Fade"  }

			Blend One OneMinusSrcAlpha
	
			CGPROGRAM

			#pragma surface surf Standard  alpha alpha:fade

			#pragma target 3.0


			struct Input {
				float4 color : COLOR;
				float3 worldPos;
				float3 viewDir;
			};

			float _RimPower;

			half3 _Emission;
			half3 _Albedo;
			half _Intensity;
			half _AlphaOffset;
			half _DotOffset;
			half _RimOuterEdge;
			half _USE_EMISSIONS;
			half _CutOff;
			half _RimInPower;

			float rand(float3 myVector, float seed) {
				return frac(sin(dot(myVector, float3(12.9898, seed, 45.5432))) * 43758.5453);
			}

			void surf(Input IN, inout SurfaceOutputStandard o) {

				fixed4 c = IN.color;
				o.Albedo = c.rgb;

				half sunPlanetDotProduct = (dot(o.Normal, normalize(half3(0, 0, 0) - IN.worldPos))) + _DotOffset;
				half viewerPlanetDotProduct = (dot(normalize(IN.viewDir), o.Normal));

				half rim = saturate(1 - viewerPlanetDotProduct);
	
				half edgeInnerFade = pow(rim, _RimInPower );
				half edgeOuterFade = pow(1.00 - rim, _RimPower);
				half edgeSecondOuterFade = pow(15, ((0.5 - min(0.5, 1 - rim)))); //Fades the weird edge highlights away


				half atmosphereThickness = min(edgeInnerFade, edgeOuterFade);

				atmosphereThickness *= sunPlanetDotProduct;
				atmosphereThickness *= _Intensity;

				float pixelRandom = 1;

				if (_USE_EMISSIONS)
					pixelRandom = 0.5 + rand(round(IN.worldPos * 25), _Time[0] *0.004);
				half finalAlpha =  saturate(atmosphereThickness - _AlphaOffset) ;


				o.Alpha = pixelRandom * finalAlpha;
				o.Emission = pixelRandom * (saturate(_Emission * atmosphereThickness  * finalAlpha) );
				o.Albedo = _Albedo;
				o.Occlusion = 0;
			}
			ENDCG

		}

			FallBack "Diffuse"
}

