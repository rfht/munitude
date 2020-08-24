#include <stdio.h>
#include <stdlib.h>
#include <mono/jit/jit.h>
#include <mono/metadata/assembly.h>
#include <mono/metadata/mono-config.h>

/*
 * needs to be called with Munitude.exe as the first argument
 */

int int_0()
{
	printf("STUB: int_0\n");
	return 0;
}

int main(int argc, char* argv[])
{
	MonoDomain *domain;
	MonoAssembly *assembly;

	int retval;
	mono_config_parse(NULL);

	domain = mono_jit_init("MunitudeDomain");
	if (!domain)
		exit(-1);
	mono_add_internal_call ("UnityEngine.SystemInfo::SupportsRenderTextureFormat", int_0);
	assembly = mono_domain_assembly_open (domain, "Munitude.exe");
	if (!assembly)
		exit(-1);
	retval = mono_jit_exec(domain, assembly, argc - 1, argv + 1);

	mono_jit_cleanup(domain);
}
