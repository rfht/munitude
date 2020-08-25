#include "munitude.h"

int get_shaderlvl() {
	/* see documentation SystemInfo.graphicsShaderLevel:
	 * 20: Shader Model 2.0
	 * 25: Shader Model 2.5
	 * 30: Shader Model 3.0
	 * 35: OpenGL ES 3.0
	 * 40: Shader Model 4.0 (DX10.0)
	 * 45: OpenGL ES 3.1
	 * 46: OpenGL 4.1 (Shader Model 4.0 + tessellation)
	 * 50: Shader Model 5.0 (DX11.0)
	 */
	return 46;
}

int get_depthtexturemode() {
	/* Camera.depthTextureMode
	 * enumeration: 0: None (Default), 1: Depth, 2: Depth + Normals, 3: MotionVectors
	 */
	return 0;
}

void void_func() { }

int init_internal_calls() {
	mono_add_internal_call ("UnityEngine.Behaviour::set_enabled", int_0);

	mono_add_internal_call ("UnityEngine.Camera::get_depthTextureMode", get_depthtexturemode);
	mono_add_internal_call ("UnityEngine.Camera::set_depthTextureMode", int_0);

	mono_add_internal_call ("UnityEngine.Component::GetComponentFastPath", ptr_null);

	mono_add_internal_call ("UnityEngine.Debug::Internal_Log", void_func);

	mono_add_internal_call ("UnityEngine.DebugLogHandler::Internal_Log", void_func);

	mono_add_internal_call ("UnityEngine.GameObject::FindGameObjectWithTag", ptr_null);
	mono_add_internal_call ("UnityEngine.GameObject::get_activeSelf", int_0);

	mono_add_internal_call ("UnityEngine.Material::Internal_CreateWithShader", int_0);
	mono_add_internal_call ("UnityEngine.Material::get_shader", ptr_null);

	mono_add_internal_call ("UnityEngine.MonoBehaviour::.ctor", void_func);

	mono_add_internal_call ("UnityEngine.Object::ToString", string_empty);
	mono_add_internal_call ("UnityEngine.Object::set_hideFlags", int_0);

	mono_add_internal_call ("UnityEngine.Rigidbody::set_freezeRotation", int_0);

	mono_add_internal_call ("UnityEngine.Shader::get_isSupported", int_1);

	mono_add_internal_call ("UnityEngine.SystemInfo::SupportsRenderTextureFormat", int_0);
	mono_add_internal_call ("UnityEngine.SystemInfo::get_graphicsShaderLevel", get_shaderlvl);
	mono_add_internal_call ("UnityEngine.SystemInfo::get_supportsComputeShaders", int_1);
	mono_add_internal_call ("UnityEngine.SystemInfo::get_supportsRenderTextures", int_1);
	mono_add_internal_call ("UnityEngine.SystemInfo::get_supportsImageEffects", int_1);	// true if supports post-processing
	return 0;
}
