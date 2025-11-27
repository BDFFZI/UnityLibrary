Shader "ProceduralTexture/DepthToPosition"
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
				float3 positionOS:POSITION;
			};

			struct Fragment
			{
				float4 positionCS:SV_POSITION;
				float2 texcoord :TEXCOORD0;
			};

			sampler2D _MainTex;
			float4x4 _ClipToWorld;

			Fragment VertexPass(Vertex vertex)
			{
				Fragment fragment;
				fragment.positionCS = TransformObjectToHClip(vertex.positionOS);
				fragment.texcoord = vertex.positionOS.xy;
				return fragment;
			}

			float4 FragmentPass(Fragment fragment):SV_TARGET
			{
				float4 positionCS = float4(fragment.texcoord * 2 - 1, tex2D(_MainTex, fragment.texcoord).r, 1);
				float4 positionWS = mul(_ClipToWorld, positionCS);

				return float4(positionWS.xyz / positionWS.w, 1);
			}
			ENDHLSL
		}
	}
}