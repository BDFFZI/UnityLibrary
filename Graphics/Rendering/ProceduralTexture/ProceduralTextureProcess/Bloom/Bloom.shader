Shader "ProceduralTexture/Bloom"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		HLSLINCLUDE
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

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

		sampler2D _MainTex;
		sampler2D _BloomTex;
		half _Intensity;
		half _Threshold;
		half _Scatter;

		v2f vert(appdata v)
		{
			v2f o;
			o.vertex = TransformObjectToHClip(v.vertex);
			o.uv = v.uv;
			return o;
		}
		ENDHLSL

		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass//找出发光区域
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag


			half4 frag(v2f i) : SV_Target
			{
				half4 col = tex2D(_MainTex, i.uv);
				half maxColor = max(max(col.r, col.g), col.b);
				half intensity = max(0, maxColor - _Threshold) / max(maxColor, 0.0001f);
				return col * intensity;
			}
			ENDHLSL
		}

		Pass//合并辉光效果
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			half4 frag(v2f i) : SV_Target
			{
				return lerp(tex2D(_MainTex, i.uv), tex2D(_BloomTex, i.uv), _Scatter);
			}
			ENDHLSL
		}

		Pass//为原图像添加辉光
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			half4 frag(v2f i) : SV_Target
			{
				return tex2D(_MainTex, i.uv) + tex2D(_BloomTex, i.uv) * _Intensity;
			}
			ENDHLSL
		}
	}
}