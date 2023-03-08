Shader "Custom/MagnifyingScope" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader {
        Tags {"Queue"="Transparent" "RenderType"="Opaque"}
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard

        sampler2D _MainTex;

        struct Input {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o) {
            // Material properties
            o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
            o.Metallic = 0.0;
            o.Smoothness = 0.8;

            // Refractive index of the lens
            float eta = 1.5;

            // Thickness of the lens
            float thickness = 0.05;

            // Calculate the direction of the incoming ray
            float3 worldPos = mul(unity_ObjectToWorld, float4(IN.uv_MainTex, 0, 1)).xyz;
            float3 viewDir = normalize(UnityWorldSpaceViewDir(worldPos));

            // Calculate the point where the ray hits the lens
            float t = -worldPos.z / viewDir.z;
            float3 hitPos = worldPos + t * viewDir;

            // Calculate the normal vector of the lens at the point where the ray hits
            float3 normal = normalize(hitPos - float3(0, 0, -thickness));

            // Calculate the direction of the refractive ray
float3 refractedDir = refract(viewDir, normal, eta);

scss
Copy code
        // Calculate the point where the refracted ray exits the lens
        float3 exitPos = hitPos + thickness * normal;

        // Calculate the texture coordinates of the exit point
        float2 exitUV = mul(unity_WorldToObject, float4(exitPos, 1.0)).xy;

        // Calculate the color of the pixel at the exit point
        o.Albedo = tex2D(_MainTex, exitUV).rgb;

        // Set the metalness and smoothness to 0 to make the lens transparent
        o.Metallic = 0.0;
        o.Smoothness = 0.0;
    }
    ENDCG
}

FallBack "Diffuse"
}