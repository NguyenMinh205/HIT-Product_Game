Shader "Shader Graphs/SpriteMorphShader" {
	Properties {
		[NoScaleOffset] _MainTex ("MainTex", 2D) = "white" {}
		_Squash ("Squash", Float) = 1
		_ShearX ("ShearX", Float) = 0
		_Scale ("Scale", Float) = 1
		_ScaleX ("ScaleX", Float) = 1
		_ScaleY ("ScaleY", Float) = 1
		_TargetColor ("TargetColor", Vector) = (0,0,0,0)
		_ColorLerp ("ColorLerp", Range(0, 1)) = 0
		_AlphaMultiply ("AlphaMultiply", Range(0, 1)) = 1
		_OffsetX ("OffsetX", Float) = 0
		_OffsetY ("OffsetY", Float) = 0
		[HideInInspector] _QueueOffset ("_QueueOffset", Float) = 0
		[HideInInspector] _QueueControl ("_QueueControl", Float) = -1
		[HideInInspector] [NoScaleOffset] unity_Lightmaps ("unity_Lightmaps", 2DArray) = "" {}
		[HideInInspector] [NoScaleOffset] unity_LightmapsInd ("unity_LightmapsInd", 2DArray) = "" {}
		[HideInInspector] [NoScaleOffset] unity_ShadowMasks ("unity_ShadowMasks", 2DArray) = "" {}
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Hidden/Shader Graph/FallbackError"
	//CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
}