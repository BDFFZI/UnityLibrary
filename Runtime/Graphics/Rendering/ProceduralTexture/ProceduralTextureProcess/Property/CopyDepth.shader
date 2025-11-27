Shader "ProceduralTexture/CopyDepth"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Pass
		{
			HLSLPROGRAM
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
			float4x4 _ClipToWorld;

			Fragment VertexPass(Vertex v)
			{
				Fragment fragment;
				fragment.positionCS_SV = TransformObjectToHClip(v.positionOS);
				fragment.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return fragment;
			}

			float4 FragmentPass(Fragment fragment) : SV_Target
			{
				float depthCS = tex2D(_MainTex, fragment.uv);
				return float4(depthCS, 0, 0, 1);
			}
			ENDHLSL
		}
	}
}