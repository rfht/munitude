#include "munitude.h"

/*
 * needs to be called with Munitude.exe as the first argument
 */

int main(int argc, char* argv[])
{
	MonoDomain *domain;
	MonoAssembly *assembly;

	int retval;
	mono_config_parse(NULL);

	domain = mono_jit_init("MunitudeDomain");
	if (!domain)
		exit(-1);
	retval = init_internal_calls();
	assembly = mono_domain_assembly_open (domain, "Munitude.exe");
	if (!assembly)
		exit(-1);
	retval = mono_jit_exec(domain, assembly, argc - 1, argv + 1);

	mono_jit_cleanup(domain);
}
