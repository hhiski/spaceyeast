// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/SunNebula"
{
	Properties{
		_Color("Text Color", Color) = (1,1,1,1)
		_Res("Noise Resolution",Float) = 128
		_NoiseOffset("Noise Offset", Range(-1,2)) = 0.5
		_NoiseOpacity("Color Opacity", Range(0,5)) = 0.9
		_NoiseSpeed("Noise Speed", Range(0,100)) = 1
		_Depth("Depth", Range(0,41)) = 0
	}

		SubShader{


			Tags {"Queue" = "AlphaTest" "ForceNoShadowCasting" = "True" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
			LOD 200

	
		Blend SrcAlpha OneMinusSrcAlpha
			Pass {
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct appdata_t {
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
					UNITY_VERTEX_INPUT_INSTANCE_ID
				};



				struct v2f {
					float4 vertex : SV_POSITION;
					fixed4 color : COLOR;
					float vertexMagnitude : TEXCOORD1;
					UNITY_VERTEX_OUTPUT_STEREO
				};


				uniform fixed4 _Color;

				float _Res;
				float _NoiseOffset;
				float _NoiseOpacity;
				float _NoiseSpeed;
				float _Depth;

				float rand(float3 myVector, float seed) {
					return frac(sin(dot(myVector, float3(12.9898, seed, 45.5432))) * 43758.5453);
				}

				v2f vert(appdata_t v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);

	

					o.vertexMagnitude = length(v.vertex);
					half magnitude = _NoiseOffset / (o.vertexMagnitude); // used as a distance from the planet core. 
					magnitude = (magnitude);


					o.color = float4(v.color.r *  magnitude, v.color.g , v.color.b *  magnitude, magnitude);;
					return o;
				}


				fixed4 frag(v2f i) : SV_Target
				{


					float Rand1 = rand(round(i.vertex * _Res),_Time[0] * _NoiseSpeed);

					Rand1 *= _NoiseOpacity;

					float alpha = (i.color.a) - _Depth;
					fixed4 col = i.color;
					col.a = alpha;
					return col;
				}

				ENDCG
			}
	}
}