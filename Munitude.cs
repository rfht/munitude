/*
 * Problem => Solution
 *
 *
 * System.IO.FileNotFoundException: Could not load file or assembly 'System.Core, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e' or one of its dependencies. => ln -sf /usr/local/lib/mono/4.8-api/System.Core.dll System.Core.dll
 *
 * Could not load file or assembly 'PlayMaker => copy PlayMaker.dll from Managed directory (e.g. Enter the Gungeon)
 *
 * Assembly-CSharp-firstpass.dll exists => move it into the directory from where to run it
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

using HarmonyLib;	// 0Harmony.dll: https://github.com/pardeike/Harmony

namespace Munitude
{
	class Reflections
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

		public Reflections()
		{
		} // Reflections.ctor

	} // Reflections

	public static class MunitudeCore
	{
		public static Type dlhType;
		public static string[] typesToPatch = {
			"UnityEngine.DebugLogHandler"
		};

		//public static bool ApplyPatch()

		public static void Main(string[] args)
		{
			List<Assembly>	munitudeAssemblies	= new List<Assembly>();
			Dictionary<Type, Assembly> munitudeTypes= new Dictionary<Type, Assembly>();
			List<Type> startTypes = new List<Type>();
			List<Type> awakeTypes = new List<Type>();
			string assemblyPattern = @"^\./(UnityEngine|Assembly\-).*dll$";

			Console.WriteLine("\nStarting Munitude...");

			// get all relevant assemblies
			var matches = Directory.GetFiles(".")
				.Where(path => Regex.Match(path, assemblyPattern).Success);
			foreach (string file in matches)
				munitudeAssemblies.Add(Assembly.LoadFrom(file));

			// get types from the assemblies
			foreach (Assembly a in munitudeAssemblies.ToArray()) {
				foreach (Type t in AccessTools.GetTypesFromAssembly(a)) {
					munitudeTypes.Add(t, a);
				}
			}

			// apply patches
			var harmony = new Harmony("com.munitude.Munitude");	// initialize Harmony instance
			dlhType = munitudeTypes.Where(kvp => kvp.Key.FullName.Equals("UnityEngine.DebugLogHandler")).First().Key;
			MethodInfo original = AccessTools.DeclaredMethod(dlhType, "LogFormat");
			HarmonyMethod standin = new HarmonyMethod(typeof(Patch).GetMethod("MyLogFormat"));
			harmony.PatchAll();
			foreach (MethodBase m in Harmony.GetAllPatchedMethods())
				Console.WriteLine("Patched Method: {0}", m);

			// acs = Assembly-CSharp.dll
			Assembly acs = Assembly.Load("Assembly-CSharp");
			//Assembly acsf = Assembly.Load("Assembly-CSharp-firstpass");

			Console.Write("\nModules:");
			foreach (Module m in acs.GetModules())
				Console.Write(" {0}", m);

			// list types
			Console.Write("\nTypes:");
			Type[] acsTypes = acs.GetTypes();
			foreach (Type t in acsTypes)
				Console.Write(" {0}", t.Name);

			// list methods
			foreach (Type t in acsTypes) {
				Console.Write("\nMethods in {0}:", t);
				// ERROR in Enter the Gungeon: Methods in dfAnimatedVector3: TypeLoadException: Parent class failed to load, due to: Invalid generic instantiation type:dfAnimatedValue`1 member:(null)
				// ERROR in Dex: Methods in DexGUIRaycaster: TypeLoadException: Cannot override a non virtual method in a base type
				foreach (MethodInfo m in t.GetMethods(BindingFlags.Public |
					BindingFlags.NonPublic | BindingFlags.Instance)) {
					Console.Write(" {0}", m.Name);
					if (m.Name.Equals(@"Start"))
						startTypes.Add(t);
					else if (m.Name.Equals(@"Awake"))
						awakeTypes.Add(t);
				}
			}
			Console.WriteLine("\n");

			if (startTypes.Count > 0)
				Console.WriteLine("Start() method found in: {0}", startTypes.ToArray());
			if (awakeTypes.Count > 0)
				Console.WriteLine("Awake() method found in: {0}", awakeTypes.ToArray());
			Console.WriteLine();

			// run Start()
			foreach (Type t in startTypes)
				t.GetMethod("Start", BindingFlags.Public |
					BindingFlags.NonPublic |
					BindingFlags.Instance)
					.Invoke(Activator.CreateInstance(t), null);
			Environment.Exit(0);
		} // Main
	} // MunitudeCore

	[Harmony]
	public class Patch
	{
                static MethodBase TargetMethod() => AccessTools.DeclaredMethod(MunitudeCore.dlhType, "LogFormat");

                static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instr) =>
                        new[]
                        {
                                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch), nameof(MyLogFormat))),
                                new CodeInstruction(OpCodes.Ret)
                        };

		public static void MyLogFormat()
		{
			Console.WriteLine("STUB: LogFormat");
		} // MyLogFormat
	} // Patch
} // Munitude
