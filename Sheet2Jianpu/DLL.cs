﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.ComponentModel;

namespace Qromodyn
{
    /// <summary>
    /// A class used by managed classes to managed unmanaged DLLs.
    /// This will extract and load DLLs from embedded binary resources.
    /// 
    /// This can be used with pinvoke, as well as manually loading DLLs your own way. If you use pinvoke, you don't need to load the DLLs, just
    /// extract them. When the DLLs are extracted, the %PATH% environment variable is updated to point to the temporary folder.
    ///
    /// To Use
    /// <list type="">
    /// <item>Add all of the DLLs as binary file resources to the project Propeties. Double click Properties/Resources.resx,
    /// Add Resource, Add Existing File. The resource name will be similar but not exactly the same as the DLL file name.</item>
    /// <item>In a static constructor of your application, call EmbeddedDllClass.ExtractEmbeddedDlls() for each DLL that is needed</item>
    /// <example>
    ///               EmbeddedDllClass.ExtractEmbeddedDlls("libFrontPanel-pinv.dll", Properties.Resources.libFrontPanel_pinv);
    /// </example>
    /// <item>Optional: In a static constructor of your application, call EmbeddedDllClass.LoadDll() to load the DLLs you have extracted. This is not necessary for pinvoke</item>
    /// <example>
    ///               EmbeddedDllClass.LoadDll("myscrewball.dll");
    /// </example>
    /// <item>Continue using standard Pinvoke methods for the desired functions in the DLL</item>
    /// </list>
    /// </summary>
    public class EmbeddedDllClass
    {
        private static string tempFolder = "";
        
        /// <summary>
        /// Extract DLLs from resources to temporary folder
        /// </summary>
        /// <param name="dllName">name of DLL file to create (including dll suffix)</param>
        /// <param name="resourceBytes">The resource name (fully qualified)</param>
        public static void ExtractEmbeddedDlls(string dllName)
        {
            Assembly assem = Assembly.GetExecutingAssembly();
            AssemblyName an = assem.GetName();

            byte[] resourceBytes;
            using (var stream = assem.GetManifestResourceStream("Sheet2Jianpu.Resources." + dllName)) //http://stackoverflow.com/questions/10412401/how-to-read-an-embedded-resource-as-array-of-bytes-without-writing-it-to-disk
            {
                resourceBytes = new byte[stream.Length];
                stream.Read(resourceBytes, 0, resourceBytes.Length);
                // TODO: use the buffer that was read
            }

            // The temporary folder holds one or more of the temporary DLLs
            // It is made "unique" to avoid different versions of the DLL or architectures.
            tempFolder = System.IO.Directory.GetCurrentDirectory();

            string dirName = Path.Combine(Path.GetTempPath(), tempFolder);
            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }
            
            // See if the file exists, avoid rewriting it if not necessary
            string dllPath = Path.Combine(dirName, dllName);
            bool rewrite = true;
            if (File.Exists(dllPath))
            {
                byte[] existing = File.ReadAllBytes(dllPath);
                if (resourceBytes.SequenceEqual(existing))
                {
                    rewrite = false; //如果檔案相同則不覆蓋
                }
            }
            if (rewrite)
            {
                File.WriteAllBytes(dllPath, resourceBytes); //寫出dll檔案到該路徑
            }
        }

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern IntPtr LoadLibrary(string lpFileName);

        /// <summary>
        /// managed wrapper around LoadLibrary
        /// </summary>
        /// <param name="dllName"></param>
        static public void LoadDll(string dllName)
        {
            if (tempFolder == "")
            {
                throw new Exception("Please call ExtractEmbeddedDlls before LoadDll");
            }
            IntPtr h = LoadLibrary(dllName);
            if (h == IntPtr.Zero)
            {
                Exception e = new Win32Exception();
                throw new DllNotFoundException("Unable to load library: " + dllName + " from " + tempFolder, e);
            }
        }

    }
}