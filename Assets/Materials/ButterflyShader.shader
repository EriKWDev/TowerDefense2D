Shader "Custom/ButterflyShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Cutoff ("Alpha cutoff", Range(0, 1)) = 0.5
		_ColorOfAlpha ("Color of Alpha", Color) = (1,1,1,1)
		_MainTex ("Albedo", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="transparent" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Lambert alpha

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _Color;
		fixed4 _ColorOfAlpha;
		float _Cutoff;

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);

			c.r=c.r-_Color.r*(2*c.r-c.g-c.b);
			c.g=c.g-_Color.g*(2*c.g-c.r-c.b);
			c.b=c.b-_Color.b*(2*c.b-c.r-c.g);

			o.Albedo = c.rgb;

			if (c.r <= _ColorOfAlpha.r+_Cutoff && c.r >= _ColorOfAlpha.r-_Cutoff && c.g <= _ColorOfAlpha.g+_Cutoff && c.g >= _ColorOfAlpha.g-_Cutoff && c.b <= _ColorOfAlpha.b+_Cutoff && c.b >= _ColorOfAlpha.b-_Cutoff)
				o.Alpha = 0;
			else 
				o.Alpha = 1;
			
		}
		ENDCG
	}
	FallBack "Diffuse"
}
