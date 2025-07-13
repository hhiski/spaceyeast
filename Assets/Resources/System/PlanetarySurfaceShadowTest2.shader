Shader "Shader Graphs/PlanetaryShadowSGTest2"
{
    Properties
    {
        _Color("Color", Color) = (0, 0, 0, 0)
        _MaxShadowDistance("MaxShadowDistance", Float) = 0
        _EdgeFade("EdgeFade", Float) = 0.85
        [HideInInspector]_CastShadows("_CastShadows", Float) = 0
        [HideInInspector]_Surface("_Surface", Float) = 1
        [HideInInspector]_Blend("_Blend", Float) = 1
        [HideInInspector]_AlphaClip("_AlphaClip", Float) = 0
        [HideInInspector]_SrcBlend("_SrcBlend", Float) = 1
        [HideInInspector]_DstBlend("_DstBlend", Float) = 0
        [HideInInspector][ToggleUI]_ZWrite("_ZWrite", Float) = 0
        [HideInInspector]_ZWriteControl("_ZWriteControl", Float) = 0
        [HideInInspector]_ZTest("_ZTest", Float) = 4
        [HideInInspector]_Cull("_Cull", Float) = 2
        [HideInInspector]_AlphaToMask("_AlphaToMask", Float) = 0
        [HideInInspector]_QueueOffset("_QueueOffset", Float) = 0
        [HideInInspector]_QueueControl("_QueueControl", Float) = -1
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "UniversalMaterialType" = "Unlit"
            "Queue"="Transparent"
            "DisableBatching"="False"
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"="UniversalUnlitSubTarget"
        }
        Pass
        {
            Name "Universal Forward"
            Tags
            {
                // LightMode: <None>
            }
        
        // Render State
        Cull [_Cull]
        Blend [_SrcBlend] [_DstBlend]
        ZTest [_ZTest]
        ZWrite [_ZWrite]
        AlphaToMask [_AlphaToMask]
            Stencil
            {
                Ref 55          // Set reference value to 1
                Comp NotEqual    // Always pass the stencil test
                Pass Replace   // Replace the stencil buffer with 1
                Fail Keep      // Keep the current stencil value if the test fails
            }
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        #pragma instancing_options renderinglayer
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma multi_compile _ LIGHTMAP_ON
        #pragma multi_compile _ DIRLIGHTMAP_COMBINED
        #pragma shader_feature _ _SAMPLE_GI
        #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
        #pragma multi_compile_fragment _ DEBUG_DISPLAY
        #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
        #pragma shader_feature_fragment _ _SURFACE_TYPE_TRANSPARENT
        #pragma shader_feature_local_fragment _ _ALPHAPREMULTIPLY_ON
        #pragma shader_feature_local_fragment _ _ALPHAMODULATE_ON
        #pragma shader_feature_local_fragment _ _ALPHATEST_ON
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_UNLIT
        #define _FOG_FRAGMENT 1
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RenderingLayers.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRendering.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 WorldSpacePosition;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
             float3 WorldSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS : INTERP0;
             float3 normalWS : INTERP1;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.positionWS.xyz = input.positionWS;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.positionWS.xyz;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _Color;
        float _MaxShadowDistance;
        float _EdgeFade;
        CBUFFER_END
        
        
        // Object and Global properties
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Distance_float3(float3 A, float3 B, out float Out)
        {
            Out = distance(A, B);
        }
        
        void Unity_Minimum_float(float A, float B, out float Out)
        {
            Out = min(A, B);
        };
        
        void Unity_Length_float3(float3 In, out float Out)
        {
            Out = length(In);
        }
        
        void Unity_Step_float(float Edge, float In, out float Out)
        {
            Out = step(Edge, In);
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Multiply_float3_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A * B;
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Clamp_float(float In, float Min, float Max, out float Out)
        {
            Out = clamp(In, Min, Max);
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Branch_float(float Predicate, float True, float False, out float Out)
        {
            Out = Predicate ? True : False;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            float _Property_c656c91f25774e7984172fa19514b92e_Out_0_Float = _MaxShadowDistance;
            float _Distance_e5ebf3ede2c749c6b7c0841481be5f81_Out_2_Float;
            Unity_Distance_float3(IN.WorldSpacePosition, float3(0, 0, 0), _Distance_e5ebf3ede2c749c6b7c0841481be5f81_Out_2_Float);
            float _Minimum_f744ee9847cc47b2bb39bb0b0560c8b4_Out_2_Float;
            Unity_Minimum_float(_Property_c656c91f25774e7984172fa19514b92e_Out_0_Float, _Distance_e5ebf3ede2c749c6b7c0841481be5f81_Out_2_Float, _Minimum_f744ee9847cc47b2bb39bb0b0560c8b4_Out_2_Float);
            float _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float;
            Unity_Length_float3(SHADERGRAPH_OBJECT_POSITION, _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float);
            float _Step_fff4387de4184026b3f63bd694128d7d_Out_2_Float;
            Unity_Step_float(_Minimum_f744ee9847cc47b2bb39bb0b0560c8b4_Out_2_Float, _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float, _Step_fff4387de4184026b3f63bd694128d7d_Out_2_Float);
            float _Multiply_a7f7b26fdabf42c688c8bac2db01dc86_Out_2_Float;
            Unity_Multiply_float_float(100, _Step_fff4387de4184026b3f63bd694128d7d_Out_2_Float, _Multiply_a7f7b26fdabf42c688c8bac2db01dc86_Out_2_Float);
            float _Add_ad0d985e0ce449cf8446a76faa7f3fce_Out_2_Float;
            Unity_Add_float(float(1), _Multiply_a7f7b26fdabf42c688c8bac2db01dc86_Out_2_Float, _Add_ad0d985e0ce449cf8446a76faa7f3fce_Out_2_Float);
            float3 _Multiply_574cc6d494324fb0859a3ea1f4bf2dc3_Out_2_Vector3;
            Unity_Multiply_float3_float3(IN.WorldSpacePosition, (_Add_ad0d985e0ce449cf8446a76faa7f3fce_Out_2_Float.xxx), _Multiply_574cc6d494324fb0859a3ea1f4bf2dc3_Out_2_Vector3);
            float3 _Transform_42f53b04feab499a923e167620802eec_Out_1_Vector3;
            _Transform_42f53b04feab499a923e167620802eec_Out_1_Vector3 = TransformWorldToObject(_Multiply_574cc6d494324fb0859a3ea1f4bf2dc3_Out_2_Vector3.xyz);
            float3 _Multiply_83fb2cd5918d43bf8baae7e9d8ef04e2_Out_2_Vector3;
            Unity_Multiply_float3_float3(_Transform_42f53b04feab499a923e167620802eec_Out_1_Vector3, float3(0.999, 0.999, 0.999), _Multiply_83fb2cd5918d43bf8baae7e9d8ef04e2_Out_2_Vector3);
            description.Position = _Multiply_83fb2cd5918d43bf8baae7e9d8ef04e2_Out_2_Vector3;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _Property_edd9705f2a5c4d80bdfc5ff8ac7b9b9d_Out_0_Vector4 = _Color;
            float _Split_e7890a9c98d44536ac4fc7b9b47ff685_R_1_Float = _Property_edd9705f2a5c4d80bdfc5ff8ac7b9b9d_Out_0_Vector4[0];
            float _Split_e7890a9c98d44536ac4fc7b9b47ff685_G_2_Float = _Property_edd9705f2a5c4d80bdfc5ff8ac7b9b9d_Out_0_Vector4[1];
            float _Split_e7890a9c98d44536ac4fc7b9b47ff685_B_3_Float = _Property_edd9705f2a5c4d80bdfc5ff8ac7b9b9d_Out_0_Vector4[2];
            float _Split_e7890a9c98d44536ac4fc7b9b47ff685_A_4_Float = _Property_edd9705f2a5c4d80bdfc5ff8ac7b9b9d_Out_0_Vector4[3];
            float _Property_c656c91f25774e7984172fa19514b92e_Out_0_Float = _MaxShadowDistance;
            float _Distance_e5ebf3ede2c749c6b7c0841481be5f81_Out_2_Float;
            Unity_Distance_float3(IN.WorldSpacePosition, float3(0, 0, 0), _Distance_e5ebf3ede2c749c6b7c0841481be5f81_Out_2_Float);
            float _Minimum_8d7eee937d2845bf9b6b429c58250008_Out_2_Float;
            Unity_Minimum_float(_Property_c656c91f25774e7984172fa19514b92e_Out_0_Float, _Distance_e5ebf3ede2c749c6b7c0841481be5f81_Out_2_Float, _Minimum_8d7eee937d2845bf9b6b429c58250008_Out_2_Float);
            float _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float;
            Unity_Length_float3(SHADERGRAPH_OBJECT_POSITION, _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float);
            float _Subtract_3fc8a02b2d9e42e0aa9b0eb631d939c5_Out_2_Float;
            Unity_Subtract_float(_Minimum_8d7eee937d2845bf9b6b429c58250008_Out_2_Float, _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float, _Subtract_3fc8a02b2d9e42e0aa9b0eb631d939c5_Out_2_Float);
            float _Property_a762658b7eee44d2a7532052f595aba4_Out_0_Float = _MaxShadowDistance;
            float _Subtract_946d1263fda345cab3d54d518174aa5c_Out_2_Float;
            Unity_Subtract_float(_Property_a762658b7eee44d2a7532052f595aba4_Out_0_Float, _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float, _Subtract_946d1263fda345cab3d54d518174aa5c_Out_2_Float);
            float _Divide_c9a883d290984549996d25597c1a190b_Out_2_Float;
            Unity_Divide_float(_Subtract_3fc8a02b2d9e42e0aa9b0eb631d939c5_Out_2_Float, _Subtract_946d1263fda345cab3d54d518174aa5c_Out_2_Float, _Divide_c9a883d290984549996d25597c1a190b_Out_2_Float);
            float _Clamp_d1c037face49410eb9d987ee5ae5ba6b_Out_3_Float;
            Unity_Clamp_float(_Divide_c9a883d290984549996d25597c1a190b_Out_2_Float, float(0), float(1), _Clamp_d1c037face49410eb9d987ee5ae5ba6b_Out_3_Float);
            float _OneMinus_347ec05d74424bd18eb0df7bd00eb215_Out_1_Float;
            Unity_OneMinus_float(_Clamp_d1c037face49410eb9d987ee5ae5ba6b_Out_3_Float, _OneMinus_347ec05d74424bd18eb0df7bd00eb215_Out_1_Float);
            float _Multiply_7ca0eb8ce2464c6faeb0dc403d041b7d_Out_2_Float;
            Unity_Multiply_float_float(_Split_e7890a9c98d44536ac4fc7b9b47ff685_A_4_Float, _OneMinus_347ec05d74424bd18eb0df7bd00eb215_Out_1_Float, _Multiply_7ca0eb8ce2464c6faeb0dc403d041b7d_Out_2_Float);
            float _Multiply_f94a5e79dae346b8a058ef3a0822f23e_Out_2_Float;
            Unity_Multiply_float_float(_Multiply_7ca0eb8ce2464c6faeb0dc403d041b7d_Out_2_Float, 2, _Multiply_f94a5e79dae346b8a058ef3a0822f23e_Out_2_Float);
            float _Branch_b8f1103dd634435c881c530c864dc3a7_Out_3_Float;
            Unity_Branch_float(1, _Multiply_f94a5e79dae346b8a058ef3a0822f23e_Out_2_Float, float(0), _Branch_b8f1103dd634435c881c530c864dc3a7_Out_3_Float);
            surface.BaseColor = (_Property_edd9705f2a5c4d80bdfc5ff8ac7b9b9d_Out_0_Vector4.xyz);
            surface.Alpha = _Branch_b8f1103dd634435c881c530c864dc3a7_Out_3_Float;
            surface.AlphaClipThreshold = float(0.5);
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
            output.WorldSpacePosition =                         TransformObjectToWorld(input.positionOS);
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
            output.WorldSpacePosition = input.positionWS;
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/UnlitPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "DepthOnly"
            Tags
            {
                "LightMode" = "DepthOnly"
            }
        
        // Render State
        Cull [_Cull]
        ZTest LEqual
        ZWrite On
        ColorMask R
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma multi_compile_instancing
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma shader_feature_local_fragment _ _ALPHATEST_ON
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define VARYINGS_NEED_POSITION_WS
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRendering.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 WorldSpacePosition;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
             float3 WorldSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS : INTERP0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.positionWS.xyz = input.positionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.positionWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _Color;
        float _MaxShadowDistance;
        float _EdgeFade;
        CBUFFER_END
        
        
        // Object and Global properties
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Distance_float3(float3 A, float3 B, out float Out)
        {
            Out = distance(A, B);
        }
        
        void Unity_Minimum_float(float A, float B, out float Out)
        {
            Out = min(A, B);
        };
        
        void Unity_Length_float3(float3 In, out float Out)
        {
            Out = length(In);
        }
        
        void Unity_Step_float(float Edge, float In, out float Out)
        {
            Out = step(Edge, In);
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Multiply_float3_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A * B;
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Clamp_float(float In, float Min, float Max, out float Out)
        {
            Out = clamp(In, Min, Max);
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Branch_float(float Predicate, float True, float False, out float Out)
        {
            Out = Predicate ? True : False;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            float _Property_c656c91f25774e7984172fa19514b92e_Out_0_Float = _MaxShadowDistance;
            float _Distance_e5ebf3ede2c749c6b7c0841481be5f81_Out_2_Float;
            Unity_Distance_float3(IN.WorldSpacePosition, float3(0, 0, 0), _Distance_e5ebf3ede2c749c6b7c0841481be5f81_Out_2_Float);
            float _Minimum_f744ee9847cc47b2bb39bb0b0560c8b4_Out_2_Float;
            Unity_Minimum_float(_Property_c656c91f25774e7984172fa19514b92e_Out_0_Float, _Distance_e5ebf3ede2c749c6b7c0841481be5f81_Out_2_Float, _Minimum_f744ee9847cc47b2bb39bb0b0560c8b4_Out_2_Float);
            float _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float;
            Unity_Length_float3(SHADERGRAPH_OBJECT_POSITION, _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float);
            float _Step_fff4387de4184026b3f63bd694128d7d_Out_2_Float;
            Unity_Step_float(_Minimum_f744ee9847cc47b2bb39bb0b0560c8b4_Out_2_Float, _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float, _Step_fff4387de4184026b3f63bd694128d7d_Out_2_Float);
            float _Multiply_a7f7b26fdabf42c688c8bac2db01dc86_Out_2_Float;
            Unity_Multiply_float_float(100, _Step_fff4387de4184026b3f63bd694128d7d_Out_2_Float, _Multiply_a7f7b26fdabf42c688c8bac2db01dc86_Out_2_Float);
            float _Add_ad0d985e0ce449cf8446a76faa7f3fce_Out_2_Float;
            Unity_Add_float(float(1), _Multiply_a7f7b26fdabf42c688c8bac2db01dc86_Out_2_Float, _Add_ad0d985e0ce449cf8446a76faa7f3fce_Out_2_Float);
            float3 _Multiply_574cc6d494324fb0859a3ea1f4bf2dc3_Out_2_Vector3;
            Unity_Multiply_float3_float3(IN.WorldSpacePosition, (_Add_ad0d985e0ce449cf8446a76faa7f3fce_Out_2_Float.xxx), _Multiply_574cc6d494324fb0859a3ea1f4bf2dc3_Out_2_Vector3);
            float3 _Transform_42f53b04feab499a923e167620802eec_Out_1_Vector3;
            _Transform_42f53b04feab499a923e167620802eec_Out_1_Vector3 = TransformWorldToObject(_Multiply_574cc6d494324fb0859a3ea1f4bf2dc3_Out_2_Vector3.xyz);
            float3 _Multiply_83fb2cd5918d43bf8baae7e9d8ef04e2_Out_2_Vector3;
            Unity_Multiply_float3_float3(_Transform_42f53b04feab499a923e167620802eec_Out_1_Vector3, float3(0.999, 0.999, 0.999), _Multiply_83fb2cd5918d43bf8baae7e9d8ef04e2_Out_2_Vector3);
            description.Position = _Multiply_83fb2cd5918d43bf8baae7e9d8ef04e2_Out_2_Vector3;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _Property_edd9705f2a5c4d80bdfc5ff8ac7b9b9d_Out_0_Vector4 = _Color;
            float _Split_e7890a9c98d44536ac4fc7b9b47ff685_R_1_Float = _Property_edd9705f2a5c4d80bdfc5ff8ac7b9b9d_Out_0_Vector4[0];
            float _Split_e7890a9c98d44536ac4fc7b9b47ff685_G_2_Float = _Property_edd9705f2a5c4d80bdfc5ff8ac7b9b9d_Out_0_Vector4[1];
            float _Split_e7890a9c98d44536ac4fc7b9b47ff685_B_3_Float = _Property_edd9705f2a5c4d80bdfc5ff8ac7b9b9d_Out_0_Vector4[2];
            float _Split_e7890a9c98d44536ac4fc7b9b47ff685_A_4_Float = _Property_edd9705f2a5c4d80bdfc5ff8ac7b9b9d_Out_0_Vector4[3];
            float _Property_c656c91f25774e7984172fa19514b92e_Out_0_Float = _MaxShadowDistance;
            float _Distance_e5ebf3ede2c749c6b7c0841481be5f81_Out_2_Float;
            Unity_Distance_float3(IN.WorldSpacePosition, float3(0, 0, 0), _Distance_e5ebf3ede2c749c6b7c0841481be5f81_Out_2_Float);
            float _Minimum_8d7eee937d2845bf9b6b429c58250008_Out_2_Float;
            Unity_Minimum_float(_Property_c656c91f25774e7984172fa19514b92e_Out_0_Float, _Distance_e5ebf3ede2c749c6b7c0841481be5f81_Out_2_Float, _Minimum_8d7eee937d2845bf9b6b429c58250008_Out_2_Float);
            float _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float;
            Unity_Length_float3(SHADERGRAPH_OBJECT_POSITION, _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float);
            float _Subtract_3fc8a02b2d9e42e0aa9b0eb631d939c5_Out_2_Float;
            Unity_Subtract_float(_Minimum_8d7eee937d2845bf9b6b429c58250008_Out_2_Float, _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float, _Subtract_3fc8a02b2d9e42e0aa9b0eb631d939c5_Out_2_Float);
            float _Property_a762658b7eee44d2a7532052f595aba4_Out_0_Float = _MaxShadowDistance;
            float _Subtract_946d1263fda345cab3d54d518174aa5c_Out_2_Float;
            Unity_Subtract_float(_Property_a762658b7eee44d2a7532052f595aba4_Out_0_Float, _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float, _Subtract_946d1263fda345cab3d54d518174aa5c_Out_2_Float);
            float _Divide_c9a883d290984549996d25597c1a190b_Out_2_Float;
            Unity_Divide_float(_Subtract_3fc8a02b2d9e42e0aa9b0eb631d939c5_Out_2_Float, _Subtract_946d1263fda345cab3d54d518174aa5c_Out_2_Float, _Divide_c9a883d290984549996d25597c1a190b_Out_2_Float);
            float _Clamp_d1c037face49410eb9d987ee5ae5ba6b_Out_3_Float;
            Unity_Clamp_float(_Divide_c9a883d290984549996d25597c1a190b_Out_2_Float, float(0), float(1), _Clamp_d1c037face49410eb9d987ee5ae5ba6b_Out_3_Float);
            float _OneMinus_347ec05d74424bd18eb0df7bd00eb215_Out_1_Float;
            Unity_OneMinus_float(_Clamp_d1c037face49410eb9d987ee5ae5ba6b_Out_3_Float, _OneMinus_347ec05d74424bd18eb0df7bd00eb215_Out_1_Float);
            float _Multiply_7ca0eb8ce2464c6faeb0dc403d041b7d_Out_2_Float;
            Unity_Multiply_float_float(_Split_e7890a9c98d44536ac4fc7b9b47ff685_A_4_Float, _OneMinus_347ec05d74424bd18eb0df7bd00eb215_Out_1_Float, _Multiply_7ca0eb8ce2464c6faeb0dc403d041b7d_Out_2_Float);
            float _Multiply_f94a5e79dae346b8a058ef3a0822f23e_Out_2_Float;
            Unity_Multiply_float_float(_Multiply_7ca0eb8ce2464c6faeb0dc403d041b7d_Out_2_Float, 2, _Multiply_f94a5e79dae346b8a058ef3a0822f23e_Out_2_Float);
            float _Branch_b8f1103dd634435c881c530c864dc3a7_Out_3_Float;
            Unity_Branch_float(1, _Multiply_f94a5e79dae346b8a058ef3a0822f23e_Out_2_Float, float(0), _Branch_b8f1103dd634435c881c530c864dc3a7_Out_3_Float);
            surface.Alpha = _Branch_b8f1103dd634435c881c530c864dc3a7_Out_3_Float;
            surface.AlphaClipThreshold = float(0.5);
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
            output.WorldSpacePosition =                         TransformObjectToWorld(input.positionOS);
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
            output.WorldSpacePosition = input.positionWS;
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthOnlyPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "DepthNormalsOnly"
            Tags
            {
                "LightMode" = "DepthNormalsOnly"
            }
        
        // Render State
        Cull [_Cull]
        ZTest LEqual
        ZWrite On
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma multi_compile_instancing
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT
        #pragma shader_feature_fragment _ _SURFACE_TYPE_TRANSPARENT
        #pragma shader_feature_local_fragment _ _ALPHAPREMULTIPLY_ON
        #pragma shader_feature_local_fragment _ _ALPHAMODULATE_ON
        #pragma shader_feature_local_fragment _ _ALPHATEST_ON
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHNORMALSONLY
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RenderingLayers.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRendering.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 WorldSpacePosition;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
             float3 WorldSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS : INTERP0;
             float3 normalWS : INTERP1;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.positionWS.xyz = input.positionWS;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.positionWS.xyz;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _Color;
        float _MaxShadowDistance;
        float _EdgeFade;
        CBUFFER_END
        
        
        // Object and Global properties
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Distance_float3(float3 A, float3 B, out float Out)
        {
            Out = distance(A, B);
        }
        
        void Unity_Minimum_float(float A, float B, out float Out)
        {
            Out = min(A, B);
        };
        
        void Unity_Length_float3(float3 In, out float Out)
        {
            Out = length(In);
        }
        
        void Unity_Step_float(float Edge, float In, out float Out)
        {
            Out = step(Edge, In);
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Multiply_float3_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A * B;
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Clamp_float(float In, float Min, float Max, out float Out)
        {
            Out = clamp(In, Min, Max);
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Branch_float(float Predicate, float True, float False, out float Out)
        {
            Out = Predicate ? True : False;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            float _Property_c656c91f25774e7984172fa19514b92e_Out_0_Float = _MaxShadowDistance;
            float _Distance_e5ebf3ede2c749c6b7c0841481be5f81_Out_2_Float;
            Unity_Distance_float3(IN.WorldSpacePosition, float3(0, 0, 0), _Distance_e5ebf3ede2c749c6b7c0841481be5f81_Out_2_Float);
            float _Minimum_f744ee9847cc47b2bb39bb0b0560c8b4_Out_2_Float;
            Unity_Minimum_float(_Property_c656c91f25774e7984172fa19514b92e_Out_0_Float, _Distance_e5ebf3ede2c749c6b7c0841481be5f81_Out_2_Float, _Minimum_f744ee9847cc47b2bb39bb0b0560c8b4_Out_2_Float);
            float _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float;
            Unity_Length_float3(SHADERGRAPH_OBJECT_POSITION, _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float);
            float _Step_fff4387de4184026b3f63bd694128d7d_Out_2_Float;
            Unity_Step_float(_Minimum_f744ee9847cc47b2bb39bb0b0560c8b4_Out_2_Float, _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float, _Step_fff4387de4184026b3f63bd694128d7d_Out_2_Float);
            float _Multiply_a7f7b26fdabf42c688c8bac2db01dc86_Out_2_Float;
            Unity_Multiply_float_float(100, _Step_fff4387de4184026b3f63bd694128d7d_Out_2_Float, _Multiply_a7f7b26fdabf42c688c8bac2db01dc86_Out_2_Float);
            float _Add_ad0d985e0ce449cf8446a76faa7f3fce_Out_2_Float;
            Unity_Add_float(float(1), _Multiply_a7f7b26fdabf42c688c8bac2db01dc86_Out_2_Float, _Add_ad0d985e0ce449cf8446a76faa7f3fce_Out_2_Float);
            float3 _Multiply_574cc6d494324fb0859a3ea1f4bf2dc3_Out_2_Vector3;
            Unity_Multiply_float3_float3(IN.WorldSpacePosition, (_Add_ad0d985e0ce449cf8446a76faa7f3fce_Out_2_Float.xxx), _Multiply_574cc6d494324fb0859a3ea1f4bf2dc3_Out_2_Vector3);
            float3 _Transform_42f53b04feab499a923e167620802eec_Out_1_Vector3;
            _Transform_42f53b04feab499a923e167620802eec_Out_1_Vector3 = TransformWorldToObject(_Multiply_574cc6d494324fb0859a3ea1f4bf2dc3_Out_2_Vector3.xyz);
            float3 _Multiply_83fb2cd5918d43bf8baae7e9d8ef04e2_Out_2_Vector3;
            Unity_Multiply_float3_float3(_Transform_42f53b04feab499a923e167620802eec_Out_1_Vector3, float3(0.999, 0.999, 0.999), _Multiply_83fb2cd5918d43bf8baae7e9d8ef04e2_Out_2_Vector3);
            description.Position = _Multiply_83fb2cd5918d43bf8baae7e9d8ef04e2_Out_2_Vector3;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _Property_edd9705f2a5c4d80bdfc5ff8ac7b9b9d_Out_0_Vector4 = _Color;
            float _Split_e7890a9c98d44536ac4fc7b9b47ff685_R_1_Float = _Property_edd9705f2a5c4d80bdfc5ff8ac7b9b9d_Out_0_Vector4[0];
            float _Split_e7890a9c98d44536ac4fc7b9b47ff685_G_2_Float = _Property_edd9705f2a5c4d80bdfc5ff8ac7b9b9d_Out_0_Vector4[1];
            float _Split_e7890a9c98d44536ac4fc7b9b47ff685_B_3_Float = _Property_edd9705f2a5c4d80bdfc5ff8ac7b9b9d_Out_0_Vector4[2];
            float _Split_e7890a9c98d44536ac4fc7b9b47ff685_A_4_Float = _Property_edd9705f2a5c4d80bdfc5ff8ac7b9b9d_Out_0_Vector4[3];
            float _Property_c656c91f25774e7984172fa19514b92e_Out_0_Float = _MaxShadowDistance;
            float _Distance_e5ebf3ede2c749c6b7c0841481be5f81_Out_2_Float;
            Unity_Distance_float3(IN.WorldSpacePosition, float3(0, 0, 0), _Distance_e5ebf3ede2c749c6b7c0841481be5f81_Out_2_Float);
            float _Minimum_8d7eee937d2845bf9b6b429c58250008_Out_2_Float;
            Unity_Minimum_float(_Property_c656c91f25774e7984172fa19514b92e_Out_0_Float, _Distance_e5ebf3ede2c749c6b7c0841481be5f81_Out_2_Float, _Minimum_8d7eee937d2845bf9b6b429c58250008_Out_2_Float);
            float _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float;
            Unity_Length_float3(SHADERGRAPH_OBJECT_POSITION, _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float);
            float _Subtract_3fc8a02b2d9e42e0aa9b0eb631d939c5_Out_2_Float;
            Unity_Subtract_float(_Minimum_8d7eee937d2845bf9b6b429c58250008_Out_2_Float, _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float, _Subtract_3fc8a02b2d9e42e0aa9b0eb631d939c5_Out_2_Float);
            float _Property_a762658b7eee44d2a7532052f595aba4_Out_0_Float = _MaxShadowDistance;
            float _Subtract_946d1263fda345cab3d54d518174aa5c_Out_2_Float;
            Unity_Subtract_float(_Property_a762658b7eee44d2a7532052f595aba4_Out_0_Float, _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float, _Subtract_946d1263fda345cab3d54d518174aa5c_Out_2_Float);
            float _Divide_c9a883d290984549996d25597c1a190b_Out_2_Float;
            Unity_Divide_float(_Subtract_3fc8a02b2d9e42e0aa9b0eb631d939c5_Out_2_Float, _Subtract_946d1263fda345cab3d54d518174aa5c_Out_2_Float, _Divide_c9a883d290984549996d25597c1a190b_Out_2_Float);
            float _Clamp_d1c037face49410eb9d987ee5ae5ba6b_Out_3_Float;
            Unity_Clamp_float(_Divide_c9a883d290984549996d25597c1a190b_Out_2_Float, float(0), float(1), _Clamp_d1c037face49410eb9d987ee5ae5ba6b_Out_3_Float);
            float _OneMinus_347ec05d74424bd18eb0df7bd00eb215_Out_1_Float;
            Unity_OneMinus_float(_Clamp_d1c037face49410eb9d987ee5ae5ba6b_Out_3_Float, _OneMinus_347ec05d74424bd18eb0df7bd00eb215_Out_1_Float);
            float _Multiply_7ca0eb8ce2464c6faeb0dc403d041b7d_Out_2_Float;
            Unity_Multiply_float_float(_Split_e7890a9c98d44536ac4fc7b9b47ff685_A_4_Float, _OneMinus_347ec05d74424bd18eb0df7bd00eb215_Out_1_Float, _Multiply_7ca0eb8ce2464c6faeb0dc403d041b7d_Out_2_Float);
            float _Multiply_f94a5e79dae346b8a058ef3a0822f23e_Out_2_Float;
            Unity_Multiply_float_float(_Multiply_7ca0eb8ce2464c6faeb0dc403d041b7d_Out_2_Float, 2, _Multiply_f94a5e79dae346b8a058ef3a0822f23e_Out_2_Float);
            float _Branch_b8f1103dd634435c881c530c864dc3a7_Out_3_Float;
            Unity_Branch_float(1, _Multiply_f94a5e79dae346b8a058ef3a0822f23e_Out_2_Float, float(0), _Branch_b8f1103dd634435c881c530c864dc3a7_Out_3_Float);
            surface.Alpha = _Branch_b8f1103dd634435c881c530c864dc3a7_Out_3_Float;
            surface.AlphaClipThreshold = float(0.5);
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
            output.WorldSpacePosition =                         TransformObjectToWorld(input.positionOS);
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
            output.WorldSpacePosition = input.positionWS;
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthNormalsOnlyPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "ShadowCaster"
            Tags
            {
                "LightMode" = "ShadowCaster"
            }
        
        // Render State
        Cull [_Cull]
        ZTest LEqual
        ZWrite On
        ColorMask 0
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma multi_compile_instancing
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW
        #pragma shader_feature_local_fragment _ _ALPHATEST_ON
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SHADOWCASTER
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRendering.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 WorldSpacePosition;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
             float3 WorldSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS : INTERP0;
             float3 normalWS : INTERP1;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.positionWS.xyz = input.positionWS;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.positionWS.xyz;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _Color;
        float _MaxShadowDistance;
        float _EdgeFade;
        CBUFFER_END
        
        
        // Object and Global properties
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Distance_float3(float3 A, float3 B, out float Out)
        {
            Out = distance(A, B);
        }
        
        void Unity_Minimum_float(float A, float B, out float Out)
        {
            Out = min(A, B);
        };
        
        void Unity_Length_float3(float3 In, out float Out)
        {
            Out = length(In);
        }
        
        void Unity_Step_float(float Edge, float In, out float Out)
        {
            Out = step(Edge, In);
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Multiply_float3_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A * B;
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Clamp_float(float In, float Min, float Max, out float Out)
        {
            Out = clamp(In, Min, Max);
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Branch_float(float Predicate, float True, float False, out float Out)
        {
            Out = Predicate ? True : False;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            float _Property_c656c91f25774e7984172fa19514b92e_Out_0_Float = _MaxShadowDistance;
            float _Distance_e5ebf3ede2c749c6b7c0841481be5f81_Out_2_Float;
            Unity_Distance_float3(IN.WorldSpacePosition, float3(0, 0, 0), _Distance_e5ebf3ede2c749c6b7c0841481be5f81_Out_2_Float);
            float _Minimum_f744ee9847cc47b2bb39bb0b0560c8b4_Out_2_Float;
            Unity_Minimum_float(_Property_c656c91f25774e7984172fa19514b92e_Out_0_Float, _Distance_e5ebf3ede2c749c6b7c0841481be5f81_Out_2_Float, _Minimum_f744ee9847cc47b2bb39bb0b0560c8b4_Out_2_Float);
            float _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float;
            Unity_Length_float3(SHADERGRAPH_OBJECT_POSITION, _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float);
            float _Step_fff4387de4184026b3f63bd694128d7d_Out_2_Float;
            Unity_Step_float(_Minimum_f744ee9847cc47b2bb39bb0b0560c8b4_Out_2_Float, _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float, _Step_fff4387de4184026b3f63bd694128d7d_Out_2_Float);
            float _Multiply_a7f7b26fdabf42c688c8bac2db01dc86_Out_2_Float;
            Unity_Multiply_float_float(100, _Step_fff4387de4184026b3f63bd694128d7d_Out_2_Float, _Multiply_a7f7b26fdabf42c688c8bac2db01dc86_Out_2_Float);
            float _Add_ad0d985e0ce449cf8446a76faa7f3fce_Out_2_Float;
            Unity_Add_float(float(1), _Multiply_a7f7b26fdabf42c688c8bac2db01dc86_Out_2_Float, _Add_ad0d985e0ce449cf8446a76faa7f3fce_Out_2_Float);
            float3 _Multiply_574cc6d494324fb0859a3ea1f4bf2dc3_Out_2_Vector3;
            Unity_Multiply_float3_float3(IN.WorldSpacePosition, (_Add_ad0d985e0ce449cf8446a76faa7f3fce_Out_2_Float.xxx), _Multiply_574cc6d494324fb0859a3ea1f4bf2dc3_Out_2_Vector3);
            float3 _Transform_42f53b04feab499a923e167620802eec_Out_1_Vector3;
            _Transform_42f53b04feab499a923e167620802eec_Out_1_Vector3 = TransformWorldToObject(_Multiply_574cc6d494324fb0859a3ea1f4bf2dc3_Out_2_Vector3.xyz);
            float3 _Multiply_83fb2cd5918d43bf8baae7e9d8ef04e2_Out_2_Vector3;
            Unity_Multiply_float3_float3(_Transform_42f53b04feab499a923e167620802eec_Out_1_Vector3, float3(0.999, 0.999, 0.999), _Multiply_83fb2cd5918d43bf8baae7e9d8ef04e2_Out_2_Vector3);
            description.Position = _Multiply_83fb2cd5918d43bf8baae7e9d8ef04e2_Out_2_Vector3;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _Property_edd9705f2a5c4d80bdfc5ff8ac7b9b9d_Out_0_Vector4 = _Color;
            float _Split_e7890a9c98d44536ac4fc7b9b47ff685_R_1_Float = _Property_edd9705f2a5c4d80bdfc5ff8ac7b9b9d_Out_0_Vector4[0];
            float _Split_e7890a9c98d44536ac4fc7b9b47ff685_G_2_Float = _Property_edd9705f2a5c4d80bdfc5ff8ac7b9b9d_Out_0_Vector4[1];
            float _Split_e7890a9c98d44536ac4fc7b9b47ff685_B_3_Float = _Property_edd9705f2a5c4d80bdfc5ff8ac7b9b9d_Out_0_Vector4[2];
            float _Split_e7890a9c98d44536ac4fc7b9b47ff685_A_4_Float = _Property_edd9705f2a5c4d80bdfc5ff8ac7b9b9d_Out_0_Vector4[3];
            float _Property_c656c91f25774e7984172fa19514b92e_Out_0_Float = _MaxShadowDistance;
            float _Distance_e5ebf3ede2c749c6b7c0841481be5f81_Out_2_Float;
            Unity_Distance_float3(IN.WorldSpacePosition, float3(0, 0, 0), _Distance_e5ebf3ede2c749c6b7c0841481be5f81_Out_2_Float);
            float _Minimum_8d7eee937d2845bf9b6b429c58250008_Out_2_Float;
            Unity_Minimum_float(_Property_c656c91f25774e7984172fa19514b92e_Out_0_Float, _Distance_e5ebf3ede2c749c6b7c0841481be5f81_Out_2_Float, _Minimum_8d7eee937d2845bf9b6b429c58250008_Out_2_Float);
            float _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float;
            Unity_Length_float3(SHADERGRAPH_OBJECT_POSITION, _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float);
            float _Subtract_3fc8a02b2d9e42e0aa9b0eb631d939c5_Out_2_Float;
            Unity_Subtract_float(_Minimum_8d7eee937d2845bf9b6b429c58250008_Out_2_Float, _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float, _Subtract_3fc8a02b2d9e42e0aa9b0eb631d939c5_Out_2_Float);
            float _Property_a762658b7eee44d2a7532052f595aba4_Out_0_Float = _MaxShadowDistance;
            float _Subtract_946d1263fda345cab3d54d518174aa5c_Out_2_Float;
            Unity_Subtract_float(_Property_a762658b7eee44d2a7532052f595aba4_Out_0_Float, _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float, _Subtract_946d1263fda345cab3d54d518174aa5c_Out_2_Float);
            float _Divide_c9a883d290984549996d25597c1a190b_Out_2_Float;
            Unity_Divide_float(_Subtract_3fc8a02b2d9e42e0aa9b0eb631d939c5_Out_2_Float, _Subtract_946d1263fda345cab3d54d518174aa5c_Out_2_Float, _Divide_c9a883d290984549996d25597c1a190b_Out_2_Float);
            float _Clamp_d1c037face49410eb9d987ee5ae5ba6b_Out_3_Float;
            Unity_Clamp_float(_Divide_c9a883d290984549996d25597c1a190b_Out_2_Float, float(0), float(1), _Clamp_d1c037face49410eb9d987ee5ae5ba6b_Out_3_Float);
            float _OneMinus_347ec05d74424bd18eb0df7bd00eb215_Out_1_Float;
            Unity_OneMinus_float(_Clamp_d1c037face49410eb9d987ee5ae5ba6b_Out_3_Float, _OneMinus_347ec05d74424bd18eb0df7bd00eb215_Out_1_Float);
            float _Multiply_7ca0eb8ce2464c6faeb0dc403d041b7d_Out_2_Float;
            Unity_Multiply_float_float(_Split_e7890a9c98d44536ac4fc7b9b47ff685_A_4_Float, _OneMinus_347ec05d74424bd18eb0df7bd00eb215_Out_1_Float, _Multiply_7ca0eb8ce2464c6faeb0dc403d041b7d_Out_2_Float);
            float _Multiply_f94a5e79dae346b8a058ef3a0822f23e_Out_2_Float;
            Unity_Multiply_float_float(_Multiply_7ca0eb8ce2464c6faeb0dc403d041b7d_Out_2_Float, 2, _Multiply_f94a5e79dae346b8a058ef3a0822f23e_Out_2_Float);
            float _Branch_b8f1103dd634435c881c530c864dc3a7_Out_3_Float;
            Unity_Branch_float(1, _Multiply_f94a5e79dae346b8a058ef3a0822f23e_Out_2_Float, float(0), _Branch_b8f1103dd634435c881c530c864dc3a7_Out_3_Float);
            surface.Alpha = _Branch_b8f1103dd634435c881c530c864dc3a7_Out_3_Float;
            surface.AlphaClipThreshold = float(0.5);
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
            output.WorldSpacePosition =                         TransformObjectToWorld(input.positionOS);
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
            output.WorldSpacePosition = input.positionWS;
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "GBuffer"
            Tags
            {
                "LightMode" = "UniversalGBuffer"
            }
        
        // Render State
        Cull [_Cull]
        Blend [_SrcBlend] [_DstBlend]
        ZTest [_ZTest]
        ZWrite [_ZWrite]
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        #pragma instancing_options renderinglayer
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
        #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
        #pragma shader_feature_fragment _ _SURFACE_TYPE_TRANSPARENT
        #pragma shader_feature_local_fragment _ _ALPHAPREMULTIPLY_ON
        #pragma shader_feature_local_fragment _ _ALPHAMODULATE_ON
        #pragma shader_feature_local_fragment _ _ALPHATEST_ON
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_GBUFFER
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRendering.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
            #if !defined(LIGHTMAP_ON)
             float3 sh;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 WorldSpacePosition;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
             float3 WorldSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
            #if !defined(LIGHTMAP_ON)
             float3 sh : INTERP0;
            #endif
             float3 positionWS : INTERP1;
             float3 normalWS : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            #if !defined(LIGHTMAP_ON)
            output.sh = input.sh;
            #endif
            output.positionWS.xyz = input.positionWS;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            #if !defined(LIGHTMAP_ON)
            output.sh = input.sh;
            #endif
            output.positionWS = input.positionWS.xyz;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _Color;
        float _MaxShadowDistance;
        float _EdgeFade;
        CBUFFER_END
        
        
        // Object and Global properties
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Distance_float3(float3 A, float3 B, out float Out)
        {
            Out = distance(A, B);
        }
        
        void Unity_Minimum_float(float A, float B, out float Out)
        {
            Out = min(A, B);
        };
        
        void Unity_Length_float3(float3 In, out float Out)
        {
            Out = length(In);
        }
        
        void Unity_Step_float(float Edge, float In, out float Out)
        {
            Out = step(Edge, In);
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Multiply_float3_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A * B;
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Clamp_float(float In, float Min, float Max, out float Out)
        {
            Out = clamp(In, Min, Max);
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Branch_float(float Predicate, float True, float False, out float Out)
        {
            Out = Predicate ? True : False;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            float _Property_c656c91f25774e7984172fa19514b92e_Out_0_Float = _MaxShadowDistance;
            float _Distance_e5ebf3ede2c749c6b7c0841481be5f81_Out_2_Float;
            Unity_Distance_float3(IN.WorldSpacePosition, float3(0, 0, 0), _Distance_e5ebf3ede2c749c6b7c0841481be5f81_Out_2_Float);
            float _Minimum_f744ee9847cc47b2bb39bb0b0560c8b4_Out_2_Float;
            Unity_Minimum_float(_Property_c656c91f25774e7984172fa19514b92e_Out_0_Float, _Distance_e5ebf3ede2c749c6b7c0841481be5f81_Out_2_Float, _Minimum_f744ee9847cc47b2bb39bb0b0560c8b4_Out_2_Float);
            float _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float;
            Unity_Length_float3(SHADERGRAPH_OBJECT_POSITION, _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float);
            float _Step_fff4387de4184026b3f63bd694128d7d_Out_2_Float;
            Unity_Step_float(_Minimum_f744ee9847cc47b2bb39bb0b0560c8b4_Out_2_Float, _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float, _Step_fff4387de4184026b3f63bd694128d7d_Out_2_Float);
            float _Multiply_a7f7b26fdabf42c688c8bac2db01dc86_Out_2_Float;
            Unity_Multiply_float_float(100, _Step_fff4387de4184026b3f63bd694128d7d_Out_2_Float, _Multiply_a7f7b26fdabf42c688c8bac2db01dc86_Out_2_Float);
            float _Add_ad0d985e0ce449cf8446a76faa7f3fce_Out_2_Float;
            Unity_Add_float(float(1), _Multiply_a7f7b26fdabf42c688c8bac2db01dc86_Out_2_Float, _Add_ad0d985e0ce449cf8446a76faa7f3fce_Out_2_Float);
            float3 _Multiply_574cc6d494324fb0859a3ea1f4bf2dc3_Out_2_Vector3;
            Unity_Multiply_float3_float3(IN.WorldSpacePosition, (_Add_ad0d985e0ce449cf8446a76faa7f3fce_Out_2_Float.xxx), _Multiply_574cc6d494324fb0859a3ea1f4bf2dc3_Out_2_Vector3);
            float3 _Transform_42f53b04feab499a923e167620802eec_Out_1_Vector3;
            _Transform_42f53b04feab499a923e167620802eec_Out_1_Vector3 = TransformWorldToObject(_Multiply_574cc6d494324fb0859a3ea1f4bf2dc3_Out_2_Vector3.xyz);
            float3 _Multiply_83fb2cd5918d43bf8baae7e9d8ef04e2_Out_2_Vector3;
            Unity_Multiply_float3_float3(_Transform_42f53b04feab499a923e167620802eec_Out_1_Vector3, float3(0.999, 0.999, 0.999), _Multiply_83fb2cd5918d43bf8baae7e9d8ef04e2_Out_2_Vector3);
            description.Position = _Multiply_83fb2cd5918d43bf8baae7e9d8ef04e2_Out_2_Vector3;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _Property_edd9705f2a5c4d80bdfc5ff8ac7b9b9d_Out_0_Vector4 = _Color;
            float _Split_e7890a9c98d44536ac4fc7b9b47ff685_R_1_Float = _Property_edd9705f2a5c4d80bdfc5ff8ac7b9b9d_Out_0_Vector4[0];
            float _Split_e7890a9c98d44536ac4fc7b9b47ff685_G_2_Float = _Property_edd9705f2a5c4d80bdfc5ff8ac7b9b9d_Out_0_Vector4[1];
            float _Split_e7890a9c98d44536ac4fc7b9b47ff685_B_3_Float = _Property_edd9705f2a5c4d80bdfc5ff8ac7b9b9d_Out_0_Vector4[2];
            float _Split_e7890a9c98d44536ac4fc7b9b47ff685_A_4_Float = _Property_edd9705f2a5c4d80bdfc5ff8ac7b9b9d_Out_0_Vector4[3];
            float _Property_c656c91f25774e7984172fa19514b92e_Out_0_Float = _MaxShadowDistance;
            float _Distance_e5ebf3ede2c749c6b7c0841481be5f81_Out_2_Float;
            Unity_Distance_float3(IN.WorldSpacePosition, float3(0, 0, 0), _Distance_e5ebf3ede2c749c6b7c0841481be5f81_Out_2_Float);
            float _Minimum_8d7eee937d2845bf9b6b429c58250008_Out_2_Float;
            Unity_Minimum_float(_Property_c656c91f25774e7984172fa19514b92e_Out_0_Float, _Distance_e5ebf3ede2c749c6b7c0841481be5f81_Out_2_Float, _Minimum_8d7eee937d2845bf9b6b429c58250008_Out_2_Float);
            float _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float;
            Unity_Length_float3(SHADERGRAPH_OBJECT_POSITION, _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float);
            float _Subtract_3fc8a02b2d9e42e0aa9b0eb631d939c5_Out_2_Float;
            Unity_Subtract_float(_Minimum_8d7eee937d2845bf9b6b429c58250008_Out_2_Float, _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float, _Subtract_3fc8a02b2d9e42e0aa9b0eb631d939c5_Out_2_Float);
            float _Property_a762658b7eee44d2a7532052f595aba4_Out_0_Float = _MaxShadowDistance;
            float _Subtract_946d1263fda345cab3d54d518174aa5c_Out_2_Float;
            Unity_Subtract_float(_Property_a762658b7eee44d2a7532052f595aba4_Out_0_Float, _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float, _Subtract_946d1263fda345cab3d54d518174aa5c_Out_2_Float);
            float _Divide_c9a883d290984549996d25597c1a190b_Out_2_Float;
            Unity_Divide_float(_Subtract_3fc8a02b2d9e42e0aa9b0eb631d939c5_Out_2_Float, _Subtract_946d1263fda345cab3d54d518174aa5c_Out_2_Float, _Divide_c9a883d290984549996d25597c1a190b_Out_2_Float);
            float _Clamp_d1c037face49410eb9d987ee5ae5ba6b_Out_3_Float;
            Unity_Clamp_float(_Divide_c9a883d290984549996d25597c1a190b_Out_2_Float, float(0), float(1), _Clamp_d1c037face49410eb9d987ee5ae5ba6b_Out_3_Float);
            float _OneMinus_347ec05d74424bd18eb0df7bd00eb215_Out_1_Float;
            Unity_OneMinus_float(_Clamp_d1c037face49410eb9d987ee5ae5ba6b_Out_3_Float, _OneMinus_347ec05d74424bd18eb0df7bd00eb215_Out_1_Float);
            float _Multiply_7ca0eb8ce2464c6faeb0dc403d041b7d_Out_2_Float;
            Unity_Multiply_float_float(_Split_e7890a9c98d44536ac4fc7b9b47ff685_A_4_Float, _OneMinus_347ec05d74424bd18eb0df7bd00eb215_Out_1_Float, _Multiply_7ca0eb8ce2464c6faeb0dc403d041b7d_Out_2_Float);
            float _Multiply_f94a5e79dae346b8a058ef3a0822f23e_Out_2_Float;
            Unity_Multiply_float_float(_Multiply_7ca0eb8ce2464c6faeb0dc403d041b7d_Out_2_Float, 2, _Multiply_f94a5e79dae346b8a058ef3a0822f23e_Out_2_Float);
            float _Branch_b8f1103dd634435c881c530c864dc3a7_Out_3_Float;
            Unity_Branch_float(1, _Multiply_f94a5e79dae346b8a058ef3a0822f23e_Out_2_Float, float(0), _Branch_b8f1103dd634435c881c530c864dc3a7_Out_3_Float);
            surface.BaseColor = (_Property_edd9705f2a5c4d80bdfc5ff8ac7b9b9d_Out_0_Vector4.xyz);
            surface.Alpha = _Branch_b8f1103dd634435c881c530c864dc3a7_Out_3_Float;
            surface.AlphaClipThreshold = float(0.5);
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
            output.WorldSpacePosition =                         TransformObjectToWorld(input.positionOS);
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
            output.WorldSpacePosition = input.positionWS;
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/UnlitGBufferPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "SceneSelectionPass"
            Tags
            {
                "LightMode" = "SceneSelectionPass"
            }
        
        // Render State
        Cull Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma shader_feature_local_fragment _ _ALPHATEST_ON
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define VARYINGS_NEED_POSITION_WS
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        #define SCENESELECTIONPASS 1
        #define ALPHA_CLIP_THRESHOLD 1
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRendering.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 WorldSpacePosition;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
             float3 WorldSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS : INTERP0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.positionWS.xyz = input.positionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.positionWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _Color;
        float _MaxShadowDistance;
        float _EdgeFade;
        CBUFFER_END
        
        
        // Object and Global properties
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Distance_float3(float3 A, float3 B, out float Out)
        {
            Out = distance(A, B);
        }
        
        void Unity_Minimum_float(float A, float B, out float Out)
        {
            Out = min(A, B);
        };
        
        void Unity_Length_float3(float3 In, out float Out)
        {
            Out = length(In);
        }
        
        void Unity_Step_float(float Edge, float In, out float Out)
        {
            Out = step(Edge, In);
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Multiply_float3_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A * B;
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Clamp_float(float In, float Min, float Max, out float Out)
        {
            Out = clamp(In, Min, Max);
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Branch_float(float Predicate, float True, float False, out float Out)
        {
            Out = Predicate ? True : False;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            float _Property_c656c91f25774e7984172fa19514b92e_Out_0_Float = _MaxShadowDistance;
            float _Distance_e5ebf3ede2c749c6b7c0841481be5f81_Out_2_Float;
            Unity_Distance_float3(IN.WorldSpacePosition, float3(0, 0, 0), _Distance_e5ebf3ede2c749c6b7c0841481be5f81_Out_2_Float);
            float _Minimum_f744ee9847cc47b2bb39bb0b0560c8b4_Out_2_Float;
            Unity_Minimum_float(_Property_c656c91f25774e7984172fa19514b92e_Out_0_Float, _Distance_e5ebf3ede2c749c6b7c0841481be5f81_Out_2_Float, _Minimum_f744ee9847cc47b2bb39bb0b0560c8b4_Out_2_Float);
            float _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float;
            Unity_Length_float3(SHADERGRAPH_OBJECT_POSITION, _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float);
            float _Step_fff4387de4184026b3f63bd694128d7d_Out_2_Float;
            Unity_Step_float(_Minimum_f744ee9847cc47b2bb39bb0b0560c8b4_Out_2_Float, _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float, _Step_fff4387de4184026b3f63bd694128d7d_Out_2_Float);
            float _Multiply_a7f7b26fdabf42c688c8bac2db01dc86_Out_2_Float;
            Unity_Multiply_float_float(100, _Step_fff4387de4184026b3f63bd694128d7d_Out_2_Float, _Multiply_a7f7b26fdabf42c688c8bac2db01dc86_Out_2_Float);
            float _Add_ad0d985e0ce449cf8446a76faa7f3fce_Out_2_Float;
            Unity_Add_float(float(1), _Multiply_a7f7b26fdabf42c688c8bac2db01dc86_Out_2_Float, _Add_ad0d985e0ce449cf8446a76faa7f3fce_Out_2_Float);
            float3 _Multiply_574cc6d494324fb0859a3ea1f4bf2dc3_Out_2_Vector3;
            Unity_Multiply_float3_float3(IN.WorldSpacePosition, (_Add_ad0d985e0ce449cf8446a76faa7f3fce_Out_2_Float.xxx), _Multiply_574cc6d494324fb0859a3ea1f4bf2dc3_Out_2_Vector3);
            float3 _Transform_42f53b04feab499a923e167620802eec_Out_1_Vector3;
            _Transform_42f53b04feab499a923e167620802eec_Out_1_Vector3 = TransformWorldToObject(_Multiply_574cc6d494324fb0859a3ea1f4bf2dc3_Out_2_Vector3.xyz);
            float3 _Multiply_83fb2cd5918d43bf8baae7e9d8ef04e2_Out_2_Vector3;
            Unity_Multiply_float3_float3(_Transform_42f53b04feab499a923e167620802eec_Out_1_Vector3, float3(0.999, 0.999, 0.999), _Multiply_83fb2cd5918d43bf8baae7e9d8ef04e2_Out_2_Vector3);
            description.Position = _Multiply_83fb2cd5918d43bf8baae7e9d8ef04e2_Out_2_Vector3;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _Property_edd9705f2a5c4d80bdfc5ff8ac7b9b9d_Out_0_Vector4 = _Color;
            float _Split_e7890a9c98d44536ac4fc7b9b47ff685_R_1_Float = _Property_edd9705f2a5c4d80bdfc5ff8ac7b9b9d_Out_0_Vector4[0];
            float _Split_e7890a9c98d44536ac4fc7b9b47ff685_G_2_Float = _Property_edd9705f2a5c4d80bdfc5ff8ac7b9b9d_Out_0_Vector4[1];
            float _Split_e7890a9c98d44536ac4fc7b9b47ff685_B_3_Float = _Property_edd9705f2a5c4d80bdfc5ff8ac7b9b9d_Out_0_Vector4[2];
            float _Split_e7890a9c98d44536ac4fc7b9b47ff685_A_4_Float = _Property_edd9705f2a5c4d80bdfc5ff8ac7b9b9d_Out_0_Vector4[3];
            float _Property_c656c91f25774e7984172fa19514b92e_Out_0_Float = _MaxShadowDistance;
            float _Distance_e5ebf3ede2c749c6b7c0841481be5f81_Out_2_Float;
            Unity_Distance_float3(IN.WorldSpacePosition, float3(0, 0, 0), _Distance_e5ebf3ede2c749c6b7c0841481be5f81_Out_2_Float);
            float _Minimum_8d7eee937d2845bf9b6b429c58250008_Out_2_Float;
            Unity_Minimum_float(_Property_c656c91f25774e7984172fa19514b92e_Out_0_Float, _Distance_e5ebf3ede2c749c6b7c0841481be5f81_Out_2_Float, _Minimum_8d7eee937d2845bf9b6b429c58250008_Out_2_Float);
            float _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float;
            Unity_Length_float3(SHADERGRAPH_OBJECT_POSITION, _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float);
            float _Subtract_3fc8a02b2d9e42e0aa9b0eb631d939c5_Out_2_Float;
            Unity_Subtract_float(_Minimum_8d7eee937d2845bf9b6b429c58250008_Out_2_Float, _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float, _Subtract_3fc8a02b2d9e42e0aa9b0eb631d939c5_Out_2_Float);
            float _Property_a762658b7eee44d2a7532052f595aba4_Out_0_Float = _MaxShadowDistance;
            float _Subtract_946d1263fda345cab3d54d518174aa5c_Out_2_Float;
            Unity_Subtract_float(_Property_a762658b7eee44d2a7532052f595aba4_Out_0_Float, _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float, _Subtract_946d1263fda345cab3d54d518174aa5c_Out_2_Float);
            float _Divide_c9a883d290984549996d25597c1a190b_Out_2_Float;
            Unity_Divide_float(_Subtract_3fc8a02b2d9e42e0aa9b0eb631d939c5_Out_2_Float, _Subtract_946d1263fda345cab3d54d518174aa5c_Out_2_Float, _Divide_c9a883d290984549996d25597c1a190b_Out_2_Float);
            float _Clamp_d1c037face49410eb9d987ee5ae5ba6b_Out_3_Float;
            Unity_Clamp_float(_Divide_c9a883d290984549996d25597c1a190b_Out_2_Float, float(0), float(1), _Clamp_d1c037face49410eb9d987ee5ae5ba6b_Out_3_Float);
            float _OneMinus_347ec05d74424bd18eb0df7bd00eb215_Out_1_Float;
            Unity_OneMinus_float(_Clamp_d1c037face49410eb9d987ee5ae5ba6b_Out_3_Float, _OneMinus_347ec05d74424bd18eb0df7bd00eb215_Out_1_Float);
            float _Multiply_7ca0eb8ce2464c6faeb0dc403d041b7d_Out_2_Float;
            Unity_Multiply_float_float(_Split_e7890a9c98d44536ac4fc7b9b47ff685_A_4_Float, _OneMinus_347ec05d74424bd18eb0df7bd00eb215_Out_1_Float, _Multiply_7ca0eb8ce2464c6faeb0dc403d041b7d_Out_2_Float);
            float _Multiply_f94a5e79dae346b8a058ef3a0822f23e_Out_2_Float;
            Unity_Multiply_float_float(_Multiply_7ca0eb8ce2464c6faeb0dc403d041b7d_Out_2_Float, 2, _Multiply_f94a5e79dae346b8a058ef3a0822f23e_Out_2_Float);
            float _Branch_b8f1103dd634435c881c530c864dc3a7_Out_3_Float;
            Unity_Branch_float(1, _Multiply_f94a5e79dae346b8a058ef3a0822f23e_Out_2_Float, float(0), _Branch_b8f1103dd634435c881c530c864dc3a7_Out_3_Float);
            surface.Alpha = _Branch_b8f1103dd634435c881c530c864dc3a7_Out_3_Float;
            surface.AlphaClipThreshold = float(0.5);
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
            output.WorldSpacePosition =                         TransformObjectToWorld(input.positionOS);
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
            output.WorldSpacePosition = input.positionWS;
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "ScenePickingPass"
            Tags
            {
                "LightMode" = "Picking"
            }
        
        // Render State
        Cull [_Cull]
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma shader_feature_local_fragment _ _ALPHATEST_ON
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define VARYINGS_NEED_POSITION_WS
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        #define SCENEPICKINGPASS 1
        #define ALPHA_CLIP_THRESHOLD 1
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRendering.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 WorldSpacePosition;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
             float3 WorldSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS : INTERP0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.positionWS.xyz = input.positionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.positionWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _Color;
        float _MaxShadowDistance;
        float _EdgeFade;
        CBUFFER_END
        
        
        // Object and Global properties
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Distance_float3(float3 A, float3 B, out float Out)
        {
            Out = distance(A, B);
        }
        
        void Unity_Minimum_float(float A, float B, out float Out)
        {
            Out = min(A, B);
        };
        
        void Unity_Length_float3(float3 In, out float Out)
        {
            Out = length(In);
        }
        
        void Unity_Step_float(float Edge, float In, out float Out)
        {
            Out = step(Edge, In);
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Multiply_float3_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A * B;
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Clamp_float(float In, float Min, float Max, out float Out)
        {
            Out = clamp(In, Min, Max);
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Branch_float(float Predicate, float True, float False, out float Out)
        {
            Out = Predicate ? True : False;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            float _Property_c656c91f25774e7984172fa19514b92e_Out_0_Float = _MaxShadowDistance;
            float _Distance_e5ebf3ede2c749c6b7c0841481be5f81_Out_2_Float;
            Unity_Distance_float3(IN.WorldSpacePosition, float3(0, 0, 0), _Distance_e5ebf3ede2c749c6b7c0841481be5f81_Out_2_Float);
            float _Minimum_f744ee9847cc47b2bb39bb0b0560c8b4_Out_2_Float;
            Unity_Minimum_float(_Property_c656c91f25774e7984172fa19514b92e_Out_0_Float, _Distance_e5ebf3ede2c749c6b7c0841481be5f81_Out_2_Float, _Minimum_f744ee9847cc47b2bb39bb0b0560c8b4_Out_2_Float);
            float _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float;
            Unity_Length_float3(SHADERGRAPH_OBJECT_POSITION, _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float);
            float _Step_fff4387de4184026b3f63bd694128d7d_Out_2_Float;
            Unity_Step_float(_Minimum_f744ee9847cc47b2bb39bb0b0560c8b4_Out_2_Float, _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float, _Step_fff4387de4184026b3f63bd694128d7d_Out_2_Float);
            float _Multiply_a7f7b26fdabf42c688c8bac2db01dc86_Out_2_Float;
            Unity_Multiply_float_float(100, _Step_fff4387de4184026b3f63bd694128d7d_Out_2_Float, _Multiply_a7f7b26fdabf42c688c8bac2db01dc86_Out_2_Float);
            float _Add_ad0d985e0ce449cf8446a76faa7f3fce_Out_2_Float;
            Unity_Add_float(float(1), _Multiply_a7f7b26fdabf42c688c8bac2db01dc86_Out_2_Float, _Add_ad0d985e0ce449cf8446a76faa7f3fce_Out_2_Float);
            float3 _Multiply_574cc6d494324fb0859a3ea1f4bf2dc3_Out_2_Vector3;
            Unity_Multiply_float3_float3(IN.WorldSpacePosition, (_Add_ad0d985e0ce449cf8446a76faa7f3fce_Out_2_Float.xxx), _Multiply_574cc6d494324fb0859a3ea1f4bf2dc3_Out_2_Vector3);
            float3 _Transform_42f53b04feab499a923e167620802eec_Out_1_Vector3;
            _Transform_42f53b04feab499a923e167620802eec_Out_1_Vector3 = TransformWorldToObject(_Multiply_574cc6d494324fb0859a3ea1f4bf2dc3_Out_2_Vector3.xyz);
            float3 _Multiply_83fb2cd5918d43bf8baae7e9d8ef04e2_Out_2_Vector3;
            Unity_Multiply_float3_float3(_Transform_42f53b04feab499a923e167620802eec_Out_1_Vector3, float3(0.999, 0.999, 0.999), _Multiply_83fb2cd5918d43bf8baae7e9d8ef04e2_Out_2_Vector3);
            description.Position = _Multiply_83fb2cd5918d43bf8baae7e9d8ef04e2_Out_2_Vector3;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _Property_edd9705f2a5c4d80bdfc5ff8ac7b9b9d_Out_0_Vector4 = _Color;
            float _Split_e7890a9c98d44536ac4fc7b9b47ff685_R_1_Float = _Property_edd9705f2a5c4d80bdfc5ff8ac7b9b9d_Out_0_Vector4[0];
            float _Split_e7890a9c98d44536ac4fc7b9b47ff685_G_2_Float = _Property_edd9705f2a5c4d80bdfc5ff8ac7b9b9d_Out_0_Vector4[1];
            float _Split_e7890a9c98d44536ac4fc7b9b47ff685_B_3_Float = _Property_edd9705f2a5c4d80bdfc5ff8ac7b9b9d_Out_0_Vector4[2];
            float _Split_e7890a9c98d44536ac4fc7b9b47ff685_A_4_Float = _Property_edd9705f2a5c4d80bdfc5ff8ac7b9b9d_Out_0_Vector4[3];
            float _Property_c656c91f25774e7984172fa19514b92e_Out_0_Float = _MaxShadowDistance;
            float _Distance_e5ebf3ede2c749c6b7c0841481be5f81_Out_2_Float;
            Unity_Distance_float3(IN.WorldSpacePosition, float3(0, 0, 0), _Distance_e5ebf3ede2c749c6b7c0841481be5f81_Out_2_Float);
            float _Minimum_8d7eee937d2845bf9b6b429c58250008_Out_2_Float;
            Unity_Minimum_float(_Property_c656c91f25774e7984172fa19514b92e_Out_0_Float, _Distance_e5ebf3ede2c749c6b7c0841481be5f81_Out_2_Float, _Minimum_8d7eee937d2845bf9b6b429c58250008_Out_2_Float);
            float _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float;
            Unity_Length_float3(SHADERGRAPH_OBJECT_POSITION, _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float);
            float _Subtract_3fc8a02b2d9e42e0aa9b0eb631d939c5_Out_2_Float;
            Unity_Subtract_float(_Minimum_8d7eee937d2845bf9b6b429c58250008_Out_2_Float, _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float, _Subtract_3fc8a02b2d9e42e0aa9b0eb631d939c5_Out_2_Float);
            float _Property_a762658b7eee44d2a7532052f595aba4_Out_0_Float = _MaxShadowDistance;
            float _Subtract_946d1263fda345cab3d54d518174aa5c_Out_2_Float;
            Unity_Subtract_float(_Property_a762658b7eee44d2a7532052f595aba4_Out_0_Float, _Length_6fa09868bf84448ea0e3ba8da3d890e0_Out_1_Float, _Subtract_946d1263fda345cab3d54d518174aa5c_Out_2_Float);
            float _Divide_c9a883d290984549996d25597c1a190b_Out_2_Float;
            Unity_Divide_float(_Subtract_3fc8a02b2d9e42e0aa9b0eb631d939c5_Out_2_Float, _Subtract_946d1263fda345cab3d54d518174aa5c_Out_2_Float, _Divide_c9a883d290984549996d25597c1a190b_Out_2_Float);
            float _Clamp_d1c037face49410eb9d987ee5ae5ba6b_Out_3_Float;
            Unity_Clamp_float(_Divide_c9a883d290984549996d25597c1a190b_Out_2_Float, float(0), float(1), _Clamp_d1c037face49410eb9d987ee5ae5ba6b_Out_3_Float);
            float _OneMinus_347ec05d74424bd18eb0df7bd00eb215_Out_1_Float;
            Unity_OneMinus_float(_Clamp_d1c037face49410eb9d987ee5ae5ba6b_Out_3_Float, _OneMinus_347ec05d74424bd18eb0df7bd00eb215_Out_1_Float);
            float _Multiply_7ca0eb8ce2464c6faeb0dc403d041b7d_Out_2_Float;
            Unity_Multiply_float_float(_Split_e7890a9c98d44536ac4fc7b9b47ff685_A_4_Float, _OneMinus_347ec05d74424bd18eb0df7bd00eb215_Out_1_Float, _Multiply_7ca0eb8ce2464c6faeb0dc403d041b7d_Out_2_Float);
            float _Multiply_f94a5e79dae346b8a058ef3a0822f23e_Out_2_Float;
            Unity_Multiply_float_float(_Multiply_7ca0eb8ce2464c6faeb0dc403d041b7d_Out_2_Float, 2, _Multiply_f94a5e79dae346b8a058ef3a0822f23e_Out_2_Float);
            float _Branch_b8f1103dd634435c881c530c864dc3a7_Out_3_Float;
            Unity_Branch_float(1, _Multiply_f94a5e79dae346b8a058ef3a0822f23e_Out_2_Float, float(0), _Branch_b8f1103dd634435c881c530c864dc3a7_Out_3_Float);
            surface.Alpha = _Branch_b8f1103dd634435c881c530c864dc3a7_Out_3_Float;
            surface.AlphaClipThreshold = float(0.5);
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
            output.WorldSpacePosition =                         TransformObjectToWorld(input.positionOS);
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
            output.WorldSpacePosition = input.positionWS;
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
    }
    CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
    CustomEditorForRenderPipeline "UnityEditor.ShaderGraphUnlitGUI" "UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset"
    FallBack "Hidden/Shader Graph/FallbackError"
}