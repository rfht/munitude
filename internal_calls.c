#include "munitude.h"

int init_internal_calls() {
	mono_add_internal_call ("UnityEngine.SystemInfo::SupportsRenderTextureFormat", int_0);
	return 0;
}
