Shader "Custom/Thickness" {

	Properties{
		_Absorption("Absorption", Range(-10, 10)) = 1
	}

		SubShader{
			Tags {
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
			}


			Pass {
				Cull Back
				Blend One OneMinusSrcAlpha


				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct a2v {
					float4 vertex : POSITION;
				};

				struct v2f {
					float4 pos : SV_POSITION;
					half dist : TEXCOORD0;
				};

				v2f vert(a2v v) {
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					COMPUTE_EYEDEPTH(o.dist);
					return o;
				}

				float _Absorption;

				fixed4 frag(v2f i, fixed facing : VFACE) : COLOR {
					float depth = -_Absorption  * i.dist;
					return half4(depth, depth, depth, depth);
				}
				ENDCG
			}
	}
}