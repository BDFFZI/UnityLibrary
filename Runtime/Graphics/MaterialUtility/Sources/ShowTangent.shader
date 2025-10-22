Shader "Unlit/ShowTangent"
{
	Properties {}
	SubShader
	{
		Tags
		{
			"RenderPipeline" = "UniversalRenderPipeline"
			"Queue" = "Geometry"
		}

		Pass
		{
			HLSLPROGRAM
			#pragma vertex VertexPass
			#pragma fragment PixelPass
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

			struct Vertex
			{
				float3 positionOS:POSITION;
				float4 tangentOS:TANGENT;
			};

			struct Pixel
			{
				float4 positionCS:SV_POSITION;
				float3 tangentWS:TANGENT;
			};

			Pixel VertexPass(const Vertex vertex)
			{
				Pixel pixel;
				pixel.positionCS = TransformObjectToHClip(vertex.positionOS);
				pixel.tangentWS = TransformObjectToWorldDir(vertex.tangentOS.xyz);
				return pixel;
			}

			float4 PixelPass(const Pixel pixel):SV_TARGET
			{
				return float4(normalize(pixel.tangentWS), 1);
			}
			ENDHLSL
		}
	}
}