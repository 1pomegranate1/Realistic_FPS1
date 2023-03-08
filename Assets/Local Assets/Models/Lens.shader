Shader "CreationWasteland/Lens" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_RenderTex("Render Texture", 2D) = "white" {}
		_ReticleTex("Reticle", 2D) = "white" {}
		_MetallicSmoothnessTex("Metallic(RGB) Smoothness(A)", 2D) = "white" {}
		_NormalTex("Normal Map", 2D) = "bump" {}
		_Vignette("Vignette", Range(0,1)) = 0.5
		_EdgeDistortion("Edge Distortion", Range(1,5)) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _RenderTex;
		sampler2D _ReticleTex;
		sampler2D _MetallicSmoothnessTex;
		sampler2D _Normal;
		float _Vignette;
		float _EdgeDistortion;

		struct Input {
			float2 uv_MainTex;
		};


		void surf (Input IN, inout SurfaceOutputStandard o) {
			float edgeWeight = distance(IN.uv_MainTex, float2(0.5, 0.5));
			float2 distortionOffset = float2(0.5, 0.5) - IN.uv_MainTex;
			float2 distortionVector = (distortionOffset / length(distortionOffset));
			float distortionMagnitude = length(distortionOffset);
			float2 distortedUV = IN.uv_MainTex + distortionVector * pow(distortionMagnitude, _EdgeDistortion);

			fixed4 rtColor = tex2D (_RenderTex, distortedUV);
			fixed4 lensColor = tex2D(_MainTex, IN.uv_MainTex);
			fixed4 reticleColor = tex2D(_ReticleTex, IN.uv_MainTex);
			fixed4 metSmoothness = tex2D(_MetallicSmoothnessTex, IN.uv_MainTex);
			float vignetteWeight = clamp((1-_Vignette) / edgeWeight, 0.0, 1.0);
			o.Albedo = (reticleColor.a > 0.85) ? reticleColor.rgb : lerp(rtColor.rgb, lensColor.rgb, lensColor.a) * (vignetteWeight);
			
			// Metallic and smoothness come from slider variables
			o.Metallic = metSmoothness.r;
			o.Smoothness = metSmoothness.a;
			o.Normal = UnpackNormal(tex2D(_Normal, IN.uv_MainTex));
			o.Alpha = 1;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
