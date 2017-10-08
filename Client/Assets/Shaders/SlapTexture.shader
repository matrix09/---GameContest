Shader "Custom/SlapTexture" {

		Properties {
				_SpalTexture ("Slap Texture", 2D) = "White"{}
				[NoScaleOffset] _Texture1 ("Texture1", 2D) = "White"{}
				[NoScaleOffset] _Texture2 ("Texture2", 2D) = "White"{}
				[NoScaleOffset] _Texture3 ("Texture3", 2D) = "White"{}
				[NoScaleOffset] _Texture4 ("Texture4", 2D) = "White"{}		
				_TintColor ("Tint Color", Color) = (1,1,1,1)
		}

		SubShader {

			Pass {

					CGPROGRAM

					#include "UnityCG.cginc"

					#pragma vertex vert
					#pragma fragment frag
					sampler2D _SpalTexture;
					float4 _SpalTexture_ST;
					sampler2D _Texture1;
					sampler2D _Texture2;
					sampler2D _Texture3;
					sampler2D _Texture4;
					float4 _TintColor;

					struct a2v {
						float4 vertex : POSITION;
						float2 uv : TEXCOORD0;

					};

					struct v2f {
						float4 pos : SV_POSITION;
						float2 uv_slap : TEXCOORD1;
						float2 uv : TEXCOORD0;
					};

					v2f vert (a2v v) {

						v2f f;
						f.pos = UnityObjectToClipPos (v.vertex);
						f.uv_slap = v.uv;
						f.uv = TRANSFORM_TEX(v.uv, _SpalTexture);
						return f;

					}
					float4 frag (v2f f) : SV_TARGET
					 {		
					 		float4 slapTexutre = tex2D (_SpalTexture, f.uv_slap);
					 		float3 color = (tex2D(_Texture1, f.uv) * slapTexutre.r +
					 					   tex2D(_Texture2, f.uv) * slapTexutre.g +
					 					   tex2D(_Texture3, f.uv) * slapTexutre.b +
					 					   tex2D(_Texture4, f.uv) * (1 - slapTexutre.r - slapTexutre.g - slapTexutre.b)).rgb * _TintColor.rgb;
	
					 		return float4(color, 1);
					 }		


					ENDCG

			}

		}

}