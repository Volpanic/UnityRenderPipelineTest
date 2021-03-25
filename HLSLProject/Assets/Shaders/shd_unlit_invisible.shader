Shader "Unlit/shd_unlit_invisible"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Alpha ("Alpha", Range(0,1)) = 1.0

        _RimColor ("Rim Color",Color) = (1,1,1,1)
        _RimPower ("Rim Power", Range(0.5,8)) = 3
    }
    SubShader
    {
        Tags {"Queue" = "Transparent" "IgnoreProjector"="True"  "RenderType"="Transparent" }
        LOD 100

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                half3 normal : NORMAL;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Alpha;

            float4 _RimColor;
            float _RimPower;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = normalize(mul(unity_ObjectToWorld, v.normal));
                o.uv.xy = TRANSFORM_TEX(v.uv.xy, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float ambiant = 0.3f;
                float lambert = saturate(dot(i.normal,_WorldSpaceLightPos0));    

                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv.xy) * (ambiant + lambert);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                col.a = _Alpha;

                //Rim Light
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.uv.xyz);

                //Rimb
                float3 rim = pow(1.0 - saturate(dot(viewDirection,i.normal)),_RimPower) * _RimColor;

                return col + float4(rim.xyz,rim.x); 
            }
            ENDCG
        }
    }
}
