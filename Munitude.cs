using System;
using System.Reflection;

namespace Munitude
{
	class MunitudeReflections
	{
		public static Assembly[] AppDomainAssemblies()
		{
			// https://docs.microsoft.com/en-us/dotnet/api/system.appdomain.getassemblies?view=netcore-3.1
			return AppDomain.CurrentDomain.GetAssemblies();
		} //AppDomainAssemblies

		public static void PrintAssemInfo(Assembly assem)
		{
			// from https://docs.microsoft.com/en-us/dotnet/api/system.reflection.assembly?view=netcore-3.1
			//Assembly assem = typeof(MonoBehaviour).Assembly;
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
		} // PrintAssemInfo

		// not needed, just use assem.GetTypes()
		/*
		public static Type[] AssemblyTypes(Assembly assem)
		{
		} */ // AssemblyTypes

		public MunitudeReflections()
		{
		} // MunitudeReflections.ctor

	} // MunitudeReflections

	class MunitudeCore
	{
		public static string[] GameAssemblies = new string[]{ "Assembly-CSharp" };

		public static void Main(string[] args)
		{
			//Assembly assemCsharp = Assembly.Load("Assembly-CSharp");
			//Console.WriteLine("\nList of assemblies loaded in current appdomain:");
			//foreach (Assembly assemb in MunitudeReflections.AppDomainAssemblies())
			foreach (string ga in GameAssemblies)
			{
				Assembly assemb = Assembly.Load(ga);
				//Console.WriteLine(assemb.ToString());
				MunitudeReflections.PrintAssemInfo(assemb);
				Console.WriteLine("\nAssembly Types:\n");
				foreach (Type assemType in assemb.GetTypes())
				{
					Console.WriteLine("Type Name: {0}", assemType.ToString());
					Console.Write("Constructors:");
					foreach (ConstructorInfo c in assemType.GetConstructors())
						Console.Write(" {0}", c);
					Console.WriteLine();
					Console.Write("Public Methods (without inherited ones):");
					foreach (MethodInfo m in assemType.GetMethods(BindingFlags.DeclaredOnly))
						Console.Write(" {0}", m);
					Console.WriteLine();
					Console.Write("Non-public Methods:");
					foreach (MethodInfo m in assemType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance))
						Console.Write(" {0}", m);
					Console.WriteLine();
				} // foreach
				Console.WriteLine();
			} // foreach
		
			Console.WriteLine("\nStarting Munitude...");


			//typeof(ExampleClass).GetMethod("Start", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(new ExampleClass(), null);
		} // Main
	} // MunitudeCore
} // Munitude
