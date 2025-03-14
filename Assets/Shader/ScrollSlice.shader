Shader "Unlit/ScrollSlice"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BumpMap ("BumpMap", 2D) = "bump" {}
        _ClipSpeed ("Clip Speed", Range(0, 2)) = 0.1
    }
    SubShader
    {
        Tags 
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"

        }
        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;
        sampler2D _BumpMap;
        float _ClipSpeed;
        struct Input 
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
            float3 worldPos;
        };

        void surf(Input IN, inout SurfaceOutput o) 
        {
            clip(frac((IN.worldPos.y + IN.worldPos.z * 0.01 + _Time.y * _ClipSpeed) * 5) - 0.5);
            o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
        } 
        ENDCG
    }
}
