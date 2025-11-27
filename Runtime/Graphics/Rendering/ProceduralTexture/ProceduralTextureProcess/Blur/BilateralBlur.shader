Shader "Hidden/BilateralBlur"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags
		{
			"RenderType"="Opaque"
		}
		LOD 100

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
				float4 positionCS : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _MainTex_TexelSize;
			float _BlurRadius;
			float _SpatialDeviation;
			float _TonalDeviation;

			float GetGaussianWeight(float difference, float sigma)
			{
				return 1 / (sigma * sqrt(2 * PI)) * exp(-(difference * difference) / (2 * sigma * sigma));
			}

			float4 GetGaussianWeight(float4 difference, float sigma)
			{
				return 1 / (sigma * sqrt(2 * PI)) * exp(-(difference * difference) / (2 * sigma * sigma));
			}

			Fragment VertexPass(Vertex vertex)
			{
				Fragment fragment;
				fragment.positionCS = TransformObjectToHClip(vertex.positionOS);
				fragment.uv = TRANSFORM_TEX(vertex.uv, _MainTex);
				return fragment;
			}

			float4 FragmentPass(Fragment fragment) : SV_Target
			{
				float2 centerUv = fragment.uv;
				float4 centerColor = tex2D(_MainTex, centerUv);

				float4 numerator = 0;
				float4 denominator = 0;
				for (int x = -2; x <= 2; ++x)
				{
					for (int y = -2; y <= 2; ++y)
					{
						float2 offset = float2(x, y) * _MainTex_TexelSize.xy * _BlurRadius;
						float2 currentUv = centerUv + offset;
						float4 currentColor = tex2D(_MainTex, currentUv);

						float spatialDifference = length(offset);
						float4 tonalDifference = currentColor - centerColor;
						float4 weight = GetGaussianWeight(spatialDifference, _SpatialDeviation) * GetGaussianWeight(tonalDifference, _TonalDeviation);
						numerator += weight * currentColor;
						denominator += weight;
					}
				}
				return numerator / denominator;
			}
			ENDHLSL
		}
	}
}