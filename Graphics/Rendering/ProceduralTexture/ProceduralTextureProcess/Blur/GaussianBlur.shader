Shader "ProceduralTexture/GaussianBlur"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

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
		half4 _MainTex_TexelSize;
		half _BlurRadius;

		v2f vert(appdata v)
		{
			v2f o;
			o.vertex = TransformObjectToHClip(v.vertex);
			o.uv = v.uv;
			return o;
		}

		half4 frag_horizontalBlur(v2f i) : SV_Target
		{
			half4 col = 0;
			col += tex2D(_MainTex, i.uv + _MainTex_TexelSize.xy * _BlurRadius * half2(-2, 0)) * 0.05; //(-2,0)
			col += tex2D(_MainTex, i.uv + _MainTex_TexelSize.xy * _BlurRadius * half2(-1, 0)) * 0.25; //(-1,0)
			col += tex2D(_MainTex, i.uv + _MainTex_TexelSize.xy * _BlurRadius * half2(0, 0)) * 0.4; //(0,0)
			col += tex2D(_MainTex, i.uv + _MainTex_TexelSize.xy * _BlurRadius * half2(1, 0)) * 0.25; //(1,0)
			col += tex2D(_MainTex, i.uv + _MainTex_TexelSize.xy * _BlurRadius * half2(2, 0)) * 0.05; //(2,0)
			return col;
		}

		half4 frag_verticalBlur(v2f i) : SV_Target
		{
			half4 col = 0;
			col += tex2D(_MainTex, i.uv + _MainTex_TexelSize.xy * _BlurRadius * half2(0, -2)) * 0.05; //(-2,0)
			col += tex2D(_MainTex, i.uv + _MainTex_TexelSize.xy * _BlurRadius * half2(0, -1)) * 0.25; //(-1,0)
			col += tex2D(_MainTex, i.uv + _MainTex_TexelSize.xy * _BlurRadius * half2(0, 0)) * 0.4; //(0,0)
			col += tex2D(_MainTex, i.uv + _MainTex_TexelSize.xy * _BlurRadius * half2(0, 1)) * 0.25; //(1,0)
			col += tex2D(_MainTex, i.uv + _MainTex_TexelSize.xy * _BlurRadius * half2(0, 2)) * 0.05; //(2,0)
			return col;
		}
		ENDHLSL

		Pass
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag_horizontalBlur
			ENDHLSL
		}

		Pass
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag_verticalBlur
			ENDHLSL
		}
	}
}