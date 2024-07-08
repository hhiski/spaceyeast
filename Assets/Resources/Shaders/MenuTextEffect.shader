Shader "Custom/MenuTextEffect"
{
	Properties{

		_MainTex("Font Texture", 2D) = "white" {}
		_Color("Text Color", Color) = (1,1,1,1)
		_Res("Noise Resolution",Float) = 128
		_NoiseOffset("Noise Opacity", Range(-1,1)) = 0.5
		_NoiseOpacity("Color Opacity", Range(0,2)) = 0.9
		_NoiseSpeed("Noise Speed", Range(0,100)) = 1
	}

		SubShader{

			Tags {
				"Queue" = "Transparent+1"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
			}
			Lighting Off Cull Off ZTest Always ZWrite On
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
				#pragma multi_compile _ UNITY_SINGLE_PASS_STEREO STEREO_INSTANCING_ON STEREO_MULTIVIEW_ON
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
					UNITY_VERTEX_OUTPUT_STEREO
				};

				sampler2D _MainTex;
				uniform float4 _MainTex_ST;
				uniform fixed4 _Color;

				float _Res;
				float _NoiseOffset;
				float _NoiseOpacity;
				float _NoiseSpeed;


				v2f vert(appdata_t v)
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.color = v.color * _Color;
					o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
					return o;
				}

				float rand(float3 myVector, float seed) {
					return frac(sin(dot(myVector, float3(12.9898, seed, 45.5432))) * 43758.5453);
				}

				fixed4 frag(v2f i) : SV_Target
				{


					float Rand1 = rand(round(i.vertex * _Res),_Time[0] * _NoiseSpeed);

					Rand1 *= _NoiseOpacity;


					fixed4 col = float4(_Color.r, _Color.g, _Color.b, Rand1 + _NoiseOffset);
					col.a *= tex2D(_MainTex, i.texcoord).a;
					col.a = saturate(col.a);
					return col;
				}
				ENDCG
			}
		}
}