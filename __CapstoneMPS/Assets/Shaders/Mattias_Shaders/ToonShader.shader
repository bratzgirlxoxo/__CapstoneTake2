Shader "Unlit/ToonShader"
{
    Properties
    {
        // the color at the shallowest point
        _DepthGradientShallow("Depth Gradient Shallow", Color) = (0.325, 0.807, 0.971, 0.725)
        // the color at the deepest point
        _DepthGradientDeep("Depth Gradient Deep", Color) = (0.086, 0.407, 1, 0.749)
        // the max depth color distanace
        _DepthMaxDistance("Depth Maximum Distance", Float) = 1
        // perlin noise texture for waves
        _SurfaceNoise("Surface Noise", 2D) = "white" {}
        // Distortion Texture
        _SurfaceDistortion("Surface Distortion", 2D) = "white" {}
        // Strength of Distortion
        _SurfaceDistortionAmount("Surface Distortion Amount", Range(0,1)) = 0.27
        // Controls scroll speed, in UV's per second
        _SurfaceNoiseScroll("Surface Noise Scroll Amount", Vector) = (0.03, 0.03, 0, 0)
        // cutoff value for surface noise waves
        _SurfaceNoiseCutoff("Surface Noise Cutoff", Range(0,1)) = 0.777
        // control the depth of the shoreline foam
        _FoamMaxDistance("Foam Max Distance", Float) = 0.4
        // control the depth of other foam
        _FoamMinDistance("Foam Min Distance", Float) = 0.04
    }
    SubShader
    {
        Tags { "Queue"="Transparent" }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
        
            CGPROGRAM


            
            #pragma vertex vert
            #pragma fragment frag


            #include "UnityCG.cginc"
            
            float4 alphaBlend (float4 top, float4 bottom) {
                float3 color = (top.rgb * top.a) + (bottom.rgb * (1 - top.a));
                float alpha = top.a + bottom.a * (1 - top.a);
                
                return float4(color, alpha);
            }

            struct vertIn
            {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 distortUV : TEXCOORD1;
                float4 screenPosition : TEXCOORD2;
                float2 noiseUV : TEXCOORD0;
                float3 viewNormal : NORMAL;
            };

            
            sampler2D _SurfaceNoise;
            float4 _SurfaceNoise_ST;
            
            sampler2D _SurfaceDistortion;
            float4 _SurfaceDistortion_ST;
            

            v2f vert (vertIn v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.screenPosition = ComputeScreenPos(o.vertex);
                
                o.noiseUV = TRANSFORM_TEX(v.uv, _SurfaceNoise);
                o.distortUV = TRANSFORM_TEX(v.uv, _SurfaceDistortion);
                
                o.viewNormal = COMPUTE_VIEW_NORMAL;
                
                return o;
            }
            

            float4 _DepthGradientShallow;
            float4 _DepthGradientDeep;
            float _DepthMaxDistance;
            float _SurfaceNoiseCutoff;
            float2 _SurfaceNoiseScroll;
            
            float _FoamMaxDistance;
            float _FoamMinDistance;
            
            sampler2D _CameraDepthTexture;
            sampler2D _CameraNormalsTexture;
            
            float _SurfaceDistortionAmount;

            
            fixed4 frag (v2f i) : SV_Target
            {
                
                float existingDepth01 = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPosition)).r;
                float existingDepthLinear = LinearEyeDepth(existingDepth01);
                float depthDifference = existingDepthLinear - i.screenPosition.w;
                
                float3 existingNormal = tex2Dproj(_CameraNormalsTexture, UNITY_PROJ_COORD(i.screenPosition));
                float3 normalDot = saturate(dot(existingNormal, i.viewNormal));
                
                float foamDistance = lerp(_FoamMaxDistance, _FoamMinDistance, normalDot);
                float foamDepthDifference01 = saturate(pow(depthDifference / foamDistance, 2));
                
                float waterDepthDifference01 = saturate(depthDifference / _DepthMaxDistance);
                float4 waterColor = lerp(_DepthGradientShallow, _DepthGradientDeep, waterDepthDifference01);

                float2 distortSample = (tex2D(_SurfaceDistortion, i.distortUV).xy * 2 - 1) * _SurfaceDistortionAmount;
                float2 noiseUV = float2(i.noiseUV.x + _Time[1] * _SurfaceNoiseScroll.x + distortSample.x, 
                                        i.noiseUV.y + _Time[1] * _SurfaceNoiseScroll.y + distortSample.y);
                float surfaceNoiseSample = tex2D(_SurfaceNoise, noiseUV).r;
                float surfaceNoiseCutoff = foamDepthDifference01 * _SurfaceNoiseCutoff;
                float surfaceNoise = surfaceNoiseSample > surfaceNoiseCutoff ? 1 : 0;

                return waterColor + surfaceNoise;
            }
            ENDCG
        }
    }
}
