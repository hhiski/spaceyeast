Shader "Custom/SunSurfaceShader" {
	Properties{
		[HDR] _Color("Color", Color) = (1, 1, 1, 1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_FlowMap("Flow (RG, A noise)", 2D) = "white" {}
		_FlowStrength("Flow Strength", Float) = 1
		_Tiling("Tiling", Float) = 1
		_Speed("Speed", Float) = 1
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_Contrast("Contrast", Range(0,10)) = 1.0
	}
		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM
			#pragma surface surf Standard  fullforwardshadows
			#pragma target 3.0


		sampler2D _MainTex, _FlowMap;
		float _UJump, _Tiling, _Speed, _FlowStrength;


		struct Input {
			float2 uv_MainTex;
			float2 _FlowMap;
		};

		half _Glossiness;
		half _Metallic;
		half _Contrast;
		fixed4 _Color;

		float3 UnpackDerivativeHeight(float4 textureData) {
			float3 dh = textureData.agb;
			dh.xy = dh.xy * 2 - 1;
			return dh;
		}

		float3 FlowUVW(
			float2 uv, float2 flowVector,float tiling, float time, bool flowB
		) {
			float phaseOffset = flowB ? 0.5 : 0;
			float progress = frac(time + phaseOffset);
			float3 uvw;
			uvw.xy = uv - flowVector * (progress + phaseOffset);
			uvw.xy *= tiling;
			uvw.xy += phaseOffset;
			uvw.xy += (time - progress) * 0.25;
			uvw.z = 1 - abs(1 - 2 * progress);
			return uvw;
		}

		void surf(Input IN, inout SurfaceOutputStandard o) {
			float noise = tex2D(_FlowMap, IN.uv_MainTex).a;
			float time = _Time.y * _Speed + noise;
			float2 flowVector = tex2D(_FlowMap, IN.uv_MainTex).gb;
			flowVector *= _FlowStrength;

			float3 uvwA = FlowUVW(
				IN.uv_MainTex, flowVector,
				 _Tiling, time, false
			);
			float3 uvwB = FlowUVW(
				IN.uv_MainTex, flowVector,
				 _Tiling, time, true
			);
			float3 uvwC = FlowUVW(
				IN._FlowMap, flowVector,
				_Tiling, 2 * time, false
			);

			fixed4 texA = tex2D(_MainTex, uvwA.xy) * uvwA.z;
			fixed4 texB = tex2D(_MainTex, uvwB.xy) * uvwB.z;

			fixed4 c =  (texA + texB) * _Color;
			c = saturate(lerp(half4(0.5, 0.5, 0.5,1), c, _Contrast));
			o.Albedo = c.rgb;
			o.Emission = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
		}

			FallBack "Diffuse"
}