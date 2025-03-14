Shader "Unlit/UnlitShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        _AtomColor ("Atmosphere Color", Color) = (0, 0, 0, 0)   //光暈的顏色
        _Size ("Size", Range(0, 1)) = 0.1                       //光暈的範圍
        _OutLightPow ("Falloff", Range(1, 10)) = 5              //光暈的係數
        _OutLightStrengh ("Transparency", Range(5, 20)) = 15    //光暈的強度
        //_RimColor ("rim color", Color) = (1, 1, 1, 1)
        //_RimPower ("rim power", range(1, 10)) = 2
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}
        LOD 100

        Pass
        {
            Name "PlaneBase"
            Tags { "LightMode" = "Always"}

            Cull Off
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            //float4 _RimColor;
            //float _RimPower;
            float4 _Color;
            float4 _AtmoColor;
            float _Size;
            float _OutLightPow;
            float _OutLightStrengh;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 worldvertpos : TEXCOORD1;
                UNITY_FOG_COORDS(1)
                float4 NdotV : COLOR;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                //o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                //float3 V = WorldSpaceViewDir(v.vertex);
                //V = mul(unity_WorldToObject, V);
                //o.NdotV.x = saturate(dot(v.uv, normalize(V)));
                //UNITY_TRANSFER_FOG(o,o.vertex);
                o.uv = v.uv;
                o.worldvertpos = mul(unity_ObjectToWorld, v.vertex).xyz;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {

                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                //col.rgb += pow((1 - i.NdotV.x), _RimPower) * _RimColor.rgb;
                return col * _Color;
            }
            ENDCG
        }
        Pass
        {
                Name "AtmosphereBase"
                Tags {"LightMode" = "Always"}
                Cull Front
                Blend SrcAlpha One

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                float4 _Color;
                float4 _AtmoColor;
                float _Size;
                float _OutLightPow;
                float _OutLightStrengh;

                struct appdata
                {
                    float4 vertex : POSITION;
                    float3 normal : TEXCOORD0;
                };

                struct v2f
                {
                    float4 pos : SV_POSITION;
                    float3 normal : TEXCOORD0;
                    float3 worldvertpos : TEXCOORD1;
                };

                v2f vert(appdata v)
                {
                    v2f o;
                    v.vertex.xyz += v.normal * _Size;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.normal = v.normal;
                    o.worldvertpos = mul(unity_ObjectToWorld, v.vertex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    i.normal = normalize(i.normal);
                    float3 viewdir = normalize(i.worldvertpos.xyz - _WorldSpaceCameraPos.xyz);
                    float4 color = _AtmoColor;
                    color.a = pow(saturate(dot(viewdir, i.normal)), _OutLightPow);
                    color.a *= _OutLightStrengh * dot(viewdir, i.normal);
                    return color;
                }
            ENDCG
        }
    }
}
