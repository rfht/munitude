using System;
using System.Reflection;
using System.Security.Policy;

public class Munitude
{
	public static void Main(string[] args)
	{
		// from https://docs.microsoft.com/en-us/dotnet/api/system.reflection.assembly?view=netcore-3.1
		//Assembly assem = typeof(MonoBehaviour).Assembly;
		Assembly assem = Assembly.Load("Assembly-CSharp");
		AssemblyName assemName = assem.GetName();
		Console.WriteLine("Name: {0}", assemName.Name);
		Console.WriteLine("Assembly Full Name: {0}", assem.FullName);
		Console.WriteLine("Version: {0}.{1}",
			assemName.Version.Major, assemName.Version.Minor);
		Console.WriteLine("Assembly CodeBase: {0}", assem.CodeBase);
		Console.WriteLine("Assembly entry point: {0}", assem.EntryPoint);

		// https://docs.microsoft.com/en-us/dotnet/api/system.reflection.assembly.getmanifestresourcenames?view=netframework-4.8
		Console.WriteLine("Assembly resource names:");
		foreach (string resname in assem.GetManifestResourceNames())
			Console.WriteLine(resname);

		// https://docs.microsoft.com/en-us/dotnet/api/system.reflection.assembly.gettypes?view=netframework-4.8
		Console.WriteLine("\nAssembly Types:");
		foreach (Type assemType in assem.GetTypes())
			Console.WriteLine(assemType.ToString());

		// https://docs.microsoft.com/en-us/dotnet/api/system.appdomain.getassemblies?view=netcore-3.1
		AppDomain currentDomain = AppDomain.CurrentDomain;
		Assembly[] assems = currentDomain.GetAssemblies();	// array for the list of assemblies
		Console.WriteLine("\nList of assemblies loaded in current appdomain:");
		foreach (Assembly assemb in assems)
			Console.WriteLine(assemb.ToString());
		
		// Start Up
		Console.WriteLine("\nStarting Munitude...");
		//typeof(ExampleClass).GetMethod("Start", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(new ExampleClass(), null);
		
	}
}
