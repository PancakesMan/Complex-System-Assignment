Shader "Hidden/Greyscale"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_DeltaX ("DeltaX", Float) = 0.01
		_DeltaY ("DeltaY", Float) = 0.01
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			float _DeltaX;
			float _DeltaY;

			fixed4 frag (v2f i) : SV_Target
			{
				float2 delta = float2(_DeltaX, _DeltaY);
				fixed4 result = 2 * tex2D(_MainTex, i.uv);
				for (int x = -5; x < 6; x++) {
					for (int y = -5; y < 6; y++) {
						result += tex2D(_MainTex, i.uv + fixed2(x, y) * delta);
					}
				}
				return result / 123.0f;
			}
			ENDCG
		}
	}
}
