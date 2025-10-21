// File: URP_SpriteUnlit_HueShift_3Targets.shader
Shader "Iralem/URP/SpriteUnlit_HueShift_3Targets"
{
    Properties
    {
        _BaseMap ("Sprite Texture", 2D) = "white" {}
        _Color   ("Tint", Color) = (1,1,1,1)

        // ---- Anti-halo básico ----
        _ZeroRGBBelow ("Zero RGB When Alpha < x", Range(0,1)) = 0.02
        _AlphaClip    ("Alpha Clip (hard cut)", Range(0,1)) = 0.0
        _PremultiplyOutput ("Premultiply RGB by A (0/1)", Float) = 0

        // Só aplicar operações se alpha > cutoff
        _AlphaCutoffForOps ("Alpha Cutoff For Ops", Range(0,1)) = 0.001

        // Pixels ~brancos são ignorados por completo
        _WhiteBypassEps ("White Bypass Epsilon", Range(0,0.1)) = 0.002

        // ---- Alvos (cada um com raio e hue shift) ----
        _TargetColor0 ("Target Color 0", Color) = (1,0,0,1)
        _MatchRadius0 ("Match Radius 0", Range(0,1)) = 0.2
        _HueShiftDegrees0 ("Hue Shift 0 (deg)", Range(-180,180)) = 0

        _TargetColor1 ("Target Color 1", Color) = (0,1,0,1)
        _MatchRadius1 ("Match Radius 1", Range(0,1)) = 0.2
        _HueShiftDegrees1 ("Hue Shift 1 (deg)", Range(-180,180)) = 0

        _TargetColor2 ("Target Color 2", Color) = (0,0,1,1)
        _MatchRadius2 ("Match Radius 2", Range(0,1)) = 0.2
        _HueShiftDegrees2 ("Hue Shift 2 (deg)", Range(-180,180)) = 0
    }

    SubShader
    {
        Tags {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
            "RenderPipeline"="UniversalPipeline"
            "CanUseSpriteAtlas"="True"
        }

        Blend One OneMinusSrcAlpha
        Cull Off
        ZWrite Off
        Stencil { Ref 1 Comp Always Pass Keep }

        Pass
        {
            Name "SpriteUnlit"
            Tags { "LightMode"="Universal2D" }

            HLSLPROGRAM
            #pragma vertex   vert
            #pragma fragment frag
            #pragma target 2.0

            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #pragma multi_compile _ _USE_SPRITE_MASK
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            float4 _BaseMap_ST;

            #ifdef ETC1_EXTERNAL_ALPHA
            TEXTURE2D(_AlphaTex);
            SAMPLER(sampler_AlphaTex);
            #endif

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;

                float  _ZeroRGBBelow;
                float  _AlphaClip;
                float  _PremultiplyOutput;
                float  _AlphaCutoffForOps;
                float  _WhiteBypassEps;

                float4 _TargetColor0;
                float  _MatchRadius0;
                float  _HueShiftDegrees0;

                float4 _TargetColor1;
                float  _MatchRadius1;
                float  _HueShiftDegrees1;

                float4 _TargetColor2;
                float  _MatchRadius2;
                float  _HueShiftDegrees2;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
                float4 color      : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv          : TEXCOORD0;
                float4 color       : COLOR;
                UNITY_VERTEX_OUTPUT_STEREO
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                OUT.color = IN.color * _Color;
                return OUT;
            }

            // ---- Helpers RGB<->HSV ----
            float3 rgb2hsv(float3 c)
            {
                float4 K = float4(0., -1./3., 2./3., -1.);
                float4 p = (c.g < c.b) ? float4(c.bg, K.wz) : float4(c.gb, K.xy);
                float4 q = (c.r < p.x) ? float4(p.xyw, c.r) : float4(c.r, p.yzx);

                float d = q.x - min(q.w, q.y);
                float e = 1e-10;
                float h = (d < e) ? 0.0 : abs(q.z + (q.w - q.y) / (6.0 * d + e));
                float s = (q.x < e) ? 0.0 : d / (q.x + e);
                float v = q.x;
                return float3(h, s, v);
            }

            float3 hsv2rgb(float3 c)
            {
                float3 rgb = clamp(abs(frac(c.x + float3(0., 2./6., 4./6.)) * 6. - 3.) - 1., 0., 1.);
                return c.z * lerp(float3(1.,1.,1.), rgb, c.y);
            }

            // Aplica hue shift (graus) e mistura com peso w
            float3 ShiftAndBlend(float3 baseRgb, float hueDeg, float w)
            {
                if (w <= 0.0 || abs(hueDeg) <= 0.0001) return baseRgb;
                float3 hsv = rgb2hsv(baseRgb);
                hsv.x = frac(hsv.x + hueDeg / 360.0);
                float3 shifted = hsv2rgb(hsv);
                shifted.rgb = shifted.rbg;
                return lerp(baseRgb, shifted, saturate(w));
            }

            // Peso por proximidade no RGB: 1 no alvo, 0 a partir do raio
            float WeightToTarget(float3 rgb, float3 target, float radius)
            {
                float r = max(radius, 1e-6);
                float d = distance(rgb, target);
                return saturate(1.0 - d / r);
            }

            half4 frag (Varyings IN) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(IN);

                half4 c = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);
                #ifdef ETC1_EXTERNAL_ALPHA
                c.a = SAMPLE_TEXTURE2D(_AlphaTex, sampler_AlphaTex, IN.uv).r;
                #endif

                // Anti-halo básico
                if (c.a < _ZeroRGBBelow)
                    c.rgb = 0.0;
                if (_AlphaClip > 0.0)
                    clip(c.a - _AlphaClip);

                // Se alpha muito baixo, ou pixel ~branco, ou abaixo do cutoff: não processa hue
                float nearWhite = max(max(abs(c.r - 1.0), abs(c.g - 1.0)), abs(c.b - 1.0));
                bool bypassWhite = (nearWhite <= _WhiteBypassEps);

                if (c.a > _AlphaCutoffForOps && !bypassWhite)
                {
                    // aplica para cada alvo, em sequência (interpolação "uma a uma")
                    float w0 = WeightToTarget(c.rgb, _TargetColor0.rgb, _MatchRadius0);
                    c.rgb = ShiftAndBlend(c.rgb, _HueShiftDegrees0, w0);

                    float w1 = WeightToTarget(c.rgb, _TargetColor1.rgb, _MatchRadius1);
                    c.rgb = ShiftAndBlend(c.rgb, _HueShiftDegrees1, w1);

                    float w2 = WeightToTarget(c.rgb, _TargetColor2.rgb, _MatchRadius2);
                    c.rgb = ShiftAndBlend(c.rgb, _HueShiftDegrees2, w2);
                }

                // Tint padrão
                c *= IN.color;

                // Premultiply opcional
                if (_PremultiplyOutput > 0.5)
                    c.rgb *= c.a;

                return c;
            }
            ENDHLSL
        }
    }

    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
