Shader "Custom/SkyboxBlend"
{
    Properties
    {
        _DayTex ("Day Texture", 2D) = "white" {}
        _NightTex ("Night Texture", 2D) = "black" {}
        _Blend ("Blend", Range(0,1)) = 0
    }

    SubShader
    {
        Tags { "Queue"="Background" "RenderType"="Opaque" }
        Cull Off ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _DayTex;
            sampler2D _NightTex;
            float _Blend;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 texcoord : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv;
                uv.x = atan2(i.texcoord.z, i.texcoord.x) / (2 * UNITY_PI) + 0.5;
                uv.y = asin(i.texcoord.y) / UNITY_PI + 0.5;

                fixed4 day = tex2D(_DayTex, uv);
                fixed4 night = tex2D(_NightTex, uv);

                return lerp(day, night, _Blend);
            }
            ENDCG
        }
    }
}