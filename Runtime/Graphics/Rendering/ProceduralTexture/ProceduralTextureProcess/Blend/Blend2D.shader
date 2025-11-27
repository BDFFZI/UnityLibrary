Shader "Hidden/Blend"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		HLSLINCLUDE
		#pragma vertex VertexPass
		#pragma fragment FragmentPass

		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

		struct Vertex
		{
			float3 positionOS : POSITION;
			float2 uv : TEXCOORD0;
		};

		struct Fragment
		{
			float4 positionCS_SV : SV_POSITION;
			float2 uv : TEXCOORD0;
		};

		sampler2D _MainTex;
		float4 _MainTex_ST;
		sampler2D _BlendTex;

		Fragment VertexPass(Vertex v)
		{
			Fragment fragment;
			fragment.positionCS_SV = TransformObjectToHClip(v.positionOS);
			fragment.uv = TRANSFORM_TEX(v.uv, _MainTex);
			return fragment;
		}

		float4 Blend(float4 source, float4 destination);

		float4 FragmentPass(Fragment fragment) : SV_Target
		{
			float4 destinationColor = tex2D(_MainTex, fragment.uv);
			float4 sourceColor = tex2D(_BlendTex, fragment.uv);

			return Blend(sourceColor, destinationColor);
		}
		ENDHLSL

		Pass
		{
			HLSLPROGRAM
			float4 Blend(float4 source, float4 destination)
			{
				return float4(source.rgb + destination.rgb, source.a);
			}
			ENDHLSL
		}
		Pass
		{
			HLSLPROGRAM
			float4 Blend(float4 source, float4 destination)
			{
				return float4(source.rgb * destination.rgb, source.a);
			}
			ENDHLSL
		}
		Pass
		{
			HLSLPROGRAM
			float4 Blend(float4 source, float4 destination)
			{
				return float4(source.rgb * source.a + destination.rgb * (1 - source.a), source.a);
			}
			ENDHLSL
		}
	}
}