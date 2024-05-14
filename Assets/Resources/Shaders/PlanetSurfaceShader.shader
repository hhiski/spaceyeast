//A planetary body universal shader
// Enables liqued terrain types, like water or lava effects. 
// Vertex magnitudes under specified 'liquidLevel' gets minium values (liquids are flat), and alternative colors scheme. 
// Enables variable terrain metallicity and smoothess based on 'closeness' to the selected 'ore' color.


Shader "Custom/PlanetSurfaceShader" {
	Properties{
		_MainTex("Splat Map", 2D) = "white" {}

		_OreColor("Ore Color", Color) = (1,1,1,1)
		_OreSmoothness("Ore Smoothness", Range(0,1)) = 0.5
		_OreMetallicity("Ore Metallic", Range(0,10)) = 1.0
		_BaseMetallicity("Base Metallicity", Range(0,1)) = 0.0
		_BaseSmoothness("Base Smoothness", Range(0,1)) = 0.0
		_BaseOreCloseness("Base/Ore Closeness", Range(0,1)) = 0.0 

		_LiquidLevel("Liquid Level", Range(0,2)) = 1
		_LiquidMaxDepth("Liquid Max Depth", Range(0, 1)) = 0.01
  [HDR] _LiquidEmission("Liquid Color", Color) = (0, 0, 0, 0)
		_LiquidShoreValue("Liquid Shore Value", Range(0,10)) = 0.5 //How much HSV values multiplies closer with vertices closer to LiquidLevel, eg. Lava might appear cooler (value < 1) and water bodies brighter ( 1 > 0)
		_LiquidMetallicity("Liquid Metallicity", Range(0,1)) = 0.5
		_LiquidSmoothness("Liquid Smoothness", Range(0,1)) = 0.5
		
		[Toggle(USE_EMISSIONS)]
		_USE_EMISSIONS("Use Emissions", Float) = 0


	}
		SubShader{
			Tags { "RenderType" = "Opaque" }





			LOD 200

			CGPROGRAM
			#pragma surface surf Standard  vertex:vert fullforwardshadows 
			#pragma target 4.0
			#include "UnityCG.cginc" // for UnityObjectToWorldNormal
			#include "UnityLightingCommon.cginc" // for _LightColor0

			struct Input {
				float3 vertexColor;
				float3 normal: NORMAL;
				float4 pos : SV_POSITION;
				float2 uv_MainTex;
				float vertexMagnitude : TEXCOORD1;
			};

			sampler2D _MainTex;

			half _BaseSmoothness;
			half _BaseMetallicity;

			half _OreMetallicity;
			half _OreSmoothness;
			half _BaseOreCloseness;

			fixed4 _OreColor;

			half _LiquidLevel;
			float4 _LiquidEmission;
			half _LiquidShoreValue;
			half _LiquidMaxDepth;
			half _LiquidMetallicity;
			half _LiquidSmoothness;
			half _USE_EMISSIONS;
			struct v2f {
				float3 pos : SV_POSITION;
				fixed4 color : COLOR;
			};

			float InverseLerp(float a, float b, float value)
			{
				return saturate((value - a) / (b - a));
			}

			void vert(inout appdata_full v, out Input o)
			{
				UNITY_INITIALIZE_OUTPUT(Input, o);
				o.pos = v.vertex;
				o.normal = v.normal;

				o.vertexColor = v.color; // Save the Vertex Color in the Input for the surf() method
				o.vertexMagnitude = length(v.vertex);

				half liquidLevel = _LiquidLevel;

				if (o.vertexMagnitude < liquidLevel) {

					v.vertex.xyz = normalize(v.vertex.xyz);
					o.pos = v.vertex* liquidLevel;
					v.normal = normalize(v.vertex.xyz);

				}
			}



			 void surf(Input IN, inout SurfaceOutputStandard o)
			 {

				   o.Albedo = IN.vertexColor * tex2D(_MainTex, IN.uv_MainTex); // normal, preset planet colors
				   o.Occlusion = 1;

				   half magnitude = IN.vertexMagnitude; // used as a distance from the planet core. 

				   half deepLiquid = _LiquidLevel - _LiquidMaxDepth;
	

				   half liquidDepth = min(1, max(0, (magnitude - deepLiquid) / (_LiquidLevel - deepLiquid)));


				   if (magnitude <= _LiquidLevel) {
					   o.Emission = lerp(_LiquidEmission * _LiquidShoreValue, _LiquidEmission, liquidDepth) * _USE_EMISSIONS;
					   o.Smoothness = _LiquidSmoothness;
					   o.Metallic = _LiquidMetallicity;
					   o.Albedo = lerp(_LiquidEmission * _LiquidShoreValue, _LiquidEmission, liquidDepth);
		
				   }
				   else {
					   o.Emission = 0;

					   if (_BaseOreCloseness != 0) {

						   float oreConcentration = 1 - length(IN.vertexColor - _OreColor);
						   oreConcentration = saturate((oreConcentration - min(oreConcentration, _BaseOreCloseness)) * (1 / (1 - _BaseOreCloseness)));

						   o.Smoothness = lerp(_BaseSmoothness, _OreSmoothness, oreConcentration);
						   o.Metallic = lerp(_BaseMetallicity, _OreMetallicity, oreConcentration);

					   }
					   else {

						   o.Smoothness = _BaseSmoothness;
						   o.Metallic = _BaseMetallicity;
					   }
				   }
			 }
			ENDCG


	}
		FallBack "Diffuse"
}