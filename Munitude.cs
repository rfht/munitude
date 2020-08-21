/*
 * Problem => Solution
 *
 *
 * System.IO.FileNotFoundException: Could not load file or assembly 'System.Core, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e' or one of its dependencies. => ln -sf /usr/local/lib/mono/4.8-api/System.Core.dll System.Core.dll
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

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

		public static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
		{
			if (assembly == null) throw new ArgumentNullException(nameof(assembly));
			try
			{
				return assembly.GetTypes();
			}
			catch (ReflectionTypeLoadException e)
			{
				Console.WriteLine("GetLoadableTypes: ReflectionTypeLoadException encountered");
				return e.Types.Where(t => t != null);
			}
		}

		public static IEnumerable<MethodInfo> GetLoadableMethods(Type type, BindingFlags bflags)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));
			try
			{
				return type.GetMethods(bflags);
			}
			catch (TypeLoadException e)
			{
				Console.WriteLine("GetLoadableMethods: TypeLoadException encountered");
				return new List<MethodInfo>();
			}
		}

		public static IEnumerable<ConstructorInfo> GetLoadableConstructors(Type type)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));
			try
			{
				return type.GetConstructors();
			}
			catch (TypeLoadException e)
			{
				Console.WriteLine("GetLoadableConstructors: TypeLoadException encountered");
				return new List<ConstructorInfo>();
			}
		}

		public MunitudeReflections()
		{
		} // MunitudeReflections.ctor

	} // MunitudeReflections

	class MunitudeCore
	{
		public static string[] GameAssemblies = new string[]{
			"Assembly-CSharp", 
			"UnityEngine",
			//"UnityEngine.CoreModule",
			//"Assembly-CSharp-firstpass",
			//"Assembly-UnityScript-firstpass"
		};

		public static void Main(string[] args)
		{
			Console.WriteLine("\nStarting Munitude...");

			Assembly assemCsharp = Assembly.Load("Assembly-CSharp");
			Console.WriteLine("\nList of assemblies loaded in current appdomain:");
			//foreach (Assembly assemb in MunitudeReflections.AppDomainAssemblies())
			foreach (string ga in GameAssemblies)
			{
				Assembly assemb = Assembly.Load(ga);
				Console.WriteLine(assemb.ToString());
				MunitudeReflections.PrintAssemInfo(assemb);
				Console.WriteLine("\nAssembly Types:\n");
				//foreach (Type assemType in assemb.GetTypes())
				foreach (Type assemType in MunitudeReflections.GetLoadableTypes(assemb))
				{
					Console.WriteLine("Type Name: {0}", assemType.ToString());
					Console.Write("Constructors:");
					//foreach (ConstructorInfo c in assemType.GetConstructors())
					foreach (ConstructorInfo c in MunitudeReflections.GetLoadableConstructors(assemType))
						Console.Write(" {0}", c);
					Console.WriteLine();
					Console.Write("Public Methods (without inherited ones):");
					//foreach (MethodInfo m in assemType.GetMethods(BindingFlags.DeclaredOnly))
					foreach (MethodInfo m in MunitudeReflections.GetLoadableMethods(assemType, BindingFlags.DeclaredOnly))
						Console.Write(" {0}", m);
					Console.WriteLine();
					Console.Write("Non-public Methods:");
					//foreach (MethodInfo m in assemType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance))
					foreach (MethodInfo m in MunitudeReflections.GetLoadableMethods(assemType, BindingFlags.NonPublic | BindingFlags.Instance))
						Console.Write(" {0}", m);
					Console.WriteLine("\n");
				} // foreach
				Console.WriteLine();
			} // foreach

			// GameObject class
			Assembly ueng = Assembly.Load("UnityEngine");
			//dynamic go = ueng.CreateInstance("GameObject");
			var myType = ueng.GetType("UnityEngine.GameObject");
			//dynamic objMyClass = Activator.CreateInstance(myType); // cant resolve internal call to "UnityEngine.GameObject::Internal_CreateGameObject"
			//Type parameterType = objMyClass.GetType();
			
			//foreach (MethodInfo m in go.GetMethods())
				//Console.WriteLine(m);
			//Console.WriteLine("{0}", parameterType.ToString());
			//myType.GetMethod("Internal_CreateGameObject").Invoke(null, null);
			foreach (MethodInfo m in myType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance))
				Console.WriteLine(m);
			//Environment.Exit(0);

			// Start
			//Type testtype = assemCsharp.GetType("IntroMovieClipPlayer");
			//object testinst = Activator.CreateInstance(testtype);
			//MethodInfo testmeth = testtype.GetMethod("Start", BindingFlags.NonPublic | BindingFlags.Instance);
			//testmeth.Invoke(testinst, null);
			//Type debugLogHandler = Assembly.Load("UnityEngine.CoreModule").GetType("DebugLogHandler", true, true);
			//Console.WriteLine("DebugLogHandler: {0}", debugLogHandler);

			Environment.Exit(0);
		
			// Awake
			foreach (string ga in GameAssemblies)
			{
				Assembly assemb = Assembly.Load(ga);
				foreach (Type assemType in assemb.GetTypes())
				{
					foreach (MethodInfo m in assemType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance))
					{
						if (m.ToString().Equals("Void Awake()"))
						{
							Console.WriteLine("Method {0} in Type {1} in Assembly {2}", m.ToString(), assemType.ToString(), ga);
							Console.WriteLine("Creating instance of {0}", assemType);
							//object instance = Activator.CreateInstance(assemType);
							//object instance = FormatterServices.GetUninitializedObject(assemType);
							//instance.GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(null, null);
							// TODO: recursive until returns "Object"
							Console.WriteLine(assemType.BaseType.Name);
							Console.WriteLine(assemType.BaseType.BaseType.Name);
							Console.WriteLine(assemType.BaseType.BaseType.BaseType.Name);
							Console.WriteLine(assemType.BaseType.BaseType.BaseType.BaseType.Name);
							Console.WriteLine(assemType.BaseType.BaseType.BaseType.BaseType.BaseType.Name);
							// instead of creating instance, use GameObject's AddComponent
							//object instance = assemb.CreateInstance(assemType.ToString());
							//object testType = go.AddComponent(assemType.ToString());
							Console.WriteLine("{0} has been created.", assemType);
							if (m != null)
							{
								/* if method with parameters, use:
								 * method.Invoke(null, new object[] { par1, par2 });
								 */
								//m.Invoke(null, null);
							} // if
							
						} // if
					} // foreach GetMethods
				} // foreach GetTypes
			} // foreach GameAssemblies


			//typeof(ExampleClass).GetMethod("Start", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(new ExampleClass(), null);
		} // Main
	} // MunitudeCore
} // Munitude
