Shader "Custom/RainShader" {
	Properties {
		_MainTex ("Color (RGB) Alpha (A)", 2D) = "gray" {}
		_TintColor ("Tint Color (RGB)", Vector) = (1,1,1,1)
		_PointSpotLightMultiplier ("Point/Spot Light Multiplier", Range(0, 10)) = 2
		_DirectionalLightMultiplier ("Directional Light Multiplier", Range(0, 10)) = 1
		_InvFade ("Soft Particles Factor", Range(0.01, 100)) = 1
		_AmbientLightMultiplier ("Ambient light multiplier", Range(0, 1)) = 0.25
	}
	SubShader {
		LOD 100
		Tags { "IGNOREPROJECTOR" = "true" "LIGHTMODE" = "Vertex" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			LOD 100
			Tags { "IGNOREPROJECTOR" = "true" "LIGHTMODE" = "Vertex" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			ColorMask RGB -1
			ZClip Off
			ZWrite Off
			Lighting On
			GpuProgramID 21411
			Program "vp" {
				SubProgram "d3d9 " {
					"vs_3_0
					
					//
					// Generated by Microsoft (R) HLSL Shader Compiler 10.1
					//
					// Parameters:
					//
					//   float _AmbientLightMultiplier;
					//   float _DirectionalLightMultiplier;
					//   float4 _MainTex_ST;
					//   float _PointSpotLightMultiplier;
					//   float4 _TintColor;
					//   float4 glstate_lightmodel_ambient;
					//   float4 unity_LightAtten[8];
					//   float4 unity_LightColor[8];
					//   float4 unity_LightPosition[8];
					//   row_major float4x4 unity_MatrixV;
					//   row_major float4x4 unity_MatrixVP;
					//   row_major float4x4 unity_ObjectToWorld;
					//
					//
					// Registers:
					//
					//   Name                        Reg   Size
					//   --------------------------- ----- ----
					//   unity_LightColor            c0       4
					//   unity_LightPosition         c4       4
					//   unity_LightAtten            c8       4
					//   unity_ObjectToWorld         c12      4
					//   unity_MatrixV               c16      4
					//   unity_MatrixVP              c20      4
					//   glstate_lightmodel_ambient  c24      1
					//   _TintColor                  c25      1
					//   _DirectionalLightMultiplier c26      1
					//   _PointSpotLightMultiplier   c27      1
					//   _AmbientLightMultiplier     c28      1
					//   _MainTex_ST                 c29      1
					//
					
					    vs_3_0
					    def c30, 1, 0, 2, 0
					    dcl_position v0
					    dcl_color v1
					    dcl_texcoord v2
					    dcl_texcoord o0.xy
					    dcl_color o1
					    dcl_position o2
					    mad r0, v0.xyzx, c30.xxxy, c30.yyyx
					    dp4 r1.x, c12, r0
					    dp4 r1.y, c13, r0
					    dp4 r1.z, c14, r0
					    dp4 r1.w, c15, r0
					    dp4 r7.x, c20, r1
					    dp4 r7.y, c21, r1
					    dp4 r7.z, c22, r1
					    dp4 r7.w, c23, r1
					    mad o0.xy, v2, c29, c29.zwzw
					    mov r0, c17
					    mul r1.x, r0.y, c4.y
					    mov r2, c16
					    mad r1.x, c4.x, r2.y, r1.x
					    mov r3, c18
					    mad r1.x, c4.z, r3.y, r1.x
					    mov r1.w, c4.w
					    mad r1.x, r1.w, c19.y, r1.x
					    mad_sat r1.x, r1.x, c30.z, c30.x
					    mul r1.xyz, r1.x, c0
					    add r4.xyz, c24, c24
					    mul r4.xyz, r4, c28.x
					    mad r1.xyz, r1, c26.x, r4
					    abs r1.w, c4.w
					    sge r1.w, -r1.w, r1.w
					    mul r5, r2.y, c13
					    mad r5, r2.x, c12, r5
					    mad r5, r2.z, c14, r5
					    mad r5, r2.w, c15, r5
					    dp4 r5.x, r5, v0
					    mul r6, r0.y, c13
					    mad r6, r0.x, c12, r6
					    mad r6, r0.z, c14, r6
					    mad r6, r0.w, c15, r6
					    dp4 r5.y, r6, v0
					    mul r6, r3.y, c13
					    mad r6, r3.x, c12, r6
					    mad r6, r3.z, c14, r6
					    mad r6, r3.w, c15, r6
					    dp4 r5.z, r6, v0
					    add r0.xzw, -r5.xyyz, c4.xyyz
					    dp3 r0.x, r0.xzww, r0.xzww
					    mov r2.x, c30.x
					    mad r0.x, r0.x, c8.z, r2.x
					    rcp r0.x, r0.x
					    mul r0.xzw, r0.x, c0.xyyz
					    mad r0.xzw, r0, c27.x, r4.xyyz
					    lrp r3.xzw, r1.w, r1.xyyz, r0
					    add r0.xzw, -r5.xyyz, c5.xyyz
					    dp3 r0.x, r0.xzww, r0.xzww
					    mad r0.x, r0.x, c9.z, r2.x
					    rcp r0.x, r0.x
					    mul r0.xzw, r0.x, c1.xyyz
					    mad r0.xzw, r0, c27.x, r3
					    mul r1.x, r0.y, c5.y
					    mad r1.x, c5.x, r2.y, r1.x
					    mad r1.x, c5.z, r3.y, r1.x
					    mov r1.w, c5.w
					    mad r1.x, r1.w, c19.y, r1.x
					    mad_sat r1.x, r1.x, c30.z, c30.x
					    mul r1.xyz, r1.x, c1
					    mad r1.xyz, r1, c26.x, r3.xzww
					    abs r1.w, c5.w
					    sge r1.w, -r1.w, r1.w
					    lrp r3.xzw, r1.w, r1.xyyz, r0
					    add r0.xzw, -r5.xyyz, c6.xyyz
					    add r1.xyz, -r5, c7
					    dp3 r1.x, r1, r1
					    mad r1.x, r1.x, c11.z, r2.x
					    rcp r1.x, r1.x
					    mul r1.xyz, r1.x, c3
					    dp3 r0.x, r0.xzww, r0.xzww
					    mad r0.x, r0.x, c10.z, r2.x
					    rcp r0.x, r0.x
					    mul r0.xzw, r0.x, c2.xyyz
					    mad r0.xzw, r0, c27.x, r3
					    mul r1.w, r0.y, c6.y
					    mad r1.w, c6.x, r2.y, r1.w
					    mad r1.w, c6.z, r3.y, r1.w
					    mov r2.w, c6.w
					    mad r1.w, r2.w, c19.y, r1.w
					    mad_sat r1.w, r1.w, c30.z, c30.x
					    mul r2.xzw, r1.w, c2.xyyz
					    mad r2.xzw, r2, c26.x, r3
					    abs r1.w, c6.w
					    sge r1.w, -r1.w, r1.w
					    lrp r3.xzw, r1.w, r2, r0
					    mad r0.xzw, r1.xyyz, c27.x, r3
					    mul r0.y, r0.y, c7.y
					    mad r0.y, c7.x, r2.y, r0.y
					    mad r0.y, c7.z, r3.y, r0.y
					    mov r1.w, c7.w
					    mad r0.y, r1.w, c19.y, r0.y
					    mad_sat r0.y, r0.y, c30.z, c30.x
					    mul r1.xyz, r0.y, c3
					    mad r1.xyz, r1, c26.x, r3.xzww
					    abs r0.y, c7.w
					    sge r0.y, -r0.y, r0.y
					    lrp r2.xyz, r0.y, r1, r0.xzww
					    mov r2.w, c30.x
					    mul r0, r2, v1
					    mul r0, r0, c25
					    min r1.x, r0.x, c25.w
					    rcp r1.y, c25.w
					    mul r1.x, r1.y, r1.x
					    mul o1, r0, r1.x
					    mad o2.xy, r7.w, c255, r7
					    mov o2.zw, r7
					
					// approximately 108 instruction slots used"
				}
				SubProgram "d3d11 " {
					"vs_4_0
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
					#if HLSLCC_ENABLE_UNIFORM_BUFFERS
					#define UNITY_UNIFORM
					#else
					#define UNITY_UNIFORM uniform
					#endif
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					layout(std140) uniform VGlobals {
						vec4 unused_0_0[4];
						vec4 _TintColor;
						float _DirectionalLightMultiplier;
						float _PointSpotLightMultiplier;
						float _AmbientLightMultiplier;
						vec4 _MainTex_ST;
					};
					layout(std140) uniform UnityLighting {
						vec4 unused_1_0[6];
						vec4 unity_LightColor[8];
						vec4 unused_1_2[7];
						vec4 unity_LightPosition[8];
						vec4 unused_1_4[7];
						vec4 unity_LightAtten[8];
						vec4 unused_1_6[24];
					};
					layout(std140) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						vec4 unused_2_1[6];
					};
					layout(std140) uniform UnityPerFrame {
						vec4 glstate_lightmodel_ambient;
						vec4 unused_3_1[8];
						mat4x4 unity_MatrixV;
						vec4 unused_3_3[4];
						mat4x4 unity_MatrixVP;
						vec4 unused_3_5[3];
					};
					in  vec4 in_POSITION0;
					in  vec4 in_COLOR0;
					in  vec2 in_TEXCOORD0;
					out vec2 vs_TEXCOORD0;
					out vec4 vs_COLOR0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec3 u_xlat2;
					vec3 u_xlat3;
					float u_xlat12;
					bool u_xlatb12;
					void main()
					{
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					    u_xlat0.xyz = unity_ObjectToWorld[1].yyy * unity_MatrixV[1].xyz;
					    u_xlat0.xyz = unity_MatrixV[0].xyz * unity_ObjectToWorld[1].xxx + u_xlat0.xyz;
					    u_xlat0.xyz = unity_MatrixV[2].xyz * unity_ObjectToWorld[1].zzz + u_xlat0.xyz;
					    u_xlat0.xyz = unity_MatrixV[3].xyz * unity_ObjectToWorld[1].www + u_xlat0.xyz;
					    u_xlat0.xyz = u_xlat0.xyz * in_POSITION0.yyy;
					    u_xlat1.xyz = unity_ObjectToWorld[0].yyy * unity_MatrixV[1].xyz;
					    u_xlat1.xyz = unity_MatrixV[0].xyz * unity_ObjectToWorld[0].xxx + u_xlat1.xyz;
					    u_xlat1.xyz = unity_MatrixV[2].xyz * unity_ObjectToWorld[0].zzz + u_xlat1.xyz;
					    u_xlat1.xyz = unity_MatrixV[3].xyz * unity_ObjectToWorld[0].www + u_xlat1.xyz;
					    u_xlat0.xyz = u_xlat1.xyz * in_POSITION0.xxx + u_xlat0.xyz;
					    u_xlat1.xyz = unity_ObjectToWorld[2].yyy * unity_MatrixV[1].xyz;
					    u_xlat1.xyz = unity_MatrixV[0].xyz * unity_ObjectToWorld[2].xxx + u_xlat1.xyz;
					    u_xlat1.xyz = unity_MatrixV[2].xyz * unity_ObjectToWorld[2].zzz + u_xlat1.xyz;
					    u_xlat1.xyz = unity_MatrixV[3].xyz * unity_ObjectToWorld[2].www + u_xlat1.xyz;
					    u_xlat0.xyz = u_xlat1.xyz * in_POSITION0.zzz + u_xlat0.xyz;
					    u_xlat1.xyz = unity_ObjectToWorld[3].yyy * unity_MatrixV[1].xyz;
					    u_xlat1.xyz = unity_MatrixV[0].xyz * unity_ObjectToWorld[3].xxx + u_xlat1.xyz;
					    u_xlat1.xyz = unity_MatrixV[2].xyz * unity_ObjectToWorld[3].zzz + u_xlat1.xyz;
					    u_xlat1.xyz = unity_MatrixV[3].xyz * unity_ObjectToWorld[3].www + u_xlat1.xyz;
					    u_xlat0.xyz = u_xlat1.xyz * in_POSITION0.www + u_xlat0.xyz;
					    u_xlat1.xyz = (-u_xlat0.xyz) + unity_LightPosition[0].xyz;
					    u_xlat12 = dot(u_xlat1.xyz, u_xlat1.xyz);
					    u_xlat12 = u_xlat12 * unity_LightAtten[0].z + 1.0;
					    u_xlat12 = float(1.0) / u_xlat12;
					    u_xlat1.xyz = vec3(u_xlat12) * unity_LightColor[0].xyz;
					    u_xlat2.xyz = glstate_lightmodel_ambient.xyz + glstate_lightmodel_ambient.xyz;
					    u_xlat2.xyz = u_xlat2.xyz * vec3(vec3(_AmbientLightMultiplier, _AmbientLightMultiplier, _AmbientLightMultiplier));
					    u_xlat1.xyz = u_xlat1.xyz * vec3(vec3(_PointSpotLightMultiplier, _PointSpotLightMultiplier, _PointSpotLightMultiplier)) + u_xlat2.xyz;
					    u_xlat12 = dot(unity_LightPosition[0], unity_MatrixV[1]);
					    u_xlat12 = u_xlat12 * 2.0 + 1.0;
					    u_xlat12 = clamp(u_xlat12, 0.0, 1.0);
					    u_xlat3.xyz = vec3(u_xlat12) * unity_LightColor[0].xyz;
					    u_xlat2.xyz = u_xlat3.xyz * vec3(_DirectionalLightMultiplier) + u_xlat2.xyz;
					    u_xlatb12 = unity_LightPosition[0].w==0.0;
					    u_xlat1.xyz = (bool(u_xlatb12)) ? u_xlat2.xyz : u_xlat1.xyz;
					    u_xlat2.xyz = (-u_xlat0.xyz) + unity_LightPosition[1].xyz;
					    u_xlat12 = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat12 = u_xlat12 * unity_LightAtten[1].z + 1.0;
					    u_xlat12 = float(1.0) / u_xlat12;
					    u_xlat2.xyz = vec3(u_xlat12) * unity_LightColor[1].xyz;
					    u_xlat2.xyz = u_xlat2.xyz * vec3(vec3(_PointSpotLightMultiplier, _PointSpotLightMultiplier, _PointSpotLightMultiplier)) + u_xlat1.xyz;
					    u_xlat12 = dot(unity_LightPosition[1], unity_MatrixV[1]);
					    u_xlat12 = u_xlat12 * 2.0 + 1.0;
					    u_xlat12 = clamp(u_xlat12, 0.0, 1.0);
					    u_xlat3.xyz = vec3(u_xlat12) * unity_LightColor[1].xyz;
					    u_xlat1.xyz = u_xlat3.xyz * vec3(_DirectionalLightMultiplier) + u_xlat1.xyz;
					    u_xlatb12 = unity_LightPosition[1].w==0.0;
					    u_xlat1.xyz = (bool(u_xlatb12)) ? u_xlat1.xyz : u_xlat2.xyz;
					    u_xlat2.xyz = (-u_xlat0.xyz) + unity_LightPosition[2].xyz;
					    u_xlat0.xyz = (-u_xlat0.xyz) + unity_LightPosition[3].xyz;
					    u_xlat0.x = dot(u_xlat0.xyz, u_xlat0.xyz);
					    u_xlat0.x = u_xlat0.x * unity_LightAtten[3].z + 1.0;
					    u_xlat0.x = float(1.0) / u_xlat0.x;
					    u_xlat0.xyz = u_xlat0.xxx * unity_LightColor[3].xyz;
					    u_xlat12 = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat12 = u_xlat12 * unity_LightAtten[2].z + 1.0;
					    u_xlat12 = float(1.0) / u_xlat12;
					    u_xlat2.xyz = vec3(u_xlat12) * unity_LightColor[2].xyz;
					    u_xlat2.xyz = u_xlat2.xyz * vec3(vec3(_PointSpotLightMultiplier, _PointSpotLightMultiplier, _PointSpotLightMultiplier)) + u_xlat1.xyz;
					    u_xlat12 = dot(unity_LightPosition[2], unity_MatrixV[1]);
					    u_xlat12 = u_xlat12 * 2.0 + 1.0;
					    u_xlat12 = clamp(u_xlat12, 0.0, 1.0);
					    u_xlat3.xyz = vec3(u_xlat12) * unity_LightColor[2].xyz;
					    u_xlat1.xyz = u_xlat3.xyz * vec3(_DirectionalLightMultiplier) + u_xlat1.xyz;
					    u_xlatb12 = unity_LightPosition[2].w==0.0;
					    u_xlat1.xyz = (bool(u_xlatb12)) ? u_xlat1.xyz : u_xlat2.xyz;
					    u_xlat0.xyz = u_xlat0.xyz * vec3(vec3(_PointSpotLightMultiplier, _PointSpotLightMultiplier, _PointSpotLightMultiplier)) + u_xlat1.xyz;
					    u_xlat12 = dot(unity_LightPosition[3], unity_MatrixV[1]);
					    u_xlat12 = u_xlat12 * 2.0 + 1.0;
					    u_xlat12 = clamp(u_xlat12, 0.0, 1.0);
					    u_xlat2.xyz = vec3(u_xlat12) * unity_LightColor[3].xyz;
					    u_xlat1.xyz = u_xlat2.xyz * vec3(_DirectionalLightMultiplier) + u_xlat1.xyz;
					    u_xlatb12 = unity_LightPosition[3].w==0.0;
					    u_xlat0.xyz = (bool(u_xlatb12)) ? u_xlat1.xyz : u_xlat0.xyz;
					    u_xlat0.w = 1.0;
					    u_xlat0 = u_xlat0 * in_COLOR0;
					    u_xlat0 = u_xlat0 * _TintColor;
					    u_xlat1.x = min(u_xlat0.x, _TintColor.w);
					    u_xlat1.x = u_xlat1.x / _TintColor.w;
					    vs_COLOR0 = u_xlat0 * u_xlat1.xxxx;
					    u_xlat0 = in_POSITION0.yyyy * unity_ObjectToWorld[1];
					    u_xlat0 = unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = u_xlat0 + unity_ObjectToWorld[3];
					    u_xlat1 = u_xlat0.yyyy * unity_MatrixVP[1];
					    u_xlat1 = unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
					    gl_Position = unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
					    return;
					}"
				}
				SubProgram "d3d9 " {
					Keywords { "SOFTPARTICLES_ON" }
					"vs_3_0
					
					//
					// Generated by Microsoft (R) HLSL Shader Compiler 10.1
					//
					// Parameters:
					//
					//   float _AmbientLightMultiplier;
					//   float _DirectionalLightMultiplier;
					//   float4 _MainTex_ST;
					//   float _PointSpotLightMultiplier;
					//   float4 _ProjectionParams;
					//   float4 _TintColor;
					//   float4 glstate_lightmodel_ambient;
					//   float4 unity_LightAtten[8];
					//   float4 unity_LightColor[8];
					//   float4 unity_LightPosition[8];
					//   row_major float4x4 unity_MatrixV;
					//   row_major float4x4 unity_MatrixVP;
					//   row_major float4x4 unity_ObjectToWorld;
					//
					//
					// Registers:
					//
					//   Name                        Reg   Size
					//   --------------------------- ----- ----
					//   unity_LightColor            c0       4
					//   unity_LightPosition         c4       4
					//   unity_LightAtten            c8       4
					//   unity_ObjectToWorld         c12      4
					//   unity_MatrixV               c16      4
					//   unity_MatrixVP              c20      4
					//   _ProjectionParams           c24      1
					//   glstate_lightmodel_ambient  c25      1
					//   _TintColor                  c26      1
					//   _DirectionalLightMultiplier c27      1
					//   _PointSpotLightMultiplier   c28      1
					//   _AmbientLightMultiplier     c29      1
					//   _MainTex_ST                 c30      1
					//
					
					    vs_3_0
					    def c31, 1, 0, 2, 0.5
					    dcl_position v0
					    dcl_color v1
					    dcl_texcoord v2
					    dcl_texcoord o0.xy
					    dcl_color o1
					    dcl_position o2
					    dcl_texcoord1 o3
					    mad r0, v0.xyzx, c31.xxxy, c31.yyyx
					    dp4 r1.x, c12, r0
					    dp4 r1.y, c13, r0
					    dp4 r1.z, c14, r0
					    dp4 r1.w, c15, r0
					    dp4 r8.z, c22, r1
					    mad o0.xy, v2, c30, c30.zwzw
					    mov r0, c17
					    mul r2.x, r0.y, c4.y
					    mov r3, c16
					    mad r2.x, c4.x, r3.y, r2.x
					    mov r4, c18
					    mad r2.x, c4.z, r4.y, r2.x
					    mov r2.w, c4.w
					    mad r2.x, r2.w, c19.y, r2.x
					    mad_sat r2.x, r2.x, c31.z, c31.x
					    mul r2.xyz, r2.x, c0
					    add r5.xyz, c25, c25
					    mul r5.xyz, r5, c29.x
					    mad r2.xyz, r2, c27.x, r5
					    abs r2.w, c4.w
					    sge r2.w, -r2.w, r2.w
					    mul r6, r3.y, c13
					    mad r6, r3.x, c12, r6
					    mad r6, r3.z, c14, r6
					    mad r6, r3.w, c15, r6
					    dp4 r6.x, r6, v0
					    mul r7, r0.y, c13
					    mad r7, r0.x, c12, r7
					    mad r7, r0.z, c14, r7
					    mad r7, r0.w, c15, r7
					    dp4 r6.y, r7, v0
					    mul r7, r4.y, c13
					    mad r7, r4.x, c12, r7
					    mad r7, r4.z, c14, r7
					    mad r7, r4.w, c15, r7
					    dp4 r6.z, r7, v0
					    add r0.xzw, -r6.xyyz, c4.xyyz
					    dp3 r0.x, r0.xzww, r0.xzww
					    mov r3.x, c31.x
					    mad r0.x, r0.x, c8.z, r3.x
					    rcp r0.x, r0.x
					    mul r0.xzw, r0.x, c0.xyyz
					    mad r0.xzw, r0, c28.x, r5.xyyz
					    lrp r4.xzw, r2.w, r2.xyyz, r0
					    add r0.xzw, -r6.xyyz, c5.xyyz
					    dp3 r0.x, r0.xzww, r0.xzww
					    mad r0.x, r0.x, c9.z, r3.x
					    rcp r0.x, r0.x
					    mul r0.xzw, r0.x, c1.xyyz
					    mad r0.xzw, r0, c28.x, r4
					    mul r2.x, r0.y, c5.y
					    mad r2.x, c5.x, r3.y, r2.x
					    mad r2.x, c5.z, r4.y, r2.x
					    mov r2.w, c5.w
					    mad r2.x, r2.w, c19.y, r2.x
					    mad_sat r2.x, r2.x, c31.z, c31.x
					    mul r2.xyz, r2.x, c1
					    mad r2.xyz, r2, c27.x, r4.xzww
					    abs r2.w, c5.w
					    sge r2.w, -r2.w, r2.w
					    lrp r4.xzw, r2.w, r2.xyyz, r0
					    add r0.xzw, -r6.xyyz, c6.xyyz
					    add r2.xyz, -r6, c7
					    dp3 r2.x, r2, r2
					    mad r2.x, r2.x, c11.z, r3.x
					    rcp r2.x, r2.x
					    mul r2.xyz, r2.x, c3
					    dp3 r0.x, r0.xzww, r0.xzww
					    mad r0.x, r0.x, c10.z, r3.x
					    rcp r0.x, r0.x
					    mul r0.xzw, r0.x, c2.xyyz
					    mad r0.xzw, r0, c28.x, r4
					    mul r2.w, r0.y, c6.y
					    mad r2.w, c6.x, r3.y, r2.w
					    mad r2.w, c6.z, r4.y, r2.w
					    mov r3.w, c6.w
					    mad r2.w, r3.w, c19.y, r2.w
					    mad_sat r2.w, r2.w, c31.z, c31.x
					    mul r3.xzw, r2.w, c2.xyyz
					    mad r3.xzw, r3, c27.x, r4
					    abs r2.w, c6.w
					    sge r2.w, -r2.w, r2.w
					    lrp r4.xzw, r2.w, r3, r0
					    mad r0.xzw, r2.xyyz, c28.x, r4
					    mul r0.y, r0.y, c7.y
					    mad r0.y, c7.x, r3.y, r0.y
					    mad r0.y, c7.z, r4.y, r0.y
					    mov r2.w, c7.w
					    mad r0.y, r2.w, c19.y, r0.y
					    mad_sat r0.y, r0.y, c31.z, c31.x
					    mul r2.xyz, r0.y, c3
					    mad r2.xyz, r2, c27.x, r4.xzww
					    abs r0.y, c7.w
					    sge r0.y, -r0.y, r0.y
					    lrp r3.xyz, r0.y, r2, r0.xzww
					    mov r3.w, c31.x
					    mul r0, r3, v1
					    mul r0, r0, c26
					    min r2.x, r0.x, c26.w
					    rcp r2.y, c26.w
					    mul r2.x, r2.y, r2.x
					    mul o1, r0, r2.x
					    dp4 r0.x, c20, r1
					    dp4 r0.w, c23, r1
					    dp4 r0.y, c21, r1
					    dp4 r0.z, c18, r1
					    mov o3.z, -r0.z
					    mul r0.z, r0.y, c24.x
					    mov r8.xyw, r0
					    mov o3.w, r0.w
					    mul r2.xzw, r0.xywz, c31.w
					    add o3.xy, r2.z, r2.xwzw
					    mad o2.xy, r8.w, c255, r8
					    mov o2.zw, r8
					
					// approximately 115 instruction slots used"
				}
				SubProgram "d3d11 " {
					Keywords { "SOFTPARTICLES_ON" }
					"vs_4_0
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
					#if HLSLCC_ENABLE_UNIFORM_BUFFERS
					#define UNITY_UNIFORM
					#else
					#define UNITY_UNIFORM uniform
					#endif
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					layout(std140) uniform VGlobals {
						vec4 unused_0_0[4];
						vec4 _TintColor;
						float _DirectionalLightMultiplier;
						float _PointSpotLightMultiplier;
						float _AmbientLightMultiplier;
						vec4 _MainTex_ST;
					};
					layout(std140) uniform UnityPerCamera {
						vec4 unused_1_0[5];
						vec4 _ProjectionParams;
						vec4 unused_1_2[3];
					};
					layout(std140) uniform UnityLighting {
						vec4 unused_2_0[6];
						vec4 unity_LightColor[8];
						vec4 unused_2_2[7];
						vec4 unity_LightPosition[8];
						vec4 unused_2_4[7];
						vec4 unity_LightAtten[8];
						vec4 unused_2_6[24];
					};
					layout(std140) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						vec4 unused_3_1[6];
					};
					layout(std140) uniform UnityPerFrame {
						vec4 glstate_lightmodel_ambient;
						vec4 unused_4_1[8];
						mat4x4 unity_MatrixV;
						vec4 unused_4_3[4];
						mat4x4 unity_MatrixVP;
						vec4 unused_4_5[3];
					};
					in  vec4 in_POSITION0;
					in  vec4 in_COLOR0;
					in  vec2 in_TEXCOORD0;
					out vec2 vs_TEXCOORD0;
					out vec4 vs_COLOR0;
					out vec4 vs_TEXCOORD1;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec3 u_xlat2;
					vec3 u_xlat3;
					float u_xlat4;
					float u_xlat12;
					bool u_xlatb12;
					void main()
					{
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					    u_xlat0.xyz = unity_ObjectToWorld[1].yyy * unity_MatrixV[1].xyz;
					    u_xlat0.xyz = unity_MatrixV[0].xyz * unity_ObjectToWorld[1].xxx + u_xlat0.xyz;
					    u_xlat0.xyz = unity_MatrixV[2].xyz * unity_ObjectToWorld[1].zzz + u_xlat0.xyz;
					    u_xlat0.xyz = unity_MatrixV[3].xyz * unity_ObjectToWorld[1].www + u_xlat0.xyz;
					    u_xlat0.xyz = u_xlat0.xyz * in_POSITION0.yyy;
					    u_xlat1.xyz = unity_ObjectToWorld[0].yyy * unity_MatrixV[1].xyz;
					    u_xlat1.xyz = unity_MatrixV[0].xyz * unity_ObjectToWorld[0].xxx + u_xlat1.xyz;
					    u_xlat1.xyz = unity_MatrixV[2].xyz * unity_ObjectToWorld[0].zzz + u_xlat1.xyz;
					    u_xlat1.xyz = unity_MatrixV[3].xyz * unity_ObjectToWorld[0].www + u_xlat1.xyz;
					    u_xlat0.xyz = u_xlat1.xyz * in_POSITION0.xxx + u_xlat0.xyz;
					    u_xlat1.xyz = unity_ObjectToWorld[2].yyy * unity_MatrixV[1].xyz;
					    u_xlat1.xyz = unity_MatrixV[0].xyz * unity_ObjectToWorld[2].xxx + u_xlat1.xyz;
					    u_xlat1.xyz = unity_MatrixV[2].xyz * unity_ObjectToWorld[2].zzz + u_xlat1.xyz;
					    u_xlat1.xyz = unity_MatrixV[3].xyz * unity_ObjectToWorld[2].www + u_xlat1.xyz;
					    u_xlat0.xyz = u_xlat1.xyz * in_POSITION0.zzz + u_xlat0.xyz;
					    u_xlat1.xyz = unity_ObjectToWorld[3].yyy * unity_MatrixV[1].xyz;
					    u_xlat1.xyz = unity_MatrixV[0].xyz * unity_ObjectToWorld[3].xxx + u_xlat1.xyz;
					    u_xlat1.xyz = unity_MatrixV[2].xyz * unity_ObjectToWorld[3].zzz + u_xlat1.xyz;
					    u_xlat1.xyz = unity_MatrixV[3].xyz * unity_ObjectToWorld[3].www + u_xlat1.xyz;
					    u_xlat0.xyz = u_xlat1.xyz * in_POSITION0.www + u_xlat0.xyz;
					    u_xlat1.xyz = (-u_xlat0.xyz) + unity_LightPosition[0].xyz;
					    u_xlat12 = dot(u_xlat1.xyz, u_xlat1.xyz);
					    u_xlat12 = u_xlat12 * unity_LightAtten[0].z + 1.0;
					    u_xlat12 = float(1.0) / u_xlat12;
					    u_xlat1.xyz = vec3(u_xlat12) * unity_LightColor[0].xyz;
					    u_xlat2.xyz = glstate_lightmodel_ambient.xyz + glstate_lightmodel_ambient.xyz;
					    u_xlat2.xyz = u_xlat2.xyz * vec3(vec3(_AmbientLightMultiplier, _AmbientLightMultiplier, _AmbientLightMultiplier));
					    u_xlat1.xyz = u_xlat1.xyz * vec3(vec3(_PointSpotLightMultiplier, _PointSpotLightMultiplier, _PointSpotLightMultiplier)) + u_xlat2.xyz;
					    u_xlat12 = dot(unity_LightPosition[0], unity_MatrixV[1]);
					    u_xlat12 = u_xlat12 * 2.0 + 1.0;
					    u_xlat12 = clamp(u_xlat12, 0.0, 1.0);
					    u_xlat3.xyz = vec3(u_xlat12) * unity_LightColor[0].xyz;
					    u_xlat2.xyz = u_xlat3.xyz * vec3(_DirectionalLightMultiplier) + u_xlat2.xyz;
					    u_xlatb12 = unity_LightPosition[0].w==0.0;
					    u_xlat1.xyz = (bool(u_xlatb12)) ? u_xlat2.xyz : u_xlat1.xyz;
					    u_xlat2.xyz = (-u_xlat0.xyz) + unity_LightPosition[1].xyz;
					    u_xlat12 = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat12 = u_xlat12 * unity_LightAtten[1].z + 1.0;
					    u_xlat12 = float(1.0) / u_xlat12;
					    u_xlat2.xyz = vec3(u_xlat12) * unity_LightColor[1].xyz;
					    u_xlat2.xyz = u_xlat2.xyz * vec3(vec3(_PointSpotLightMultiplier, _PointSpotLightMultiplier, _PointSpotLightMultiplier)) + u_xlat1.xyz;
					    u_xlat12 = dot(unity_LightPosition[1], unity_MatrixV[1]);
					    u_xlat12 = u_xlat12 * 2.0 + 1.0;
					    u_xlat12 = clamp(u_xlat12, 0.0, 1.0);
					    u_xlat3.xyz = vec3(u_xlat12) * unity_LightColor[1].xyz;
					    u_xlat1.xyz = u_xlat3.xyz * vec3(_DirectionalLightMultiplier) + u_xlat1.xyz;
					    u_xlatb12 = unity_LightPosition[1].w==0.0;
					    u_xlat1.xyz = (bool(u_xlatb12)) ? u_xlat1.xyz : u_xlat2.xyz;
					    u_xlat2.xyz = (-u_xlat0.xyz) + unity_LightPosition[2].xyz;
					    u_xlat0.xyz = (-u_xlat0.xyz) + unity_LightPosition[3].xyz;
					    u_xlat0.x = dot(u_xlat0.xyz, u_xlat0.xyz);
					    u_xlat0.x = u_xlat0.x * unity_LightAtten[3].z + 1.0;
					    u_xlat0.x = float(1.0) / u_xlat0.x;
					    u_xlat0.xyz = u_xlat0.xxx * unity_LightColor[3].xyz;
					    u_xlat12 = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat12 = u_xlat12 * unity_LightAtten[2].z + 1.0;
					    u_xlat12 = float(1.0) / u_xlat12;
					    u_xlat2.xyz = vec3(u_xlat12) * unity_LightColor[2].xyz;
					    u_xlat2.xyz = u_xlat2.xyz * vec3(vec3(_PointSpotLightMultiplier, _PointSpotLightMultiplier, _PointSpotLightMultiplier)) + u_xlat1.xyz;
					    u_xlat12 = dot(unity_LightPosition[2], unity_MatrixV[1]);
					    u_xlat12 = u_xlat12 * 2.0 + 1.0;
					    u_xlat12 = clamp(u_xlat12, 0.0, 1.0);
					    u_xlat3.xyz = vec3(u_xlat12) * unity_LightColor[2].xyz;
					    u_xlat1.xyz = u_xlat3.xyz * vec3(_DirectionalLightMultiplier) + u_xlat1.xyz;
					    u_xlatb12 = unity_LightPosition[2].w==0.0;
					    u_xlat1.xyz = (bool(u_xlatb12)) ? u_xlat1.xyz : u_xlat2.xyz;
					    u_xlat0.xyz = u_xlat0.xyz * vec3(vec3(_PointSpotLightMultiplier, _PointSpotLightMultiplier, _PointSpotLightMultiplier)) + u_xlat1.xyz;
					    u_xlat12 = dot(unity_LightPosition[3], unity_MatrixV[1]);
					    u_xlat12 = u_xlat12 * 2.0 + 1.0;
					    u_xlat12 = clamp(u_xlat12, 0.0, 1.0);
					    u_xlat2.xyz = vec3(u_xlat12) * unity_LightColor[3].xyz;
					    u_xlat1.xyz = u_xlat2.xyz * vec3(_DirectionalLightMultiplier) + u_xlat1.xyz;
					    u_xlatb12 = unity_LightPosition[3].w==0.0;
					    u_xlat0.xyz = (bool(u_xlatb12)) ? u_xlat1.xyz : u_xlat0.xyz;
					    u_xlat0.w = 1.0;
					    u_xlat0 = u_xlat0 * in_COLOR0;
					    u_xlat0 = u_xlat0 * _TintColor;
					    u_xlat1.x = min(u_xlat0.x, _TintColor.w);
					    u_xlat1.x = u_xlat1.x / _TintColor.w;
					    vs_COLOR0 = u_xlat0 * u_xlat1.xxxx;
					    u_xlat0 = in_POSITION0.yyyy * unity_ObjectToWorld[1];
					    u_xlat0 = unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = u_xlat0 + unity_ObjectToWorld[3];
					    u_xlat1 = u_xlat0.yyyy * unity_MatrixVP[1];
					    u_xlat1 = unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
					    u_xlat1 = unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
					    gl_Position = u_xlat1;
					    u_xlat4 = u_xlat0.y * unity_MatrixV[1].z;
					    u_xlat0.x = unity_MatrixV[0].z * u_xlat0.x + u_xlat4;
					    u_xlat0.x = unity_MatrixV[2].z * u_xlat0.z + u_xlat0.x;
					    u_xlat0.x = unity_MatrixV[3].z * u_xlat0.w + u_xlat0.x;
					    vs_TEXCOORD1.z = (-u_xlat0.x);
					    u_xlat0.x = u_xlat1.y * _ProjectionParams.x;
					    u_xlat0.w = u_xlat0.x * 0.5;
					    u_xlat0.xz = u_xlat1.xw * vec2(0.5, 0.5);
					    vs_TEXCOORD1.w = u_xlat1.w;
					    vs_TEXCOORD1.xy = u_xlat0.zz + u_xlat0.xw;
					    return;
					}"
				}
			}
			Program "fp" {
				SubProgram "d3d9 " {
					"ps_3_0
					
					//
					// Generated by Microsoft (R) HLSL Shader Compiler 10.1
					//
					// Parameters:
					//
					//   sampler2D _MainTex;
					//
					//
					// Registers:
					//
					//   Name         Reg   Size
					//   ------------ ----- ----
					//   _MainTex     s0       1
					//
					
					    ps_3_0
					    dcl_texcoord_pp v0.xy
					    dcl_color v1
					    dcl_2d s0
					    texld r0, v0, s0
					    mul_pp oC0, r0, v1
					
					// approximately 2 instruction slots used (1 texture, 1 arithmetic)"
				}
				SubProgram "d3d11 " {
					"ps_4_0
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					uniform  sampler2D _MainTex;
					in  vec2 vs_TEXCOORD0;
					in  vec4 vs_COLOR0;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					void main()
					{
					    u_xlat0 = texture(_MainTex, vs_TEXCOORD0.xy);
					    SV_Target0 = u_xlat0 * vs_COLOR0;
					    return;
					}"
				}
				SubProgram "d3d9 " {
					Keywords { "SOFTPARTICLES_ON" }
					"ps_3_0
					
					//
					// Generated by Microsoft (R) HLSL Shader Compiler 10.1
					//
					// Parameters:
					//
					//   sampler2D _CameraDepthTexture;
					//   float _InvFade;
					//   sampler2D _MainTex;
					//   float4 _ZBufferParams;
					//
					//
					// Registers:
					//
					//   Name                Reg   Size
					//   ------------------- ----- ----
					//   _ZBufferParams      c0       1
					//   _InvFade            c1       1
					//   _CameraDepthTexture s0       1
					//   _MainTex            s1       1
					//
					
					    ps_3_0
					    dcl_texcoord_pp v0.xy
					    dcl_color v1
					    dcl_texcoord1 v2
					    dcl_2d s0
					    dcl_2d s1
					    texldp r0, v2, s0
					    mad r0.x, c0.z, r0.x, c0.w
					    rcp r0.x, r0.x
					    add r0.x, r0.x, -v2.z
					    mul_sat r0.x, r0.x, c1.x
					    mul_pp r0.w, r0.x, v1.w
					    texld r1, v0, s1
					    mov r0.xyz, v1
					    mul_pp oC0, r0, r1
					
					// approximately 9 instruction slots used (2 texture, 7 arithmetic)"
				}
				SubProgram "d3d11 " {
					Keywords { "SOFTPARTICLES_ON" }
					"ps_4_0
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
					#if HLSLCC_ENABLE_UNIFORM_BUFFERS
					#define UNITY_UNIFORM
					#else
					#define UNITY_UNIFORM uniform
					#endif
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					layout(std140) uniform PGlobals {
						vec4 unused_0_0[5];
						float _InvFade;
						vec4 unused_0_2;
					};
					layout(std140) uniform UnityPerCamera {
						vec4 unused_1_0[7];
						vec4 _ZBufferParams;
						vec4 unused_1_2;
					};
					uniform  sampler2D _CameraDepthTexture;
					uniform  sampler2D _MainTex;
					in  vec2 vs_TEXCOORD0;
					in  vec4 vs_COLOR0;
					in  vec4 vs_TEXCOORD1;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					void main()
					{
					    u_xlat0.xy = vs_TEXCOORD1.xy / vs_TEXCOORD1.ww;
					    u_xlat0 = texture(_CameraDepthTexture, u_xlat0.xy);
					    u_xlat0.x = _ZBufferParams.z * u_xlat0.x + _ZBufferParams.w;
					    u_xlat0.x = float(1.0) / u_xlat0.x;
					    u_xlat0.x = u_xlat0.x + (-vs_TEXCOORD1.z);
					    u_xlat0.x = u_xlat0.x * _InvFade;
					    u_xlat0.x = clamp(u_xlat0.x, 0.0, 1.0);
					    u_xlat0.w = u_xlat0.x * vs_COLOR0.w;
					    u_xlat1 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat0.xyz = vs_COLOR0.xyz;
					    SV_Target0 = u_xlat0 * u_xlat1;
					    return;
					}"
				}
			}
		}
	}
	Fallback "Particles/Alpha Blended"
}