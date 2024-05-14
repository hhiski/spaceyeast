Shader "Custom/3dUIEffect"
{
	Properties{

		_MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_Res("Noise Resolution",Float) = 128
		_NoiseOffset("Noise Opacity", Range(-1,1)) = 0.5
		_NoiseOpacity("Color Opacity", Range(0,2)) = 0.9
		_NoiseSpeed("Noise Speed", Range(0,100)) = 1
	}

		SubShader{

			Tags {
				"Queue" = "Transparent"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
			}
			Lighting Off Cull Off  ZWrite On
			Blend SrcAlpha OneMinusSrcAlpha


			Stencil
			{
				Ref 2
				Comp NotEqual
				Pass keep
			}

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
					float2 texcoord : TEXCOORD0;
					float4 worldSpacePosition : TEXCOORD1;
					UNITY_VERTEX_OUTPUT_STEREO
				};

				sampler2D _MainTex;
				uniform float4 _MainTex_ST;
				uniform fixed4 _Color;

				float _Res;
				float _NoiseOffset;
				float _NoiseOpacity;
				float _NoiseSpeed;
				float Time_based_random;

				v2f vert(appdata_t v)
				{
					v2f o;

					o.vertex = UnityObjectToClipPos(v.vertex);
					o.color = v.color * _Color;
					o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
	                o.worldSpacePosition = mul( unity_ObjectToWorld, v.vertex);
					return o;
				}

				float rand(float3 myVector, float seed) {
					return frac(sin(dot(myVector, float3(12.9898, seed, 45.5432))) * 43758.5453);
				}

				fixed4 frag(v2f i) : SV_Target
				{
					
				    float4 pixelWorldSpacePosition = i.worldSpacePosition;

				   // Time_based_random = rand(floor( acos(dot(normalize(float3(0.99,0,0.01)),normalize(i.worldSpacePosition))) * _Res),frac(_Time.y * _NoiseSpeed ));
					//Time_based_random = rand(floor( acos(dot(normalize(float3(0.99,0,0.01)),normalize(i.worldSpacePosition))) * _Res),frac(_Time.y * _NoiseSpeed ));
					
					Time_based_random = rand(floor(i.worldSpacePosition * _Res+1888),frac(_Time[0] * _NoiseSpeed ));
					Time_based_random = Time_based_random % 1; 

					Time_based_random *= _NoiseOpacity;

					fixed4 col = float4(_Color.r, _Color.g, _Color.b, saturate(Time_based_random + _NoiseOffset));
					col.a *= (tex2D(_MainTex, i.texcoord).a);
					return col;
				}
				ENDCG
			}
		}
}