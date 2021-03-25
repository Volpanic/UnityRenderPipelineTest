Shader "PostProcessing/shd_gaussian"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _GaussianStrength("_Gaussian Strength", Range(0,100)) = 8
        _GaussianQuality("_Gaussian Quality", Range(1, 10)) = 3
        
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float _GaussianStrength;
            float _GaussianQuality;

            const float Pi = 6.283185f;

            fixed4 frag (v2f i) : SV_Target
            {
                float2 radius = (_GaussianStrength / 2) / _ScreenParams.xy; 
                fixed4 col = tex2D(_MainTex, i.uv);

                for(float d = 0.0f; d < Pi; d += Pi/_GaussianStrength)
                {
                    for(float k = 1.0f / _GaussianQuality; k <= 1.0f; k += 1.0/_GaussianQuality)
                    {
                        col += tex2D(_MainTex,i.uv + float2(cos(d),sin(d)) * (_GaussianStrength / 2.0f) * k);
                    }
                }

                col /= _GaussianQuality * _GaussianStrength - (_GaussianStrength-1);
                return col;
            }
            ENDCG
        }
    }
}
