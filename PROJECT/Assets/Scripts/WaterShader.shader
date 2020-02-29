Shader "Custom/WaterShader"
{
	Properties
	{
	}
	SubShader
	{//E17Left4
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0
		const static int maxColourCount = 2;
	const static float epsilon = 0.0001;

		int waterColourCount;
		float3 waterColours[maxColourCount];
		float waterStartHeights[maxColourCount];
		float waterBlends[maxColourCount];

		float minHeight;
		float maxHeight;

		struct Input {
			float3 worldPos;
		};

		float inverseLerp(float a, float b, float value) {
			return saturate((value - a) / (b - a));
		}

		void surf(Input IN, inout SurfaceOutputStandard o) {
			//sets colours based on height and blends them together
			float heightPercent = inverseLerp(minHeight,maxHeight, IN.worldPos.y);
			for (int i = 0; i < 1; i++) {
				float drawStrength = inverseLerp(-waterBlends[i] / 2 - epsilon, waterBlends[i] / 2, heightPercent - waterStartHeights[i]);
				o.Albedo = o.Albedo * (1 - drawStrength) + waterColours[i] * drawStrength;
			}
		}


		ENDCG
	}
		FallBack "Diffuse"
}
