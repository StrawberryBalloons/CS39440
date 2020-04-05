Shader "Custom/ExampleShader"
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
		const static int maxColourCount = 6;
	const static float epsilon = 0.0001;

		int baseColourCount;
		float3 baseColours[maxColourCount];
		float baseStartHeights[maxColourCount];
		float baseBlends[maxColourCount];

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
			for (int i = 0; i < maxColourCount; i++) {
				float drawStrength = inverseLerp(-baseBlends[i]/2 - epsilon, baseBlends[i]/2, heightPercent - baseStartHeights[i]);
				o.Albedo = o.Albedo * (1 - drawStrength) + baseColours[i] * drawStrength;
			}
		}


        ENDCG
    }
    FallBack "Diffuse"
}
