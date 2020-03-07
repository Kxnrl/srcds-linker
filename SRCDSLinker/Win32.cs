using System;
using System.Runtime.InteropServices;

namespace SRCDSLinker
{
    class Win32
    {
        [DllImport("kernel32.dll", EntryPoint = "CreateSymbolicLink", SetLastError = true)]
        private static extern int createSymbolicLink(string lpSymlinkFileName, string lpTargetFileName, SymbolicLink dwFlags);

        public enum SymbolicLink
        {
            File = 0,
            Directory = 1
        }

        public static void CreateSymbolicLink(string source, string target, SymbolicLink typeLink)
        {
            createSymbolicLink(source, target, typeLink);
        }
    }
}
