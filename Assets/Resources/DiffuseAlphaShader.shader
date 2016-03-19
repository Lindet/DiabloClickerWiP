Shader "Separate Alpha Mask" {
    Properties {
         _MainTex ("Base (RGB)", 2D) = "white" {}
         _MainTexMask ("Mask (RGB)", 2D) = "white" {}
     }
     SubShader {
        Tags {"Queue"="Transparent" "RenderType"="Transparent" }
		Pass {
			ZWrite Off
			Blend OneMinusDstColor One
		}																						


         CGPROGRAM
         #pragma surface surf Lambert alpha
 
         sampler2D _MainTex;
		 sampler2D _MainTexMask;
 
         struct Input {
             float2 uv_MainTex;
			 float2 uv_MainTexMask;
         };
         float4 _Color;
 
         void surf (Input IN, inout SurfaceOutput o) {

             half4 c = tex2D (_MainTex, IN.uv_MainTex);
             half4 a = tex2D(_MainTexMask, IN.uv_MainTexMask);
			 
			 if( a.r > 0.7 && a.g > 0.7 && a.b > 0.7)
			 {
				o.Albedo *= c.rgb * a.a;
			 }
			 else
			 {
				o.Albedo = c.rgb;
				o.Alpha = 1;
			 }	

			  		
         }
         ENDCG
     } 
     FallBack "Diffuse"
}