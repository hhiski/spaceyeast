Shader "Custom/StarShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" { }
		_Color("Main Color", Color) = (1,1,1,1)
		_Glow("Glow Intensity", Range(0, 1)) = 0.5
		_Speed("Movement Speed", Range(0, 10)) = 1
	}

		CGINCLUDE
#include "UnityCG.cginc"

			struct appdata
		{
			float4 vertex : POSITION;
		};

		struct v2f
		{
			float4 pos : POSITION;
		};



		v2f vert(appdata v)
		{
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			return o;
		}
		ENDCG

			SubShader
		{
			Tags {"Queue" = "Overlay" }
			LOD 100

			CGPROGRAM
			#pragma surface surf Lambert

			sampler2D _MainTex;
			fixed4 _Color;
			fixed _Glow;
			fixed _Speed;

			struct Input
			{
				float2 uv_MainTex;
			};

			void surf(Input IN, inout SurfaceOutput o)
			{
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				o.Albedo = c.rgb;
				o.Alpha = c.a;

				float time = _Time.y * _Speed;
				float pulse = sin(time) * 0.5 + 0.5;

				o.Emission = c.rgb * pulse * _Glow;
			}
			ENDCG
		}

			Fallback "Diffuse"
}