Shader "Outline Shaders/Outline (Solid)" {
	Properties {
		_OutlineColor ("Outline Color", Color) = (1,1,1,1)
		_Outline ("Outline width", Range (0.0, 5)) = .005
	}
	
	Subshader {
		Tags {"Queue"="Transparent+1000" "IgnoreProjector"="True" "RenderType"="Transparent"}

		Pass {
			Name "BASE"
			Cull Back
			Blend Zero One

			SetTexture [_OutlineColor] {
				ConstantColor (0,0,0,0)
				Combine constant
			}
		}
 
		Pass {
			Name "OUTLINE"
			Tags { "LightMode" = "Always" }
			Cull Front
			Blend One SrcColor
 
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			fixed4 _OutlineColor;
			float _Outline;

			struct appdata {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};
			
			struct v2f { 
				float4 pos : POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);

				float3 norm   = mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal);
				float2 offset = TransformViewToProjection(norm.xy);
				
				o.pos.xy += offset * o.pos.z * _Outline;
				return o;
			}

			half4 frag(v2f i) :COLOR {
				return _OutlineColor;
			}
			ENDCG
		}
 
 
	}
}