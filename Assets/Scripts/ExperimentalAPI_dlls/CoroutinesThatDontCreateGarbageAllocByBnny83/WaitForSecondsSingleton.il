
//  Microsoft (R) .NET Framework IL Disassembler.  Version 4.6.1055.0
//  Copyright (c) Microsoft Corporation. Alle Rechte vorbehalten.



// Metadata version: v4.0.30319
.assembly extern mscorlib
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}
.assembly extern UnityEngine
{
  .ver 0:0:0:0
}
.assembly WaitForSecondsSingleton
{
  .hash algorithm 0x00008004
  .ver 1:0:0:0
}
.module WaitForSecondsSingleton.dll
.imagebase 0x10000000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000001    //  ILONLY
// Image base: 0x050B0000


// =============== CLASS MEMBERS DECLARATION ===================

.class public auto ansi beforefieldinit WaitForSecondsSingleton
       extends [mscorlib]System.Object
{
  .field private static class [UnityEngine]UnityEngine.WaitForSeconds m_Instance
  .method private hidebysig static void  SetTime(class [UnityEngine]UnityEngine.WaitForSeconds aInstance,
                                                 float32 aTime) cil managed
  {
    // Code size       8 (0x8)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldarg.1
    IL_0002:  stfld      float32 [UnityEngine]UnityEngine.WaitForSeconds::m_Seconds
    IL_0007:  ret
  } // end of method WaitForSecondsSingleton::SetTime

  .method public hidebysig static class [UnityEngine]UnityEngine.WaitForSeconds 
          Get(float32 aTime) cil managed
  {
    // Code size       17 (0x11)
    .maxstack  8
    IL_0000:  ldsfld     class [UnityEngine]UnityEngine.WaitForSeconds WaitForSecondsSingleton::m_Instance
    IL_0005:  ldarg.0
    IL_0006:  call instance void [UnityEngine]UnityEngine.WaitForSeconds::.ctor(float32)
    IL_000b:  ldsfld     class [UnityEngine]UnityEngine.WaitForSeconds WaitForSecondsSingleton::m_Instance
    IL_0010:  ret
  } // end of method WaitForSecondsSingleton::Get

  .method public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  call       instance void [mscorlib]System.Object::.ctor()
    IL_0006:  ret
  } // end of method WaitForSecondsSingleton::.ctor

  .method private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  {
    // Code size       16 (0x10)
    .maxstack  8
    IL_0000:  ldc.r4     1.
    IL_0005:  newobj     instance void [UnityEngine]UnityEngine.WaitForSeconds::.ctor(float32)
    IL_000a:  stsfld     class [UnityEngine]UnityEngine.WaitForSeconds WaitForSecondsSingleton::m_Instance
    IL_000f:  ret
  } // end of method WaitForSecondsSingleton::.cctor

} // end of class WaitForSecondsSingleton

