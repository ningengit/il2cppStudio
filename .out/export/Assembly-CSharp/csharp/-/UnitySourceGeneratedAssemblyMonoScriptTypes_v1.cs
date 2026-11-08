using System;
using System.Runtime.CompilerServices;
using Il2CppStudio.Native;
using Mono.Xml;

// Token: 0x02000002 RID: 2
internal class UnitySourceGeneratedAssemblyMonoScriptTypes_v1
{
	// Token: 0x02000003 RID: 3
	private struct MonoScriptData
	{
		// Token: 0x04000001 RID: 1 Offset: 0x0
		public byte[] FilePathsData;

		// Token: 0x04000002 RID: 2 Offset: 0x8
		public byte[] TypesData;

		// Token: 0x04000003 RID: 3 Offset: 0x10
		public int TotalTypes;

		// Token: 0x04000004 RID: 4 Offset: 0x14
		public int TotalFiles;

		// Token: 0x04000005 RID: 5 Offset: 0x18
		public bool IsEditorOnly;
	}

	// Token: 0x06000001 RID: 1 RVA: 0x010DB1E0 File Offset: 0x010D97E0
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private unsafe static MonoScriptData Get()
	{
		MonoScriptData result = default(MonoScriptData);
		((MonoScriptData*)(&result))->FilePathsData = NativeIntrinsics.Reinterpret<byte[]>(0f);
		((MonoScriptData*)(&result))->TotalTypes = 0;
		byte[] array = new byte[6521];
		RuntimeHelpers.InitializeArray(array, NativeIntrinsics.Reinterpret<RuntimeFieldHandle>((nint)NativeIntrinsics.Asm("<PrivateImplementationDetails>::4FD1BA63F59EF28DCC0AE3B817277254C4FED9EACF574BC730F7F8947CB170F4_FieldInfo")));
		((MonoScriptData*)(&result))->FilePathsData = array;
		byte[] array2 = new byte[3020];
		RuntimeHelpers.InitializeArray(array2, NativeIntrinsics.Reinterpret<RuntimeFieldHandle>((nint)NativeIntrinsics.Asm("<PrivateImplementationDetails>::EAA1865ADC35520EC9A936D69C0A170C00A36B478E3737217DBC0E517A8297CC_FieldInfo")));
		((MonoScriptData*)(&result))->TypesData = array2;
		((MonoScriptData*)(&result))->TotalFiles = 106;
		((MonoScriptData*)(&result))->TotalTypes = 114;
		((MonoScriptData*)(&result))->IsEditorOnly = false;
		return result;
	}

	// Token: 0x06000002 RID: 2 RVA: 0x00263AB0 File Offset: 0x002620B0
	public UnitySourceGeneratedAssemblyMonoScriptTypes_v1()
	{
		((Mono.Xml.SecurityParser)(object)this).OnStartParsing((Mono.Xml.SmallXmlParser)null);
	}
}

