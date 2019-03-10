Shader "Unlit/OrbShader"
{
    Properties
    {
        _MainColor ("Main Color", Color) = (1, 1, 1, 1)
        _OutlineColor ("Outline Color", Color) = (0, 0, 0, 1)
        _OutlineThickness ("Outline Thickness", Float) = 0.1
    }
    SubShader
    {
        

        Pass
        {
            Tags { "RenderType"="Opaque" }
        
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            float4 _MainColor;

            fixed4 frag (v2f i) : SV_Target
            {
                return _MainColor;
            }
            ENDCG
        }
        
        Pass
        {
            Tags { "RenderType"="Opaque" }
            Blend SrcAlpha OneMinusSrcAlpha
            Zwrite Off
        
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            float _OutlineThickness;

            v2f vert (appdata v)
            {
                v2f o;
                float3 newPos = v.vertex + normalize(v.normal) * _OutlineThickness;
                o.vertex = UnityObjectToClipPos(float4(newPos, 0));
                return o;
            }

            float4 _OutlineColor;

            fixed4 frag (v2f i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDCG
        }
    }
}
