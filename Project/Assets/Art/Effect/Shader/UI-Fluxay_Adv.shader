//使用说明：
//1.Light Interval值要比Light Duration大 
//2.流光图的流光线 要居中 保持竖直 （以便于调整角度精确性）
//3.流光图的寻址模式：Clamp
Shader "UI/Fluxay_Adv"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
		_MaskTex("Mask Texture(A)",2D)="white"{} 

		_FluxayTex("Fluxay Texture", 2D) = "white" {}
        _FluxayTexRot ("Fluxay rotation", Range(0,360) ) = 0
        [Toggle] _FluxayMoveX ("FluxayMoveX", float) = 0
        [Toggle] _FluxayMoveY ("FluxayMoveY", float) = 0
        [Enum(Back,0,Forward,1)]_MoveDir("Move Direction", int) = 0
        _FluxayMoveRange("FluxayMoveRange", Range(1, 2)) = 1.4
        _LightPower("Light Power", Range(0, 5)) = 1
        _LightDuration("Light Duration",Range(0,10)) = 1 //流动时间
		_LightInterval("Light Interval",Range(0,20)) = 3 //这个间隔包（含流动时间 理论上这个值大于等于_LightDuration）

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="False"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "Default"
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile __ UNITY_UI_CLIP_RECT
            #pragma multi_compile __ UNITY_UI_ALPHACLIP

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;

				half2 texflowlight : TEXCOORD2;
	
                UNITY_VERTEX_OUTPUT_STEREO
            };

            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;

            sampler2D _MainTex;
            float4 _MainTex_ST;  

			sampler2D _MaskTex;

			sampler2D _FluxayTex;
            float4 _FluxayTex_ST;
            float _FluxayTexRot;
            fixed _FluxayMoveX;
            fixed _FluxayMoveY;
            fixed _FluxayMoveRange;
            fixed _LightPower;
            float _MoveDir;
            half _LightInterval;
			half _LightDuration;
            
            float2 TransFormUV(float2 argUV,float4 argTM)
			{
				float2 result = argUV.xy * argTM.xy + (argTM.zw + float2(0.5,0.5) - argTM.xy * 0.5);
				return result;
			}

            half2 RotateUV(half2 uv,half uvRotate)
			{
				half2 outUV;
				half s;
				half c;
				s = sin(uvRotate/57.2958);
				c = cos(uvRotate/57.2958);
				
				outUV = uv - half2(0.5f, 0.5f);
				outUV = half2(outUV.x * c - outUV.y * s, outUV.x * s + outUV.y * c);
				outUV = outUV + half2(0.5f, 0.5f);
				return outUV;
			}

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(v.vertex); 
                OUT.texcoord = v.texcoord;

                fixed2 FluxayUV = TransFormUV(v.texcoord, _FluxayTex_ST);
                FluxayUV = RotateUV(FluxayUV, _FluxayTexRot);
                
                _LightInterval = max(_LightInterval, _LightDuration); //确保_LightInterval>=_LightDuration
                fixed currentTimePassed = fmod(_Time.y, _LightInterval);
                fixed offsetX = currentTimePassed / _LightDuration; 
                offsetX = saturate(offsetX);
                float fitDis = abs(cos(radians(fmod(_FluxayTexRot, 90)-45)))*sqrt(2); //不同的角度，到之后流动范围要有变化

                FluxayUV +=  (offsetX-0.5)* fitDis * _FluxayMoveRange * float2(_FluxayMoveX, _FluxayMoveY) * (_MoveDir*2-1);  //MoveDir做区间变换

                OUT.texflowlight = FluxayUV;

                OUT.color = v.color * _Color;

                return OUT;
            }
                  
            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 cadd = tex2D(_FluxayTex, IN.texflowlight) * _LightPower;  
                float mask = tex2D(_MaskTex, IN.texcoord).r;
                cadd.rgb *= mask;

                half4 color = tex2D(_MainTex, IN.texcoord);
                cadd.rgb *= color.rgb;
                color.rgb += cadd.rgb;
                color.rgb *= color.a;

                color *= IN.color;
                
                #ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif

                return color;
            }
        ENDCG
        }
    }
}


